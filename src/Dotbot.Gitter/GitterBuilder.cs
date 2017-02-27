using System;
using Microsoft.Extensions.DependencyInjection;

namespace Dotbot.Gitter
{
    public static class GitterBuilder
    {
        public static RobotBuilder UseGitter(this RobotBuilder builder, string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            // Configuration
            builder.Services.AddSingleton(new GitterConfiguration() { Token = token });

            // Adapter
            builder.Services.AddSingleton<GitterAdapter>();
            builder.Services.AddSingleton<IAdapter>(s => s.GetService<GitterAdapter>());
            builder.Services.AddSingleton<IWorker>(s => s.GetService<GitterAdapter>());

            // Broker
            builder.Services.AddSingleton<GitterBroker>();
            builder.Services.AddSingleton<IBroker>(s => s.GetService<GitterBroker>());

            return builder;
        }
    }
}
