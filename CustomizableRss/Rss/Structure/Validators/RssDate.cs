using System;
using System.Globalization;
using System.Xml.Serialization;
using CustomizableRss.Rss.Exceptions;

namespace CustomizableRss.Rss.Structure.Validators
{

    public class RssDate
    {
        #region Constants and Fields

        private DateTime? date;
        private string dateString;

        #endregion

        #region Constructors and Destructors

        public RssDate() {}

        public RssDate(string date)
        {
            DateString = date;
        }

        public RssDate(DateTime? date)
        {
            Date = date;
        }

        #endregion

        #region Properties

        [XmlText]
        public string DateString
        {
            get { return dateString; }

            set
            {
                DateTime? parseDate = null;
                if (value != null)
                {
                    try
                    {
                        parseDate = DateTime.ParseExact(value, "R", CultureInfo.InvariantCulture);
                    } catch (Exception ex)
                    {
                        throw new RssParameterException("date", value, ex);
                    }
                }

                Date = parseDate;
            }
        }

        [XmlIgnore]
        public DateTime? Date
        {
            get { return date; }

            set
            {
                if (value != null)
                {
                    if (value > DateTime.Now)
                    {
                        throw new RssParameterException("newDate", value);
                    }

                    date = value;
                    dateString = date.Value.ToString("R");
                } else
                {
                    date = null;
                    dateString = null;
                }
            }
        }

        #endregion
    }
}