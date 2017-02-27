namespace Dotbot.Diagnostics
{
    public class AdapterLog : ILog
    {
        private readonly string _name;
        private readonly ILog _log;

        public AdapterLog(string name, ILog log)
        {
            _name = name;
            _log = log;
        }

        public void Write(LogLevel level, string format, params object[] args)
        {
            var a = new object[args.Length + 1];
            for (var i = 0; i < a.Length - 1; i++)
            {
                a[i] = args[i];
            }
            a[args.Length] = _name;
            var f = string.Concat("[{", args.Length, "}] ", format);
            _log.Write(level, f, a);
        }
    }
}
