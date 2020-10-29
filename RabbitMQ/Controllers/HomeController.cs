using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace RabbitMQ.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IQueueProvider _queue;
        public HomeController(IQueueProvider queue)
        {
            _queue = queue;
        }

        [HttpGet("Index")]
        public IActionResult Index()
        {
            Log.Information($"Sending data from controller - {DateTime.Now.ToString()}"); 
            _queue.Send(DateTime.Now.ToString()); ;
            Log.Information($"Sending data from controller successful.");
            return Ok();
        }

    }
}
