using Confluent.Kafka;
using KafkaEfCore.Domain.Model;
using KafkaEFCore.Producer.Common;
using KafkaEFCore.Producer.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaEFCore.Producer.Implementations
{
    public class IgnorableCollectionPropertiesContractResolver : DefaultContractResolver
    {
        public IgnorableCollectionPropertiesContractResolver()
        {
        }

        protected override JsonProperty CreateProperty(System.Reflection.MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            if (property.PropertyType.IsGenericType && property.PropertyType.IsAssignableTo(typeof(IEnumerable)))
            {
                property.ShouldSerialize = _ => false;
            }

            var type = property.PropertyType;
            if (type.IsClass && !type.IsPrimitive && type.Namespace != null && !type.Namespace.StartsWith("System"))
            {
                property.ShouldSerialize = _ => false;
            }

            return property;
        }
    }

    public class DbContextProducer
    {
        private readonly KafkaProducerOption _Option;
        private readonly ILogger<DbContextProducer> _Logger;
        private readonly JsonSerializerSettings _JsonConvertSetting;
        public DbContextProducer(IOptions<KafkaProducerOption> options, ILogger<DbContextProducer> logger)
        {
            _Option = options.Value;
            _Logger = logger;
            _JsonConvertSetting = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Converters = new List<JsonConverter> { new Newtonsoft.Json.Converters.StringEnumConverter() },
                ContractResolver = new IgnorableCollectionPropertiesContractResolver()
            };
        }

        public bool IsNeedProccess(Type topic)
        {
            return _Option.FullDatabase || _Option.TopicEntities.Contains(topic);
        }

        public IEnumerable<TopicMessage> GetTopicMessages(IEnumerable<TopicMessage> data)
        {
            _Logger.LogInformation($"Input topic: " + string.Join(",", data.Select(x => x.Topic)));

            if (_Option.FullDatabase) return data;

            data = data.Where(x => IsNeedProccess(x.TopicEntity));
            if (!data.Any()) { return null; }

            return data;
        }
        public string GetTopic(string topic)
        {
            return string.IsNullOrEmpty(_Option.DatabaseTopic) ? topic : _Option.DatabaseTopic;
        }

        public async Task ProcessProducerAsync(IEnumerable<TopicMessage> data, CancellationToken cancellationToken = default)
        {
            data = GetTopicMessages(data);
            if (data == null) { return; }

            _Logger.LogInformation($"Processing topic: " + string.Join(",", data.Select(x => x.Topic)));
            using (var producer = new ProducerBuilder<Null, string>(_Option.Config).Build())
            {
                foreach (var entity in data)
                {
                    foreach (var item in entity.Entities.Batches(_Option.PushBatch))
                    {
                        var Value = JsonConvert.SerializeObject(item, _JsonConvertSetting);
                        var result = await producer.ProduceAsync(GetTopic(entity.Topic), new Message<Null, string> { Value = Value }, cancellationToken);
                    }
                }
            }
        }
    }
}
