using Domain.Builders;
using Domain.Entities.UserAggregate;
using Infrastructure.Cache.Interfaces;

namespace Infrastructure.Cache
{
    internal class CachedUserViewRepository : ICachedUserViewRepository
    {
        private List<User> _inMemoryUsersCache = new List<User>();
        public CachedUserViewRepository()
        {
            _inMemoryUsersCache.Add(GenerateFakeUser(1, true, DateTime.UtcNow.AddHours(-1)));
            _inMemoryUsersCache.Add(GenerateFakeUser(2, false, DateTime.UtcNow.AddHours(-8)));
            _inMemoryUsersCache.Add(GenerateFakeUser(3, true, DateTime.UtcNow.AddDays(-1)));
            _inMemoryUsersCache.Add(GenerateFakeUser(4, false, DateTime.UtcNow.AddDays(-7)));
        }

        public async Task<User> GetUserById(long id, CancellationToken cancellationToken)
        {
            try
            {
                return _inMemoryUsersCache.FirstOrDefault(x => x.Id == id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private User GenerateFakeUser(long id, bool isNotificationOn, DateTime lastOpeningNotificationDate)
            => UserBuilder.CreateUser().WithId(id).WithNotificationDeliveryControl(lastOpeningNotificationDate).WithNotificationSettings(isNotificationOn);
    }
}
