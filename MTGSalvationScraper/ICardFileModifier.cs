using System.Collections.Generic;

namespace MTGSalvationScraper
{
    public interface ICardFileModifier
    {
        string AugmentCards(string setName, string longSetName, string xmlData, IEnumerable<CardElement> newCards);
    }
}