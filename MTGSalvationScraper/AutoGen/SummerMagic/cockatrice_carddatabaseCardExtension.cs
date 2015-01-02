#region Windows Form Designer generated code
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using cockatrice_carddatabase = MTGSalvationScraper.AutoGen.SummerMagic.cockatrice_carddatabase;
using cockatrice_carddatabaseCard = MTGSalvationScraper.AutoGen.SummerMagic.cockatrice_carddatabaseCard;
using cockatrice_carddatabaseCardSet = MTGSalvationScraper.AutoGen.SummerMagic.cockatrice_carddatabaseCardSet;
using cockatrice_carddatabaseSet = MTGSalvationScraper.AutoGen.SummerMagic.cockatrice_carddatabaseSet;
namespace MTGSalvationScraper.AutoGen.SummerMagic
{
    public partial class cockatrice_carddatabaseCard
    {
        private const char colorlessCardCharacter = 'X';
        private static readonly Regex digitsOnly = new Regex(@"[^\d]");
        private static readonly char[] _colorCharacters = { 'U', 'W', 'G', 'R', 'B' };
        public cockatrice_carddatabaseCard() { }
        public cockatrice_carddatabaseCard(IEnumerable<cockatrice_carddatabaseCardSet> cardSets,
            CardElement sourceCardElement)
        {
            ciptSpecified = false;
            set = cardSets.ToArray();
            text = HtmlStringToXmlString(sourceCardElement.OracleText);
            type = HtmlStringToXmlString(sourceCardElement.Type);
            manacost = sourceCardElement.ManaCost;
            name = HtmlStringToXmlString(sourceCardElement.CardName);

            ColorFromCardElement(sourceCardElement);
            TableRowFromCardElement(sourceCardElement);
            StatsFromCardElement(sourceCardElement);
        }
        private static string HtmlStringToXmlString(string htmlText)
        {
            if (string.IsNullOrWhiteSpace(htmlText)) return htmlText;
            const string htmlApostrophe = "&amp;#x27;";
            const string htmlApostropheSpecial = "&#x27;";
            const string xmlApostrophe = "'";
            const string htmlArrow = "â€”";
            const string xmlArrow = "—";
            const string htmlWideSpace = "  ";
            const string xmlWideSpace = " ";
            const string htmlAE = "Ã†";
            const string xmlAE = "AE";
            htmlText = htmlText.Trim();
            htmlText = htmlText.Replace(htmlApostrophe, xmlApostrophe);
            htmlText = htmlText.Replace(htmlApostropheSpecial, xmlApostrophe);
            htmlText = htmlText.Replace(htmlWideSpace, xmlWideSpace);
            htmlText = htmlText.Replace(htmlAE, xmlAE);
            htmlText = Regex.Replace(htmlText, @"( |\r?\n)\1+", "\n");
            htmlText = Regex.Replace(htmlText, @"(?:(?:\r?\n)+ +){2,}", @"\n");
            htmlText = Regex.Replace(htmlText, @"\s\s+", "\n");
            htmlText = htmlText.Replace(htmlArrow, xmlArrow);
            return htmlText;
        }
        private void ColorFromCardElement(CardElement sourceCardElement)
        {
            string[] cardColorNodes = (from manaCostCharacter in sourceCardElement.ManaCost.ToUpper().ToCharArray()
                                       where _colorCharacters.Contains(manaCostCharacter)
                                       select manaCostCharacter.ToString(CultureInfo.InvariantCulture))
                                       .GroupBy(character => character)
                                       .Select(character => character.First())
                                       .ToArray();
            if (!cardColorNodes.Any())
            {
                string colorNode = colorlessCardCharacter.ToString();
                cardColorNodes = new[] { colorNode };
            }
            color = cardColorNodes.ToArray();
        }

        private void StatsFromCardElement(CardElement sourceCardElement)
        {
            const string planeswalkerTypeName = "planeswalker";
            bool isPlaneswalker = sourceCardElement
                .Type
                .ToLower()
                .Contains(planeswalkerTypeName);
            if (isPlaneswalker)
            {
                string loyaltyString = digitsOnly.Replace(sourceCardElement.Stats, string.Empty);
                byte parsedLoyalty;
                loyaltySpecified = byte.TryParse(loyaltyString, out parsedLoyalty);
                loyalty = parsedLoyalty;
            }
            else
            {
                pt = sourceCardElement.Stats;
            }
        }

        private void TableRowFromCardElement(CardElement sourceCardElement)
        {
            const string landTypeName = "land";
            const byte landRow = 0;
            const byte nonLandRow = 1;
            tablerow = sourceCardElement
                .Type
                .ToLower()
                .Contains(landTypeName)
                ? landRow
                : nonLandRow;
        }
    }
} 
#endregion