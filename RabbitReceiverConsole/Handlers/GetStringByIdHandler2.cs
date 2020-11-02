using System;
using MediatR;
using System.Threading.Tasks;
using RabbitReceiverConsole.Models;
using RabbitReceiverConsole.Commands;
using System.Threading;
using AKDbHelpers.Helpers;

namespace RabbitReceiverConsole.Handlers
{
    public class GetStringByIdHandler2 : BaseCommandHandler<NewCommand, GenericResult<MyTestNameModel2>>, IRequestHandler<NewCommand, GenericResult<MyTestNameModel2>>
    {

        public GetStringByIdHandler2(IMediator mediator) : base(mediator)
        {

        }

        public async Task<GenericResult<MyTestNameModel2>> Handle(NewCommand request, CancellationToken cancellationToken)
        {
            var model = new MyTestNameModel2
            {
                Id = request.Id
            };
            return await Task.FromResult(GenericResult<MyTestNameModel2>.Success(model));
        }
    }
}
