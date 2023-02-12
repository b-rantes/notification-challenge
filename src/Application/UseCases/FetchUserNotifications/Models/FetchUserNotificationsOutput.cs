using Application.Shared.Errors;

namespace Application.UseCases.FetchUserNotifications.Models
{
    public class FetchUserNotificationsOutput : BaseError
    {
        public FetchUserNotificationsOutput() : base()
        {

        }

        public FetchUserNotificationsOutput(Error error) : base(error)
        {

        }

        public static FetchUserNotificationsOutput Success() => new FetchUserNotificationsOutput();
        public static FetchUserNotificationsOutput Fail(Error error) => new FetchUserNotificationsOutput(error);
    }
}
