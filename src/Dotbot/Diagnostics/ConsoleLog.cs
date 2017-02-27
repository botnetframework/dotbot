using System;
using System.Collections.Generic;

namespace Dotbot.Diagnostics
{
    internal sealed class ConsoleLog : ILog
    {
        private readonly object _lock;
        private readonly Dictionary<LogLevel, Tuple<ConsoleColor, string>> _lookup;

        public ConsoleLog()
        {
            _lock = new object();
            _lookup = new Dictionary<LogLevel, Tuple<ConsoleColor, string>>
            {
                { LogLevel.Fatal, Tuple.Create(ConsoleColor.Red, "FAT") },
                { LogLevel.Error, Tuple.Create(ConsoleColor.Red, "ERR") },
                { LogLevel.Warning, Tuple.Create(ConsoleColor.Yellow, "WRN") },
                { LogLevel.Information, Tuple.Create(ConsoleColor.White, "INF") },
                { LogLevel.Debug, Tuple.Create(ConsoleColor.Gray, "DBG") },
                { LogLevel.Verbose, Tuple.Create(ConsoleColor.DarkGray, "VRB") },
            };
        }

        public void Write(LogLevel level, string format, params object[] args)
        {
            lock (_lock)
            {
                try
                {
                    Console.ForegroundColor = _lookup[level].Item1;
                    Console.Write("[{0} ", DateTime.Now.ToString("HH:mm:ss"));
                    Console.Write(_lookup[level].Item2);
                    Console.Write("] ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(format, args);
                }
                finally
                {
                    Console.ResetColor();
                }
            }
        }
    }
}