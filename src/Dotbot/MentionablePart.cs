namespace Dotbot
{
    public abstract class MentionablePart : RobotPart
    {
        public sealed override bool Handle(ReplyContext context)
        {
            if (context.Bot.Equals(context.Message.Recipient))
            {
                return HandleMention(context, context.Message.Text);
            }
            return false;
        }

        protected abstract bool HandleMention(ReplyContext context, string message);
    }
}
