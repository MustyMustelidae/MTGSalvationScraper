using System;

namespace MTGSalvationScraper
{
    public class CardFileGeneratorException : ScraperException
    {
        public CardFileGeneratorException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
    public class ScraperException : Exception
    {
        public ScraperException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}