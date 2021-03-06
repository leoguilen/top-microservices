using System.Threading.Tasks;

namespace TOP.NotificationConsumer.Domain.Interfaces.Infra
{
    public interface IConsumer
    {
        Task Consume();
    }
}
