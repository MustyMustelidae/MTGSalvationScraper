namespace MTGSalvationScraper
{
    class WorkingDirCardFileLocatorSource : ICardFileLocatorSource
    {
        public WorkingDirCardFileLocatorSource()
        {
            SourceDirectory = System.IO.Directory.GetCurrentDirectory();
        }

        public string SourceName { get { return "current directory"; } }
        public string SourceDirectory { get; private set; }
    }
}