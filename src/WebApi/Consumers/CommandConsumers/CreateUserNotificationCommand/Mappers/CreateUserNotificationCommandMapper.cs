using Application.UseCases.CreateUserNotification.Models;
using WebApi.Consumers.CommandConsumers.CreateUserNotificationCommand.Model;

namespace WebApi.Consumers.CommandConsumers.CreateUserNotificationCommand.Mappers
{
    public static class CreateUserNotificationCommandMapper
    {
        public static CreateUserNotificationInput MapMessageToInput(this CreateUserNotificationCommandMessage message) =>
            new CreateUserNotificationInput
            {
                NotificationContent = message.NotificationContent,
                NotificationGuid = message.NotificationId,
                UserId = message.UserId,
            };
    }
}
