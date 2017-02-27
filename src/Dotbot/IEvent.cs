namespace Dotbot
{
    public interface IEvent
    {
        void Accept(IEventDispatcher dispatcher);
    }
}
