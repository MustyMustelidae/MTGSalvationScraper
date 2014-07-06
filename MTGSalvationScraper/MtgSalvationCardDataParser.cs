using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace MTGSalvationScraper
{
    internal class MtgSalvationCardDataParser : ICardDataParser
    {
        public IEnumerable<CardElement> ParseElements(string siteData)
        {
            try
            {
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(siteData);
                const string spoilerRootClassName = "t-spoiler-wrapper";
                var spoilerRootElements =
                    htmlDoc.DocumentNode.SearchChildNodesByAttributes(
                        attribute => attribute.Value.Equals(spoilerRootClassName));

                return spoilerRootElements.Select(ExtractNewCardFromNode);
            }
            catch (Exception ex)
            {
                throw new CardDataParserException("Parsing card data failed", ex);
            }
        }

        private static CardElement ExtractNewCardFromNode(HtmlNode spoilerRoot)
        {
            const string cardNameElementClassName = "t-spoiler";
            const string cardNameAttribute = "id";

            const string cardRarityPrefix = "t-spoiler-header";
            const string cardRarityAttributeName = "class";
            const string imageUrlClassName = "spoiler-image-link lightbox";
            const string imageUrlAttribute = "href";
            const string cardTypeElementClassName = "t-spoiler-type";
            const string cardStatElementClassName = "t-spoiler-stat";
            const string manaElementRootClassName = "t-spoiler-mana";
            const string manaElementPartialClassName = "mana-icon";
            const string oracleTextElementClassName = "t-spoiler-ability";

            var spoilerElement =
                spoilerRoot.SearchChildNodeByAttributes(
                    attribute => attribute.Value.Equals(cardNameElementClassName));

            var cardName = spoilerElement.GetAttributeValue(cardNameAttribute, null);

            var cardRarityString =
                spoilerRoot.SearchChildNodeByAttributes(attribute => attribute.Value.StartsWith(cardRarityPrefix))
                    .GetAttributeValue(cardRarityAttributeName, null);

            var cardRarity = CardRarityExt.FromString(cardRarityString);

            var imageUrlNode = spoilerRoot
                .SearchChildNodeByAttributes(attribute => attribute.Value.StartsWith(cardRarityPrefix))
                .Descendants()
                .FirstOrDefault(node => node.Attributes.Any(attribute => attribute.Value.Equals(imageUrlClassName)));

            var imageUrlString = (imageUrlNode != null)
                ? imageUrlNode.GetAttributeValue(imageUrlAttribute, null)
                : string.Empty;

            var cardType =
                spoilerRoot.SearchChildNodeByAttributes(
                    attribute => attribute.Value.Equals(cardTypeElementClassName)).InnerText;
            var cardStatElement = spoilerRoot.SearchChildNodeByAttributes(
                attribute => attribute.Value.Equals(cardStatElementClassName));
            var cardStats = (cardStatElement != null)
                ? cardStatElement.InnerText
                : string.Empty;

            var manaCost = string.Empty;
            var manaCostElements = spoilerRoot
                .SearchChildNodeByAttributes(attribute => attribute.Value.Equals(manaElementRootClassName))
                .SearchChildNodesByAttributes(attribute => attribute.Value.StartsWith(manaElementPartialClassName));
            if (manaCostElements != null)
            {
                var manaCostBuilder = new StringBuilder();
                foreach (var manaCostElement in manaCostElements)
                {
                    manaCostBuilder.Append(manaCostElement.InnerText);
                }
                manaCost = manaCostBuilder.ToString();
            }
            var oracleTextElement = spoilerRoot.SearchChildNodeByAttributes(
                attribute => attribute.Value.Equals(oracleTextElementClassName));
            var oracleText = string.Empty;
            if (oracleTextElement != null)
            {
                oracleText = oracleTextElement.InnerText;
            }
            var newCard = new CardElement
            {
                CardName = cardName,
                ImageUrl = imageUrlString,
                ManaCost = manaCost,
                OracleText = oracleText,
                Rarity = cardRarity,
                Stats = cardStats,
                Type = cardType
            };
            return newCard;
        }
    }
}