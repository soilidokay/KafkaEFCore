

using KafkaEFCore.Producer.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace KafkaEFCore.Producer.Implementations
{
    public class KafkaDbContextBase : DbContext
    {
        public readonly ILogger<KafkaDbContextBase> _logger;
        public readonly DbContextProducer _Producer;
        protected KafkaDbContextBase(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
            _logger = Database.GetService<ILogger<KafkaDbContextBase>>();
            _Producer = Database.GetService<DbContextProducer>();
        }
        public KafkaDbContextBase(DbContextOptions<KafkaDbContextBase> dbContextOptions) : this(dbContextOptions as DbContextOptions)
        {
        }
        public virtual Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return Database.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted, cancellationToken);
        }
        public static EntityState[] EntityStates { get => new[] { EntityState.Deleted, EntityState.Added, EntityState.Modified }; }
        public virtual IEnumerable<TopicMessage> GetEntityMessages()
        {
            return ChangeTracker.Entries()
                .Where(x => EntityStates.Contains(x.State))
                .GroupBy(x => x.Metadata.Name)
                .Select(x => new TopicMessage()
                {
                    Entities = x.Select(x => new EntityMessage { Data = x.Entity, State = x.State }).ToList(),
                    Topic = x.Key,
                    TopicEntity = x.First().Entity.GetType(),
                });
        }
        public virtual async Task<TResult> ProducerTriggerAsync<TResult>(Func<Task<TResult>> action, CancellationToken cancellationToken = default)
        {
            var strategy = Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async (ct) =>
            {
                using var tran = await BeginTransactionAsync(ct);
                try
                {
                    var data = GetEntityMessages().ToList();
                    var res = await action();
                    await _Producer.ProcessProducerAsync(data, ct);
                    await tran.CommitAsync(ct);
                    return res;
                }
                catch (Exception ex)
                {
                    await tran.RollbackAsync();
                    _logger.LogError(ex.Message, ex);
                    throw;
                }
            }, cancellationToken);

        }
        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            return await ProducerTriggerAsync(() => base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken), cancellationToken);
        }
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return ProducerTriggerAsync(() => Task.FromResult(base.SaveChanges(acceptAllChangesOnSuccess))).GetAwaiter().GetResult();
        }
    }
}
