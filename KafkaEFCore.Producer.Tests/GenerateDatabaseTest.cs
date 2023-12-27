using Confluent.Kafka;
using KafkaEfCore.DbContexts;
using KafkaEfCore.DbContexts.Models;
using KafkaEfCore.Domain.Model;
using KafkaEFCore.Producer.Tests.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static KafkaEFCore.Producer.Tests.ProducerTests;

namespace KafkaEFCore.Producer.Tests
{
    public class GenerateDatabaseTest
    {
        public bool FilterTopic(Type[] types, EntityMessage entityMessage)
        {
            return types.Any(x => entityMessage.FullClassName.Split('.').Last() == x.Name);
        }

        public class AtmAssetManagementProductionContext2 : AtmAssetManagementProductionContext
        {

            public AtmAssetManagementProductionContext2(DbContextOptions<AtmAssetManagementProductionContext2> dbContextOptions) : base(dbContextOptions)
            {
                Database.EnsureCreated();
            }
        }

        [Fact]
        public async Task ReplicateDatabaseTest()
        {
            var service = HostServie.GetServices();
            service.AddDbContext<AtmAssetManagementProductionContext>(option => option.UseSqlServer(HostServie.ConnectionStringATM));
            service.AddDbContext<AtmAssetManagementProductionContext2>(option => option.UseSqlServer(HostServie.ConnectionStringReplicas));
            var provider = service.BuildServiceProvider();

            var contextS = provider.GetService<AtmAssetManagementProductionContext>();
            var contextT = provider.GetService<AtmAssetManagementProductionContext2>();

            foreach (var t in contextS.GetAllTableType())
            {
                var data = contextS.GetData(t);
                foreach (var item in data)
                {
                   await contextT.AddAsync(item);
                }
            }
            await contextT.SaveChangesAsync();
        }
        [Fact]
        public async Task KafkaCreateDbContextTest()
        {
            var service = HostServie.GetServices();
            service.AddDbContext<NoSqlAtmTargetContext>(option => option.UseSqlServer(HostServie.ConnectionStringTarget));
            var provider = service.BuildServiceProvider();
            var dbcontextS = ActivatorUtilities.CreateInstance<NoSqlAtmTargetContext>(provider);

            var builder = new ConsumerBuilder<Ignore, string>(HostServie.ConsumerConfig());

            //https://docs.confluent.io/kafka-clients/dotnet/current/overview.html#committing-during-a-rebalance
            builder.SetPartitionsRevokedHandler((c, partitions) => { try { c.Commit(); } catch { } });
            using var consumer = builder.Build();

            consumer.Subscribe("AtmTargetContext");

            var tokenSource = new CancellationTokenSource();
            int index = 0;

            var types = dbcontextS.GetAllTableType().ToArray();
            int insertCount = 0;
            while (!tokenSource.IsCancellationRequested)
            {
                var consumeResult = consumer.Consume(tokenSource.Token);

                var data = consumeResult.Message.GetValue<EntityMessage[]>() ?? Array.Empty<EntityMessage>();

                Debug.WriteLine($"received {data.Count()}");

                var joins = data.Join(types, x => x.FullClassName.Split('.').Last(), x => x.Name, (a, b) => new { a, b });

                foreach (var result in joins)
                {
                    Debug.WriteLine(++index + " . " + result.a.State + " . " + result.a.Data);

                    var dataRes = result.a.Data.ConvertJsonToObject(result.b.FullName, typeof(KafkaEfCore.DbContexts.NoSqlModel.Asset).Assembly.FullName);

                    object value = result.a.KState switch
                    {
                        KEntityState.Added => dbcontextS.Add(dataRes),
                        KEntityState.Modified => dbcontextS.Attach(dataRes).State == EntityState.Modified,
                        KEntityState.Detached => dbcontextS.Remove(dataRes),
                        _ => null
                    };
                    insertCount++;
                    if (insertCount % 1000 == 0)
                    {
                        await dbcontextS.SaveChangesAsync();
                        insertCount = 0;
                        consumer.StoreOffset(consumeResult);
                    }
                }

                if (insertCount > 0)
                {
                    await dbcontextS.SaveChangesAsync();
                    insertCount = 0;
                    consumer.StoreOffset(consumeResult);
                }
                //consumer.Commit(consumeResult);
                // process message here.
                //consumer.StoreOffset(consumeResult);
            }
        }
    }
}
