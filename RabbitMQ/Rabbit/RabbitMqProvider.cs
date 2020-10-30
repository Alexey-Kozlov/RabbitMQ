using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Logging;
using System.Text;
using Serilog;

namespace RabbitMQ
{
    public class RabbitMqProvider : IQueueProvider, IDisposable
    {
        private readonly string _QueueName;
        private IConnection _connection;
        private IModel _channel;
        private string _consumer;
        private readonly ConnectionFactory _connectionFactory;

        public RabbitMqProvider(MqCredentials credentials, string QueueName)
        {
            _QueueName = QueueName;
            _connectionFactory = new ConnectionFactory
            {
                HostName = credentials.HostName,
                UserName = credentials.UserName,
                Password = credentials.Password,                
                Port = AmqpTcpEndpoint.UseDefaultPort
            };
        }

        public bool IsBinded { get; private set; }

        public int MaxThreads { get; set; }

        public bool ConnectionIsOpen => _connection?.IsOpen ?? false;

        public void Subscribe(Action<string> callback)
        {
            if (!IsBinded)
            {
                throw new InvalidOperationException("Связь с очередью отутствует");
            }
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Log.Information($"Сообщение получено из очереди {_QueueName}: {message}");
                    callback(message);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, $"Ошибка получения сообщения из очереди {_QueueName}");
                }
                finally
                {
                    _channel.BasicAck(ea.DeliveryTag, false);
                }
            };
            _consumer = _channel.BasicConsume(queue: _QueueName, consumer: consumer);
            Log.Information($"Подписка на очередь {_QueueName} создана");
        }

        public void Send(string message)
        {
            Send(message, _QueueName);
        }

        public void Send(string message, string routingKey)
        {
            if (!IsBinded)
            {
                throw new InvalidOperationException("Связь с очередью отутствует");
            }

            Log.Information($"Сообщение отправляется: {message} в очередь {_QueueName}, routing key {routingKey}");
            Serilog.Log.Information("в очередь");
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish("", _QueueName, null, body);
            Log.Information($"Сообщение отправлено: {message} в очередь {_QueueName}, routing key {routingKey}");
        }

        public void Bind()
        {
            if (IsBinded)
            {
                throw new InvalidOperationException("Связь с очередью уже установлена");
            }
            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            if (MaxThreads != 0)
                _channel.BasicQos(0, (ushort)MaxThreads, false);
            IsBinded = true;
        }

        public void Unbind()
        {
            if (!IsBinded)
            {
                throw new InvalidOperationException("Связь с очередью отутствует");
            }

            if (!string.IsNullOrEmpty(_consumer))
            {
                _channel.BasicCancel(_consumer);
            }

            _channel.Close();
            _connection.Close();
            IsBinded = false;
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}
