using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TOP.IdentityService.Domain.Constants;
using TOP.IdentityService.Domain.Interfaces.Infra;

namespace TOP.IdentityService.Application.UseCases.PasswordRecovery
{
    public class PasswordRecoveryRequestEvent : INotification
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }

    public class PasswordRecoveryRequestEventHandler : INotificationHandler<PasswordRecoveryRequestEvent>
    {
        private readonly IProducer _producer;

        public PasswordRecoveryRequestEventHandler(IProducer producer)
        {
            _producer = producer;
        }

        public async Task Handle(PasswordRecoveryRequestEvent notification, CancellationToken cancellationToken)
        {
            var eventData = new
            {
                notification.Email,
                notification.Token
            };

            await _producer.Publish(eventData, RoutingKeys.ResetUserPassword);
        }
    }
}
