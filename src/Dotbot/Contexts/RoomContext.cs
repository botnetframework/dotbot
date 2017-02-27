using Dotbot.Domain;

namespace Dotbot.Contexts
{
    public class RoomContext : RobotContext
    {
        public Room Room { get; }

        public RoomContext(IBroker broker, User bot, Room room)
            : base(broker, bot)
        {
            Room = room;
        }

        public void Broadcast(string message)
        {
            Broker.Broadcast(Room, message).Wait();
        }
    }
}