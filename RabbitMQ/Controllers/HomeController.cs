using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using MediatR;
using RabbitMQ.Commands;
using AKDbHelpers.Helpers;
using RabbitMQ.Models;

namespace RabbitMQ.Controllers
{
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly IQueueProvider _queue;
        private readonly IMediator _mediator;
        public HomeController(IQueueProvider queue, IMediator mediator)
        {
            _queue = queue;
            _mediator = mediator;
        }

        [HttpGet("Index")]
        public IActionResult Index()
        {
            Log.Information($"Sending data from controller - {DateTime.Now.ToString()}"); 
            _queue.Send(DateTime.Now.ToString()); ;
            Log.Information($"Sending data from controller successful.");
            return Ok();
        }

        [HttpGet("C1")]
        public IActionResult C1()
        {
            var getStringCommand = new GetStringByIdQuery
            {
                Id = 1556
            };
            
            var command = new BaseCommand<GetStringByIdQuery, GenericResult<MyTestNameModel>>(getStringCommand, $"Выполнение c1 - {DateTime.Now.ToString()}");
            var rez = _mediator.Send(command);           
            return Ok(rez.Result.Entity.TestName);
        }

        [HttpGet("C2/{id:int}")]
        public IActionResult C2(int id)
        {
            var getStringCommand = new NewCommand
            {
                Id = id
            };

            var command = new BaseCommand<NewCommand, GenericResult<MyTestNameModel2>>(getStringCommand, $"Выполнение c2 - {DateTime.Now.ToString()}");
            var result = _mediator.Send(command);
            if(result.Result.IsSuccess)
                return Ok(result.Result.Entity.TestName);
            return BadRequest("не найдено");
        }

    }
}
