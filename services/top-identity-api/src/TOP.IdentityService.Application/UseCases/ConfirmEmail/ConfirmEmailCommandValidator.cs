using FluentValidation;

namespace TOP.IdentityService.Application.UseCases.ConfirmEmail
{
    public class ConfirmEmailCommandValidator : AbstractValidator<ConfirmEmailCommand>
    {
        public ConfirmEmailCommandValidator()
        {
            RuleFor(prop => prop.Email).EmailAddress();
            RuleFor(prop => prop.Token).NotNull().NotEmpty();
        }
    }
}
