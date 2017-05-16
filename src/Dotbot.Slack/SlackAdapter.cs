using System;
using System.IO;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Dotbot.Slack
{
    internal sealed class SlackAdapter : IAdapter, IWorker
    {
        private readonly SlackEngine _engine;
        private readonly SlackBroker _broker;

        public string FriendlyName => "Slack";
        public IBroker Broker => _broker;

        public SlackAdapter(SlackEngine engine, SlackBroker broker)
        {
            _engine = engine;
            _broker = broker;
        }

        public async Task<bool> Run(CancellationToken token)
        {
            // Initialize Slack and get the bot user.
            var (bot, url) = await _engine.Handshake();

            // Connect
            using (var ws = new ClientWebSocket())
            {
                // Connect to the websocket endpoint.
                await ws.ConnectAsync(url, token);

                // Create the buffer.
                var buffer = new ArraySegment<byte>(new byte[4096]);

                // Start reading messages.
                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        using (var stream = new MemoryStream())
                        {
                            while (true)
                            {
                                // Receive data.
                                var received = await ws.ReceiveAsync(buffer, token);

                                // Write it to the result stream.
                                await stream.WriteAsync(buffer.Array, buffer.Offset, received.Count, token);

                                // End of message?
                                if (received.EndOfMessage)
                                {
                                    break;
                                }
                            }

                            _engine.ProcessEvent(stream, bot);
                        }
                    }
                    catch (WebSocketException e) when (e.WebSocketErrorCode == WebSocketError.InvalidState)
                    {
                        // We get this exception if the task was cancelled.
                    }
                }
            }

            return true;
        }
    }
}
