using System;
using Microsoft.Extensions.DependencyInjection;

namespace Dotbot.Slack
{
    public static class SlackBuilder
    {
        public static RobotBuilder UseSlack(this RobotBuilder builder, string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            // Configuration
            builder.Services.AddSingleton(new SlackConfiguration(token));

            // Adapter
            builder.Services.AddSingleton<SlackAdapter>();
            builder.Services.AddSingleton<IAdapter>(s => s.GetService<SlackAdapter>());
            builder.Services.AddSingleton<IWorker>(s => s.GetService<SlackAdapter>());

            // Engine
            builder.Services.AddSingleton<SlackEngine>();

            // Broker
            builder.Services.AddSingleton<SlackBroker>();
            builder.Services.AddSingleton<IBroker>(s => s.GetService<SlackBroker>());

            return builder;
        }
    }
}
