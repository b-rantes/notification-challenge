using Application.Shared.Errors;
using Domain.Services.Models;

namespace Application.UseCases.FetchUserNotifications.Models
{
    public class FetchUserNotificationsOutput : BaseError
    {
        public List<NotificationItem> Notifications { get; set; } = new List<NotificationItem>();
        public int NewNotificationsCount { get; set; }

        public FetchUserNotificationsOutput() : base()
        {

        }

        public FetchUserNotificationsOutput(Error error) : base(error)
        {

        }

        public FetchUserNotificationsOutput(List<NotificationItem> notifications, int newNotificationsCount)
        {
            Notifications = notifications;
            NewNotificationsCount = newNotificationsCount;
        }

        public static FetchUserNotificationsOutput Success() => new FetchUserNotificationsOutput();
        public static FetchUserNotificationsOutput Success(List<NotificationItem> notifications, int newNotificationsCount) => new(notifications, newNotificationsCount);
        public static FetchUserNotificationsOutput Fail(Error error) => new FetchUserNotificationsOutput(error);
    }
}
