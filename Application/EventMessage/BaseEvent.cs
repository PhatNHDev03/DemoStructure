using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.EventMessage
{
    public abstract class BaseEvent
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("eventType")]
        public string EventType { get; set; }

        [BsonElement("timestamp")]
        public DateTime Timestamp { get; set; }

        protected BaseEvent(string eventType)
        {
            EventType = eventType;
            Timestamp = DateTime.Now;
        }
    }
}
