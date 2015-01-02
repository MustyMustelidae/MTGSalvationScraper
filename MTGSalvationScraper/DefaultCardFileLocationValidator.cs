using System.IO;

namespace MTGSalvationScraper
{
    class DefaultCardFileLocationValidator : ICardFileLocationValidator
    {
        public bool IsValidCardFileLocation(string cardFilePath)
        {
            return !string.IsNullOrWhiteSpace(cardFilePath) && File.Exists(cardFilePath);
        }
    }
}