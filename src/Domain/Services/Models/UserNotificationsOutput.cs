namespace Domain.Services.Models
{
    public sealed class UserNotificationsOutput
    {
        public List<NotificationItem> Notifications { get; set; }
        public int NewNotificationsCount => Notifications.Count(x => x.IsNewNotification);
    }
}
