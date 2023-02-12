using Domain.Repositories.NotificationRepository.Models;
using Infrastructure.Cache.CachedNotificationView.Mapper;
using Infrastructure.Cache.CachedNotificationView.Models;
using Infrastructure.Cache.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Infrastructure.Cache.CachedNotificationView
{
    public class CachedNotificationViewRepository : ICachedNotificationViewRepository
    {
        private readonly IDistributedCache _cache;
        private const string NotificationSettingsKey = "user_notifications_{0}";
        private static JsonSerializerOptions _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
        };

        public CachedNotificationViewRepository(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task GetNotificationByGuid(Guid notificationId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<NotificationsViewByUserOutput> GetNotificationsByUserId(long userId, CancellationToken cancellationToken)
        {
            try
            {
                var key = string.Format(NotificationSettingsKey, userId);

                var data = await _cache.GetStringAsync(key, cancellationToken);

                if (data is null)
                    return null;

                var cachedView = JsonSerializer.Deserialize<NotificationsViewByUserOutput>(data!, _options);

                return cachedView!;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task UpdateInCacheNotificationViewByUser(UpdateInCacheNotificationInput input, CancellationToken cancellationToken)
        {
            try
            {
                var key = string.Format(NotificationSettingsKey, input.UserId);

                var data = await _cache.GetStringAsync(key, cancellationToken);

                if (data is null)
                {
                    var firstCachedNotificationView = input.MapInputToCachedNotificationViewModel();

                    await _cache.SetStringAsync(key, firstCachedNotificationView, cancellationToken);

                    return;
                }

                var oldCachedNotificationViewModel = JsonSerializer.Deserialize<NotificationsViewByUserOutput>(data!, _options);

                var newCachedNotificationView = oldCachedNotificationViewModel!.MapInputToCachedNotificationViewModel(input);

                await _cache.SetStringAsync(key, newCachedNotificationView, cancellationToken);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
