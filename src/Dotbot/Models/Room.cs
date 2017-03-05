namespace Dotbot.Models
{
    public sealed class Room
    {
        public string Id { get; }
        public string Name { get; }

        public Room(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}