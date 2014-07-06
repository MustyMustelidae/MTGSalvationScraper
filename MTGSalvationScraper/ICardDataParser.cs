using System.Collections.Generic;

namespace MTGSalvationScraper
{
    public interface ICardDataParser
    {
        ///<exception cref="CardDataParserException"></exception>
        IEnumerable<CardElement> ParseElements(string siteData);
    }
}