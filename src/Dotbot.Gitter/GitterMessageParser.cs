using System;
using System.Text.RegularExpressions;
using Dotbot.Models;

namespace Dotbot.Gitter
{
    internal static class GitterMessageParser
    {
        private static readonly Regex Regex;

        static GitterMessageParser()
        {
            Regex = new Regex("^(?<recipient>[a-z@\\d](?:[a-z\\d]|-(?=[a-z\\d])){0,38}) (?<message>.*)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        public static Tuple<string, string> Parse(string text)
        {
            var match = Regex.Match(text);
            if (match.Success)
            {
                var recipient = match.Groups["recipient"].Value;
                var message = match.Groups["message"] != null ? match.Groups["message"].Value : string.Empty;

                return Tuple.Create(recipient.TrimStart('@'), message);
            }
            return Tuple.Create((string)null, text);
        }
    }
}
