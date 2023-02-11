using Domain.DomainModels.Entities.NotificationAggregate;
using Domain.Repositories.NotificationRepository;
using Domain.Repositories.NotificationRepository.Models;
using Infrastructure.Repositories.DTOs;
using Infrastructure.Repositories.Mappers;
using MongoDB.Driver;

namespace Infrastructure.Repositories.CommandRepositories
{
    public class NotificationCommandRepository : INotificationCommandRepository
    {
        private readonly IMongoCollection<NotificationCommandCollection> _collection;

        public NotificationCommandRepository(IMongoDatabase db)
        {
            _collection = db.GetCollection<NotificationCommandCollection>(CollectionsConstants.NotificationCollectionName);
        }

        public async Task<SaveIdempotentNotificationOutput> SaveIdempotentNotificationAsync(Notification notification, CancellationToken cancellationToken)
        {
            var document = notification.MapNotificationToNotificationCommandCollection();
            try
            {
                await _collection.InsertOneAsync(document);

                return new SaveIdempotentNotificationOutput();
            }
            catch (Exception ex)
            {
                return new SaveIdempotentNotificationOutput(ex.Message);
            }
        }
    }
}
