using System;
using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Threading.Tasks;

namespace RabbitReceiverConsole
{
    public interface IQueueProvider : IProvider
    {
        void Send(string message);
        void Send(string message, string routingKey);
        void Subscribe(Action<string> callback);
    }

    public interface IProvider
    {
        bool IsBinded { get; }
        void Bind();
        void Unbind();
    }

    public interface IMqLogger
    {
        void Error(Exception excepion, string message);
        void Info(string message);
        void Trace(string message);
    }

    public interface IRabbitLogger : IMqLogger
    {

    }

}
