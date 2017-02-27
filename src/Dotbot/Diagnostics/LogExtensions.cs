namespace Dotbot.Diagnostics
{
    public static class LogExtensions
    {
        public static void Fatal(this ILog log, string format, params object[] args)
        {
            log?.Write(LogLevel.Fatal, format, args);
        }

        public static void Error(this ILog log, string format, params object[] args)
        {
            log?.Write(LogLevel.Error, format, args);
        }

        public static void Warning(this ILog log, string format, params object[] args)
        {
            log?.Write(LogLevel.Warning, format, args);
        }

        public static void Information(this ILog log, string format, params object[] args)
        {
            log?.Write(LogLevel.Information, format, args);
        }

        public static void Debug(this ILog log, string format, params object[] args)
        {
            log?.Write(LogLevel.Debug, format, args);
        }

        public static void Verbose(this ILog log, string format, params object[] args)
        {
            log?.Write(LogLevel.Verbose, format, args);
        }
    }
}