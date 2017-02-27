using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dotbot.Gitter.Models;
using Dotbot.Models;
using Newtonsoft.Json;

namespace Dotbot.Gitter
{
    internal sealed class GitterBroker : IBroker
    {
        private readonly HttpClient _client;

        public GitterBroker(GitterConfiguration configuration)
        {
            _client = new HttpClient { Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite) };
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", configuration.Token);
        }

        public async Task<IEnumerable<Room>> GetRooms()
        {
            var result = await Get<IEnumerable<GitterRoom>>("https://api.gitter.im/v1/rooms").ConfigureAwait(false);
            return result.Select(r => r.CreateRoom());
        }

        public async Task<User> GetCurrentUser()
        {
            var result = await Get<IEnumerable<GitterUser>>("https://api.gitter.im/v1/user").ConfigureAwait(false);
            var user = result.SingleOrDefault();
            return user != null
                ? new User()
                {
                    Id = user.Id,
                    Username = user.Username,
                    DisplayName = user.DisplayName
                }
                : null;
        }

        public async Task Broadcast(Room room, string text)
        {
            var message = new GitterMessage { Text = text };
            await Post($"https://api.gitter.im/v1/rooms/{room.Id}/chatMessages", message);
        }

        public async Task Reply(Room room, User fromUser, string text)
        {
            var message = new Message { Text = $"@{fromUser.Username}: {text}" };
            await Post($"https://api.gitter.im/v1/rooms/{room.Id}/chatMessages", message);
        }

        private async Task<T> Get<T>(string url)
        {
            var response = await _client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException($"Received HTTP status code {(int)response.StatusCode} when contacting {url}.");
            }
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }

        public async Task Post<T>(string url, T message)
        {
            var json = JsonConvert.SerializeObject(message, Formatting.None, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            });
            await _client.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
        }
    }
}
