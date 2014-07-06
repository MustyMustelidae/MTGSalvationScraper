using System.Linq;

namespace MTGSalvationScraper
{
    class CardFileGenerator
    {
        public readonly ICardDataProvider CardDataProvider;
        public readonly ICardDataParser CardDataParser;
        public readonly ICardFileModifier CardFileModifier;
        public CardFileGenerator(ICardDataProvider cardDataProvider, ICardDataParser cardDataParser,ICardFileModifier cardFileModifier)
        {
            CardDataProvider = cardDataProvider;
            CardDataParser = cardDataParser;
            CardFileModifier = cardFileModifier;
        }
        /// <exception cref="CardFileGeneratorException"></exception>
        /// <exception cref="CardDataParserException"></exception>
        /// <exception cref="CardDataProviderException"></exception>
        public string GenerateCardFile(string oldFile,string setName,string longSetName,out int newCards)
        {
            
                var unparsedData = CardDataProvider.GetUnparsedData();
           
            var parsedCards = CardDataParser.ParseElements(unparsedData);
            var cardElements = parsedCards as CardElement[] ?? parsedCards.ToArray();
            newCards = cardElements.Count();


            var newXmlFileString = CardFileModifier.AugmentCards(setName, longSetName, oldFile, cardElements);

            return newXmlFileString;
        }
    }
}
