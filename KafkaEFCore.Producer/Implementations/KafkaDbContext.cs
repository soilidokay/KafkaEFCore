using Confluent.Kafka;
using KafkaEFCore.Producer.Common;
using KafkaEFCore.Producer.DTOs;
using KafkaEFCore.Producer.EntityTest;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KafkaEFCore.Producer.Implementations
{
    public class KafkaDbContext : DbContext
    {
        public KafkaDbContext(DbContextOptions<KafkaDbContext> dbContextOptions) : base(dbContextOptions)
        {
        }
        public static EntityState[] EntityStates { get => new[] { EntityState.Deleted, EntityState.Added, EntityState.Modified }; }

        public async Task ProcessProducer()
        {
            var data = ChangeTracker.Entries()
                .Where(x => EntityStates.Contains(x.State));

            var config = new ProducerConfig
            {
                BootstrapServers = "172.16.1.58:9092",
            };
            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {

                foreach (var entity in data.GroupBy(x => x.Metadata.Name))
                {
                    foreach (var item in entity.Batches(1000))
                    {
                        var Value = JsonConvert.SerializeObject(item.Select(x => new EntityMessage
                        {
                            Data = x.Entity,
                            State = x.State
                        }),
                        new Newtonsoft.Json.Converters.StringEnumConverter());
                        var result = await producer.ProduceAsync(entity.Key, new Message<Null, string> { Value = Value });
                    }
                }
            }
        }
        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            await ProcessProducer();
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ProcessProducer().Wait();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }


        public DbSet<Student> Students { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Student>(e =>
            {
            });
        }
    }
}
