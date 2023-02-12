namespace WebApi.Controllers.AdminControllerModels
{
    public class CreateNotificationRequest
    {
        public Guid NotificationId { get; set; }
        public Dictionary<string, object> NotificationContent { get; set; }
        public long UserId { get; set; }
    }
}
