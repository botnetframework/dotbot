using System;
using System.Collections.Generic;
using System.Text;
using Bayeux.Diagnostics;

namespace Dotbot.Gitter.Diagnostics
{
    internal sealed class ConsoleLog : IBayeuxLogger
    {
        public void Write(BayeuxLogLevel level, string format, params object[] args)
        {
            Console.WriteLine(format, args);
        }
    }
}
