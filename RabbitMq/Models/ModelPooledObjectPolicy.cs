using RabbitMQ.Client;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;

namespace RabbitMq.Models
{
    public class ModelPooledObjectPolicy : IPooledObjectPolicy<IModel>
    {

        private readonly RabbitOptions _options;

        private readonly IConnection _connection;

        public ModelPooledObjectPolicy(IOptions<RabbitOptions> optionsAccs)
        {
            _options = optionsAccs.Value;
            _connection = GetConnection();
        }

        private IConnection GetConnection()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _options.HostName,
                UserName = _options.UserName,
                Password = _options.Password,
                VirtualHost = "/"
            };

            return factory.CreateConnection();
        }

        public IModel Create()
        {
            return _connection.CreateModel();
        }

        public bool Return(IModel obj)
        {
            if (obj.IsOpen)
            {
                return true;
            }
            else
            {
                obj?.Dispose();
                return false;
            }
        }
    }
}
