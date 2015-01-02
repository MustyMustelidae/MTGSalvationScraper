#if false
using System;
using System.Linq;
using MTGSalvationScraper.Properties;

namespace MTGSalvationScraper
{
    class CardFileGenerator
    {
        public readonly ICardDataProvider CardDataProvider;
        public readonly ICardDataParser CardDataParser;
        public readonly ICardFileModifier CardFileModifier;
        public CardFileGenerator(ICardDataProvider cardDataProvider, ICardDataParser cardDataParser, ICardFileModifier cardFileModifier)
        {
            CardDataProvider = cardDataProvider;
            CardDataParser = cardDataParser;
            CardFileModifier = cardFileModifier;
        }
        /// <exception cref="CardFileGeneratorException"></exception>
        /// <exception cref="CardDataParserException"></exception>
        /// <exception cref="CardDataProviderException"></exception>
        public string GenerateCardFile(string oldFile, string setName, string longSetName, out int newCards)
        {
            Console.WriteLine(Resources.FetchingDataPrompt);
            var unparsedData = CardDataProvider.GetUnparsedData();
            Console.WriteLine(Resources.CardsParsingPrompt);
            var parsedCards = CardDataParser.ParseElements(unparsedData);
            var cardElements = parsedCards as CardElement[] ?? parsedCards.ToArray();
            newCards = cardElements.Count();

            Console.WriteLine(Resources.CardsParsedPrompt, newCards);
            Console.WriteLine(Resources.GeneratingCardsPrompt);
            var newXmlFileString = CardFileModifier.AugmentCards(setName, longSetName, oldFile, cardElements);
            Console.WriteLine(Resources.GeneratedCardsPrompt);
            return newXmlFileString;
        }
    }
}

#endif