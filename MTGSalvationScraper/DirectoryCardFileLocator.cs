using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MTGSalvationScraper
{
    class DirectoryCardFileLocator : ICardFileLocator
    {
        private readonly IEnumerable<string> _validNames;
        private readonly IEnumerable<string> _validExtensions;
        public DirectoryCardFileLocator(IEnumerable<string> validNames, IEnumerable<string> validExtensions)
        {
            if (validNames == null) throw new ArgumentNullException("validNames", "validNames cannot be null.");
            if (validExtensions == null) throw new ArgumentNullException("validExtensions", "validExtensions cannot be null.");

            _validNames = validNames;
            _validExtensions = validExtensions;
        }

        public bool TryFindCardFile(ICardFileLocatorSource source,out string cardFilePath)
        {
            if (source == null) throw new ArgumentNullException("source", "source cannot be null.");

            var searchDirectory = source.SourceDirectory; 
            
            if (string.IsNullOrWhiteSpace(searchDirectory) || !Directory.Exists(searchDirectory))
            {
                cardFilePath = null;
                return false;
            }

            

           

            if (File.Exists(searchDirectory))
            {
                cardFilePath = searchDirectory;
                return true;
            }


            cardFilePath = Directory.GetFiles(searchDirectory)
                .Where(IsValidFilePath)
                .FirstOrDefault();


            return true;
        }

        bool IsValidFilePath(string filePath)
        {
            return HasValidExtension(filePath) && HasValidFileName(filePath);
        }
        bool HasValidFileName(string filePath)
        {
            if (filePath == null) throw new ArgumentNullException("filePath", "filePath cannot be null.");

            return File.Exists(filePath)
                   && _validNames.Any(names => names == Path.GetFileNameWithoutExtension(filePath));
        }
        bool HasValidExtension(string filePath)
        {
            if (filePath == null) throw new ArgumentNullException("filePath", "filePath cannot be null.");
            
            return Path.HasExtension(filePath) 
                   && _validExtensions.Any(extension => extension == Path.GetExtension(filePath));
        }

    }
}