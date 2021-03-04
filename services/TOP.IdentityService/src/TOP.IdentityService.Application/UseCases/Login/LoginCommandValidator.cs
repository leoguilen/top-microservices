using FluentValidation;

namespace TOP.IdentityService.Application.UseCases.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(prop => prop.Email).EmailAddress();
            RuleFor(prop => prop.Password).NotNull().NotEmpty();
        }
    }
}
