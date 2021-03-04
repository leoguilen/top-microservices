using FluentValidation;

namespace TOP.IdentityService.Application.UseCases.PasswordRecovery
{
    public class PasswordRecoveryCommandValidator : AbstractValidator<PasswordRecoveryCommand>
    {
        public PasswordRecoveryCommandValidator()
        {
            RuleFor(prop => prop.Email).EmailAddress();
        }
    }
}
