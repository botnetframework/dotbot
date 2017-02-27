using Newtonsoft.Json;

namespace Dotbot.Gitter.Models
{
    internal sealed class Envelope<T>
    {
        [JsonProperty(PropertyName = "operation")]
        public string Operation { get; set; }

        [JsonProperty(PropertyName = "model")]
        public T Model { get; set; }
    }
}
