using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;
using RabbitMq.Models;
using Application.Contracts;
using RabbitMq.Services.Implementations;

namespace RabbitMq.Extensions
{
    public static class RabbitServiceExtensions
    {
        public static void AddRabbitMq(this IServiceCollection services)
        {

            services.AddSingleton<IPooledObjectPolicy<IModel>, ModelPooledObjectPolicy>();

            services.AddSingleton<IBus, RabbitMqBus>();
        }
    }
}
