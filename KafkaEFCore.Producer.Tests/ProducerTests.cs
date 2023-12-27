using Confluent.Kafka;
using KafkaEfCore.DbContexts;
using KafkaEfCore.Domain.Model;
using KafkaEFCore.Producer.EntityTest;
using KafkaEFCore.Producer.Implementations;
using KafkaEFCore.Producer.Options;
using KafkaEFCore.Producer.Tests.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
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
     
        [Fact]
        public async Task KafkaDbContextTest()
        {
            var provider = HostServie.GetServices().BuildServiceProvider();
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
        public class AtmTargetContext : AtmAssetManagementProductionContext
        {
            public AtmTargetContext(DbContextOptions<AtmTargetContext> options) : base(options)
            {
                Database.EnsureCreated();
            }
        }

        [Fact]
        public async Task KafkaDbContext2Test()
        {
            var service = HostServie.GetServices();
            service.AddDbContext<AtmAssetManagementProductionContext>(opt => opt.UseSqlServer(HostServie.ConnectionStringATM));
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
