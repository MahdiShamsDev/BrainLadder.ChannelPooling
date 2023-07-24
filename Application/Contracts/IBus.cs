using Application.Models;

namespace Application.Contracts
{
    public interface IBus
    {
        Task PublishAsync(string topic, RabbitMessage message, CancellationToken cancellationToken = default(CancellationToken));
    }
}
