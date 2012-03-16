using System.Xml.Serialization;
using CustomizableRss.Rss.Structure.Validators;

namespace CustomizableRss.Rss.Structure
{

    /// <summary>
    /// Its value is the name of the Rss channel that the item came from, derived from its 'title'.
    ///   The purpose of this element is to propagate credit for links, to publicize the sources 
    ///   of news items. It can be used in the Post command of an aggregator. It should be generated
    ///   automatically when forwarding an item from an aggregator to a weblog authoring tool.
    /// </summary>
    public class RssSource
    {
        #region Properties

        [XmlIgnore]
        public RssUrl Url
        {
            get { return new RssUrl(InternalUrl); }

            set { InternalUrl = value.UrlString; }
        }

        [XmlAttribute("url")]
        public string InternalUrl { get; set; }

        #endregion
    }
}