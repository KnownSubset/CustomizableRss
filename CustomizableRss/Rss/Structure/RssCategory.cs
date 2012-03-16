using System.Xml.Serialization;

namespace CustomizableRss.Rss.Structure
{

    public class RssCategory
    {
        #region Properties

        [XmlAttribute("domain")]
        public string Domain { get; set; }

        [XmlText]
        public string Text { get; set; }

        #endregion
    }
}