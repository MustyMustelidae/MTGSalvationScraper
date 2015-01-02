namespace MTGSalvationScraper
{
    public interface IMagicSet
    {
        ICardDataProvider SetDataProvider { get; }

        string SetLongName { get; }
        string SetShortName { get; }
    }

    class KtKMagicSet : IMagicSet
    {
        public ICardDataProvider SetDataProvider { get; private set; }
        public string SetLongName { get; private set; }
        public string SetShortName { get; private set; }
    }
}