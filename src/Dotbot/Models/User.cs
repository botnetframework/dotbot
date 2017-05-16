using System;

namespace Dotbot.Models
{
    public sealed class User : IEquatable<User>
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

        public bool Equals(User other)
        {
            return other != null && Id.Equals(other.Id, StringComparison.Ordinal);
        }
    }
}
