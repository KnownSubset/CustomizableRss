using System;
using System.Xml.Serialization;
using CustomizableRss.Rss.Exceptions;

namespace CustomizableRss.Rss.Structure.Validators
{
    public class RssUrl
    {
        #region Constants and Fields

        private Uri url;

        private string urlString;

        #endregion

        #region Constructors and Destructors

        public RssUrl() {}

        public RssUrl(string newUrl)
        {
            UrlString = newUrl;
        }

        public RssUrl(Uri newUrl)
        {
            Url = newUrl;
        }

        #endregion

        #region Properties

        [XmlIgnore]
        public Uri Url
        {
            get { return url; }

            set
            {
                url = value;
                if (url == null)
                {
                    urlString = null;
                } else
                {
                    urlString = url.AbsoluteUri;
                }
            }
        }

        [XmlText]
        public string UrlString
        {
            get { return urlString; }

            set
            {
                Uri parseUrl = null;
                if (value != null)
                {
                    try
                    {
                        parseUrl = new Uri(value, UriKind.Absolute);
                    } catch (Exception ex)
                    {
                        throw new RssParameterException("url", value, ex);
                    }
                }

                Url = parseUrl;
            }
        }

        #endregion
    }
}