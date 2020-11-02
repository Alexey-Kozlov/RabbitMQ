using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitMQ
{
    public class Credentials
    {
        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; } 
    }

    public class RabbitOptions
    {
        public List<string> Queues { get; set; }
        public Credentials Credentials { get; set; }
        public string ExchangeName { get; set; }
    }

}
