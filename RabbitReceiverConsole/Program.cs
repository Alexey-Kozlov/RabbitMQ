using System;
using Serilog;

namespace RabbitReceiverConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();
            var credentials = new MqCredentials(Environment.GetEnvironmentVariable("RabbitMq__Credentials__HostName"),
                Environment.GetEnvironmentVariable("RabbitMq__Credentials__UserName"),
                Environment.GetEnvironmentVariable("RabbitMq__Credentials__Password"),
                Environment.GetEnvironmentVariable("RabbitMq__QueueName"));
            var rabbitProvider = new RabbitMqProvider(credentials, Environment.GetEnvironmentVariable("RabbitMq__QueueName"));
            rabbitProvider.Bind();
            rabbitProvider.Subscribe(ReceiveMessage.GetMes);
            Console.ReadLine();
        }
    }
}
