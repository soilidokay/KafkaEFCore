using Confluent.Kafka;
using Confluent.Kafka.Admin;
using KafkaEfCore.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace KafkaEFCore.Producer.Tests.Common
{
    public static class Helper
    {
        public static TModel GetValue<TModel>(this Message<Ignore, string> message) where TModel : class
        {
            return string.IsNullOrEmpty(message.Value) ? null : JsonConvert.DeserializeObject<TModel>(message.Value);
        }
        public static object CreateObject(this string fullclassname)
        {
            var type = Type.GetType(fullclassname);
            return Activator.CreateInstance(type);
        }
        public static object ConvertJsonToObject(this object json, string fullclassname, string assemblyname)
        {
            var type = Type.GetType(fullclassname + "," + assemblyname);
            return JsonConvert.DeserializeObject(json.ToString(), type);
        }
        public static IEnumerable<Type> GetAllTableType(this DbContext dbContext)
        {
            foreach (var item in dbContext.Model.GetEntityTypes().Where(x=>!(x.ClrType.IsGenericType && x.ClrType.GetGenericTypeDefinition() == typeof(Dictionary<,>))))
            {
                yield return item.ClrType;
            }
        }
        public static async Task CheckOrCreateTopic(string topicName, int desiredNumPartitions)
        {
            var adminConfig = new AdminClientConfig
            {
                BootstrapServers = "your_bootstrap_servers",
            };

            using (var adminClient = new AdminClientBuilder(adminConfig).Build())
            {
                // Kiểm tra xem chủ đề đã tồn tại hay chưa
                var existingTopics = adminClient.GetMetadata(topicName, TimeSpan.FromMinutes(1));
                var topicExists = existingTopics.Topics.Any(t => t.Topic == topicName);

                if (!topicExists)
                {
                    // Nếu chủ đề chưa tồn tại, tạo mới với số lượng partition mong muốn
                    var newTopic = new TopicSpecification
                    {
                        Name = topicName,
                        NumPartitions = desiredNumPartitions,
                        ReplicationFactor = 1, // Thay đổi replication factor nếu cần
                    };

                    await adminClient.CreateTopicsAsync(new List<TopicSpecification> { newTopic });

                    Debug.WriteLine($"Topic '{topicName}' created with {desiredNumPartitions} partitions.");
                }
            }
        }

        public static IEnumerable<T> GetDbSet<T>(DbContext dbContext) where T : class
        {
            return dbContext.Set<T>();
        }

        public static IEnumerable GetData(this DbContext dbContext, Type type)
        {
            var method = typeof(Helper).GetMethod(nameof(GetDbSet)).MakeGenericMethod(type);
            return (IEnumerable)method.Invoke(null, new[] { dbContext });
        }
    }
}
