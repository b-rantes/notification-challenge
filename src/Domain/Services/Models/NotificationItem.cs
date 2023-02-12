namespace Domain.Services.Models
{
    public sealed class NotificationItem
    {
        public NotificationItem()
        {

        }

        public bool IsNewNotification { get; set; }
        public object? NotificationContent { get; set; }
        public DateTime NotificationCreationDate { get; set; }
    }
}
