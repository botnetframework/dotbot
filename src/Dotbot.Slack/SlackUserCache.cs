using System.Collections.Concurrent;
using Dotbot.Models;
using Dotbot.Slack.Models;

namespace Dotbot.Slack
{
    internal sealed class SlackUserCache
    {
        private readonly ConcurrentDictionary<string, User> _dictionary;

        public SlackUserCache()
        {
            _dictionary = new ConcurrentDictionary<string, User>();
        }

        public void Initialize(SlackHandshake handshake)
        {
            // Clear the dictionary.
            _dictionary.Clear();

            // Add channels
            foreach (var slackUser in handshake.Users)
            {
                var user = new User(slackUser.Id, slackUser.Profile.FirstName, slackUser.Name);
                _dictionary.AddOrUpdate(slackUser.Id, user, (k, v) => user);
            }
        }

        public User Get(string id)
        {
            return _dictionary.TryGetValue(id, out User user) ? user : null;
        }

        public void Add(SlackUser slackUser)
        {
            var user = new User(slackUser.Id, slackUser.Profile.FirstName, slackUser.Name);
            _dictionary.AddOrUpdate(slackUser.Id, user, (k, v) => user);
        }
    }
}