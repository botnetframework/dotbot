using Dotbot.Domain;

namespace Dotbot.Events
{
    public sealed class MessageEvent : IEvent
    {
        public User Bot { get; set; }
        public Room Room { get; set; }
        public Message Message { get; set; }
        public IBroker Broker { get; }

        public MessageEvent(IBroker broker)
        {
            Broker = broker;
        }

        public void Accept(IEventDispatcher dispatcher)
        {
            dispatcher.Visit(this);
        }
    }
}
