using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.EventMessage
{

    public class InventoryEventType
    {
        public static string Created = "inventory-created";
        public static string Failed = "inventory-failed";
        public static string Success = "inventory-success";
    
    }
    public abstract class InventoryEvent : BaseEvent
    {
        [BsonElement("inventoryId")]
        public string InventoryId { get; set; }

        [BsonElement("status")]
        public string Status { get; set; }

        protected InventoryEvent(string eventType) : base(eventType) { }
    }


    public class InventoryCreatedEvent : InventoryEvent
    {
        [BsonElement("productId")]
        public string ProductId { get; set; }

        [BsonElement("userId")]
        public string UserId { get; set; }

        [BsonElement("quantity")]
        public int Quantity { get; set; }

        [BsonElement("initialStatus")]
        public string InitialStatus { get; set; } = "pending";

        public InventoryCreatedEvent() : base(InventoryEventType.Created)
        {
            Status = "created";
        }
    }
    public class InventoryFailEvent : InventoryEvent
    {
        [BsonElement("failureReason")]
        public string FailureReason { get; set; }

        [BsonElement("errorCode")]
        public string ErrorCode { get; set; }

        [BsonElement("failedAt")]
        public DateTime FailedAt { get; set; }

        public InventoryFailEvent() : base(InventoryEventType.Failed)
        {
            Status = "failed";
            FailedAt = DateTime.UtcNow;
        }
    }
    public class InventorySuccessEvent : InventoryEvent
    {
        [BsonElement("completedAt")]
        public DateTime CompletedAt { get; set; }

        [BsonElement("processedBy")]
        public string ProcessedBy { get; set; }

        [BsonElement("finalQuantity")]
        public int? FinalQuantity { get; set; }

        public InventorySuccessEvent() : base(InventoryEventType.Success)
        {
            Status = "success";
            CompletedAt = DateTime.UtcNow;
        }
    }
}
