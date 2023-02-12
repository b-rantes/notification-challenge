using Domain.Repositories.UserRepository;
using Domain.Repositories.UserRepository.Models;
using Infrastructure.Cache.Interfaces;
using Infrastructure.Repositories.DTOs;
using Infrastructure.Repositories.Mappers;
using MongoDB.Driver;

namespace Infrastructure.Repositories.ViewRepositories
{
    public class UserViewRepository : IUserViewRepository
    {
        private readonly ICachedUserViewRepository _cachedUserViewRepository;
        private readonly IMongoCollection<UserCommandCollection> _collection;

        public UserViewRepository(ICachedUserViewRepository cachedUserViewRepository, IMongoDatabase db)
        {
            _collection = db.GetCollection<UserCommandCollection>(CollectionsConstants.UserCollectionName);

            _cachedUserViewRepository = cachedUserViewRepository;
        }

        public async Task<UserControlView> GetUserById(long id, CancellationToken cancellationToken)
        {
            var cachedUserData = await _cachedUserViewRepository.GetUserById(id, cancellationToken);

            if (cachedUserData is not null) return cachedUserData;

            var user = (await _collection.FindAsync(x => x.Id == id, cancellationToken: cancellationToken)).FirstOrDefault();

            if (user is null) return null;

            return user.MapUserCollectionToUserControlView();
        }
    }
}
