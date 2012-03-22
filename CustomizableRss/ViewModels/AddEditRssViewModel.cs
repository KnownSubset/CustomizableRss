using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Net;
using System.Runtime.Serialization;
using System.Windows;
using CustomizableRss.MiniRss;
using CustomizableRss.Rss;

namespace CustomizableRss.ViewModels
{
    public class AddEditRssViewModel : INotifyPropertyChanged
    {
        private const string RssFeedsKey = "rssFeed";
        private ObservableCollection<RssFeed> _rssFeeds = new ObservableCollection<RssFeed>();
        private readonly IsolatedStorageSettings isolatedStorageSettings = IsolatedStorageSettings.ApplicationSettings;
        private string uri = "http://www.npr.org/rss/rss?1014";
        private bool validRssFeed;

        public string Uri
        {
            get { return uri; }
            private set
            {
                uri = value;
                NotifyPropertyChanged("uri");
            }
        }        
        

        public bool ValidRssFeed
        {
            get { return validRssFeed; }
            private set
            {
                validRssFeed = value;
                NotifyPropertyChanged("ValidRssFeed");
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
            //var rssFeeds = isolatedStorageSettings[RssFeedsKey] as Collection<RssFeed>;
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

        public void VerifyRssFeed() {
            if (string.IsNullOrWhiteSpace(uri)) return;
            var requestUri = new Uri(Uri);
            WebRequest request = WebRequest.Create(requestUri);
            request.BeginGetResponse(EndGetResponse, new MainViewModel.RequestState { Request = request, Address = requestUri });
        }

        private void EndGetResponse(IAsyncResult result)
        {
            try
            {
                var state = result.AsyncState as MainViewModel.RequestState;
                WebResponse response = state.Request.EndGetResponse(result);
                Rss.Structure.RssFeed rss = RssHelper.ReadRss(response.GetResponseStream());
                Deployment.Current.Dispatcher.BeginInvoke(() => ValidRssFeed = true);
            }
            catch (Exception exception)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => ValidRssFeed = false);
            }
        }
    }


}