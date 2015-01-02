using System;
using System.IO;
using MTGSalvationScraper.Properties;

namespace MTGSalvationScraper
{
    class SummerMagicCardFileLocator : ICardFileLocatorSource
    {

        public SummerMagicCardFileLocator(string relativeFilePath)
        {
            SourceDirectory = Path.Combine(AppDataRootDir, relativeFilePath);
        }
        public SummerMagicCardFileLocator(Settings programSettings)
            : this(programSettings.SummerMagicAppDataRelativeFilePath)
        {
        }

        private static readonly string AppDataRootDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        

        public string SourceName { get { return "default summer magic location"; } }
        public string SourceDirectory { get; private set; }
    }
}