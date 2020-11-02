using System;
using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Threading.Tasks;

namespace RabbitMQ
{
    public interface IQueueProvider : IProvider
    {
        void Send(string message);
        void Subscribe(Action<string> callback, string queueName);
    }

    public interface IProvider
    {
        bool IsBinded { get; }
        void Bind();
        void Unbind();
    }

}
