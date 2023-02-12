using Application.Shared.Errors;

namespace Application.UseCases.CreateUserNotification.Models
{
    public class CreateUserNotificationOutput : BaseError
    {
        public CreateUserNotificationOutput() : base()
        {

        }

        public CreateUserNotificationOutput(Error error) : base(error)
        {

        }

        public static CreateUserNotificationOutput Success() => new CreateUserNotificationOutput();
        public static CreateUserNotificationOutput Fail(Error error) => new CreateUserNotificationOutput(error);
    }
}
