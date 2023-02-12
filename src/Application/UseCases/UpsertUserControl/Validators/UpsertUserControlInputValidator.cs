using Application.UseCases.UpsertUserControl.Models;
using FluentValidation;

namespace Application.UseCases.UpsertUserControl.Validators
{
    public class UpsertUserControlInputValidator : AbstractValidator<UpsertUserControlInput>
    {
        public UpsertUserControlInputValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
