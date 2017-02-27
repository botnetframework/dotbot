using System;
using Dotbot.Diagnostics;
using Serilog;
using Serilog.Context;

namespace Dotbot.Example.Diagnostics
{
    public sealed class SerilogLogAdapter : ILog
    {
        private readonly ILogger _logger;

        public SerilogLogAdapter(ILogger logger)
        {
            _logger = logger;
        }

        public void Write(LogLevel level, string format, params object[] args)
        {
            switch (level)
            {
                case LogLevel.Fatal:
                    _logger.Fatal(format, args);
                    break;
                case LogLevel.Error:
                    _logger.Error(format, args);
                    break;
                case LogLevel.Warning:
                    _logger.Warning(format, args);
                    break;
                case LogLevel.Information:
                    _logger.Information(format, args);
                    break;
                case LogLevel.Verbose:
                    _logger.Verbose(format, args);
                    break;
                case LogLevel.Debug:
                    _logger.Debug(format, args);
                    break;
                default:
                    throw new InvalidOperationException("Undefined log level.");
            }
        }

        public IDisposable Push(string key, object value)
        {
            return LogContext.PushProperty(key, value);
        }
    }
}