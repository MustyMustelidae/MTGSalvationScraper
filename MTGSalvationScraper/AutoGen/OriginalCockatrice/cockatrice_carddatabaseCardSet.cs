#region Windows Form Designer generated code
using System.Xml.Serialization;
using cockatrice_carddatabase = MTGSalvationScraper.AutoGen.OriginalCockatrice.cockatrice_carddatabase;
using cockatrice_carddatabaseCard = MTGSalvationScraper.AutoGen.OriginalCockatrice.cockatrice_carddatabaseCard;
using cockatrice_carddatabaseCardSet = MTGSalvationScraper.AutoGen.OriginalCockatrice.cockatrice_carddatabaseCardSet;
using cockatrice_carddatabaseSet = MTGSalvationScraper.AutoGen.OriginalCockatrice.cockatrice_carddatabaseSet;
namespace MTGSalvationScraper.AutoGen.OriginalCockatrice
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlType(AnonymousType = true)]
    public partial class cockatrice_carddatabaseCardSet
    {

        private string picURLField;

        private string picURLHqField;

        private string picURLStField;

        private string valueField;

        /// <remarks/>
        [XmlAttribute()]
        public string picURL
        {
            get
            {
                return this.picURLField;
            }
            set
            {
                this.picURLField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string picURLHq
        {
            get
            {
                return this.picURLHqField;
            }
            set
            {
                this.picURLHqField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string picURLSt
        {
            get
            {
                return this.picURLStField;
            }
            set
            {
                this.picURLStField = value;
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
#endregion