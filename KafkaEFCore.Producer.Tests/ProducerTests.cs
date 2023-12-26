using Confluent.Kafka;
using KafkaEfCore.DbContexts;
using KafkaEFCore.Producer.EntityTest;
using KafkaEFCore.Producer.Implementations;
using KafkaEFCore.Producer.Options;
using KafkaEFCore.Producer.Tests.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaEFCore.Producer.Tests
{
    public class ProducerTests
    {
        [Fact]
        public async Task ProducerTest()
        {
            var config = new ProducerConfig
            {
                BootstrapServers = "172.16.1.58:9092",
            };
            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                var result = await producer.ProduceAsync("my-topic", new Message<Null, string> { Value = "hello world" });
            }
        }
        public const string ConnectionString = "Server=master.node.local,32433;Database=ATM.KafkaDbContext.Test;TrustServerCertificate=True;MultipleActiveResultSets=true;User ID=Sa;Password=Sa123@@@";

        public IServiceCollection GetServices()
        {
            var service = new ServiceCollection();
            service.AddOptions();
            service.AddLogging();
            service.AddDbContext<KafkaDbContextTest>(option => option.UseSqlServer(ConnectionString));
            service.AddScoped<DbContextProducer>();
            service.Configure<KafkaProducerOption>(option =>
            {
                option.Config = new ProducerConfig
                {
                    BootstrapServers = "172.16.1.58:9092",
                };
                option.TopicEntities = new Type[] { typeof(Student) };
            });
            return service;
        }
        [Fact]
        public async Task KafkaDbContextTest()
        {
            var provider = GetServices().BuildServiceProvider();
            var dbcontext = ActivatorUtilities.CreateInstance<KafkaDbContextTest>(provider);

            for (int i = 0; i < 1000; i++)
            {
                dbcontext.Students.Add(new Student
                {
                    Age = Faker.RandomNumber.Next(30, 50),
                    Birthday = DateTime.UtcNow,
                    Name = Faker.Name.FullName()
                });
            }

            await dbcontext.SaveChangesAsync();
        }
        public const string ConnectionStringATM = "Server=master.node.local,32433;Database=ATM.AssetManagement.Production;TrustServerCertificate=True;MultipleActiveResultSets=true;User ID=Sa;Password=Sa123@@@";
        public const string ConnectionStringTarget = "Server=master.node.local,32433;Database=ATM.KafkaDbContext2.Test;TrustServerCertificate=True;MultipleActiveResultSets=true;User ID=Sa;Password=Sa123@@@";

        public class AtmTargetContext : AtmAssetManagementProductionContext
        {
            public AtmTargetContext(DbContextOptions<AtmTargetContext> options) : base(options)
            {
                Database.EnsureCreated();
            }
        }

        public IServiceCollection GetServices2()
        {
            var service = new ServiceCollection();
            service.AddOptions();
            service.AddLogging();
            service.AddDbContext<KafkaDbContextTest>(option => option.UseSqlServer(ConnectionString));
            service.AddScoped<DbContextProducer>();
            service.Configure<KafkaProducerOption>(option =>
            {
                option.Config = new ProducerConfig
                {
                    BootstrapServers = "172.16.1.58:9092",
                };
                option.FullDatabase = true;
                option.DatabaseTopic = "AtmTargetContext";
            });
            return service;
        }
        [Fact]
        public async Task KafkaDbContext2Test()
        {
            var service = GetServices2();
            service.AddDbContext<AtmAssetManagementProductionContext>(opt => opt.UseSqlServer(ConnectionStringATM));
            var provider = service.BuildServiceProvider();
            var dbcontextS = ActivatorUtilities.CreateInstance<AtmAssetManagementProductionContext>(provider);

            foreach (var entityType in dbcontextS.Model.GetEntityTypes().Where(x => x.GetNavigations().All(y => y.IsCollection)))
            {
                Debug.WriteLine($"Table: {entityType.GetTableName()}");

                // Duyệt qua tất cả các quan hệ của entity
                foreach (var navigation in entityType.GetNavigations().Where(x => x.IsCollection))
                {
                    Debug.WriteLine($"  Navigation Property: {navigation.Name}");
                }
            }

            //await dbcontextS.PushAllToKafkaAsync();
        }
    }
}
