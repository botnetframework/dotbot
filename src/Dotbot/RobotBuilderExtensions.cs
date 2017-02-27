using Dotbot.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Dotbot
{
    public static class RobotBuilderExtensions
    {
        public static RobotBuilder AddPart<TPart>(this RobotBuilder builder)
            where TPart : RobotPart
        {
            builder.Services.AddSingleton<RobotPart, TPart>();
            return builder;
        }

        public static RobotBuilder UseNoLog(this RobotBuilder builder)
        {
            builder.Services.AddSingleton<ILog, NullLog>();
            return builder;
        }
    }
}
