using Domain.Repositories.UserRepository.Models;
using Infrastructure.Cache.CachedUserView.Models;
using System.Text.Json;

namespace Infrastructure.Cache.CachedUserView.Mapper
{
    public static class CachedUserViewMapper
    {
        private static JsonSerializerOptions _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
        };

        public static string MapInputToUserControlView(this UpdateCachedUserControlViewInput input) =>
            JsonSerializer.Serialize(new UserControlView
            {
                Id = input.Id,
                CanReceiveNotification = input.CanReceiveNotification,
                LastOpenedNotificationDate = input.LastOpenedNotificationDate,
                LastUpdate = DateTime.UtcNow
            }, _options);

        public static string MapInputToUserControlView(this UserControlView oldView, UpdateCachedUserControlViewInput input) =>
            JsonSerializer.Serialize(new UserControlView
            {
                Id = input.Id,
                CanReceiveNotification = input.CanReceiveNotification ?? oldView.CanReceiveNotification,
                LastOpenedNotificationDate = input.LastOpenedNotificationDate ?? oldView.LastOpenedNotificationDate,
                LastUpdate = DateTime.UtcNow
            }, _options);
    }
}
