using System.Collections.Concurrent;
using Dotbot.Models;
using Dotbot.Slack.Models;

namespace Dotbot.Slack
{
    internal sealed class SlackRoomCache
    {
        private readonly ConcurrentDictionary<string, Room> _dictionary;

        public SlackRoomCache()
        {
            _dictionary = new ConcurrentDictionary<string, Room>();
        }

        public void Initialize(SlackHandshake handshake)
        {
            // Clear the dictionary.
            _dictionary.Clear();

            // Add channels
            foreach (var channel in handshake.Channels)
            {
                var room = new Room { Id = channel.Id, Name = channel.Name };
                _dictionary.AddOrUpdate(channel.Id, room, (k, v) =>
                    new Room { Id = k, Name = v.Name });
            }

            // Add groups
            foreach (var group in handshake.Groups)
            {
                var room = new Room { Id = group.Id, Name = group.Name };
                _dictionary.AddOrUpdate(group.Id, room, (k, v) =>
                    new Room { Id = k, Name = v.Name });
            }
        }

        public void Add(SlackChannel channel)
        {
            var room = new Room { Id = channel.Id, Name = channel.Name };
            _dictionary.AddOrUpdate(channel.Id, room, (k, v) => room);
        }

        public Room Get(string id)
        {
            return _dictionary.TryGetValue(id, out Room room) ? room : null;
        }

        public Room Remove(string id)
        {
            _dictionary.TryRemove(id, out Room value);
            return value;
        }
    }
}
