using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using RabbitMQ.Models;
using AKDbHelpers.Helpers;

namespace RabbitMQ.Commands
{
    public class GetStringByIdQuery : IRequest<GenericResult<MyTestNameModel>>
    {
        public int Id { get; set; }
    }
}
