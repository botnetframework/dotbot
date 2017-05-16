using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Dotbot.Models;
using Dotbot.Slack.Models;

namespace Dotbot.Slack
{
    internal sealed class SlackUserCache : IEnumerable<User>
    {
        private readonly ConcurrentDictionary<string, User> _idLookup;
        private readonly ConcurrentDictionary<string, User> _userNameLookup;

        public SlackUserCache()
        {
            _idLookup = new ConcurrentDictionary<string, User>(StringComparer.OrdinalIgnoreCase);
            _userNameLookup = new ConcurrentDictionary<string, User>(StringComparer.OrdinalIgnoreCase);
        }

        public void Initialize(SlackHandshake handshake)
        {
            // Clear the dictionary.
            _idLookup.Clear();

            // Add channels
            foreach (var slackUser in handshake.Users)
            {
                var user = new User(slackUser.Id, slackUser.Profile.FirstName, slackUser.Name);
                _idLookup.AddOrUpdate(slackUser.Id, user, (k, v) => user);
                _userNameLookup.AddOrUpdate(slackUser.Name, user, (k, v) => user);
            }
        }

        public User GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }
            return _idLookup.TryGetValue(id, out User user) ? user : null;
        }

        public User GetByUserName(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return null;
            }
            return _userNameLookup.TryGetValue(username, out User user) ? user : null;
        }

        public void Add(SlackUser slackUser)
        {
            var user = new User(slackUser.Id, slackUser.Profile.FirstName, slackUser.Name);
            _idLookup.AddOrUpdate(slackUser.Id, user, (k, v) => user);
            _userNameLookup.AddOrUpdate(slackUser.Name, user, (k, v) => user);
        }

        public IEnumerator<User> GetEnumerator()
        {
            return _idLookup.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}