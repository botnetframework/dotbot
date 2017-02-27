using Dotbot.Models;

namespace Dotbot
{
    public abstract class RobotContext
    {
        public IBroker Broker { get; }
        public User Bot { get; }
        
        protected RobotContext(IBroker broker, User bot)
        {
            Broker = broker;
            Bot = bot;
        }
    }
}