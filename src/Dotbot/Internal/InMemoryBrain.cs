using System.Collections.Concurrent;
using Dotbot.Diagnostics;
using Newtonsoft.Json;

namespace Dotbot.Internal
{
    internal sealed class InMemoryBrain : IBrainProvider
    {
        private readonly ILog _log;
        private readonly ConcurrentDictionary<string, string> _data;

        public InMemoryBrain(ILog log)
        {
            _log = log;
            _data = new ConcurrentDictionary<string, string>();
        }

        public void Connect()
        {
            _log.Information("Connected in-memory brain.");
        }

        public void Disconnect()
        {
            _log.Information("Disconnected in-memory brain.");
        }

        public string Get(string key)
        {
            return _data.TryGetValue(key, out string value) ? value : null;
        }

        public void Set(string key, string data)
        {
            var json = JsonConvert.SerializeObject(data);
            _data.AddOrUpdate(key, json, (k, v) => json);
        }
    }
}
