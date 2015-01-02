using System;

namespace MTGSalvationScraper
{
    public class ScraperException : Exception
    {
        public ScraperException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public ScraperException(string message) 
            : base(message)
        {
        }
    }
}