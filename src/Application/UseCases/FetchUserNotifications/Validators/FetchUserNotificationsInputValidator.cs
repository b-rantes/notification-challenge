using Application.UseCases.FetchUserNotifications.Models;
using FluentValidation;

namespace Application.UseCases.FetchUserNotifications.Validators
{
    public class FetchUserNotificationsInputValidator : AbstractValidator<FetchUserNotificationsInput>
    {
        public FetchUserNotificationsInputValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
