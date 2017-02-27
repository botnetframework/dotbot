using Newtonsoft.Json;

namespace Dotbot.Slack.Models
{
    internal sealed class SlackGroupLeft : SlackEvent
    {
        [JsonProperty(PropertyName = "channel")]
        public string ChannelId { get; set; }
    }
}
