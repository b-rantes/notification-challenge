using Application.SyncServices.UpdateNotificationView.Models;
using Infrastructure.Cache.CachedNotificationView.Models;

namespace Application.SyncServices.UpdateNotificationView.Mapper
{
    public static class UpdateNotificationViewInputMapper
    {
        public static UpdateInCacheNotificationInput MapInputToCachedNotificationView(this UpdateNotificationViewInput input)
        {
            return new UpdateInCacheNotificationInput
            {
                NotificationId = input.NotificationId,
                LastOpenedNotificationDate = input.LastOpenedNotificationDate,
                NotificationContent = input.NotificationContent,
                NotificationCreationDate = input.NotificationCreationDate,
                UserId = input.UserOwnerId
            };
        }
    }
}
