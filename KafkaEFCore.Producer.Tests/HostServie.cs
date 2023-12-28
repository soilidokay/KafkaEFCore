using Confluent.Kafka;
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
    public static class HostServie
    {
        public const string ConnectionString = "Server=master.node.local,32433;Database=ATM.KafkaDbContext.Test;TrustServerCertificate=True;MultipleActiveResultSets=true;User ID=Sa;Password=Sa123@@@";
        public const string ConnectionStringATM = "Server=master.node.local,32433;Database=ATM.AssetManagement.Production;TrustServerCertificate=True;MultipleActiveResultSets=true;User ID=Sa;Password=Sa123@@@";
        public const string ConnectionStringTarget = "Server=master.node.local,32433;Database=ATM.AtmTargetContext.Test;TrustServerCertificate=True;MultipleActiveResultSets=true;User ID=Sa;Password=Sa123@@@";
        public const string ConnectionStringReplicas = "Server=master.node.local,32433;Database=KafkaSourceDatabase;TrustServerCertificate=True;MultipleActiveResultSets=true;User ID=Sa;Password=Sa123@@@";
        public const string ConnectionStringdb_distibutted = "Server=master.node.local,32433;Database=db_distibutted;TrustServerCertificate=True;MultipleActiveResultSets=true;User ID=Sa;Password=Sa123@@@";

        public static IServiceCollection GetServices()
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


        public static ConsumerConfig ConsumerConfig()
        {
            return new ConsumerConfig
            {
                BootstrapServers = "172.16.1.58:9092",
                GroupId = "foo",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true,
                EnableAutoOffsetStore = false,
                ClientId = "cl-foo",
            };
        }
    }
}
