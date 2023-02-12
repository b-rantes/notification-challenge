using Domain.Repositories.NotificationRepository.Models;
using Infrastructure.Cache.CachedNotificationView.Models;
using System.Text.Json;

namespace Infrastructure.Cache.CachedNotificationView.Mapper
{
    public static class CachedNotificationViewMappers
    {
        private static JsonSerializerOptions _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
        };

        public static string MapInputToCachedNotificationViewModel(this UpdateInCacheNotificationInput input)
        {
            return JsonSerializer.Serialize(new NotificationsViewByUserOutput
            {
                LastOpenedNotificationDate = input.LastOpenedNotificationDate ?? default,
                UserId = input.UserId,
                LastUpdate = DateTime.UtcNow,
                Notifications = new List<NotificationViewItem> { new()
                {
                    NotificationContent = input.NotificationContent,
                    NotificationCreationDate= input.NotificationCreationDate,
                    NotificationId = input.NotificationId.ToString(),
                    InsertedAt = DateTime.UtcNow,
                } }
            }, _options);
        }

        public static string MapInputToCachedNotificationViewModel(
            this NotificationsViewByUserOutput oldView, UpdateInCacheNotificationInput input)
        {
            oldView.Notifications.Insert(0, input.CreateNewNotificationItem());
            return JsonSerializer.Serialize(new NotificationsViewByUserOutput
            {
                LastOpenedNotificationDate = input.LastOpenedNotificationDate ?? default,
                UserId = input.UserId,
                Notifications = oldView.Notifications,
                LastUpdate = DateTime.UtcNow
            }, _options);
        }

        private static NotificationViewItem CreateNewNotificationItem(
            this UpdateInCacheNotificationInput input) =>
            new NotificationViewItem
            {
                NotificationId = input.NotificationId.ToString(),
                NotificationCreationDate = input.NotificationCreationDate,
                NotificationContent = input.NotificationContent,
                InsertedAt = DateTime.UtcNow
            };
    }
}
