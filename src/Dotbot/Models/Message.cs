namespace Dotbot.Models
{
    public sealed class Message
    {
        public User From { get; }

        public User Recipient { get; }

        public string Text { get; }

        public Message(User from, User recipient, string text)
        {
            From = from;
            Recipient = recipient;
            Text = text;
        }
    }
}