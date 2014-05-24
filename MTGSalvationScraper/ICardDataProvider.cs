namespace MTGSalvationScraper
{
    public interface ICardDataProvider
    {
        /// <exception cref="CardDataProviderException"></exception>
        string GetUnparsedData();
    }
}