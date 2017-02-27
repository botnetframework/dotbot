using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dotbot.Contexts;
using Dotbot.Diagnostics;
using Dotbot.Events;
using Dotbot.Utilities;

namespace Dotbot.Internal
{
    internal sealed class EventDispatcher : IEventDispatcher
    {
        private readonly ILog _log;
        private readonly List<RobotPart> _parts;
        private readonly object _lock;

        public EventDispatcher(IEnumerable<RobotPart> parts, ILog log)
        {
            _log = log;
            _parts = new List<RobotPart>(parts);
            _lock = new object();
        }

        public void Visit(MessageEvent @event)
        {
            lock (_lock)
            {
                var context = new MessageContext(@event.Broker, @event.Bot, @event.Room, @event.Message);
                foreach (var part in _parts)
                {
                    if (part.Handle(context))
                    {
                        _log.Verbose("Handled message {0} in {1}.", @event.Message.Text, @event.Room.Name);
                        break;
                    }
                }
            }
        }

        internal void Visit(HelpEvent @event)
        {
            lock (_lock)
            {
                var context = new RoomContext(@event.Broker, @event.Bot, @event.Room);

                var commands = new Dictionary<string, string>();
                foreach (var command in _parts.OfType<CommandPart>())
                {
                    var name = command.Aliases.FirstOrDefault() ?? string.Empty;
                    if (!string.IsNullOrWhiteSpace(name.Trim()) && !string.IsNullOrWhiteSpace(command.Help))
                    {
                        commands[name] = command.Help.Trim();
                    }
                }

                var builder = new StringBuilder();
                var index = 1;
                foreach (var command in commands)
                {
                    builder.AppendLine($"{index}. `{command.Key}` {command.Value}");
                    index++;
                }

                context.Broadcast(builder.ToString());
            }
        }
    }
}
