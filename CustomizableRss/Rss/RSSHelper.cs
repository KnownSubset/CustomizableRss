using System.IO;
using System.Xml.Serialization;

namespace CustomizableRss.Rss
{

    public class RssHelper
    {
        #region Public Methods

        public static void WriteRss(Structure.Rss value, Stream destination)
        {
            var xsn = new XmlSerializerNamespaces();
            xsn.Add("atom", "http://www.w3.org/2005/Atom");
            xsn.Add("dc", "http://purl.org/dc/elements/1.1/");
            xsn.Add("content", "http://purl.org/rss/1.0/modules/content/");

            var ser = new XmlSerializer(value.GetType());
            ser.Serialize(destination, value, xsn);
        }

        public static Structure.Rss ReadRss(Stream source)
        {
            var xsn = new XmlSerializerNamespaces();
            xsn.Add("atom", "http://www.w3.org/2005/Atom");
            xsn.Add("dc", "http://purl.org/dc/elements/1.1/");
            xsn.Add("content", "http://purl.org/rss/1.0/modules/content/");

            var ser = new XmlSerializer(typeof(Structure.Rss));
            return (Structure.Rss)ser.Deserialize(source);
        }

        #endregion
    }
}