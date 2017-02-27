using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Dotbot.Models;
using Dotbot.Slack.Models;
using Newtonsoft.Json;

namespace Dotbot.Slack
{
    internal sealed class SlackBroker : IBroker
    {
        private readonly SlackConfiguration _configuration;
        private readonly HttpClient _client;

        private const string Url = "https://slack.com/api/chat.postMessage";

        public SlackBroker(SlackConfiguration configuration)
        {
            _configuration = configuration;
            _client = new HttpClient();
        }

        public async Task<SlackHandshake> Handshake()
        {
            var uri = $"https://slack.com/api/rtm.start?token={_configuration.Token}&no_unreads=true&simple_latest=true";
            var json = await _client.GetStringAsync(uri);
            return JsonConvert.DeserializeObject<SlackHandshake>(json);
        }

        public async Task Reply(Room room, User fromUser, string text)
        {
            await Post(room, $"@{fromUser.Username}: {text}");
        }

        public async Task Broadcast(Room room, string text)
        {
            await Post(room, text);
        }

        public async Task Post(Room room, string text)
        {
            var data = new Dictionary<string, string>
            {
                {"token", _configuration.Token},
                {"as_user", "True"},
                {"channel", room.Id},
                {"text", text}
            };

            await _client.PostAsync(Url, new FormUrlEncodedContent(data));
        }
    }
}
