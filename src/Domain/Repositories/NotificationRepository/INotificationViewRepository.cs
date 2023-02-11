namespace Domain.Repositories.NotificationRepository
{
    public interface INotificationViewRepository
    {
        public Task GetNotificationsByUserId(long userId, CancellationToken cancellationToken);
        public Task GetNotificationByGuid(Guid notificationId, CancellationToken cancellationToken);
    }
}
