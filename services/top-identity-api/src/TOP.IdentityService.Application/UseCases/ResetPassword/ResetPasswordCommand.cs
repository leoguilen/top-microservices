using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TOP.IdentityService.Domain.Interfaces.Domain.Services;
using TOP.IdentityService.Domain.Models;

namespace TOP.IdentityService.Application.UseCases.ResetPassword
{
    public class ResetPasswordCommand : IRequest<AuthenticationResult>
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string Token { get; set; }
    }

    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, AuthenticationResult>
    {
        private readonly IIdentityService _identityService;

        public ResetPasswordCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<AuthenticationResult> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            return await _identityService
                .ResetPasswordAsync(request.Email, request.NewPassword, request.Token);
        }
    }
}
