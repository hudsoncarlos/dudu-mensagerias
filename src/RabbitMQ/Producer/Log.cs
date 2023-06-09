using Microsoft.Extensions.Logging;

namespace Producer
{
    public class Log
    {
        private static ILoggerFactory loggerFactory { get; set; }

        public static ILogger CriarLog()
        {
            loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddFilter("LoggingConsoleApp.Program", LogLevel.Debug);
            });

            return loggerFactory.CreateLogger<Program>();
        }
    }
}
