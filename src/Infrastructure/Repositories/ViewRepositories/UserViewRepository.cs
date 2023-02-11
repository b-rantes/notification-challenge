using Domain.Builders;
using Domain.Entities.UserAggregate;
using Domain.Repositories.UserAggregateRepository;
using Infrastructure.Cache.Interfaces;

namespace Infrastructure.Repositories.ViewRepositories
{
    public class UserViewRepository : IUserViewRepository
    {
        private readonly ICachedUserViewRepository _cachedUserViewRepository;
        private List<User> _inMongoUsersCache = new List<User>();

        public UserViewRepository(ICachedUserViewRepository cachedUserViewRepository)
        {
            _cachedUserViewRepository = cachedUserViewRepository;
            _inMongoUsersCache.Add(GenerateFakeUser(5, true, DateTime.UtcNow.AddHours(-1)));
            _inMongoUsersCache.Add(GenerateFakeUser(6, false, DateTime.UtcNow.AddHours(-8)));
            _inMongoUsersCache.Add(GenerateFakeUser(7, true, DateTime.UtcNow.AddDays(-1)));
            _inMongoUsersCache.Add(GenerateFakeUser(8, false, DateTime.UtcNow.AddDays(-7)));
        }

        public async Task<User> GetUserById(long id, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _cachedUserViewRepository.GetUserById(id, cancellationToken);

                if (user is not null) return user;

                //Find in actual repository
                return _inMongoUsersCache.FirstOrDefault(x => x.Id == id);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private User GenerateFakeUser(long id, bool isNotificationOn, DateTime lastOpeningNotificationDate)
            => UserBuilder.CreateUser().WithId(id).WithNotificationDeliveryControl(lastOpeningNotificationDate).WithNotificationSettings(isNotificationOn);
    }
}
