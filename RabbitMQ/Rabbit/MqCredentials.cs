using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitMQ
{
    public class MqCredentials
    {
        public MqCredentials() { }
        public MqCredentials(string hostName, string userName, string password, string queueName)
        {
            HostName = hostName;
            UserName = userName;
            Password = password;
            QueueName = queueName;
        }

        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string QueueName { get; set; }
    }
}
