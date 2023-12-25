using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaEFCore.Producer.Options
{
    public class KafkaProducerOption
    {
        public KafkaProducerOption()
        {
            TopicEntities = new Type[] { };
        }
        public ProducerConfig Config { get; set; }
        public int PushBatch { get; set; } = 10;
        public Type[] TopicEntities { get; set; }
    }
}
