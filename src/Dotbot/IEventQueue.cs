namespace Dotbot
{
    public interface IEventQueue
    {
        void Enqueue(IEvent @event);
    }
}