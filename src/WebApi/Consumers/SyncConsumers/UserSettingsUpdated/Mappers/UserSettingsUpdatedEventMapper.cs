using Application.SyncServices.UpdateUserView.Models;
using Infrastructure.EventProducer.Models;

namespace WebApi.Consumers.SyncConsumers.UserSettingsUpdated.Mappers
{
    public static class UserSettingsUpdatedEventMapper
    {
        public static UpdateUserViewInput MapMessageToInput(this UserSettingsUpdatedEventMessage message) =>
            new UpdateUserViewInput
            {
                UserId = message.UserId,
                LastOpenedNotificationDate = message.LastOpenedNotificationDate,
                CanReceiveNotification = message.CanReceiveNotification
            };
    }
}
