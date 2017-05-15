using System;
using System.Threading;
using System.Threading.Tasks;
using Dotbot.Diagnostics;

namespace Dotbot.Internal
{
    internal sealed class TaskWrapper
    {
        private readonly ILog _log;
        private CancellationTokenSource _source;
        private readonly IWorker _worker;

        public string FriendlyName => _worker.FriendlyName;
        public Task Task { get; private set; }

        public TaskWrapper(IWorker worker, ILog log)
        {
            _worker = worker;
            _log = log;
        }

        public Task Start(CancellationTokenSource source)
        {
            _log.Verbose("Starting {0}...", FriendlyName.ToLowerInvariant());
            _source = source;

            Task = Task.Factory.StartNew(() =>
            {
                try
                {
                    _log.Debug("{0} started.", FriendlyName);
                    if (!_worker.Run(_source.Token).GetAwaiter().GetResult())
                    {
                        _source.Cancel();
                    }
                }
                catch (OperationCanceledException)
                {
                    _log.Verbose("{0} aborted.", FriendlyName);
                }
                catch (Exception ex)
                {
                    _log.Error("{0}: {1} ({2})", FriendlyName, ex.Message, ex.GetType().FullName);
                    _source.Cancel();
                }
                finally
                {
                    _log.Debug("{0} stopped.", FriendlyName);
                }
            }, TaskCreationOptions.LongRunning);

            return Task;
        }
    }
}