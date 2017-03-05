namespace Dotbot.Models.Events
{
    public sealed class MessageEvent : IEvent
    {
        public User Bot { get; }

        public Room Room { get; }

        public Message Message { get; }

        public IBroker Broker { get; }

        public MessageEvent(User bot, Room room, Message message, IBroker broker)
        {
            Bot = bot;
            Room = room;
            Message = message;
            Broker = broker;
        }
    }
}
