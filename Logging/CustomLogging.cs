using Serilog.Core;
using Serilog.Events;
using System.Runtime.CompilerServices;

namespace HIDAeroService.Logging
{
    public class CustomLogging : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (logEvent.Properties.TryGetValue("SourceContext", out var value))
            {
                var fullName = value.ToString().Trim('"'); // remove quotes
                var shortName = fullName.Split('.').Last(); // take last part
                logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("ShortSourceContext", shortName));
            }
        }

        public static void LogCri<T>(ILogger<T> logger,string message, [CallerMemberName] string methodName = "")
        {
            logger.LogInformation("[{Method}] {Message}", methodName, message);
        }

        public static void LogErr<T>(ILogger<T> logger, string message, [CallerMemberName] string methodName = "")
        {
            logger.LogInformation("[{Method}] {Message}", methodName, message);
        }

        public static void LogInfo<T>(ILogger<T> logger, string message, [CallerMemberName] string methodName = "")
        {
            logger.LogInformation("[{Method}] {Message}", methodName, message);
        }

        public static void LogWarn<T>(ILogger<T> logger, string message, [CallerMemberName] string methodName = "")
        {
            logger.LogInformation("[{Method}] {Message}", methodName, message);
        }
    }
}
