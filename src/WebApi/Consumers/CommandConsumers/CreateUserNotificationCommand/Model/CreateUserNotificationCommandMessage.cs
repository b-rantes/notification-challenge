namespace WebApi.Consumers.CommandConsumers.CreateUserNotificationCommand.Model
{
    public class CreateUserNotificationCommandMessage
    {
        public long UserId { get; set; }
        public Guid NotificationId { get; set; }
        public object? NotificationContent { get; set; }
    }
}
