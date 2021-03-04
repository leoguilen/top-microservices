using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TOP.IdentityService.Domain.Interfaces.Domain.Services;
using TOP.IdentityService.Domain.Models;

namespace TOP.IdentityService.Application.UseCases.PasswordRecovery
{
    public class PasswordRecoveryCommand : IRequest<AuthenticationResult>
    {
        public string Email { get; set; }
    }

    public class PasswordRecoveryCommandHandler : IRequestHandler<PasswordRecoveryCommand, AuthenticationResult>
    {
        private readonly IIdentityService _identityService;
        private readonly IMediator _mediator;

        public PasswordRecoveryCommandHandler(
            IIdentityService identityService,
            IMediator mediator)
        {
            _identityService = identityService;
            _mediator = mediator;
        }

        public async Task<AuthenticationResult> Handle(PasswordRecoveryCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService
                .PasswordRecoveryAsync(request.Email);

            if (result.Item1.Success)
            {
                await _mediator.Publish(new PasswordRecoveryRequestEvent 
                {
                    Email = request.Email,
                    ConfirmationResetPasswordToken = result.Item2
                }, 
                cancellationToken);
            }

            return result.Item1;
        }
    }
}
