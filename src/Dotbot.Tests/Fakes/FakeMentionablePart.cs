namespace Dotbot.Tests.Fakes
{
    public sealed class FakeMentionablePart : MentionablePart
    {
        protected override bool HandleMention(ReplyContext context, string message)
        {
            return true;
        }
    }
}
