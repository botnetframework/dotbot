using Newtonsoft.Json;

namespace Dotbot.Slack.Models
{
    internal class SlackEvent
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
    }
}
