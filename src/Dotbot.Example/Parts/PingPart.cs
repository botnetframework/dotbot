namespace Dotbot.Example.Parts
{
    public sealed class PingPart : CommandPart
    {
        public override string Help => "Replies with pong.";

        public PingPart()
            : base(new[] { "ping" })
        {
        }

        protected override void HandleCommand(ReplyContext context, string[] args)
        {
            context.Broadcast("Pong!");
        }
    }
}
