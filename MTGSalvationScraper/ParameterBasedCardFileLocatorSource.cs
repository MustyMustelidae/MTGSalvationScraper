namespace MTGSalvationScraper
{
    class ParameterBasedCardFileLocatorSource : ICardFileLocatorSource
    {
        public ParameterBasedCardFileLocatorSource(Parameters parameters, int parameterIndex)
        {
            string searchDirectory;
            parameters.TryGetUnnamedArgument(parameterIndex, out searchDirectory);
            SourceDirectory = searchDirectory;
        }

        public string SourceName { get { return "path passed in parameters"; } }
        public string SourceDirectory { get; private set; }
    }
}