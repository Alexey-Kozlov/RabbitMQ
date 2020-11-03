using System;
using RabbitReceiverConsole.Models;
using MediatR;
using Newtonsoft.Json;
using AKDbHelpers.Helpers;
using Serilog;
using Autofac;
using RabbitReceiverConsole.Commands;
using RabbitReceiverConsole.Handlers;
using System.Threading.Tasks;

namespace RabbitReceiverConsole
{
    public class ReceiveMessage
    {
        private readonly IMediator _mediator;
        public ReceiveMessage(IContainer container)
        {
            using (var scope = container.BeginLifetimeScope())
            {
                _mediator = scope.Resolve<IMediator>();
            }

        }

        public async void GetMes(string message)
        {

            var command = JsonConvert.DeserializeObject(message, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
            });
            var rez = _mediator.Send(command).Result;

            switch(rez)
            {
                case GenericResult<MyTestNameModel> command2:
                    Log.Information($"Получено сообщение - {command2.Entity.TestName}");

                    //Task.Run(async () => await _mediator.Publish(new Notifications("Тест уведомления 1")));
                    await _mediator.Publish(new Notifications("Тест уведомления 1"));
                    break;
                case GenericResult<MyTestNameModel2> command2:
                    var sourceCommand = (BaseCommand<NewCommand, GenericResult<MyTestNameModel2>>)command;
                    Log.Information($"Получено сообщение - {command2.Entity.TestName}, параметр команды - {sourceCommand.Command.Message}");
                    //Task.Run(async () => await _mediator.Publish(new Notifications("Тест уведомления 2")));
                    await _mediator.Publish(new Notifications("Тест уведомления 2"));
                    break;
            }


        }
    }
}
