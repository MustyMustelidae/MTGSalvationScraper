﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using MTGSalvationScraper.AutoGen.SummerMagic;
using MTGSalvationScraper.Properties;

namespace MTGSalvationScraper
{
    internal class SummerMagicCardFileModifier : ICardFileModifier
    {
        private const char colorlessCardCharacter = 'X';
        private const string ReminderText = "Restore the backup after the set has been spoiled! -Ninja";
        private static readonly char[] _colorCharacters = {'U', 'W', 'G', 'R', 'B'};


        public string AugmentCards(string setName, string longSetName, string xmlData, IEnumerable<CardElement> newCards)
        {
            var serializer = new XmlSerializer(typeof (cockatrice_carddatabase));
            var sourceDb = serializer.Deserialize(new StringReader(xmlData)) as cockatrice_carddatabase;
            Debug.Assert(sourceDb != null, "Not a valid summer magic DB");
            cockatrice_carddatabase newDb = CreateNewDbFromOld(setName, longSetName, sourceDb);
            AddSet(setName, longSetName, newDb);
            AddCard(setName, newDb,
                new CardElement
                {
                    CardName = "!A reminder!",
                    ImageUrl = "",
                    ManaCost = "U",
                    OracleText = ReminderText,
                    Rarity = CardRarity.Mythic,
                    Stats = "",
                    Type = "Reminder Text"
                });

            foreach (CardElement cardElement in newCards)
            {
                AddCard(setName, newDb, cardElement);
            }
            var textWriter = new Utf8StringWriter();
            serializer.Serialize(textWriter, newDb);
            textWriter.Flush();

            return textWriter.ToString();
        }

        private static cockatrice_carddatabase CreateNewDbFromOld(string setName, string longSetName,
            cockatrice_carddatabase sourceDb)
        {
            var newDb = new cockatrice_carddatabase
            {
                cards = sourceDb.cards.Where(card => !card.set
                    .Any(set => set.Value.Equals(longSetName) && !set.Value.Equals(setName)))
                    .ToArray(),
                sets = sourceDb.sets,
                version = sourceDb.version
            };
            return newDb;
        }


        public void AddSet(string setName, string longSetName, cockatrice_carddatabase cardDatabase)
        {
            if (cardDatabase.sets.Any(set => set.name == setName))
            {
                return;
            }
            cockatrice_carddatabaseSet[] dbSets = cardDatabase.sets;
            int numSets = dbSets.Length;
            Array.Resize(ref dbSets, numSets + 1);
            dbSets[numSets] = new cockatrice_carddatabaseSet {longname = longSetName, name = setName};
            cardDatabase.sets = dbSets;
        }

        public void AddCard(string setName, cockatrice_carddatabase cardDatabase, CardElement newCardElement)
        {
            IEnumerable<cockatrice_carddatabaseCard> nonDuplicateCards =
                cardDatabase.cards.Where(
                    card =>
                        !(string.Equals(card.name, newCardElement.CardName) &&
                          card.set.Any(set => string.Equals(set.Value, setName))))
                    .Where(card => !card.set.Any(set => set.Value.ToLower().Contains("basal")));

            var cardList = new List<cockatrice_carddatabaseCard>(nonDuplicateCards);

            uint newCardMuid = 0;
            foreach (
                var sourceCard in
                    cardDatabase.cards.Where(card => string.Equals(card.name, newCardElement.CardName)))
            {
                foreach (var muid in sourceCard.set
                    .Select(set => set.muId)
                    .Where(muid => muid != 0))
                {
                    newCardMuid = muid;
                    break;
                }
            }
            var newCardSet = new cockatrice_carddatabaseCardSet { Value = setName, picURL = newCardElement.ImageUrl, muId = newCardMuid };
          
            var cardSetList = new List<cockatrice_carddatabaseCardSet> {newCardSet};

            foreach (
                var currentSets in
                    cardDatabase.cards.Where(card => string.Equals(card.name, newCardElement.CardName)).Select(card => card.set))
            {
                cardSetList.AddRange(currentSets.Where(set => !string.Equals(set.Value, setName)));
            }
           
            cardList.RemoveAll(card => string.Equals(card.name, newCardElement.CardName));

            var newDatabaseCard = new cockatrice_carddatabaseCard(cardSetList, newCardElement);
            string oldImageUrl = newCardElement.ImageUrl;

            if (string.IsNullOrWhiteSpace(newCardElement.ImageUrl))
            {
                foreach (
                    cockatrice_carddatabaseCard sourceCard in
                        cardDatabase.cards.Where(card => string.Equals(card.name, newCardElement.CardName)))
                {
                    foreach (string picUrl in sourceCard.set
                        .Select(set => set.picURL)
                        .Where(picUrl => !string.IsNullOrEmpty(picUrl)))
                    {
                        newCardElement.ImageUrl = picUrl;
                        break;
                    }
                }
            }

            if (!oldImageUrl.Equals(newCardElement.ImageUrl))
            {
                newDatabaseCard = new cockatrice_carddatabaseCard(cardSetList, newCardElement);
            }
            cardList.Add(newDatabaseCard);
            cardDatabase.cards = cardList.ToArray();
        }

        public sealed class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding
            {
                get { return Encoding.UTF8; }
            }
        }
    }
}