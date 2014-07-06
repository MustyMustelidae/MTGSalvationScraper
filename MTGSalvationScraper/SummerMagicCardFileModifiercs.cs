using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using MTGSalvationScraper.AutoGen.SummerMagic;
using cockatrice_carddatabase = MTGSalvationScraper.AutoGen.SummerMagic.cockatrice_carddatabase;
using cockatrice_carddatabaseCard = MTGSalvationScraper.AutoGen.SummerMagic.cockatrice_carddatabaseCard;
using cockatrice_carddatabaseCardSet = MTGSalvationScraper.AutoGen.SummerMagic.cockatrice_carddatabaseCardSet;
using cockatrice_carddatabaseSet = MTGSalvationScraper.AutoGen.SummerMagic.cockatrice_carddatabaseSet;

namespace MTGSalvationScraper
{
    internal class SummerMagicCardFileModifier : ICardFileModifier
    {
        private const char colorlessCardCharacter = 'X';
        private static readonly char[] _colorCharacters = {'U', 'W', 'G', 'R', 'B'};

        public string AugmentCards(string setName, string longSetName, string xmlData, IEnumerable<CardElement> newCards)
        {
            var serializer = new XmlSerializer(typeof (cockatrice_carddatabase));
            var sourceDb = serializer.Deserialize(new StringReader(xmlData)) as MTGSalvationScraper.AutoGen.SummerMagic.cockatrice_carddatabase;
            Debug.Assert(sourceDb != null, "Not a valid summer magic DB");

            var newDb = new MTGSalvationScraper.AutoGen.SummerMagic.cockatrice_carddatabase
            {
                cards = sourceDb.cards,
                sets = sourceDb.sets,
                version = sourceDb.version
            };
            AddSet(setName, longSetName, newDb);
            foreach (CardElement cardElement in newCards)
            {
                AddCard(setName, newDb, cardElement);
            }
            var textWriter = new Utf8StringWriter();
            serializer.Serialize(textWriter, newDb);
            textWriter.Flush();
            return textWriter.ToString();
        }
        public sealed class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding { get { return Encoding.UTF8; } }
        }
        private void AddSet(string setName, string longSetName, cockatrice_carddatabase cardDatabase)
        {
            if (cardDatabase.sets.Any(set => set.name == setName))
            {
                return;
            }
            cockatrice_carddatabaseSet[] dbSets = cardDatabase.sets;
            int numSets = dbSets.Length;
            Array.Resize(ref dbSets, numSets + 1);
            dbSets[numSets] = new cockatrice_carddatabaseSet { longname = longSetName, name = setName };
            cardDatabase.sets = dbSets;
        }

        private void AddCard(string setName, cockatrice_carddatabase cardDatabase, CardElement newCardElement)
        {
            IEnumerable<cockatrice_carddatabaseCard> nonDuplicateCards =
                cardDatabase.cards.Where(
                    card =>
                        !(string.Equals(card.name, newCardElement.CardName) &&
                          card.set.Any(set => string.Equals(set.Value, setName))));

            var cardList = new List<cockatrice_carddatabaseCard>(nonDuplicateCards);

            var cardSetList = new List<cockatrice_carddatabaseCardSet>();
            foreach (
                var currentSets in cardList.Where(card => string.Equals(card.name, newCardElement.CardName)).Select(card => card.set))
            {
                cardSetList.AddRange(currentSets.Where(set => !string.Equals(set.Value, setName)));
            }
            cardSetList.Add(new cockatrice_carddatabaseCardSet { Value = setName });
            cardList.RemoveAll(card => string.Equals(card.name, newCardElement.CardName));
            cardList.Add(new cockatrice_carddatabaseCard(cardSetList, newCardElement));
            cardDatabase.cards = cardList.ToArray();

        }
    }
}