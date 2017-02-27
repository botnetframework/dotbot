using Newtonsoft.Json;

namespace Dotbot.Slack.Models
{
    internal sealed class SlackProfile
    {
        [JsonProperty(PropertyName = "first_name")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "last_name")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "real_name")]
        public string RealName { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
    }
}
