using Application.SyncServices.UpdateUserView.Models;
using FluentValidation;

namespace Application.SyncServices.UpdateUserView.Validators
{
    public class UpdateUserViewInputValidator : AbstractValidator<UpdateUserViewInput>
    {
        public UpdateUserViewInputValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
