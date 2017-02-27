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
            // Brain
            services.AddSingleton<Brain>();
            services.AddSingleton<IBrainProvider, InMemoryBrain>();

            // Built in parts
            services.AddSingleton<RobotPart, HelpCommand>();

            // Event queue
            var inbox = new EventQueue();
            services.AddSingleton<IEventQueue>(inbox);
            services.AddSingleton(inbox);

            // Event routing and dispatching
            services.AddSingleton<IWorker, EventRouter>();
            services.AddSingleton<EventDispatcher>();
            services.AddSingleton<IEventDispatcher>(provider => provider.GetService<EventDispatcher>());

            // Logging
            services.AddSingleton<ILog, ConsoleLog>();

            // Robot
            services.AddSingleton<IRobot, Robot>();
        }
    }
}
