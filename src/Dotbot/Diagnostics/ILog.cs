namespace Dotbot.Diagnostics
{
    public interface ILog
    {
        void Write(LogLevel level, string format, params object[] args);
    }
}
