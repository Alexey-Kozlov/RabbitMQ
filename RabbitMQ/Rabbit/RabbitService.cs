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

            foreach (string queue in _settings.Queues)
            {
                new ConnectionFactory
                {
                    HostName = _settings.Credentials.HostName,
                    UserName = _settings.Credentials.UserName,
                    Password = _settings.Credentials.Password
                }.CreateConnection().CreateModel().QueueDeclare(queue, false, false, false);
            }
            _rabbitProvider = new RabbitMqProvider(
                new MqCredentials(
                    _settings.Credentials.HostName,
                    _settings.Credentials.UserName,
                    _settings.Credentials.Password,
                    _settings.Queues),
                _settings.Queues,
                _settings.ExchangeName);
            _rabbitProvider.Bind();
        }

        public void Publish<T>(string message)
        {
            _rabbitProvider.Send(message);
        }

        public void Dispose()
        {
            _rabbitProvider?.Unbind();
            _rabbitProvider?.Dispose();
        }
    }
}
