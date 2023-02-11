using Domain.Builders;
using Domain.DomainModels.Entities.UserAggregate;
using Domain.Repositories.UserRepository;
using Infrastructure.Cache.Interfaces;
using Infrastructure.Repositories.DTOs;
using MongoDB.Driver;

namespace Infrastructure.Repositories.ViewRepositories
{
    public class UserViewRepository : IUserViewRepository
    {
        private readonly ICachedUserViewRepository _cachedUserViewRepository;
        private readonly IMongoCollection<UserViewCollection> _collection;

        public UserViewRepository(ICachedUserViewRepository cachedUserViewRepository, IMongoDatabase db)
        {
            _collection = db.GetCollection<UserViewCollection>(CollectionsConstants.UserCollectionName);

            _cachedUserViewRepository = cachedUserViewRepository;
        }


        public async Task<User> GetUserById(long id, CancellationToken cancellationToken)
        {
            var cachedUserData = await _cachedUserViewRepository.GetUserById(id, cancellationToken);

            if (cachedUserData is not null) return cachedUserData;

            var user = (await _collection.FindAsync(x => x.Id == id, cancellationToken: cancellationToken)).FirstOrDefault();

            if (user is null) return null;

            return UserBuilder
                .CreateUser()
                .WithId(user.Id)
                .WithNotificationDeliveryControl(user.LastOpenedNotificationDate.ToLocalTime())
                .WithNotificationSettings(user.CanReceiveNotification);
        }
    }
}
