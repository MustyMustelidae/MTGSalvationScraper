using System;

namespace MTGSalvationScraper
{
    public class CardDataProviderException : ScraperException
    {
        public CardDataProviderException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}