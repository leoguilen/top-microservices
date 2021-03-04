using System.Threading.Tasks;

namespace TOP.IdentityService.Domain.Interfaces.Infra
{
    public interface IProducer
    {
        Task Publish<T>(T message, string routingKey = "");
    }
}
