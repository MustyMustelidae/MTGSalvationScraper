using System;

namespace MTGSalvationScraper
{
    public class CardDataParserException : ScraperException
    {
        public CardDataParserException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}