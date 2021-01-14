using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace RProducer
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
