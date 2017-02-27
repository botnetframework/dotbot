namespace Dotbot.Diagnostics
{
    internal sealed class NullLog : ILog
    {
        public void Write(LogLevel level, string format, params object[] args)
        {
        }
    }
}
