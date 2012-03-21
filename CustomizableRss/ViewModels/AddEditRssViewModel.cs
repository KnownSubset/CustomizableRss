using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;
using CustomizableRss.MiniRss;

namespace CustomizableRss.ViewModels
{
    public class AddEditRssViewModel : INotifyPropertyChanged
    {
        private const string RssFeedsKey = "rssFeed";
        private ObservableCollection<RssFeed> _rssFeeds = new ObservableCollection<RssFeed>();
        private readonly IsolatedStorageSettings isolatedStorageSettings = IsolatedStorageSettings.ApplicationSettings;
        private string uriString = "";

        public string UriString
        {
            get { return uriString; }
            private set
            {
                uriString = value;
                NotifyPropertyChanged("UriString");
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
            //RssFeeds = new ObservableCollection<RssFeed>(rssFeeds);
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

    }


}