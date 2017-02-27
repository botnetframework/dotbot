using System;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Dotbot.Diagnostics;
using Dotbot.Models;
using Dotbot.Models.Events;
using Dotbot.Slack.Models;
using Newtonsoft.Json;

namespace Dotbot.Slack
{
    internal sealed class SlackAdapter : IAdapter, IWorker
    {
        private readonly SlackBroker _broker;
        private readonly IMessageQueue _messageQueue;
        private readonly ILog _log;
        private readonly JsonSerializer _serializer;
        private readonly SlackRoomCache _rooms;
        private readonly SlackUserCache _users;

        public string FriendlyName => "Slack adapter";
        public IBroker Broker => _broker;

        public SlackAdapter(SlackBroker broker, IMessageQueue messageQueue, ILog log)
        {
            _broker = broker;
            _messageQueue = messageQueue;
            _log = new LogNameDecorator("Slack", log);
            _serializer = new JsonSerializer();
            _rooms = new SlackRoomCache();
            _users = new SlackUserCache();
        }

        public async Task<bool> Run(CancellationToken token)
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
            var bot = new User
            {
                Id = handshake.Self.Id,
                Username = handshake.Self.Name,
                DisplayName = handshake.Self.Profile?.RealName ?? handshake.Self.Name
            };

            _log.Information("Current user is {0}.", bot.Username);

            foreach (var channel in handshake.Channels.Where(x => x.IsChannel && x.Members != null))
            {
                _log.Information("Subscribed to channel {0} ({1}).", channel.Name, channel.Id);
            }
            foreach (var group in handshake.Groups)
            {
                _log.Information("Subscribed to group {0} ({1}).", group.Name, group.Id);
            }

            using (var ws = new ClientWebSocket())
            {
                // Connect to the websocket endpoint.
                await ws.ConnectAsync(new Uri(handshake.Url), token);

                // Create the buffer.
                var buffer = new ArraySegment<byte>(new byte[4096]);

                // Start reading messages.
                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        using (var stream = new MemoryStream())
                        {
                            while (true)
                            {
                                // Receive data.
                                var received = await ws.ReceiveAsync(buffer, token);

                                // Write it to the result stream.
                                await stream.WriteAsync(buffer.Array, buffer.Offset, received.Count, token);

                                // End of message?
                                if (received.EndOfMessage)
                                {
                                    break;
                                }
                            }

                            ProcessEvent(stream, handshake, bot);
                        }
                    }
                    catch (WebSocketException e) when (e.WebSocketErrorCode == WebSocketError.InvalidState)
                    {
                        // We get this exception if the task was cancelled.
                    }
                }
            }

            return true;
        }

        private void ProcessEvent(Stream stream, SlackHandshake handshake, User bot)
        {
            // Get the event type.
            var eventType = Deserialize<SlackEvent>(stream).Type ?? string.Empty;
            stream.Seek(0, SeekOrigin.Begin);

            if (eventType.Equals("message", StringComparison.OrdinalIgnoreCase))
            {
                // We received a message.
                ProcessMessage(stream, handshake, bot);
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

        private void ProcessMessage(Stream stream, SlackHandshake handshake, User bot)
        {
            var message = Deserialize<SlackMessage>(stream);

            if (string.IsNullOrWhiteSpace(message.SubType))
            {
                var room = _rooms.Get(message.ChannelId);
                var user = _users.Get(message.UserId);

                // Unknown room or user?
                if (room == null || user == null)
                {
                    return;
                }

                // Replace <@USERID> with the user name.
                var text = handshake.Users.Aggregate(message.Text,
                    (m, u) => Regex.Replace(m, $"<@{u.Id}>", $"@{u.Name}"));

                _messageQueue.Enqueue(new MessageEvent(_broker)
                {
                    Bot = bot,
                    Message = new Message { Text = text, User = user },
                    Room = room
                });
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
            var message = Deserialize<SlackTeamJoin>(stream);
            _users.Add(message.User);
            _log.Information("User {0} ({1}) joined the team.", message.User.Profile.RealName, message.User.Name);
        }

        private void ProcessChannelCreated(Stream stream)
        {
            var message = Deserialize<SlackChannelCreated>(stream);
            _rooms.Add(message.Channel);
            _log.Information("Channel {0} was created.", message.Channel.Name);
        }

        private void ProcessGroupJoined(Stream stream)
        {
            var message = Deserialize<SlackChannelCreated>(stream);
            _rooms.Add(message.Channel);
            _log.Information("We joined the group {0}.", message.Channel.Name);
        }

        private void ProcessGroupLeft(Stream stream)
        {
            var message = Deserialize<SlackGroupLeft>(stream);
            var room = _rooms.Remove(message.ChannelId);
            _log.Information("We left the group {0}.", room?.Name ?? message.ChannelId);
        }

        private T Deserialize<T>(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);

            using (var reader = new StreamReader(stream, Encoding.UTF8, true, 4096, true))
            using (var jsonReader = new JsonTextReader(reader))
            {
                return _serializer.Deserialize<T>(jsonReader);
            }
        }
    }
}
