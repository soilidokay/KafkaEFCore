

using KafkaEfCore.Domain.Model;
using KafkaEFCore.Producer.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System.Linq;

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

        public virtual IEnumerable<TopicMessage> GetEntityMessages<TModel>(DbSet<TModel> models) where TModel : class
        {
            return models.Batches(1000)
                .Select(t => new TopicMessage
                {
                    Entities = t.Select(x => new EntityMessage
                    {
                        Data = x,
                        State = KEntityState.Added.ToString(),
                        FullClassName = x.GetType().FullName
                    }).ToList(),
                    Topic = GetTopic(typeof(TModel)),
                    TopicEntity = typeof(TModel)
                });
        }
        public async Task PushAllToKafkaAsync(CancellationToken cancellationToken = default)
        {
            var typeDbSets = GetType()
              .GetProperties()
              .Where(x => x.PropertyType.IsGenericType)
              .Where(x => x.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>)).ToList();

            foreach (var item in typeDbSets)
            {
                var data = Enumerable.AsEnumerable(item.GetValue(this) as dynamic);
                await _Producer.ProcessProducerAsync(GetEntityMessages(data), cancellationToken);
            }
        }
        

        public virtual Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return Database.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted, cancellationToken);
        }
        public static KEntityState[] EntityStates { get; } = new[] { KEntityState.Deleted, KEntityState.Added, KEntityState.Modified };
        public string GetTopic(Type type)
        {
            return type.FullName;
        }
        public virtual IEnumerable<TopicMessage> GetEntityMessages()
        {
            return ChangeTracker.Entries()
                .Where(x => EntityStates.Any(y => y.ToString() == x.State.ToString()))
                .GroupBy(x => x.Entity.GetType())
                .Select(x => new TopicMessage
                {
                    Entities = x.Select(x => new EntityMessage
                    {
                        Data = x.Entity,
                        State = x.State.ToString(),
                        FullClassName = x.Entity.GetType().FullName
                    }).ToList(),
                    Topic = GetTopic(x.Key),
                    TopicEntity = x.Key,
                });
        }
        public virtual async Task<TResult> ProducerTriggerAsync<TResult>(Func<Task<TResult>> action, CancellationToken cancellationToken = default)
        {
            var data = GetEntityMessages().ToList();
            if (!data.Any(x => _Producer.IsNeedProccess(x.TopicEntity)))
            {
                return await action();
            }

            var strategy = Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async (ct) =>
            {
                using var tran = await BeginTransactionAsync(ct);
                try
                {
                    var res = await action();
                    await _Producer.ProcessProducerAsync(data, ct);
                    await tran.CommitAsync(ct);
                    return res;
                }
                catch (Exception ex)
                {
                    await tran.RollbackAsync(ct);
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
