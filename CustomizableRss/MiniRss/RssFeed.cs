using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace CustomizableRss.MiniRss
{

    [DataContract]
    public class RssFeed
    {
        public RssFeed()
        {
            this.LastUpdated = new DateTime();
            this.Stories = new ObservableCollection<RssStory>();
        }

        public RssFeed(Rss.Structure.RssFeed rss):base(){
            this.Stories = new ObservableCollection<RssStory>(CloneStories(rss));
            this.RssTitle = rss.Channel.Title;
            this.LastUpdated = DateTime.Now;
        }

        public static List<RssStory> CloneStories(Rss.Structure.RssFeed rss)
        {
            var stories = new List<RssStory>();
            foreach (var rssItem in rss.Channel.Item) {
                var rssStory = new RssStory();
                rssStory.Description = StripTagsCharArray(rssItem.Description);
                rssStory.StoryLink = rssItem.Link.Url;
                rssStory.Title = StripTagsCharArray(rssItem.Title);
                stories.Add(rssStory);
            }
            return stories;
        }

        [DataMember]
        public Uri RssLink { get; set; }

        private ObservableCollection<RssStory> _stories;

        [DataMember]
        public ObservableCollection<RssStory> Stories
        {
            get { return _stories; }
            set { _stories = value; }
        }

        [DataMember]
        public string RssTitle { get; set; }
        [DataMember]
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Compiled regular expression for performance.
        /// </summary>
        static Regex htmlRegex = new Regex("<.*?>", RegexOptions.Compiled);
        /// <summary>
        /// Remove HTML tags from string using char array.
        /// </summary>
        public static string StripTagsCharArray(string source){
            return htmlRegex.Replace(source, string.Empty).Replace("\t",string.Empty).Replace("\n", string.Empty);
        }
    }

    [DataContract]
    public class RssStory
    {
        [DataMember]
        public Uri StoryLink { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Description { get; set; }
    }
}
