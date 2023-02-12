namespace Domain.Repositories.NotificationRepository.Models
{
    public class SaveNotificationCommandInput
    {
        public string NotificationId { get; set; }
        public long UserOwnerId { get; set; }
        public DateTime NotificationCreationDate { get; set; }
        public object? NotificationContent { get; set; }
    }
}
