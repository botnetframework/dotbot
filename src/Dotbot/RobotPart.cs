using Dotbot.Contexts;

namespace Dotbot
{
    public abstract class RobotPart
    {
        public virtual void Initialize()
        {
            
        }

        public abstract bool Handle(MessageContext context);
    }
}
