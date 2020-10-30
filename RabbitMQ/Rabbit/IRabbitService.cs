using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitMQ
{
    public interface IRabbitService
    {
        void Publish<T>(string routingKey, string message);
    }
}
