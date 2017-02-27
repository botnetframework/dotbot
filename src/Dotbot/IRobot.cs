using System.Threading;

namespace Dotbot
{
    public interface IRobot
    {
        WaitHandle Stopped { get; }

        void Start();
        void Stop();

        void Join();
    }
}
