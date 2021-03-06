using FluentValidation;

namespace TOP.IdentityService.Application.UseCases.ResetPassword
{
    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator()
        {
            RuleFor(prop => prop.Email).EmailAddress();

            RuleFor(prop => prop.NewPassword)
                .NotNull()
                .NotEmpty();

            RuleFor(prop => prop.Token)
                .NotNull()
                .NotEmpty();
        }
    }
}
