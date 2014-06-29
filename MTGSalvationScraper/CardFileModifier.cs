using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text.RegularExpressions;
using System.Xml;
using MTGSalvationScraper.Properties;

namespace MTGSalvationScraper
{
    class CardFileModifier : ICardFileModifier
    {
        public string AugmentCards(string setName,string longSetName,string xmlData, IEnumerable<CardElement> newCards)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlData);
            const string rootElementName = "cockatrice_carddatabase";
            
           
            var rootElement = xmlDoc[rootElementName];
            Debug.Assert(rootElement != null, "rootElement != null");
            AppendSetInfo(setName, longSetName,xmlDoc, rootElement);
            AppendCardInfo(longSetName, newCards, rootElement, xmlDoc);
          //  var escapedText = HtmlStringToXmlString(xmlDoc.OuterXml);
            return xmlDoc.OuterXml;
        }

        private static readonly char[] _colorCharacters = {'U', 'W', 'G', 'R', 'B'};
        private const char colorlessCardCharacter = 'X';
        private static void AppendCardInfo(string setName, IEnumerable<CardElement> newCards, XmlElement rootElement, XmlDocument xmlDoc)
        {
            const string cardListElementName = "cards";
            var cardListElement = rootElement[cardListElementName];
            Debug.Assert(cardListElement != null, "cardListElement != null");
            var uri = cardListElement.BaseURI;

            const string cardElementName = "card";
            const string cardNameElementName = "name";
            const string setElementName = "set";
            const string imageUrlAttributeName = "picURL";
            const string manaCostElementName = "manacost";
            const string cardTypeElementName = "type";
            const string statsElementName = "pt";
            const string tablerowElementName = "tablerow";
            const string oracleTextElementName = "text";
            const string colorElementName = "color";
            
            foreach (var newCard in newCards)
            {

                var nameNode = xmlDoc.CreateNode(XmlNodeType.Element, cardNameElementName, uri);
                nameNode.InnerText = HtmlStringToXmlString(newCard.CardName);
                var setNode = xmlDoc.CreateNode(XmlNodeType.Element, setElementName, uri);
                
                var setElement = (XmlElement) setNode;
                setElement.InnerText = setName;
                setElement.SetAttribute(imageUrlAttributeName, newCard.ImageUrl);
                var manaCostNode = xmlDoc.CreateNode(XmlNodeType.Element, manaCostElementName, uri);
                manaCostNode.InnerText = newCard.ManaCost;
                var cardTypeNode = xmlDoc.CreateNode(XmlNodeType.Element, cardTypeElementName, uri);
                cardTypeNode.InnerText = newCard.Type;
                var statsTypeNode = xmlDoc.CreateNode(XmlNodeType.Element, statsElementName, uri);
                statsTypeNode.InnerText = newCard.Stats;
                var oracleTextNode = xmlDoc.CreateNode(XmlNodeType.Element, oracleTextElementName, uri);
                
                oracleTextNode.InnerText = HtmlStringToXmlString(newCard.OracleText);
                var cardColorNodes = (from manaCostCharacter in newCard.ManaCost.ToUpper().ToCharArray() 
                                      where _colorCharacters.Contains(manaCostCharacter) 
                                      select AddCardColor(xmlDoc, colorElementName, uri, manaCostCharacter))
                                      .ToList();
                if (cardColorNodes.Count < 1)
                {
                  var colorNode =    AddCardColor(xmlDoc, colorElementName, uri, colorlessCardCharacter);
                    cardColorNodes.Add(colorNode);
                }
                const string landTypeName = "Land";
                const int landRow = 0;
                const int nonLandRow = 1;
                var tableRow = newCard.Type.Contains(landTypeName) ? landRow : nonLandRow;
                var tableRowNode = xmlDoc.CreateNode(XmlNodeType.Element, tablerowElementName, uri);
                tableRowNode.InnerText = tableRow.ToString(CultureInfo.InvariantCulture);
                var cardNode = xmlDoc.CreateNode(XmlNodeType.Element, cardElementName, uri);
                cardNode.AppendChild(nameNode);
                cardNode.AppendChild(setNode);
                cardNode.AppendChild(manaCostNode);
                cardNode.AppendChild(cardTypeNode);
                foreach (var cardColorNode in cardColorNodes)
                {

                    cardNode.AppendChild(cardColorNode);
                }
                if (!string.IsNullOrEmpty(newCard.Stats))
                {
                    cardNode.AppendChild(statsTypeNode);
                }
                cardNode.AppendChild(tableRowNode);
                cardNode.AppendChild(oracleTextNode);
                
                var isOldCard = true;
                foreach (var oldCardNode in cardListElement.ChildNodes.OfType<XmlNode>()
                            .Where(oldCardNode => oldCardNode.Name.Equals(cardElementName)))
                {
                    isOldCard = oldCardNode.ChildNodes.OfType<XmlNode>()
                        .Where(oldCardNodeInfo => oldCardNodeInfo.Name.Equals(cardNameElementName))
                        .Any(oldCardNodeInfo => oldCardNodeInfo.InnerText.Equals(newCard.CardName));
                    if (Settings.Default.AddReprints)
                    {
                        isOldCard &= oldCardNode.ChildNodes.OfType<XmlNode>()
                        .Where(oldCardNodeInfo => oldCardNodeInfo.Name.Equals(setElementName))
                        .Any(oldCardNodeInfo => oldCardNodeInfo.InnerText.Equals(setName) );
                    }
                }
                if (isOldCard) continue;
                cardListElement.AppendChild(cardNode);
            }
        }

        private static XmlNode AddCardColor(XmlDocument xmlDoc, string colorElementName, string uri, char manaCostCharacter)
        {
            var colorNode = xmlDoc.CreateNode(XmlNodeType.Element, colorElementName, uri);
            colorNode.InnerText = manaCostCharacter.ToString(CultureInfo.InvariantCulture);
            return colorNode;
        }

        private static string HtmlStringToXmlString(string htmlText)
        {
            const string htmlApostrophe = "&amp;#x27;";
            const string xmlApostrophe = "'";
            const string htmlArrow = "â€”";
            const string xmlArrow = "—";
            const string htmlWideSpace = "  ";
            const string xmlWideSpace = " ";
            htmlText = htmlText.Trim();
            htmlText = htmlText.Replace(htmlApostrophe, xmlApostrophe);
            htmlText = htmlText.Replace(htmlWideSpace, xmlWideSpace);

            htmlText = Regex.Replace(htmlText, @"( |\r?\n)\1+", "\n");
            htmlText = Regex.Replace(htmlText, @"(?:(?:\r?\n)+ +){2,}", @"\n");
            htmlText = Regex.Replace(htmlText, @"\s\s+", "\n");
            htmlText = htmlText.Replace(htmlArrow, xmlArrow);
            return htmlText;
        }

        private static void AppendSetInfo(string setName, string longSetName, XmlDocument xmlDocument,XmlElement rootElement)
        {
            const string setListElementName = "sets";
            const string setNameElementName = "name";
            const string setLongNameElementName = "longname";
            const string setElementName = "set";
            var setListElement = rootElement[setListElementName];
            var uri = xmlDocument.BaseURI;
            Debug.Assert(setListElement != null, "setListElement != null");
            
            if (setListElement[setName] != null) return;

            var setNode = xmlDocument.CreateNode(XmlNodeType.Element, setElementName, uri);

            var nameElement = xmlDocument.CreateNode(XmlNodeType.Element, setNameElementName, uri);
            nameElement.InnerText = setName;

            var longNameElement = xmlDocument.CreateNode(XmlNodeType.Element, setLongNameElementName, uri);
            longNameElement.InnerText = longSetName;
            
            setNode.AppendChild(nameElement);
            setNode.AppendChild(longNameElement);

            setListElement.AppendChild(setNode);
        }
    }
}