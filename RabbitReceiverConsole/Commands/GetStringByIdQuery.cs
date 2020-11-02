using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using RabbitReceiverConsole.Models;
using AKDbHelpers.Helpers;

namespace RabbitReceiverConsole.Commands
{
    public class GetStringByIdQuery : IRequest<GenericResult<MyTestNameModel>>
    {
        public int Id { get; set; }
    }
}
