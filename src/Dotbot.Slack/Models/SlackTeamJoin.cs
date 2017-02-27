using Newtonsoft.Json;

namespace Dotbot.Slack.Models
{
    internal sealed class SlackTeamJoin : SlackEvent
    {
        [JsonProperty(PropertyName = "user")]
        public SlackUser User { get; set; }
    }
}
