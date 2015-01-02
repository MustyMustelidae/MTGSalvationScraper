namespace MTGSalvationScraper
{
    interface ILogger
    {
        LogLevel LoggedLevels { get; set; }
        void Log(LogLevel logLevel, object logObject, params object[] formatObjects);
    }
}