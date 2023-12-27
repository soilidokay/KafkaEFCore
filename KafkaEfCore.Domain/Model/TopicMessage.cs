using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaEfCore.Domain.Model
{
    public enum KEntityState
    {
        /// <summary>
        ///     The entity is not being tracked by the context.
        /// </summary>
        Detached = 0,

        /// <summary>
        ///     The entity is being tracked by the context and exists in the database. Its property
        ///     values have not changed from the values in the database.
        /// </summary>
        Unchanged = 1,

        /// <summary>
        ///     The entity is being tracked by the context and exists in the database. It has been marked
        ///     for deletion from the database.
        /// </summary>
        Deleted = 2,

        /// <summary>
        ///     The entity is being tracked by the context and exists in the database. Some or all of its
        ///     property values have been modified.
        /// </summary>
        Modified = 3,

        /// <summary>
        ///     The entity is being tracked by the context but does not yet exist in the database.
        /// </summary>
        Added = 4,
        Unknown = 5,
    }
    public class EntityMessage<TModel>
    {
        public TModel Data { get; set; }
        public string State { get; set; }
        public string FullClassName { get; set; }
        [JsonIgnore]
        public KEntityState KState
        {
            get => Enum.TryParse(State, out KEntityState state) ? state : KEntityState.Unknown;
        }
    }
    public class EntityMessage: EntityMessage<object>
    {
    }
    public class TopicMessage
    {
        public string Topic { get; set; }
        public Type TopicEntity { get; set; }
        public IList<EntityMessage> Entities { get; set; }
    }
}
