using Application.UseCases.CreateUserNotification.Models;
using FluentValidation;

namespace Application.UseCases.CreateUserNotification.Validators
{
    public class CreateUserNotificationInputValidator : AbstractValidator<CreateUserNotificationInput>
    {
        public CreateUserNotificationInputValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(x => x.NotificationGuid)
                .NotEmpty();
        }
    }
}
