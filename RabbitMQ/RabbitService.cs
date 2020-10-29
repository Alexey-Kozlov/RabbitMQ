using System;
using RabbitMQ.Client;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace RabbitMQ
{
    public class RabbitService : IRabbitService, IDisposable
    {
        private readonly RabbitOptions _settings;
        private readonly RabbitMqProvider _rabbitProvider;

        public RabbitService(Action<RabbitOptions> settings)
        {
            _settings = new RabbitOptions();
            settings.Invoke(_settings);

            new ConnectionFactory
            {
                HostName = _settings.Credentials.HostName,
                UserName = _settings.Credentials.UserName,
                Password = _settings.Credentials.Password
            }.CreateConnection().CreateModel().QueueDeclare(queue: _settings.QueueName, durable: false, exclusive: false, autoDelete: _settings.AutoDelete, arguments: null);

            _rabbitProvider = new RabbitMqProvider(
                new MqCredentials(
                    _settings.Credentials.HostName,
                    _settings.Credentials.UserName,
                    _settings.Credentials.Password,
                    _settings.QueueName),
                _settings.QueueName);
            _rabbitProvider.Bind();
        }

        public void Publish<T>(string routingKey, string message) where T : IMessage
        {
            _rabbitProvider.Send(message, routingKey);
        }

        public void Dispose()
        {
            _rabbitProvider?.Unbind();
            _rabbitProvider?.Dispose();
        }
    }
}
