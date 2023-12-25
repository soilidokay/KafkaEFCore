using Confluent.Kafka;
using KafkaEFCore.Producer.Common;
using KafkaEFCore.Producer.DTOs;
using KafkaEFCore.Producer.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaEFCore.Producer.Implementations
{
    public class DbContextProducer
    {
        private readonly KafkaProducerOption _Option;
        private readonly ILogger<DbContextProducer> _Logger;
        public DbContextProducer(IOptions<KafkaProducerOption> options, ILogger<DbContextProducer> logger)
        {
            _Option = options.Value;
            _Logger = logger;
        }
        public async Task ProcessProducerAsync(IEnumerable<TopicMessage> data, CancellationToken cancellationToken = default)
        {
            _Logger.LogInformation($"Input topic: " + string.Join(",", data.Select(x => x.Topic)));

            data = data.Where(x => _Option.TopicEntities.Contains(x.TopicEntity));
            if (!data.Any()) { return; }

            _Logger.LogInformation($"Processing topic: " + string.Join(",", data.Select(x => x.Topic)));

            using (var producer = new ProducerBuilder<Null, string>(_Option.Config).Build())
            {
                foreach (var entity in data)
                {
                    foreach (var item in entity.Entities.Batches(_Option.PushBatch))
                    {
                        var Value = JsonConvert.SerializeObject(item, new Newtonsoft.Json.Converters.StringEnumConverter());
                        var result = await producer.ProduceAsync(entity.Topic, new Message<Null, string> { Value = Value }, cancellationToken);
                    }
                }
            }
        }
    }
}
