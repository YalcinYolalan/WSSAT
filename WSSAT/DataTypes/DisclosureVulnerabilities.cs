namespace WSSAT.DataTypes
{
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class DisclosureVulnerabilities
    {

        private InformationDisclosureVulnerability[] vulnerabilityField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Vulnerability")]
        public InformationDisclosureVulnerability[] Vulnerability
        {
            get
            {
                return this.vulnerabilityField;
            }
            set
            {
                this.vulnerabilityField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class InformationDisclosureVulnerability
    {

        private byte idField;

        private string titleField;

        private string descriptionField;

        private string linkField;

        private string tagNameField;
        private int missingField;

        private string severityField;

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
        public string tagName
        {
            get
            {
                return this.tagNameField;
            }
            set
            {
                this.tagNameField = value;
            }
        }

        public int missing
        {
            get
            {
                return this.missingField;
            }
            set
            {
                this.missingField = value;
            }
        }

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
    }


}
