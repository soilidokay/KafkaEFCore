using Confluent.Kafka;
using KafkaEFCore.Producer.EntityTest;
using KafkaEFCore.Producer.Implementations;
using KafkaEFCore.Producer.Options;
using KafkaEFCore.Producer.Tests.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaEFCore.Producer.Tests
{
    public class MainTests
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
    }
}
