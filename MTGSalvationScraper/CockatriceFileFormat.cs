using System.Collections.Generic;
using MTGSalvationScraper.Properties;

namespace MTGSalvationScraper
{
    public class CockatriceFileFormat : IFileFormat
    {
        public CockatriceFileFormat(Settings settings, string fileNotFoundPrompt, Parameters parameters,
            bool isSummerMagic)
        {
            var locatorSources = new List<ICardFileLocatorSource>();

            locatorSources.Add(new ParameterBasedCardFileLocatorSource(parameters, settings.CardFileParameterIndex));
            locatorSources.Add(new WorkingDirCardFileLocatorSource());

            if (isSummerMagic)
            {
                locatorSources.Add(new SummerMagicCardFileLocator(settings));
            }

            locatorSources.Add(new CockatriceRegistrySettingsCardFileLocator(settings));
            locatorSources.Add(new PreviousRunCardFileLocatorSource(settings));
            locatorSources.Add(new ConsoleInputCardFileLocatorSource(fileNotFoundPrompt));
            FileLocatorSources = locatorSources;
            FileModifier = isSummerMagic
                ? (ICardFileModifier) new SummerMagicCardFileModifier()
                : (ICardFileModifier) new CockatriceCardFileModifier();
        }

        public CockatriceFileFormat(Settings settings, Parameters parameters, bool summerMagicMode)
            : this(settings, Resources.FileSearchFailedPrompt, parameters, summerMagicMode)
        {
        }

        public ICardFileModifier FileModifier { get; private set; }
        public IEnumerable<ICardFileLocatorSource> FileLocatorSources { get; private set; }
    }
}