using System.Text.RegularExpressions;
using System.Xml.Serialization;
using CustomizableRss.Rss.Exceptions;

namespace CustomizableRss.Rss.Structure.Validators
{

    public class RssEmail
    {
        #region Constants and Fields

        private readonly Regex r =
            new Regex(
                "[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?",
                RegexOptions.IgnoreCase);

        private string email;

        #endregion

        #region Constructors and Destructors

        public RssEmail() {}

        public RssEmail(string email)
        {
            Email = email;
        }

        #endregion

        #region Properties

        [XmlText]
        public string Email
        {
            get { return email; }

            set
            {
                if (value != null && !r.IsMatch(value))
                {
                    throw new RssParameterException("email", value);
                }

                email = value;
            }
        }

        #endregion
    }
}