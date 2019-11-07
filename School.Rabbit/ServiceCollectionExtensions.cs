using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;
using School.Event;
using System;

namespace School.Rabbit
{
    public class RabbitModelPooledObjectPolicy : IPooledObjectPolicy<IModel>
    {
        private readonly Uri _uri;

        private readonly IConnection _connection;

        public RabbitModelPooledObjectPolicy(Uri uri)
        {
            _uri = uri;
            _connection = GetConnection();
        }

        private IConnection GetConnection()
        {
            var connectionFactory = new ConnectionFactory();
            connectionFactory.Uri = _uri;
            connectionFactory.HandshakeContinuationTimeout = TimeSpan.FromMinutes(1);
            connectionFactory.RequestedConnectionTimeout = 60000;
            connectionFactory.SocketReadTimeout = 60000;
            connectionFactory.SocketWriteTimeout = 60000;
            return connectionFactory.CreateConnection();
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

    public static class ServiceCollectionExtensions
    {
        public static void AddRabbitPublisher(this IServiceCollection serviceCollection, Uri uri, string exchangeName)
        {
            serviceCollection.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
            serviceCollection.AddSingleton(s =>
            {
                var provider = s.GetRequiredService<ObjectPoolProvider>();
                return provider.Create(new RabbitModelPooledObjectPolicy(uri));
            });
            serviceCollection.AddScoped<IEventPublisher>(s =>
            {
                var objectPool = s.GetRequiredService<ObjectPool<IModel>>();
                return new EventPublisher(objectPool, exchangeName);
            });
           
        }
    }
}
