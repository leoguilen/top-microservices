using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TOP.IdentityService.Domain.Constants;
using TOP.IdentityService.Domain.Interfaces.Infra;
using TOP.IdentityService.Domain.Models;

namespace TOP.IdentityService.Application.UseCases.Register
{
    public class RegisteredEvent : INotification
    {
        public ApplicationUser User { get; set; }
        public string Token { get; set; }
    }

    public class RegisteredEventHandler : INotificationHandler<RegisteredEvent>
    {
        private readonly IProducer _producer;

        public RegisteredEventHandler(IProducer producer)
        {
            _producer = producer;
        }

        public async Task Handle(RegisteredEvent notification, CancellationToken cancellationToken)
        {
            var eventData = new
            {
                notification.User.Id,
                notification.User.Email,
                notification.User.UserName,
                notification.User.PhoneNumber,
                notification.Token
            };

            await _producer.Publish(eventData, RoutingKeys.RegisterUser);
        }
    }
}
