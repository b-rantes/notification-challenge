namespace Domain.Repositories.UserRepository.Models
{
    public class UserControlView
    {
        public long Id { get; set; }
        public DateTime? LastOpenedNotificationDate { get; set; }
        public bool? CanReceiveNotification { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
