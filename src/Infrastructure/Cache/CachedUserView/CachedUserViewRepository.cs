using Domain.Builders;
using Domain.DomainModels.Entities.UserAggregate;
using Infrastructure.Cache.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Infrastructure.Cache.CachedUserView
{
    internal class CachedUserViewRepository : ICachedUserViewRepository
    {
        private readonly IDistributedCache _cache;
        private const string UserSettingsKey = "user_settings_{0}";
        private static JsonSerializerOptions _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
        };

        public CachedUserViewRepository(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<User> GetUserById(long id, CancellationToken cancellationToken)
        {
            try
            {
                var key = string.Format(UserSettingsKey, id);

                var data = await _cache.GetStringAsync(key, cancellationToken);

                var userViewModel = JsonSerializer.Deserialize<CachedUserViewModel>(data, _options);

                if (userViewModel is null) return null;

                return UserBuilder.CreateUser()
                    .WithId(userViewModel.Id)
                    .WithNotificationDeliveryControl(userViewModel.LastOpenedNotificationDate)
                    .WithNotificationSettings(userViewModel.CanReceiveNotification);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
