using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Generic;
using System.Text;
using Serilog;

namespace RabbitMQ
{
    public class RabbitMqProvider : IQueueProvider, IDisposable
    {
        private readonly List<string> _queues;
        private IConnection _connection;
        private IModel _channel;
        private string _consumer;
        private string _exchangeName;
        private readonly ConnectionFactory _connectionFactory;

        public RabbitMqProvider(MqCredentials credentials, List<string> Queues, string exchangeName)
        {
            _queues = Queues;
            _exchangeName = exchangeName;
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

        public void Subscribe(Action<string> callback, string queueName)
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
                    Log.Information($"Получено сообщение : {message}");
                    callback(message);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, $"Ошибка получения сообщения из очереди ");
                }
                finally
                {
                    _channel.BasicAck(ea.DeliveryTag, false);
                }
            };
            _channel.BasicConsume(queue: queueName, consumer: consumer);
            Log.Information($"Подписка на очередь {queueName} создана");
        }


        public void Send(string message)
        {
            if (!IsBinded)
            {
                throw new InvalidOperationException("Связь с очередью отутствует");
            }

            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(_exchangeName, "", null, body);
            Log.Information($"Сообщение: {message} отправлено.");
        }

        public void Bind()
        {
            if (IsBinded)
            {
                throw new InvalidOperationException("Связь с очередью уже установлена");
            }
            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(_exchangeName, ExchangeType.Fanout, true, false);
            if (MaxThreads != 0)
                _channel.BasicQos(0, (ushort)MaxThreads, false);
            foreach(string queue in _queues)
            {
                _channel.QueueBind(queue, _exchangeName, "");
            }
            
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
