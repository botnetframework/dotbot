using Newtonsoft.Json;

namespace Dotbot.Slack.Models
{
    internal sealed class SlackUser
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "deleted")]
        public bool Deleted { get; set; }

        [JsonProperty(PropertyName = "profile")]
        public SlackProfile Profile { get; set; }
    }
}