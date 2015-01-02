using System.Collections.Generic;
using MTGSalvationScraper.Properties;

namespace MTGSalvationScraper
{
    interface ICardFileLocator
    {
        bool TryFindCardFile(ICardFileLocatorSource source,out string cardFilePath);
    }

   
}