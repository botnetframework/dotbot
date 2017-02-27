using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dotbot.Gitter.Models
{
    internal sealed class GitterMessage
    {
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "sent")]
        public DateTime Sent { get; set; }

        [JsonProperty(PropertyName = "fromUser")]
        public GitterUser FromUser { get; set; }

        [JsonProperty(PropertyName = "urls")]
        public List<string> Urls { get; set; }

        [JsonProperty(PropertyName = "mentions")]
        public List<GitterMention> Mentions { get; set; }

        [JsonProperty(PropertyName = "issues")]
        public List<GitterIssue> Issues { get; set; }
    }
}
