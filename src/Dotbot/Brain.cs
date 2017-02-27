using Newtonsoft.Json;

namespace Dotbot
{
    public sealed class Brain
    {
        private readonly IBrainProvider _provider;

        public Brain(IBrainProvider provider)
        {
            _provider = provider;
        }

        public T Get<T>(string key)
            where T : class
        {
            var json = _provider.Get(key) ?? string.Empty;
            return JsonConvert.DeserializeObject<T>(json);
        }

        public void Set<T>(string key, T data)
            where T : class
        {
            var json = JsonConvert.SerializeObject(data);
            _provider.Set(key, json);
        }
    }
}