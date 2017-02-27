using Dotbot.Models.Events;

namespace Dotbot
{
    public interface IEventDispatcher
    {
        void Visit(MessageEvent @event);
    }
}