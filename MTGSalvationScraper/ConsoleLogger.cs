using System;

namespace MTGSalvationScraper
{
    class ConsoleLogger : ILogger
    {
        public LogLevel LoggedLevels { get; set; }
        public void Log(LogLevel logLevel, object logObject,params object[] formatObjects)
        {
            if (LoggedLevels.HasFlag(logLevel))
            {
                Console.WriteLine(logObject.ToString(), formatObjects);
            }
        }
    }
}