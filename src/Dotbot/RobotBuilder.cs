using Dotbot.Diagnostics;
using Dotbot.Internal;
using Dotbot.Internal.Parts;
using Microsoft.Extensions.DependencyInjection;

namespace Dotbot
{
    public sealed class RobotBuilder
    {
        public IServiceCollection Services { get; }

        public RobotBuilder()
        {
            Services = new ServiceCollection();
            ConfigureDefaultRegistrations(Services);
        }

        public IRobot Build()
        {
            var provider = Services.BuildServiceProvider();
            return provider.GetRequiredService<IRobot>();
        }

        private static void ConfigureDefaultRegistrations(IServiceCollection services)
        {
            // Tamperer
            services.AddSingleton<Brain>();
            services.AddSingleton<IBrainProvider, InMemoryBrain>();

            // Parts
            services.AddSingleton<RobotPart, HelpCommand>();

            // Message queue
            var inbox = new MessageQueue();
            services.AddSingleton<IMessageQueue>(inbox);
            services.AddSingleton(inbox);

            // Logging
            services.AddSingleton<ILog, DefaultLog>();

            // Robot
            services.AddSingleton<IRobot, Robot>();
            services.AddSingleton<IWorker, MessageRouter>();
            services.AddSingleton<EventDispatcher>();
            services.AddSingleton<IEventDispatcher>(provider => provider.GetService<EventDispatcher>());
        }
    }
}
