using System;
using System.IO;
using Dotbot.Example.Parts;
using Dotbot.Gitter;
using Dotbot.Slack;
using Microsoft.Extensions.Configuration;
using Serilog.Events;

namespace Dotbot.Example
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Read the configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                .AddEnvironmentVariables()
                .Build();

            // Build the robot.
            var robot = new RobotBuilder()
                .UseGitter(configuration["Gitter:Token"])
                .UseSlack(configuration["Slack:Token"])
                .AddPart<PingPart>()
                .UseSerilogConsole(LogEventLevel.Verbose)
                .Build();

            // Start the robot.
            robot.Start();

            // Setup cancellation.
            Console.CancelKeyPress += (s, e) =>
            {
                e.Cancel = true;
                robot.Stop();
            };

            // Wait for termination.
            robot.Join();
        }
    }
}
