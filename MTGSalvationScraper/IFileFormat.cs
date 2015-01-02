using System.Collections.Generic;

namespace MTGSalvationScraper
{
    public interface IFileFormat
    {
        ICardFileModifier FileModifier { get; }
        IEnumerable<ICardFileLocatorSource> FileLocatorSources { get; }
    }

}