using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;

namespace RabbitMQ
{
    public class ReceiveMessage
    {

        public ReceiveMessage()
        {

        }

        public static void GetMes(string mes)
        {

            Log.Information($"Получено сообщение - {mes}");
        }
    }
}
