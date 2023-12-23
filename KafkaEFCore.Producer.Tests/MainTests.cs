using Confluent.Kafka;
using KafkaEFCore.Producer.EntityTest;
using KafkaEFCore.Producer.Implementations;
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

        [Fact]
        public async Task KafkaDbContextTest()
        {
            var service = new ServiceCollection();
            service.AddDbContext<KafkaDbContext>(option => option.UseInMemoryDatabase("KafkaDbContext"));
            var provider = service.BuildServiceProvider();
            var dbcontext = ActivatorUtilities.CreateInstance<KafkaDbContext>(provider);
            for (int i = 0; i < 10; i++)
            {
                dbcontext.Students.Add(new Student
                {
                    Age = Faker.RandomNumber.Next(30, 50),
                    Birthday = DateTime.UtcNow,
                    Name = Faker.Name.FullName()
                });
            }

            await dbcontext.SaveChangesAsync();
            dbcontext.Students.RemoveRange(dbcontext.Students);
            for (int i = 0; i < 10; i++)
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
