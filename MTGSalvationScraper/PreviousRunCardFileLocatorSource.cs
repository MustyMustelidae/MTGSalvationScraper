using MTGSalvationScraper.Properties;

namespace MTGSalvationScraper
{
    class PreviousRunCardFileLocatorSource : ICardFileLocatorSource
    {
        public PreviousRunCardFileLocatorSource(Settings settings)
        {
            SourceDirectory = settings.LastCardFileLocation;
        }

        public string SourceName { get { return "location set on previous run"; } }
        public string SourceDirectory { get; private set; }
        public static void SetPreviousRunLocation(Settings settings,string previousLocation)
        {
            settings.LastCardFileLocation = previousLocation;
        }
        public static void SetPreviousRunLocation(string previousLocation)
        {
            SetPreviousRunLocation(Settings.Default, previousLocation);
        }
    }
}