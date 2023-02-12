using Domain.Repositories.NotificationRepository.Models;

namespace Domain.Repositories.NotificationRepository
{
    public interface INotificationViewRepository
    {
        public Task<NotificationsViewByUserOutput> GetNotificationsByUserId(long userId, CancellationToken cancellationToken);
    }
}
