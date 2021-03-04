using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TOP.IdentityService.Domain.Interfaces.Domain.Services;
using TOP.IdentityService.Domain.Models;

namespace TOP.IdentityService.Application.UseCases.Register
{
    public class RegisterCommand : IRequest<AuthenticationResult>
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthenticationResult>
    {
        private readonly IIdentityService _identityService;
        private readonly IMediator _mediator;

        public RegisterCommandHandler(
            IIdentityService identityService,
            IMediator mediator)
        {
            _identityService = identityService;
            _mediator = mediator;
        }

        public async Task<AuthenticationResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var authenticationResult = await _identityService
                .RegisterAsync(request.UserName, request.Email, request.Password);

            if (authenticationResult.Item1.Success)
            {
                await _mediator.Publish(new RegisteredEvent 
                { 
                    User = authenticationResult.Item2, 
                    ConfirmationEmailToken = authenticationResult.Item3 
                }, 
                cancellationToken);
            }

            return authenticationResult.Item1;
        }
    }
}
