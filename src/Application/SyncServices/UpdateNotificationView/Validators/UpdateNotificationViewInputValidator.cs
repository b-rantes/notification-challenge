using Application.SyncServices.UpdateNotificationView.Models;
using FluentValidation;

namespace Application.SyncServices.UpdateNotificationView.Validators
{
    public class UpdateNotificationViewInputValidator : AbstractValidator<UpdateNotificationViewInput>
    {
        public UpdateNotificationViewInputValidator()
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
