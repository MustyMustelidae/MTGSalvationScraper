using System.Collections.Generic;

namespace MTGSalvationScraper
{
    public interface IMtgSalvationCardDataParser
    {
        IEnumerable<CardElement> ParseElements(string unparsedData);
    }
}