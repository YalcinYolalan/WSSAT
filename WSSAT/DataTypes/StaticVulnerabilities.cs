namespace WSSAT.DataTypes
{

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class StaticVulnerabilities
    {

        private StaticVulnerabilitiesStaticVulnerability[] staticVulnerabilityField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("StaticVulnerability")]
        public StaticVulnerabilitiesStaticVulnerability[] StaticVulnerability
        {
            get
            {
                return this.staticVulnerabilityField;
            }
            set
            {
                this.staticVulnerabilityField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class StaticVulnerabilitiesStaticVulnerability
    {

        private byte idField;

        private string titleField;

        private string descriptionField;

        private string linkField;

        private string severityField;

        private bool attributeValueField;

        private string searchElementField;

        private string searchValueField;

        private string missingTagField;

        /// <remarks/>
        public byte id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public string title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
            }
        }

        /// <remarks/>
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public string link
        {
            get
            {
                return this.linkField;
            }
            set
            {
                this.linkField = value;
            }
        }

        /// <remarks/>
        public string severity
        {
            get
            {
                return this.severityField;
            }
            set
            {
                this.severityField = value;
            }
        }

        /// <remarks/>
        public bool attributeValue
        {
            get
            {
                return this.attributeValueField;
            }
            set
            {
                this.attributeValueField = value;
            }
        }

        /// <remarks/>
        public string searchElement
        {
            get
            {
                return this.searchElementField;
            }
            set
            {
                this.searchElementField = value;
            }
        }

        /// <remarks/>
        public string searchValue
        {
            get
            {
                return this.searchValueField;
            }
            set
            {
                this.searchValueField = value;
            }
        }

        /// <remarks/>
        public string missingTag
        {
            get
            {
                return this.missingTagField;
            }
            set
            {
                this.missingTagField = value;
            }
        }
    }


}
