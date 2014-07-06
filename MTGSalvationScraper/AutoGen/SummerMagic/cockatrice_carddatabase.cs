using System.Xml.Serialization;
using cockatrice_carddatabase = MTGSalvationScraper.AutoGen.SummerMagic.cockatrice_carddatabase;
using cockatrice_carddatabaseCard = MTGSalvationScraper.AutoGen.SummerMagic.cockatrice_carddatabaseCard;
using cockatrice_carddatabaseCardSet = MTGSalvationScraper.AutoGen.SummerMagic.cockatrice_carddatabaseCardSet;
using cockatrice_carddatabaseSet = MTGSalvationScraper.AutoGen.SummerMagic.cockatrice_carddatabaseSet;
namespace MTGSalvationScraper.AutoGen.SummerMagic
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public partial class cockatrice_carddatabase
    {

        private cockatrice_carddatabaseSet[] setsField;

        private cockatrice_carddatabaseCard[] cardsField;

        private byte versionField;

        /// <remarks/>
        [XmlArrayItem("set", IsNullable = false)]
        public cockatrice_carddatabaseSet[] sets
        {
            get
            {
                return this.setsField;
            }
            set
            {
                this.setsField = value;
            }
        }

        /// <remarks/>
        [XmlArrayItem("card", IsNullable = false)]
        public cockatrice_carddatabaseCard[] cards
        {
            get
            {
                return this.cardsField;
            }
            set
            {
                this.cardsField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public byte version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }
    }
}