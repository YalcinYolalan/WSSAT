using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService2Test.DataTypes
{

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.wstest.com/DataTypes")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.wstest.com/DataTypes", IsNullable = false)]
    public partial class Books
    {

        private BooksBook[] bookField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Book")]
        public BooksBook[] Book
        {
            get
            {
                return this.bookField;
            }
            set
            {
                this.bookField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class BooksBook
    {

        private string titleField;

        private string authorField;

        private string publishDateField;

        /// <remarks/>
        public string Title
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
        public string Author
        {
            get
            {
                return this.authorField;
            }
            set
            {
                this.authorField = value;
            }
        }

        /// <remarks/>
        public string PublishDate
        {
            get
            {
                return this.publishDateField;
            }
            set
            {
                this.publishDateField = value;
            }
        }
    }
}