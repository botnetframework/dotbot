using Newtonsoft.Json;

namespace Dotbot.Slack.Models
{
    internal sealed class SlackMessage : SlackEvent
    {
        [JsonProperty(PropertyName = "channel")]
        public string ChannelId { get; set; }

        [JsonProperty(PropertyName = "user")]
        public string UserId { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "subtype")]
        public string SubType { get; set; }
    }
}