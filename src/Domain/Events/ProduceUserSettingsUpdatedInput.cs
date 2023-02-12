namespace Domain.Events
{
    public class ProduceUserSettingsUpdatedInput
    {
        public long Id { get; set; }
        public bool? CanReceiveNotification { get; set; }
        public DateTime? LastOpenedNotificationDate { get; set; }
    }
}