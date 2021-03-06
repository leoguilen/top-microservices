using System.Threading.Tasks;
using TOP.NotificationConsumer.Domain.Models.Message;

namespace TOP.NotificationConsumer.Domain.Interfaces.Domain
{
    public interface INotificationSender
    {
        INotificationSender SetAction(string actionType);
        Task Handle(UserMessage args);
    }
}
