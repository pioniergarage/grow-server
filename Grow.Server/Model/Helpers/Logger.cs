using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grow.Server.Model.Helpers
{
    internal class Logger : ILogger
    {
        public AppSettings AppSettings { get; }

        public Logger(IOptions<AppSettings> appSettings)
        {
            AppSettings = appSettings.Value;
        }

        public void Log(string message, LogLevel logLevel = LogLevel.Debug)
        {
            // Clean
            message = message.Trim();

            // Console
            Console.WriteLine(message);

            // Tracing
            switch (logLevel)
            {
                case LogLevel.Error:
                    Trace.TraceError(message);
                    break;
                case LogLevel.Warning:
                    Trace.TraceWarning(message);
                    break;
                case LogLevel.Information:
                    Trace.TraceInformation(message);
                    break;
            }
        }

        public void LogError(string message, Exception e = null)
        {
            if (e == null)
            {
                Log(message, LogLevel.Error);
                return;
            }

            var sb = new StringBuilder();
            var indent = 0;

            // Append cascade of inner exception messages
            sb.AppendLine(message);
            while (e != null)
            {
                sb.Append(new string(' ', indent));
                sb.AppendLine(e.Message);
                e = e.InnerException;
                indent += 2;
            }

            Log(sb.ToString(), LogLevel.Error);
        }
    }
}
