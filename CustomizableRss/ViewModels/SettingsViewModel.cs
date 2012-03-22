using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;
using CustomizableRss.MiniRss;

namespace CustomizableRss.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private const string RssFeedsKey = "rssFeeds";
        private const string RssFeedKey = "rssFeed";
        private ObservableCollection<RssFeed> _rssFeeds = new ObservableCollection<RssFeed>();
        private readonly IsolatedStorageSettings isolatedStorageSettings = IsolatedStorageSettings.ApplicationSettings;

        public ObservableCollection<RssFeed> RssFeeds
        {
            get { return _rssFeeds; }
            private set
            {
                _rssFeeds = value;
                NotifyPropertyChanged("RssFeeds");
            }
        }

        public bool IsDataLoaded { get; private set; }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            var rssFeeds = isolatedStorageSettings[RssFeedsKey] as Collection<RssFeed>;
            RssFeeds = new ObservableCollection<RssFeed>(rssFeeds);
            IsDataLoaded = true;
        }

        
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void RemoveRssFeed(RssFeed rssFeed){
            var rssFeeds = isolatedStorageSettings[RssFeedsKey] as Collection<RssFeed>;
            rssFeeds.Remove(rssFeed);
            isolatedStorageSettings.Save();
        }

        public void UpdateRssFeeds(){
            if (isolatedStorageSettings.Contains(RssFeedKey)){
                var rssFeed = isolatedStorageSettings[RssFeedKey] as RssFeed;
                if (!RssFeeds.Contains(rssFeed)){
                    RssFeeds.Add(rssFeed);
                    var rssFeeds = isolatedStorageSettings[RssFeedsKey] as Collection<RssFeed>;
                    rssFeeds.Add(rssFeed);
                }
                isolatedStorageSettings.Remove(RssFeedKey);
            }
        }
    }


}