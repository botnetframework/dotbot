using System.Threading;
using System.Threading.Tasks;

namespace Dotbot.Internal
{
    internal sealed class MessageRouter : IWorker
    {
        private readonly MessageQueue _messageQueue;
        private readonly EventDispatcher _dispatcher;

        public string FriendlyName => "Message router";

        public MessageRouter(MessageQueue messageQueue, EventDispatcher dispatcher) 
        {
            _messageQueue = messageQueue;
            _dispatcher = dispatcher;
        }

        public Task<bool> Run(CancellationToken token)
        {
            while (true)
            {
                // Wait for any messages to arrive.
                var message = _messageQueue.Dequeue(token);
                message?.Accept(_dispatcher);

                if (token.IsCancellationRequested)
                {
                    break;
                }
            }

            return Task.FromResult(true);
        }
    }
}
