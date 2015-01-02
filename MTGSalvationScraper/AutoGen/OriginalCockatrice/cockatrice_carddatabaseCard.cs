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
    public partial class cockatrice_carddatabaseCard
    {

        private string nameField;

        private cockatrice_carddatabaseCardSet[] setField;

        private string[] colorField;

        private string manacostField;

        private string typeField;

        private string ptField;

        private byte tablerowField;

        private string textField;

        private byte loyaltyField;

        private bool loyaltyFieldSpecified;

        private byte ciptField;

        private bool ciptFieldSpecified;

        /// <remarks/>
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [XmlElement("set")]
        public cockatrice_carddatabaseCardSet[] set
        {
            get
            {
                return this.setField;
            }
            set
            {
                this.setField = value;
            }
        }

        /// <remarks/>
        [XmlElement("color")]
        public string[] color
        {
            get
            {
                return this.colorField;
            }
            set
            {
                this.colorField = value;
            }
        }

        /// <remarks/>
        public string manacost
        {
            get
            {
                return this.manacostField;
            }
            set
            {
                this.manacostField = value;
            }
        }

        /// <remarks/>
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        public string pt
        {
            get
            {
                return this.ptField;
            }
            set
            {
                this.ptField = value;
            }
        }

        /// <remarks/>
        public byte tablerow
        {
            get
            {
                return this.tablerowField;
            }
            set
            {
                this.tablerowField = value;
            }
        }

        /// <remarks/>
        public string text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        public byte loyalty
        {
            get
            {
                return this.loyaltyField;
            }
            set
            {
                this.loyaltyField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool loyaltySpecified
        {
            get
            {
                return this.loyaltyFieldSpecified;
            }
            set
            {
                this.loyaltyFieldSpecified = value;
            }
        }

        /// <remarks/>
        public byte cipt
        {
            get
            {
                return this.ciptField;
            }
            set
            {
                this.ciptField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ciptSpecified
        {
            get
            {
                return this.ciptFieldSpecified;
            }
            set
            {
                this.ciptFieldSpecified = value;
            }
        }
    }
}