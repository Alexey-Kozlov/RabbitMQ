using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace RabbitMQ
{
    public class RabbitLogger : IRabbitLogger
    {
        private readonly ILogger<RabbitLogger> _logger;

        public RabbitLogger(ILogger<RabbitLogger> logger)
        {
            _logger = logger;
        }

        public void Info(string message)
        {
            _logger.LogInformation("Rabbit info: " + message);
        }

        public void Trace(string message)
        {
            _logger.LogTrace("Rabbit trace: " + message);
        }

        public void Error(Exception excepion, string message)
        {
            _logger.LogError(excepion, "Rabbit error: " + message);
        }
    }
}
