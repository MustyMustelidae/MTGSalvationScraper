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
    public partial class cockatrice_carddatabaseCardSet
    {

        private uint muIdField;

        private string valueField;

        /// <remarks/>
        [XmlAttribute()]
        public uint muId
        {
            get
            {
                return this.muIdField;
            }
            set
            {
                this.muIdField = value;
            }
        }

        /// <remarks/>
        [XmlText()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }
}