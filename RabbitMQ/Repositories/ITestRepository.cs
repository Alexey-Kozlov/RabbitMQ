using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AKDbHelpers.Helpers;
using RabbitMQ.Models;

namespace RabbitMQ.Repositories
{
    public interface ITestRepository
    {
        Task<GenericResult<MyTestNameModel>> GetStringById(int Id, CancellationToken cancellationToken);
        Task<int> WriteLog(string mes, CancellationToken cancellationToken);
        Task<GenericResult<MyTestNameModel2>> GetStringById2(int Id, CancellationToken cancellationToken);
    }
}
