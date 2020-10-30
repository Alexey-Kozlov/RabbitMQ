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
    public class GetStringByIdHandler2 : BaseCommandHandler<NewCommand, GenericResult<MyTestNameModel2>>, IRequestHandler<NewCommand, GenericResult<MyTestNameModel2>>
    {
        private readonly ITestRepository _testRepository;

        public GetStringByIdHandler2(IMediator mediator, ITestRepository testRepository, IQueueProvider queue) : base(mediator, testRepository, queue)
        {
            _testRepository = testRepository;
        }

        public async Task<GenericResult<MyTestNameModel2>> Handle(NewCommand request, CancellationToken cancellationToken)
        {
            return await _testRepository.GetStringById2(request.Id, cancellationToken);
        }
    }
}
