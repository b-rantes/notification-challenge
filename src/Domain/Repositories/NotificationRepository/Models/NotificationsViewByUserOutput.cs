namespace Domain.Repositories.NotificationRepository.Models
{
    public class NotificationsViewByUserOutput
    {
        public List<NotificationViewItem> Notifications { get; set; } = new List<NotificationViewItem>();
        public long UserId { get; set; }
        public DateTime? LastOpenedNotificationDate { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
