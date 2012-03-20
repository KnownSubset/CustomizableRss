using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Runtime.Serialization;
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
            this.Stories = new ObservableCollection<RssStory>();
            this.RssTitle = rss.Channel.Title;
            this.LastUpdated = DateTime.Now;
            foreach (var rssItem in rss.Channel.Item)
            {
                var rssStory = new RssStory();
                rssStory.Description = rssItem.Description;
                rssStory.StoryLink = rssItem.Link.Url;
                rssStory.Title = rssItem.Title;
                Stories.Add(rssStory);
            }
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
