using System.Linq;
using System.Text.RegularExpressions;

namespace Dotbot.Slack
{
    internal static class SlackMessageParser
    {
        private static readonly Regex Regex;

        static SlackMessageParser()
        {
            Regex = new Regex("^(?<recipient>[a-z@\\d](?:[a-z\\d]|-(?=[a-z\\d])){0,38}) (?<message>.*)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        public static (string recipient, string text) Parse(SlackUserCache users, string text)
        {
            text = users.Aggregate(text, (m, u) => Regex.Replace(m, $"<@{u.Id}>", $"@{u.Username}"));

            var match = Regex.Match(text);
            if (match.Success)
            {
                var recipient = match.Groups["recipient"].Value;
                var message = match.Groups["message"] != null ? match.Groups["message"].Value : string.Empty;

                return (recipient.TrimStart('@'), message);
            }
            return (null, text);
        }
    }
}
