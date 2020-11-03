using MediatR;
using RabbitMQ.Commands;
using RabbitMQ.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMQ.Handlers
{
    public class BaseCommandHandler<T, R> : IRequestHandler<BaseCommand<T, R>, R> where T : IRequest<R>
    {
        private readonly IMediator _mediator;
        private readonly ITestRepository _testRepository;
        private readonly IQueueProvider _queue;
        public BaseCommandHandler(IMediator mediator, ITestRepository testRepository, IQueueProvider queue)
        {
            _mediator = mediator;
            _testRepository = testRepository;
            _queue = queue;
        }

        public async Task<R> Handle(BaseCommand<T, R> message, CancellationToken cancellationToken)
        {
            var command = message.Command;
            var logMessage = message.LogMessage;
            //делаем что-то общее для всех команд. Например, пишем в лог
            await _testRepository.WriteLog(logMessage, cancellationToken);
            //собственно выполнение команды
            var result = await _mediator.Send(command, cancellationToken);
            //опять что-то общее, например, посылаем в очередь
            var serString = JsonConvert.SerializeObject(message, Formatting.None, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
                SerializationBinder = new TypeNameAssemblyExcludingSerializationBinder("RabbitReceiverConsole")
            });

            _queue.Send($"{serString}");
            return result;
        }
    }

    public class TypeNameAssemblyExcludingSerializationBinder : ISerializationBinder
    {
        public TypeNameAssemblyExcludingSerializationBinder(string namespaceToTypes) 
        {
            _namespaceToTypes = namespaceToTypes;
        }
        private readonly string _namespaceToTypes;

        public void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            assemblyName = _namespaceToTypes;
            typeName = serializedType.FullName.Replace("RabbitMQ", _namespaceToTypes).Trim('.');
        }

        public Type BindToType(string assemblyName, string typeName)
        {
            var typeNameWithNamespace = $"{_namespaceToTypes}.{typeName}";
            return Type.GetType(typeNameWithNamespace);
        }
    }
}
