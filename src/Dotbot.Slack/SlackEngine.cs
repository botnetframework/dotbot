using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dotbot.Diagnostics;
using Dotbot.Models;
using Dotbot.Models.Events;
using Dotbot.Slack.Models;

namespace Dotbot.Slack
{
    internal sealed class SlackEngine
    {
        private readonly SlackBroker _broker;
        private readonly IEventQueue _queue;
        private readonly ILog _log;
        private readonly SlackRoomCache _rooms;
        private readonly SlackUserCache _users;

        public SlackEngine(SlackBroker broker, IEventQueue queue, ILog log)
        {
            _broker = broker;
            _queue = queue;
            _log = new LogNameDecorator("Slack", log);
            _rooms = new SlackRoomCache();
            _users = new SlackUserCache();
        }

        public async Task<(User bot, Uri handshake)> Handshake()
        {
            // Connect to Slack.
            var handshake = await _broker.Handshake();
            if (!handshake.Ok)
            {
                throw new InvalidOperationException("Could not connect to Slack.");
            }

            // Initialize the caches.
            _rooms.Initialize(handshake);
            _users.Initialize(handshake);

            // Create an representation of ourself.
            var self = handshake.Self;
            var bot = new User(self.Id, self.Name, handshake.Self.Profile?.RealName ?? handshake.Self.Name);
            _log.Information("Current user is {0}.", bot.Username);

            // Log information about channels and groups we've joined.
            foreach (var channel in handshake.Channels.Where(x => x.IsChannel && x.Members != null))
            {
                _log.Information("Subscribed to channel {0} ({1}).", channel.Name, channel.Id);
            }
            foreach (var group in handshake.Groups)
            {
                _log.Information("Subscribed to group {0} ({1}).", group.Name, group.Id);
            }

            return (bot, new Uri(handshake.Url));
        }

        public void ProcessEvent(Stream stream, User bot)
        {
            // Get the event type.
            var eventType = stream.Deserialize<SlackEvent>().Type ?? string.Empty;
            stream.Seek(0, SeekOrigin.Begin);

            if (eventType.Equals("message", StringComparison.OrdinalIgnoreCase))
            {
                // We received a message.
                ProcessMessage(stream, bot);
            }
            else if (eventType.Equals("team_join", StringComparison.OrdinalIgnoreCase))
            {
                // Someone joined the team.
                ProcessTeamJoin(stream);
            }
            else if (eventType.Equals("channel_created", StringComparison.OrdinalIgnoreCase))
            {
                // Someone created a channel.
                ProcessChannelCreated(stream);
            }
            else if (eventType.Equals("group_joined", StringComparison.OrdinalIgnoreCase))
            {
                // We joined a group.
                ProcessGroupJoined(stream);
            }
            else if (eventType.Equals("group_left", StringComparison.OrdinalIgnoreCase))
            {
                // We left a group.
                ProcessGroupLeft(stream);
            }
        }

        private void ProcessMessage(Stream stream, User bot)
        {
            var message = stream.Deserialize<SlackMessage>();

            if (string.IsNullOrWhiteSpace(message.SubType))
            {
                // From ourselves?
                if (message.UserId?.Equals(bot.Id) ?? true)
                {
                    return;
                }

                // Find the room and the sender.
                var room = _rooms.GetById(message.ChannelId);
                var sender = _users.GetById(message.UserId);
                if (room == null || sender == null)
                {
                    return;
                }

                // Parse message text.
                var parts = SlackMessageParser.Parse(_users, message.Text);
                var recipient = _users.GetByUserName(parts.recipient);

                // Enqueue the message.
                _queue.Enqueue(new MessageEvent(bot, room, new Message(sender, recipient, parts.text), _broker));
            }
            else if (message.SubType.Equals("channel_join", StringComparison.OrdinalIgnoreCase))
            {
                // A user joined the channel.
            }
            else if (message.SubType.Equals("channel_left", StringComparison.OrdinalIgnoreCase))
            {
                // A user left the channel.
            }
        }

        private void ProcessTeamJoin(Stream stream)
        {
            var message = stream.Deserialize<SlackTeamJoin>();
            _users.Add(message.User);
            _log.Information("User {0} ({1}) joined the team.", message.User.Profile.RealName, message.User.Name);
        }

        private void ProcessChannelCreated(Stream stream)
        {
            var message = stream.Deserialize<SlackChannelCreated>();
            _rooms.Add(message.Channel);
            _log.Information("Channel {0} was created.", message.Channel.Name);
        }

        private void ProcessGroupJoined(Stream stream)
        {
            var message = stream.Deserialize<SlackChannelCreated>();
            _rooms.Add(message.Channel);
            _log.Information("We joined the group {0}.", message.Channel.Name);
        }

        private void ProcessGroupLeft(Stream stream)
        {
            var message = stream.Deserialize<SlackGroupLeft>();
            var room = _rooms.Remove(message.ChannelId);
            _log.Information("We left the group {0}.", room?.Name ?? message.ChannelId);
        }
    }
}
