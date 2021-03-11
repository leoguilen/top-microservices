using System.Threading.Tasks;

namespace TOP.SyncConsumer.Domain.Interfaces.Infra
{
    public interface IConsumer
    {
        Task Consume();
    }
}
