using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Grow.Server.Model.Helpers
{
    public interface ILogger
    {
        void Log(string message, LogLevel logLevel = LogLevel.Debug);
        void LogError(string message, Exception e = null);
    }

    public enum LogLevel
    {
        Debug,
        Information,
        Warning,
        Error
    }

    public static class LoggerExtensions
    {
        public static void LogDebug(this ILogger logger, string message) => logger.Log(message, LogLevel.Debug);
        public static void LogInformation(this ILogger logger, string message) => logger.Log(message, LogLevel.Information);
        public static void LogWarning(this ILogger logger, string message) => logger.Log(message, LogLevel.Warning);
        public static void LogError(this ILogger logger, Exception e) => logger.LogError(e.Message, e);
    }
}
