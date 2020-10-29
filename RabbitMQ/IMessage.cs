using Newtonsoft.Json;

namespace RabbitMQ
{
    public interface IMessage
    {
        [JsonProperty(PropertyName = "id")]
        int Id { get; set; }
    }
}
