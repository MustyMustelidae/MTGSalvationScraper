using System;

namespace MTGSalvationScraper
{
    [Flags]
    enum LogLevel
    {
        Info = 1,
        Warning = 2,
        Error = 4,
        Debug = 8
    }
}