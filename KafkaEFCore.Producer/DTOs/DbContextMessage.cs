using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KafkaEFCore.Producer.DTOs
{
    public class EntityMessage
    {
        public object Data { get; set; }
        public EntityState State { get; set; }
    }
    public class DbContextMessage
    {
        public IList<EntityMessage> Entity { get; set; }
    }
}
