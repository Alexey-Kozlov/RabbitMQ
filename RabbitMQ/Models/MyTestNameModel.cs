﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitMQ.Models
{
    [JsonObject]
    public class MyTestNameModel
    {
        [JsonProperty]
        public string TestName { get; set; }
        [JsonProperty]
        public int Id { get; set; }
    }
}
