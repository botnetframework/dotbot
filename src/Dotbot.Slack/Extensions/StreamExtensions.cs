using System.IO;
using System.Text;
using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace Dotbot.Slack
{
    public static class StreamExtensions
    {
        private static readonly JsonSerializer Serializer;

        static StreamExtensions()
        {
            Serializer = new JsonSerializer();
        }

        public static T Deserialize<T>(this Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);

            using (var reader = new StreamReader(stream, Encoding.UTF8, true, 4096, true))
            using (var jsonReader = new JsonTextReader(reader))
            {
                return Serializer.Deserialize<T>(jsonReader);
            }
        }
    }
}
