namespace Application.UseCases.CreateUserNotification.Models
{
    public class CreateUserNotificationInput
    {
        public long UserId { get; set; }
        public Guid NotificationGuid { get; set; }
        public object? NotificationContent { get; set; }
    }
}
