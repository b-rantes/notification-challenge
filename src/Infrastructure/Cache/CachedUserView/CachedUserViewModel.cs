namespace Infrastructure.Cache.CachedUserView
{
    internal class CachedUserViewModel
    {
        public long Id { get; set; }
        public DateTime LastOpenedNotificationDate { get; set; }
        public bool CanReceiveNotification { get; set; }
    }
}
