using Application.SyncServices.UpdateNotificationView.Models;
using Infrastructure.EventProducer.Models;

namespace WebApi.Consumers.SyncConsumers.UserNotificationCreated.Mappers
{
    public static class CreateUserNotificationCommandMapper
    {
        public static UpdateNotificationViewInput MapMessageToInput(this UserNotificationCreatedEventMessage message) =>
            new UpdateNotificationViewInput
            {
                LastOpenedNotificationDate = message.LastOpenedNotificationDate,
                NotificationContent = message.NotificationContent,
                NotificationCreationDate = message.NotificationCreationDate,
                NotificationId = message.NotificationId,
                UserOwnerId = message.UserOwnerId,
            };
    }
}
