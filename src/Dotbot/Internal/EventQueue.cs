using System.Collections.Concurrent;
using System.Threading;

namespace Dotbot.Internal
{
    internal sealed class EventQueue : IMessageQueue
    {
        private readonly BlockingCollection<IEvent> _queue;

        public EventQueue()
        {
            _queue = new BlockingCollection<IEvent>(new ConcurrentQueue<IEvent>());
        }

        public void Enqueue(IEvent @event)
        {
            if (!_queue.IsAddingCompleted)
            {
                _queue.Add(@event);
            }
        }

        public IEvent Dequeue(CancellationToken token)
        {
            IEvent entry;
            return _queue.TryTake(out entry, Timeout.Infinite, token) ? entry : null;
        }
    }
}
