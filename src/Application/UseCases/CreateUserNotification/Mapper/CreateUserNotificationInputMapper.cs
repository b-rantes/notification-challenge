using Application.UseCases.CreateUserNotification.Models;
using Domain.DomainModels.Entities.NotificationAggregate;

namespace Application.UseCases.CreateUserNotification.Mapper
{
    public static class CreateUserNotificationInputMapper
    {
        public static Notification MapInputToNotification(this CreateUserNotificationInput input)
        {
            var notification = new Notification(input.NotificationGuid, input.UserId);
            notification.SetNotificationContent(input.NotificationContent);

            return notification;
        }
    }
}
