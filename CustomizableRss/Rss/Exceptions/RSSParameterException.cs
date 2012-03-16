using System;
using System.Runtime.Serialization;

namespace CustomizableRss.Rss.Exceptions
{
    public class RssParameterException : Exception
    {
        #region Constants and Fields

        private const string MessageText = "RssParameterException field '{0}', value '{1}'";
        private readonly string field;

        private readonly object value;

        #endregion

        #region Constructors and Destructors

        public RssParameterException(string field, object value) : base(string.Format(MessageText, field, value))
        {
            this.field = field;
            this.value = value;
        }

        public RssParameterException(string field, object value, Exception innerException)
            : base(string.Format(MessageText, field, value), innerException)
        {
            this.field = field;
            this.value = value;
        }

        protected RssParameterException(StreamingContext context, string field, object value)
        {
            this.field = field;
            this.value = value;
        }

        #endregion

        #region Properties

        public string Field
        {
            get { return field; }
        }

        public object Value
        {
            get { return value; }
        }

        #endregion
    }
}