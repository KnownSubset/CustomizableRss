using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;

namespace CustomizableRss.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private const string RssFeedsKey = "rssFeeds";
        private ObservableCollection<RssFeed> _rssFeeds;

        public SettingsViewModel()
        {
            Items = new ObservableCollection<ItemViewModel>();
            RssFeeds = new ObservableCollection<RssFeed>();
        }


        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<ItemViewModel> Items { get; private set; }

        public ObservableCollection<RssFeed> RssFeeds
        {
            get { return _rssFeeds; }
            private set
            {
                _rssFeeds = value;
                NotifyPropertyChanged("RssFeeds");
            }
        }

        public RssFeed RssFeed
        {
            get { return rssFeed; }
            private set
            {
                rssFeed = value;
                NotifyPropertyChanged("RssFeed");
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
            foreach (RssFeed loadRssFeed in LoadRssFeeds())
            {
                RssFeeds.Add(loadRssFeed);
            }
            IsDataLoaded = true;
        }

        private IEnumerable<RssFeed> LoadRssFeeds()
        {
            IsolatedStorageSettings isolatedStorageSettings = IsolatedStorageSettings.ApplicationSettings;
            if (!isolatedStorageSettings.Contains(RssFeedsKey))
            {
                isolatedStorageSettings[RssFeedsKey] = new List<RssFeed>();
            }
            return isolatedStorageSettings[RssFeedsKey] as IList<RssFeed>;
        }

        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    #region Nested type: RequestState

    [DataContract]
    public class RssFeed
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public Uri Address { get; set; }
    }

    #endregion
}