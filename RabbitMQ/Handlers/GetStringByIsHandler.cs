using System;
using RabbitMQ.Repositories;
using MediatR;
using System.Threading.Tasks;
using RabbitMQ.Models;
using RabbitMQ.Commands;
using System.Threading;
using AKDbHelpers.Helpers;

namespace RabbitMQ.Handlers
{
    public class GetStringByIdHandler : BaseCommandHandler<GetStringByIdQuery, GenericResult<MyTestNameModel>>, IRequestHandler<GetStringByIdQuery, GenericResult<MyTestNameModel>>
    {
        private readonly ITestRepository _testRepository;

        public GetStringByIdHandler(IMediator mediator, ITestRepository testRepository, IQueueProvider queue) : base(mediator, testRepository, queue)
        {
            _testRepository = testRepository;
        }

        public async Task<GenericResult<MyTestNameModel>> Handle(GetStringByIdQuery request, CancellationToken cancellationToken)
        {
            return await _testRepository.GetStringById(request.Id, cancellationToken);
        }
    }
}
