namespace Infrastructure.EventProducer.Models
{
    public class UserSettingsUpdatedEventMessage
    {
        public long UserId { get; set; }
        public DateTime? LastOpenedNotificationDate { get; set; }
        public bool? CanReceiveNotification { get; set; }
    }
}
