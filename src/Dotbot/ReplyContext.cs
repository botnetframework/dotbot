using Dotbot.Models;

namespace Dotbot
{
    public sealed class ReplyContext : RoomContext
    {
        public Message Message { get; }

        public ReplyContext(IBroker broker, User bot, Room room, Message message)
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
