namespace MTGSalvationScraper
{
    public static class CardRarityExt
    {
        public static CardRarity FromString( string rarityString)
        {
            CardRarity returnRarity;
            const string mythicRarity = "mythic";
            const string rareRarity = "rare";
            const string uncommonRarity = "uncommon";
            const string commonRarity = "common";
            if (rarityString.Contains(mythicRarity))
            {
                returnRarity = CardRarity.Mythic;

            }
            else if (rarityString.Contains(rareRarity))
            {
                returnRarity = CardRarity.Rare;
            }
            else if (rarityString.Contains(uncommonRarity))
            {
                returnRarity = CardRarity.Uncommon;
            }
            else if (rarityString.Contains(commonRarity))
            {
                returnRarity = CardRarity.Common;
            }
            else
            {
                returnRarity = CardRarity.Undef;
            }
            return returnRarity;
        }
        public static void Parse(this CardRarity rarity, string rarityString, out CardRarity returnRarity)
        {
            const string mythicRarity = "mythic";
            const string rareRarity = "rare";
            const string uncommonRarity = "uncommon";
            const string commonRarity = "common";
            if (rarityString.Contains(mythicRarity))
            {
                returnRarity = CardRarity.Mythic;

            }
            else if (rarityString.Contains(rareRarity))
            {
                returnRarity = CardRarity.Rare;
            }
            else if (rarityString.Contains(uncommonRarity))
            {
                returnRarity = CardRarity.Uncommon;
            }
            else if (rarityString.Contains(commonRarity))
            {
                returnRarity = CardRarity.Common;
            }
            else
            {
                returnRarity = CardRarity.Undef;
            }
        }
    }
}