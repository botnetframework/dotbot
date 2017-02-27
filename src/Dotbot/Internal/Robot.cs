using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dotbot.Diagnostics;

namespace Dotbot.Internal
{
    internal sealed class Robot : IRobot
    {
        private readonly List<IAdapter> _adapters;
        private readonly List<IWorker> _workers;
        private readonly List<RobotPart> _parts;
        private readonly IBrainProvider _brain;
        private readonly ILog _log;
        private readonly ManualResetEvent _stopped;
        private readonly List<Task> _tasks;

        private CancellationTokenSource _source;

        public IReadOnlyList<IAdapter> Adapters => _adapters;
        public WaitHandle Stopped => _stopped;

        public Robot(IEnumerable<IAdapter> adapters, IEnumerable<IWorker> workers, IEnumerable<RobotPart> parts, IBrainProvider brain, ILog log)
        {
            _adapters = new List<IAdapter>(adapters ?? Enumerable.Empty<IAdapter>());
            _workers = new List<IWorker>(workers ?? Enumerable.Empty<IWorker>());
            _parts = new List<RobotPart>(parts ?? Enumerable.Empty<RobotPart>());
            _brain = brain;
            _log = log;

            _tasks = new List<Task>();
            _stopped = new ManualResetEvent(true);
        }

        public void Start()
        {
            if (_stopped.WaitOne(0))
            {
                // We're not stopped.
                _stopped.Reset();

                // Create the cancellation token source.
                _source = new CancellationTokenSource();

                // Connect the brain.
                _brain.Connect();

                // Initialize all parts.
                foreach (var part in _parts)
                {
                    part.Initialize();
                }

                // Start all tasks.
                _tasks.Clear();
                foreach (var worker in _workers)
                {
                    _tasks.Add(new TaskWrapper(worker, _log).Start(_source));
                }

                // Configure the tasks.
                Task.WhenAll(_tasks)
                    .ContinueWith(task => _brain.Disconnect())
                    .ContinueWith(task => _stopped.Set());
            }
        }

        public void Stop()
        {
            if (!_source.IsCancellationRequested)
            {
                _source.Cancel();
            }
        }

        public void Join()
        {
            // Wait until we're stopped.
            _stopped.WaitOne();
        }
    }
}
