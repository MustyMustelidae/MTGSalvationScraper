using System;

namespace MTGSalvationScraper
{
    class ConsoleInputCardFileLocatorSource : ICardFileLocatorSource
    {
        private readonly string _inputPrompt;
        private bool _inputRequested;
        private string _searchDirectory;

        public ConsoleInputCardFileLocatorSource(string inputPrompt)
        {
            _inputPrompt = inputPrompt;
        }

        public string GetInput()
        {
            Console.WriteLine(_inputPrompt);
            return Console.ReadLine();
        }

        public string SourceName { get { return "console input"; } }

        public string SourceDirectory
        {
            get
            {
                if (_inputRequested)
                {
                    return _searchDirectory;
                }
                _inputRequested = true;
                return (_searchDirectory = GetInput());
            }
        }
    }
}