using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TOP.IdentityService.Domain.Interfaces.Domain.Services;
using TOP.IdentityService.Domain.Models;

namespace TOP.IdentityService.Application.UseCases.Login
{
    public class LoginCommand : IRequest<AuthenticationResult>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthenticationResult>
    {
        private readonly IIdentityService _identityService;

        public LoginCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<AuthenticationResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            return await _identityService
                .LoginAsync(request.Email, request.Password);
        }
    }
}
