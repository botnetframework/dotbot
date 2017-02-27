using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Dotbot.Contexts;

namespace Dotbot.Utilities
{
    public abstract class CommandPart : MentionablePart
    {
        private readonly List<Regex> _regexes;

        public string[] Aliases { get; }
        public abstract string Help { get; }

        protected CommandPart(IEnumerable<string> commands)
        {
            Aliases = commands.ToArray();

            _regexes = new List<Regex>();
            foreach (var alias in Aliases)
            {
                _regexes.Add(new Regex($"(?<command>{alias})( (?<args>.*))?", RegexOptions.Compiled | RegexOptions.IgnoreCase));
            }
        }

        protected sealed override bool HandleMention(MessageContext context, string message)
        {
            foreach (var regex in _regexes)
            {
                var match = regex.Match(context.Message.Text);
                if (match.Success)
                {
                    var argsGroup = match.Groups["args"] != null ? match.Groups["args"].ToString() : string.Empty;
                    var args = argsGroup.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    HandleCommand(context, args);
                    return true;
                }

            }
            return false;
        }

        protected abstract void HandleCommand(MessageContext context, string[] args);
    }
}
