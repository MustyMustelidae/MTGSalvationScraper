using Microsoft.Win32;
using MTGSalvationScraper.Properties;

namespace MTGSalvationScraper
{
    class CockatriceRegistrySettingsCardFileLocator : ICardFileLocatorSource
    {
        public CockatriceRegistrySettingsCardFileLocator(string registryKey, string registryValue)
        {
            const object defaultPath = null;
            SourceDirectory = Registry.GetValue(registryKey, registryValue, defaultPath) as string;
        }
        public CockatriceRegistrySettingsCardFileLocator(Settings settings)
            : this(settings.CockatriceRegistryKey, settings.CockatriceRegistryValue)
        {
        }

        public string SourceName { get { return "cockatrice registry settings"; } }
        public string SourceDirectory { get; private set; }
    }
}