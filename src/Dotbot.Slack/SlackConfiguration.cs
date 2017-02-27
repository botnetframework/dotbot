namespace Dotbot.Slack
{
    internal sealed class SlackConfiguration
    {
        public string Token { get; }

        public SlackConfiguration(string token)
        {
            Token = token;
        }
    }
}
