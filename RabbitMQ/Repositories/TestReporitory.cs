using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using AKDbHelpers.DataBaseHelpers;
using AKDbHelpers.Helpers;
using RabbitMQ.Models;

namespace RabbitMQ.Repositories
{
    public class TestReporitory : BaseRepository, ITestRepository
    {
        private readonly IConfiguration _configuration;
        public TestReporitory(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public async Task<GenericResult<MyTestNameModel>> GetStringById(int Id, CancellationToken cancellationToken)
        {
            var cmd = Db.CreateProcedureCommand("dbo.GetStringById");
            cmd.Parameters.AddWithValue("@Id", Id);
            var result = await Db.ExecuteReaderAsync( reader => new MyTestNameModel
            {
                TestName = reader.GetValue<string>("TestName"),
                Id = Id
            }, cmd, cancellationToken);
            if(result.Count == 0)
            {
                return GenericResult<MyTestNameModel>.Error("Нет данных");
            }
            return GenericResult<MyTestNameModel>.Success(result.FirstOrDefault());
        }

        public async Task<int> WriteLog(string mes, CancellationToken cancellationToken)
        {
            var cmd = Db.CreateProcedureCommand("dbo.WriteLog");
            cmd.Parameters.AddWithValue("@mes", mes);
            cmd.Parameters.Add("@Id", SqlDbType.Int).Direction = ParameterDirection.Output;
            await Db.ExecuteNonQueryAsync(cmd, cancellationToken);
            return cmd.ReadOutputValue<int>("@id");
        }

        public async Task<GenericResult<MyTestNameModel2>> GetStringById2(int Id, CancellationToken cancellationToken)
        {
            var cmd = Db.CreateProcedureCommand("dbo.GetStringById2");
            cmd.Parameters.AddWithValue("@Id", Id);
            var result = await Db.ExecuteReaderAsync(reader => new MyTestNameModel2
            {
                TestName = reader.GetValue<string>("TestName"),
                Id = Id
            }, cmd, cancellationToken);
            if (result.Count == 0)
            {
                return GenericResult<MyTestNameModel2>.Error("Нет данных");
            }
            return GenericResult<MyTestNameModel2>.Success(result.FirstOrDefault());
        }
    }
}
