using System;
using Serilog;
using System.Configuration;
using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Autofac.Extensions.DependencyInjection;

namespace RabbitReceiverConsole
{
    class Program
    {
        private static IContainer container { get; set; }
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();


            string HostName = Environment.GetEnvironmentVariable("RabbitMq__Credentials__HostName") ?? ConfigurationManager.AppSettings["HostName"];
            string UserName = Environment.GetEnvironmentVariable("RabbitMq__Credentials__UserName") ?? ConfigurationManager.AppSettings["UserName"];
            string Password = Environment.GetEnvironmentVariable("RabbitMq__Credentials__Password") ?? ConfigurationManager.AppSettings["Password"];
            string QueueName = Environment.GetEnvironmentVariable("RabbitMq__QueueName") ?? ConfigurationManager.AppSettings["QueueName"];


            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule(new MediatorModule());
            container = containerBuilder.Build();


            var credentials = new MqCredentials(HostName, UserName, Password, QueueName);
            var rabbitProvider = new RabbitMqProvider(credentials, QueueName);

            rabbitProvider.Bind();
            ReceiveMessage recMessage = new ReceiveMessage(container);
            rabbitProvider.Subscribe(recMessage.GetMes);
            Console.ReadLine();
        }

    }
}
