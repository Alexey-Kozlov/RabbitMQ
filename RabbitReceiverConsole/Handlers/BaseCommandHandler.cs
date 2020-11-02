using MediatR;
using RabbitReceiverConsole.Commands;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitReceiverConsole.Handlers
{
    public class BaseCommandHandler<T, R> : IRequestHandler<BaseCommand<T, R>, R> where T : IRequest<R>
    {
        private readonly IMediator _mediator;

        public BaseCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<R> Handle(BaseCommand<T, R> message, CancellationToken cancellationToken)
        {
            var command = message.Command;
            var logMessage = message.LogMessage;

            //собственно выполнение команды
            var result = await _mediator.Send(command, cancellationToken);

            return result;
        }
    }
}
