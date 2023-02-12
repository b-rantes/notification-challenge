namespace Infrastructure.Cache.CachedUserView.Models
{
    public class UpdateCachedUserControlViewInput
    {
        public long Id { get; set; }
        public DateTime? LastOpenedNotificationDate { get; set; }
        public bool? CanReceiveNotification { get; set; }
    }
}
