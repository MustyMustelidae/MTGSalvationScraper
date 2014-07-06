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
    [XmlType(AnonymousType=true)]
    public partial class cockatrice_carddatabaseSet {
    
        private string nameField;
    
        private string longnameField;
    
        /// <remarks/>
        public string name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
    
        /// <remarks/>
        public string longname {
            get {
                return this.longnameField;
            }
            set {
                this.longnameField = value;
            }
        }
    }
}