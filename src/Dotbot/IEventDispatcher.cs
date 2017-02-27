namespace Dotbot
{
    public interface IEventDispatcher
    {
        void Dispatch(IEvent @event);
    }
}