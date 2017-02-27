using System.Threading;
using System.Threading.Tasks;

namespace Dotbot.Internal
{
    internal sealed class EventRouter : IWorker
    {
        private readonly EventQueue _eventQueue;
        private readonly EventDispatcher _dispatcher;

        public string FriendlyName => "Message router";

        public EventRouter(EventQueue eventQueue, EventDispatcher dispatcher) 
        {
            _eventQueue = eventQueue;
            _dispatcher = dispatcher;
        }

        public Task<bool> Run(CancellationToken token)
        {
            while (true)
            {
                // Wait for any messages to arrive.
                var @event = _eventQueue.Dequeue(token);

                // Dispatch the message.
                _dispatcher.Dispatch(@event);

                if (token.IsCancellationRequested)
                {
                    break;
                }
            }

            return Task.FromResult(true);
        }
    }
}
