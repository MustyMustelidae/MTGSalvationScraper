using System;
using System.Net;
using MTGSalvationScraper.Properties;

namespace MTGSalvationScraper
{
    class MtgSalvationCardDataProvider : ICardDataProvider
    {
        private static string SpoilerSite
        {
            get
            {
                return Resources.MTGSalvationJourneySpoilerUrl;
            }
        }

        public string GetUnparsedData()
        {
            using (var webClient = new WebClient())
            {
                try
                {
                    return webClient.DownloadString(SpoilerSite);
                }
                catch (Exception ex)
                {
                    throw new CardDataProviderException("Could not download data from MTGSalvation", ex);
                }
            }
        }
    }
}