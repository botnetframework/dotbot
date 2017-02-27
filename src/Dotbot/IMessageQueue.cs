namespace Dotbot
{
    public interface IMessageQueue
    {
        void Enqueue(IEvent @event);
    }
}