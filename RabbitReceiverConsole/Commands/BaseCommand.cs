using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitReceiverConsole.Commands
{
    public class BaseCommand<T, R> : IRequest<R> where T: IRequest<R>
    {
        public T Command { get;  }
        public string LogMessage { get;  }
        public BaseCommand(T command, string logMessage)
        {
            Command = command;
            LogMessage = logMessage;
        }
    }
}
