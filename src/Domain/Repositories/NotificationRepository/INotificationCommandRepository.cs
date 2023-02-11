using Domain.DomainModels.Entities.NotificationAggregate;
using Domain.Repositories.NotificationRepository.Models;

namespace Domain.Repositories.NotificationRepository
{
    public interface INotificationCommandRepository
    {
        public Task<SaveIdempotentNotificationOutput> SaveIdempotentNotificationAsync(Notification notification, CancellationToken cancellationToken);
    }
}
