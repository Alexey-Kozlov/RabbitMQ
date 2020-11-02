using System;
using MediatR;
using System.Threading.Tasks;
using RabbitReceiverConsole.Models;
using RabbitReceiverConsole.Commands;
using System.Threading;
using AKDbHelpers.Helpers;

namespace RabbitReceiverConsole.Handlers
{
    public class GetStringByIdHandler : BaseCommandHandler<GetStringByIdQuery, GenericResult<MyTestNameModel>>, IRequestHandler<GetStringByIdQuery, GenericResult<MyTestNameModel>>
    {

        public GetStringByIdHandler(IMediator mediator) : base(mediator)
        {

        }

        public async Task<GenericResult<MyTestNameModel>> Handle(GetStringByIdQuery request, CancellationToken cancellationToken)
        {
            var model = new MyTestNameModel
            {
                Id = request.Id,
                TestName = "что-то тестовое 1"
            };
            return await Task.FromResult(GenericResult<MyTestNameModel>.Success(model));
        }
    }
}
