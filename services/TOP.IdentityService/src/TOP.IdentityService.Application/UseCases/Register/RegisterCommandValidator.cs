using FluentValidation;

namespace TOP.IdentityService.Application.UseCases.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(prop => prop.Email)
                .EmailAddress();

            RuleFor(prop => prop.UserName)
                .NotNull()
                .NotEmpty()
                .MinimumLength(5);

            RuleFor(prop => prop.Password)
                .NotNull()
                .NotEmpty();
        }
    }
}
