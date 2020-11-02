using MediatR;
using RabbitReceiverConsole.Models;
using AKDbHelpers.Helpers;

namespace RabbitReceiverConsole.Commands
{
    public class NewCommand : IRequest<GenericResult<MyTestNameModel2>>
    {
        public int Id { get; set; }
        public string Message { get; set; }
    }
}
