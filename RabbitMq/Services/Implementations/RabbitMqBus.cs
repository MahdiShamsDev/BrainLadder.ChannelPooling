using Application.Contracts;
using Application.Models;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMq.Models;
using RabbitMQ.Client;
using System.Text;

namespace RabbitMq.Services.Implementations
{
    public class RabbitMqBus : IBus
    {
        private readonly IConnection _subConnection;

        private readonly ObjectPool<IModel> _pubChannelPool;

        private readonly RabbitOptions _options;

        private readonly string _busId;


        public RabbitMqBus(IPooledObjectPolicy<IModel> _objectPolicy
            , IOptions<RabbitOptions> rabbitMqOptions)
        {
            _options = rabbitMqOptions.Value;
            var factory = new ConnectionFactory
            {
                HostName = _options.HostName,
                UserName = _options.UserName,
                Port = _options.Port,
                Password = _options.Password
            };

            _subConnection = factory.CreateConnection();

            var provider = new DefaultObjectPoolProvider();

            _pubChannelPool = provider.Create(_objectPolicy);

            _busId = Guid.NewGuid().ToString("N");

        }

        public Task PublishAsync(string topic, RabbitMessage message, CancellationToken cancellationToken = default)
        {
            var channel = _pubChannelPool.Get();
            try
            {
                var json = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(json);

                channel.ExchangeDeclare(_options.TopicExchangeName, ExchangeType.Topic, true, false, null);
                channel.BasicPublish(_options.TopicExchangeName, topic, false, null, body);
            }
            catch (Exception ex)
            {
                //log the exception
            }
            finally
            {
                _pubChannelPool.Return(channel);
            }
            return Task.CompletedTask;
        }
    }
}
