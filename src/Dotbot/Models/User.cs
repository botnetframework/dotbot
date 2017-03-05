namespace Dotbot.Models
{
    public sealed class User
    {
        public string Id { get; }

        public string Username { get; }

        public string DisplayName { get; }

        public User(string id, string userName, string displayName)
        {
            Id = id;
            Username = userName;
            DisplayName = displayName;
        }
    }
}
