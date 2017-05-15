using System.Collections.Concurrent;
using System.Threading;

namespace Dotbot.Gitter.Collections
{
    internal sealed class MessageEventQueue : IEventQueue
    {
        private readonly BlockingCollection<IEvent> _queue;

        public MessageEventQueue()
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
