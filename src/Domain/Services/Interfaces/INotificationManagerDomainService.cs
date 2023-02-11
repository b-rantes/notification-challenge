using Domain.DomainModels.Entities.NotificationAggregate;
using Domain.DomainModels.Entities.UserAggregate;

namespace Domain.Services.Interfaces
{
    public interface INotificationManagerDomainService
    {
        public Task CreateUserNotificationAsync(User user, Notification notification, CancellationToken cancellationToken);
    }
}
