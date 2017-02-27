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
    }
}
