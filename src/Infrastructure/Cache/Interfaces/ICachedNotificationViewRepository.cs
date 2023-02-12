using Domain.Repositories.NotificationRepository;
using Infrastructure.Cache.CachedNotificationView.Models;

namespace Infrastructure.Cache.Interfaces
{
    public interface ICachedNotificationViewRepository : INotificationViewRepository
    {
        public Task UpdateInCacheNotificationViewByUser(UpdateInCacheNotificationInput input, CancellationToken cancellationToken);
    }
}
