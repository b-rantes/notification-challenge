using Domain.DomainModels.Entities.NotificationAggregate;
using Domain.DomainModels.Entities.UserAggregate;
using Domain.Services.Models;

namespace Domain.Services.Interfaces
{
    public interface INotificationManagerDomainService
    {
        public Task CreateUserNotificationAsync(User user, Notification notification, CancellationToken cancellationToken);
        public Task<UserNotificationsOutput> FetchUserNotificationsAsync(long userId, CancellationToken cancellationToken);
    }
}
