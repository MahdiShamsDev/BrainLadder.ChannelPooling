using Application.Contracts;
using Application.Models;

namespace BrainLadder.ChannelPooling.Services
{
    public class MessagePublihserHostedService : IHostedService
    {
        private readonly IBus _bus;
        private Timer? _timer = null;
        public MessagePublihserHostedService(IBus bus)
        {
            _bus = bus;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                    TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        private void DoWork(object? state)
        {
            var message = new RabbitMessage
            {
                Content = "Test",
                CreatedDate = DateTime.Now,
                Id = Guid.NewGuid().ToString()
            };
            _bus.PublishAsync("test", message);
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
    }
}
