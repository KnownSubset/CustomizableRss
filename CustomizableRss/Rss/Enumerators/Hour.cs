using System.Xml.Serialization;
using CustomizableRss.Rss.Exceptions;

namespace CustomizableRss.Rss.Enumerators
{

    public class Hour
    {
        #region Constants and Fields

        private byte value;

        #endregion

        #region Constructors and Destructors

        public Hour() {}

        public Hour(byte newValue)
        {
            value = newValue;
        }

        #endregion

        #region Properties

        [XmlText]
        public byte Value
        {
            get { return value; }

            set
            {
                if (value < 0 || value > 23)
                {
                    throw new RssParameterException("hour", value);
                }

                this.value = value;
            }
        }

        #endregion
    }
}