namespace Dotbot.Diagnostics
{
    public class LogNameDecorator : ILog
    {
        private readonly string _name;
        private readonly ILog _log;

        public LogNameDecorator(string name, ILog log)
        {
            _name = name;
            _log = log;
        }

        public void Write(LogLevel level, string format, params object[] args)
        {
            // TODO: Optimize this
            var arguments = new object[args.Length + 1];
            for (var index = 0; index < arguments.Length - 1; index++)
            {
                arguments[index] = args[index];
            }
            arguments[args.Length] = _name;
            format = string.Concat("[{", arguments.Length - 1, "}] ", format);
            _log.Write(level, format, arguments);
        }
    }
}
