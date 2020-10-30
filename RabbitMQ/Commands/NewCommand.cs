using MediatR;
using RabbitMQ.Models;
using AKDbHelpers.Helpers;

namespace RabbitMQ.Commands
{
    public class NewCommand : IRequest<GenericResult<MyTestNameModel2>>
    {
        public int Id { get; set; }
    }
}
