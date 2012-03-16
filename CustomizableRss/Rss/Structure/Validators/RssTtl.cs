using System;
using System.Xml.Serialization;
using CustomizableRss.Rss.Exceptions;

namespace CustomizableRss.Rss.Structure.Validators
{

    public class RssTtl
    {
        #region Constants and Fields

        private int ttl;

        private string ttlString;

        #endregion

        #region Constructors and Destructors

        public RssTtl() {}

        public RssTtl(string ttl)
        {
            TTLString = ttl;
        }

        public RssTtl(int ttl)
        {
            TTL = ttl;
        }

        #endregion

        #region Properties

        [XmlIgnore]
        public int TTL
        {
            get { return ttl; }

            set
            {
                if (value < 0)
                {
                    throw new RssParameterException(string.Format("{0}.ttl", GetType()), value);
                }

                if (value != 0)
                {
                    ttl = value;
                    ttlString = ttl.ToString();
                } else
                {
                    ttl = 0;
                    ttlString = null;
                }
            }
        }

        [XmlText]
        public string TTLString
        {
            get { return ttlString; }

            set
            {
                int parseTtl = 0;
                if (value != null)
                {
                    try
                    {
                        parseTtl = int.Parse(value);
                    } catch (Exception ex)
                    {
                        throw new RssParameterException("ttl", value, ex);
                    }
                }

                TTL = parseTtl;
            }
        }

        #endregion
    }
}