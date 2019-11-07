using School.Event;
using RabbitMQ.Client;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Extensions.ObjectPool;

namespace School.Rabbit
{
    public class EventPublisher : IEventPublisher
    {
        private ObjectPool<IModel> _objectPool;
        private string _exchangeName;
        public EventPublisher(ObjectPool<IModel> objectPool, string exchangeName)
        {
            _objectPool = objectPool;
            _exchangeName = exchangeName;
        }

        public void Publish(IEvent domainEvent)
        {
            var model = _objectPool.Get();
            try
            {
                var properties = model.CreateBasicProperties();
                properties.ContentType = "text/plain";
                properties.DeliveryMode = 2;
                model.BasicPublish(_exchangeName,
                    string.Empty,
                    properties,
                    GetObjectBytes(domainEvent));
            }
            finally
            {
                _objectPool.Return(model);
            }
        }

        private byte[] GetObjectBytes(IEvent domainEvent)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(domainEvent));
        }
    }
}
