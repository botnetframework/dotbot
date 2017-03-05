namespace Dotbot.Models
{
    public sealed class Message
    {
        public User From { get; }
        public string Text { get; }

        public Message(User from, string text)
        {
            From = from;
            Text = text;
        }
    }
}