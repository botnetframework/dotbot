using Dotbot.Domain;

namespace Dotbot.Contexts
{
    public sealed class MessageContext : RoomContext
    {
        public Message Message { get; }

        public MessageContext(IBroker broker, User bot, Room room, Message message)
            : base(broker, bot, room)
        {
            Message = message;
        }

        public void Reply(string message)
        {
            Broker.Reply(Room, Message.User, message).Wait();
        }
    }
}
