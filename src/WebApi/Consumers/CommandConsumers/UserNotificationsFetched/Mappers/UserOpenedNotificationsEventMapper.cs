using Application.UseCases.UpsertUserControl.Models;
using Infrastructure.EventProducer.Models;

namespace WebApi.Consumers.CommandConsumers.UserNotificationsFetched.Mappers
{
    public static class UserOpenedNotificationsEventMapper
    {
        public static UpsertUserControlInput MapMessageToInput(this UserOpenedNotificationsEventMessage message) =>
            new UpsertUserControlInput
            {
                UserId = message.UserId,
                LastOpenedNotificationDate = message.LastOpenedNotificationDate,
            };
    }
}
