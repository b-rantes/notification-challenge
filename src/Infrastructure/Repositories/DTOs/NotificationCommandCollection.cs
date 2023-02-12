using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.Repositories.DTOs
{
    public class NotificationCommandCollection
    {
        [BsonId]
        public ObjectId Identification { get; set; }
        
        public string NotificationId { get; set; }
        public long UserOwnerId { get; set; }
        public DateTime NotificationCreationDate { get; set; }
        public BsonDocument NotificationContent { get; set; } = new BsonDocument();
        public DateTime LastUpdate { get; set; }
    }
}
