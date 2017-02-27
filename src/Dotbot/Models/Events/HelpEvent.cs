using Dotbot.Internal;

namespace Dotbot.Models.Events
{
    internal sealed class HelpEvent : IEvent
    {
        public User Bot { get; }
        public Room Room { get; }
        public IBroker Broker { get; }

        public HelpEvent(RoomContext context)
        {
            Bot = context.Bot;
            Room = context.Room;
            Broker = context.Broker;
        }

        public void Accept(EventDispatcher dispatcher)
        {
            dispatcher?.Visit(this);
        }

        public void Accept(IEventDispatcher dispatcher)
        {
            Accept(dispatcher as EventDispatcher);
        }
    }
}
