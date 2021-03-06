using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TOP.IdentityService.Domain.Interfaces.Domain.Services;
using TOP.IdentityService.Domain.Models;

namespace TOP.IdentityService.Application.UseCases.ConfirmEmail
{
    public class ConfirmEmailCommand : IRequest<AuthenticationResult>
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }

    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, AuthenticationResult>
    {
        private readonly IIdentityService _identityService;

        public ConfirmEmailCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<AuthenticationResult> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            return await _identityService
                .ConfirmEmailAsync(request.Email, request.Token);
        }
    }
}
