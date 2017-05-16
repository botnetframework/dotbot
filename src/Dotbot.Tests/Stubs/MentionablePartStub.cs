namespace Dotbot.Tests.Stubs
{
    public sealed class MentionablePartStub : MentionablePart
    {
        protected override bool HandleMention(ReplyContext context, string message)
        {
            return true;
        }
    }
}
