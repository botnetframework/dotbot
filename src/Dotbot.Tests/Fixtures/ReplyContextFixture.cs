using Dotbot.Models;

namespace Dotbot.Tests.Fixtures
{
    public sealed class ReplyContextFixture
    {
        public User Bot { get; set; }
        public User Human { get; set; }
        public Room Room { get; set; }

        public ReplyContextFixture()
        {
            Bot = new User("1", "bot", "Botty McBotface");
            Human = new User("2", "human", "Human McHumanface");
            Room = new Room("1", "room");
        }

        public ReplyContext Create(User from, User to)
        {
            return new ReplyContext(null, Bot, Room, new Message(from, to, ""));
        }
    }
}
