using MongoDB.Bson;

namespace Infrastructure.Repositories.DTOs
{
    public class NotificationCommandCollection
    {
        public Guid NotificationId { get; set; }
        public long UserOwnerId { get; set; }
        public DateTime NotificationCreationDate { get; set; }
        public BsonDocument NotificationContent { get; set; } = new BsonDocument();
    }
}
