using Domain.DomainModels.Entities.UserAggregate;
using Domain.Repositories.NotificationRepository.Models;
using Domain.Services.Models;

namespace Domain.Services.Mappers
{
    public static class NotificationsViewByUserMapper
    {
        public static UserNotificationsOutput MapNotificationsByUserToOutput(this NotificationsViewByUserOutput notificationsByUser, User user)
        {
            var notifications = new List<NotificationItem>();

            var result = new UserNotificationsOutput { Notifications = notifications };

            if (notificationsByUser != null)
                result.Notifications = notificationsByUser.Notifications.Select(x => new NotificationItem
                {
                    NotificationContent = x.NotificationContent,
                    NotificationCreationDate = x.NotificationCreationDate,
                    IsNewNotification = x.NotificationCreationDate >= user.LastOpenedNotificationDate
                }).ToList();

            return result;
        }
    }
}
