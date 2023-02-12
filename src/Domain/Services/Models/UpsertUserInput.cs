namespace Domain.Services.Models
{
    public class UpsertUserInput
    {
        public long Id { get; set; }
        public bool? CanReceiveNotification { get; set; }
        public DateTime? LastOpenedNotificationDate { get; set; }
    }
}
