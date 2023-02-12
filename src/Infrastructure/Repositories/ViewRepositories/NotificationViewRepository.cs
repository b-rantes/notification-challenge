using Domain.Repositories.NotificationRepository;
using Domain.Repositories.NotificationRepository.Models;
using Infrastructure.Cache.Interfaces;
using Infrastructure.Repositories.DTOs;
using Infrastructure.Repositories.Mappers;
using MongoDB.Driver;

namespace Infrastructure.Repositories.ViewRepositories
{
    public class NotificationViewRepository : INotificationViewRepository
    {
        private readonly ICachedNotificationViewRepository _cachedNotificationViewRepository;
        private readonly IMongoCollection<NotificationCommandCollection> _collection;

        public NotificationViewRepository(ICachedNotificationViewRepository cachedNotificationViewRepository, IMongoDatabase db)
        {
            _cachedNotificationViewRepository = cachedNotificationViewRepository;
            _collection = db.GetCollection<NotificationCommandCollection>(CollectionsConstants.NotificationCollectionName);
        }

        public async Task<NotificationsViewByUserOutput> GetNotificationsByUserId(long userId, CancellationToken cancellationToken)
        {
            try
            {   
                var cachedNotifications = await _cachedNotificationViewRepository.GetNotificationsByUserId(userId, cancellationToken);

                if (cachedNotifications is not null) return cachedNotifications;

                var findOptions = new FindOptions<NotificationCommandCollection, NotificationCommandCollection>();

                findOptions.Sort.Ascending(x => x.NotificationCreationDate);

                var notifications = (await _collection
                    .FindAsync(x => x.UserOwnerId == userId, findOptions, cancellationToken: cancellationToken)).ToList();

                if (notifications is null) return null;

                return notifications.MapNotificationCollectionToViewOutput();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
