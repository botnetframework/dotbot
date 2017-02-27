using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dotbot.Slack.Models
{
    internal sealed class SlackHandshake
    {
        [JsonProperty(PropertyName = "ok")]
        public bool Ok { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "self")]
        public SlackUser Self { get; set; }

        [JsonProperty(PropertyName = "team")]
        public SlackTeam Team { get; set; }

        [JsonProperty(PropertyName = "users")]
        public List<SlackUser> Users { get; set; }

        [JsonProperty(PropertyName = "channels")]
        public List<SlackChannel> Channels { get; set; }

        [JsonProperty(PropertyName = "groups")]
        public List<SlackChannel> Groups { get; set; }
    }
}
