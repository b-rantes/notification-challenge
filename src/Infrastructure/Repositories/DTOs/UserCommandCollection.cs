using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.Repositories.DTOs
{
    public class UserCommandCollection
    {
        [BsonId]
        public ObjectId Identification { get; set; }

        public long Id { get; set; }

        public DateTime LastOpenedNotificationDate { get; set; }

        public bool CanReceiveNotification { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
