using System.Xml.Serialization;
using CustomizableRss.Rss.Enumerators;
using CustomizableRss.Rss.Structure.Validators;

namespace CustomizableRss.Rss.Structure
{

    /// <summary>
    /// &lt;atom:link href = "http://bash.org.ru/rss/" rel = "self" type = "application/rss+xml" /&gt;
    /// </summary>
    public class RssLink
    {
        #region Constructors and Destructors

        public RssLink()
        {
            Type = "application/rss+xml";
            Rel = Rel.self;
        }

        #endregion

        #region Properties

        [XmlIgnore]
        public RssUrl Href
        {
            get { return new RssUrl(InternalHref); }

            set { InternalHref = value.UrlString; }
        }

        [XmlAttribute("rel")]
        public Rel Rel { get; set; }

        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("href")]
        public string InternalHref { get; set; }

        #endregion
    }
}