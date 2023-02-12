namespace Application.SyncServices.UpdateNotificationView.Models
{
    public class UpdateNotificationViewInput
    {
        public Guid NotificationId { get; set; }
        public long UserOwnerId { get; set; }
        public DateTime NotificationCreationDate { get; set; }
        public DateTime? LastOpenedNotificationDate { get; set; }
        public object? NotificationContent { get; set; }
    }
}
