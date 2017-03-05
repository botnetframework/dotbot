using Dotbot.Models;

namespace Dotbot.Tests.Fixtures
{
    public sealed class ReplyContextFixture
    {
        public User Bot { get; }

        public User Human { get; }

        public Room Room { get; }

        public string Text { get; set; }

        public ReplyContextFixture(string botUsername = null, string botDisplayName = null,
            string text = null)
        {
            Bot = new User
            {
                Id = "1",
                Username = botUsername ?? "bot",
                DisplayName = botDisplayName ?? "Botty McBotface"
            };

            Human = new User
            {
                Id = "2",
                Username = "human",
                DisplayName = "Human McHumanface"
            };

            Room = new Room
            {
                Id = "1",
                Name = "room"
            };

            Text = text ?? "Hello World!";
        }

        public ReplyContext Create()
        {
            return new ReplyContext(null, Bot, Room, new Message
            {
                Text = Text,
                User = Human
            });
        }
    }
}
