using System;
using System.Text.RegularExpressions;
using Dotbot.Contexts;

namespace Dotbot.Utilities
{
    public abstract class MentionablePart : RobotPart
    {
        private readonly Regex _regex;

        protected MentionablePart()
        {
            _regex = new Regex("^(?<to>[a-z@\\d](?:[a-z\\d]|-(?=[a-z\\d])){0,38}) (?<message>.*)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        public sealed override bool Handle(MessageContext context)
        {
            var match = _regex.Match(context.Message.Text);
            if (match.Success)
            {
                var to = match.Groups["to"].Value;
                if (to.Equals(string.Concat("@", context.Bot.Username), StringComparison.OrdinalIgnoreCase) ||
                    to.Equals(context.Bot.DisplayName, StringComparison.OrdinalIgnoreCase))
                {
                    var message = match.Groups["message"] != null ? match.Groups["message"].Value : string.Empty;
                    return HandleMention(context, message.Trim());
                }
            }

            return false;
        }

        protected abstract bool HandleMention(MessageContext context, string message);
    }
}
