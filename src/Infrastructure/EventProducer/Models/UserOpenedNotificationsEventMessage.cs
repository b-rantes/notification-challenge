namespace Infrastructure.EventProducer.Models
{
    public class UserOpenedNotificationsEventMessage
    {
        public long UserId { get; set; }
        public DateTime LastOpenedNotificationDate { get; set; }
    }
}
