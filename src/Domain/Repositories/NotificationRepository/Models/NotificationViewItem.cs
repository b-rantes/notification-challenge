namespace Domain.Repositories.NotificationRepository.Models
{
    public class NotificationViewItem
    {
        public DateTime NotificationCreationDate { get; set; }
        public object? NotificationContent { get; set; }
        public string NotificationId { get; set; }
    }
}
