using Confluent.Kafka;
using Faker;
using KafkaEfCore.Domain.Model;
using KafkaEFCore.Producer.EntityTest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KafkaEFCore.Producer.Tests
{
    public class ConsumerTest
    {
        /// <summary>
        /// https://docs.confluent.io/kafka-clients/dotnet/current/overview.html#auto-offset-commit
        /// </summary>
        [Fact]
        public void Test1()
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "172.16.1.58:9092",
                GroupId = "foo",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true,
                EnableAutoOffsetStore = false,
                ClientId = "cl-foo",
            };

            var builder = new ConsumerBuilder<Ignore, string>(config);
            //https://docs.confluent.io/kafka-clients/dotnet/current/overview.html#committing-during-a-rebalance
            builder.SetPartitionsRevokedHandler((c, partitions) => { try { c.Commit(); } catch { } });
            using var consumer = builder.Build();
            consumer.Subscribe("KafkaEFCore.Producer.EntityTest.Student");
            var tokenSource = new CancellationTokenSource();
            int index = 0;
            while (!tokenSource.IsCancellationRequested)
            {
                var consumeResult = consumer.Consume(tokenSource.Token);

                foreach (var result in JsonConvert.DeserializeObject<EntityMessage[]>(consumeResult.Message.Value) ?? Array.Empty<EntityMessage>())
                {
                    Debug.WriteLine(++index + " . " + result.State + " . " + result.Data);
                }
                //consumer.Commit(consumeResult);

                // process message here.
                consumer.StoreOffset(consumeResult);
            }
        }
    }
}
