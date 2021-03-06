﻿using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using RabbitMQ.Models;
using AKDbHelpers.Helpers;
using Newtonsoft.Json;

namespace RabbitMQ.Commands
{
    [JsonObject]
    public class GetStringByIdQuery : IRequest<GenericResult<MyTestNameModel>>
    {
        [JsonProperty]
        public int Id { get; set; }
    }
}
