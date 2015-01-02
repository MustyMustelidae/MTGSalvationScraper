using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Cache;

namespace MTGSalvationScraper
{
    class MtgSalvationCardDataProvider : ICardDataProvider
    {
        private readonly IMtgSalvationCardDataParser _parser;
        private readonly string _spoilerSite;
        public MtgSalvationCardDataProvider(IMtgSalvationCardDataParser parser, string spoilerSite)
        {
            _parser = parser;
            _spoilerSite = spoilerSite;
        }

        private string GetUnparsedData()
        {
            using (var webClient = new WebClient())
            {
                webClient.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.Default);
                try
                {
                    return webClient.DownloadString(_spoilerSite);
                }
                catch (Exception ex)
                {
                    var exceptionString = string.Format("Could not download data from MTGSalvation ({0})",_spoilerSite);
                    throw new CardDataProviderException(exceptionString, ex);
                }
            }
        }

        public List<CardElement> GetCardElements()
        {
            var unparsedData = GetUnparsedData();
            return _parser.ParseElements(unparsedData)
                .ToList();
        }
    }
}