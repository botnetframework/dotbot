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
    }
}
