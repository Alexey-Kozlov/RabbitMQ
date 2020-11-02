using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitMQ
{
    public class MqCredentials
    {
        public MqCredentials() { }
        public MqCredentials(string hostName, string userName, string password, List<string> queues)
        {
            HostName = hostName;
            UserName = userName;
            Password = password;
            Queues = queues;
        }

        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public List<string> Queues { get; set; }
    }
}
