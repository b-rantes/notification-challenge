using Application.SyncServices.UpdateUserView.Models;
using Infrastructure.Cache.CachedUserView.Models;

namespace Application.SyncServices.UpdateUserView.Mapper
{
    public static class UpdateUserViewInputMapper
    {
        public static UpdateCachedUserControlViewInput MapInputToCachedUserView(this UpdateUserViewInput input)
        {
            return new UpdateCachedUserControlViewInput
            {
                Id = input.UserId,
                LastOpenedNotificationDate= input.LastOpenedNotificationDate,
                CanReceiveNotification = input.CanReceiveNotification
            };
        }
    }
}
