namespace Application.UseCases.UpsertUserControl.Models
{
    public class UpsertUserControlInput
    {
        public long UserId { get; set; }
        public bool? CanReceiveNotification { get; set; }
        public DateTime? LastOpenedNotificationDate { get; set; }
    }
}
