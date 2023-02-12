using Application.SyncServices.UpdateNotificationView.Models;
using FluentValidation;

namespace Application.SyncServices.UpdateUserView.Validators
{
    public class UpdateUserViewInputValidator : AbstractValidator<UpdateNotificationViewInput>
    {
        public UpdateUserViewInputValidator()
        {
            RuleFor(x => x.UserOwnerId)
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(x => x.NotificationId)
                .NotEmpty();

            RuleFor(x => x.NotificationCreationDate).NotEmpty();
        }
    }
}
