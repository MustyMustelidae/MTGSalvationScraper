using System.Collections.Generic;

namespace MTGSalvationScraper
{
    public interface ICardDataProvider
    {
        /// <exception cref="CardDataProviderException"></exception>
        List<CardElement> GetCardElements();

    }
}