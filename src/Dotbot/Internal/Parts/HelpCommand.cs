using Dotbot.Models.Events;

namespace Dotbot.Internal.Parts
{
    internal sealed class HelpCommand : CommandPart
    {
        private readonly EventQueue _eventQueue;

        public override string Help => "Displays all of the help commands that I know about.";

        public HelpCommand(EventQueue eventQueue)
            : base(new [] { "help" })
        {
            _eventQueue = eventQueue;
        }

        protected override void HandleCommand(ReplyContext context, string[] args)
        {
            _eventQueue.Enqueue(new HelpEvent(context));
        }
    }
}
