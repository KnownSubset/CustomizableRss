﻿using System.Xml.Serialization;

namespace CustomizableRss.Rss.Structure
{

    /// <summary>
    /// Rss is a Web content syndication format.
    ///   Its name is an acronym for Really Simple Syndication.
    ///   Rss is a dialect of XML. All Rss files must conform to the XML 1.0 specification, 
    ///   as published on the World Wide Web Consortium (W3C) website.
    ///   http://www.w3.org/TR/REC-xml
    /// </summary>
    [XmlRoot("rss")]
    public class Rss
    {
        #region Constructors and Destructors

        public Rss()
        {
            Channel = new RssChannel();
            Version = "2.0";
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets subordinate to the 'rss' element is a single 'channel' element, 
        ///   which contains information about the channel (metadata) and its contents.
        /// </summary>
        [XmlElement("channel")]
        public RssChannel Channel { get; set; }

        /// <summary>
        ///   Gets or sets at the top level, a Rss document is a 'rss' element, 
        ///   with a mandatory attribute called version, that specifies 
        ///   the version of Rss that the document conforms to.
        /// </summary>
        [XmlAttribute("version")]
        public string Version { get; set; }

        #endregion
    }
}