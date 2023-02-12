namespace Infrastructure.Cache.CachedNotificationView.Models
{
    public class UpdateInCacheNotificationInput
    {
        public long UserId { get; set; }
        public Guid NotificationId { get; set; }
        public object? NotificationContent { get; set; }
        public DateTime NotificationCreationDate { get; set; }
        public DateTime? LastOpenedNotificationDate { get; set; }
    }
}
