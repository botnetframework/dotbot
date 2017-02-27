# Dotbot [![Build status](https://ci.appveyor.com/api/projects/status/u48j10fdk3jlf804?svg=true)](https://ci.appveyor.com/project/patriksvensson/dotbot)

Dotbot is an extensible bot framework built using .NET Core.

## Example

### 1. Add the bot entry point

Create a new .NET Core console application and reference `Dotbot.Slack` and/or `Dotbot.Gitter`.  
Add this code in your `Program.cs` and press F5.

```csharp
using System;
using Dotbot;
using Dotbot.Gitter;
using Dotbot.Slack;

namespace Dotbot.Example
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Build the robot.
            var robot = new RobotBuilder()
                .UseGitter("MY_GITTER_TOKEN")
                .UseSlack("MY_SLACK_TOKEN")
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
```

As you notice, it doesn't do anything yet. That's because there isn't any 
parts associated with our bot.

## 2. Write and add a new part

Start by creating a new class called `PingPart`.

```csharp
public sealed class PingPart : CommandPart
{
    public override string Help => "Replies with pong.";

    public PingPart()
        : base(new[] { "ping" })
    {
    }

    protected override void HandleCommand(ReplyContext context, string[] args)
    {
        // Broadcast to everyone in the channel.
        context.Broadcast("Pong!");

        // Or reply to the user.
        context.Reply("Here's your pong!");
    }
}
```

Now you have to wire up the new part.  
Locate your `RobotBuilder` in `Program.cs` and add `.AddPart<PingPart>()` to it.

```csharp
    // Build the robot.
    var robot = new RobotBuilder()
        .UseGitter("MY_GITTER_TOKEN")
        .UseSlack("MY_SLACK_TOKEN")
        .AddPart<PingPart>() // The new line
        .Build();
```

## 3. Try it out

Invite the bot into a Gitter or Slack channel and try it out.

```
[patrik]
@mybot ping

[mybot]
Pong!

[mybot]
@patrik Here's your pong!
```