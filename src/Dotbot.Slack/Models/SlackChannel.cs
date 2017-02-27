using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;

namespace Dotbot.Slack.Models
{
    [DebuggerDisplay("{Name,nq} ({Id,nq})")]
    internal sealed class SlackChannel
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "is_channel")]
        public bool IsChannel { get; set; }

        [JsonProperty(PropertyName = "is_group")]
        public bool IsGroup { get; set; }

        [JsonProperty(PropertyName = "members")]
        public List<string> Members { get; set; }
    }
}