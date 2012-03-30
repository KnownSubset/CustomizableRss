using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Windows;
using CustomizableRss.MiniRss;
using CustomizableRss.Rss;
using Microsoft.Phone.Net.NetworkInformation;

namespace CustomizableRss.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private const string RssFeedsKey = "rssFeeds";
        private ObservableCollection<RssFeed> _storySources;
        private readonly IsolatedStorageSettings _applicationSettings = IsolatedStorageSettings.ApplicationSettings;
        private bool _isDataLoaded;
        private bool feedsExists;

        public MainViewModel()
        {
            _storySources = new ObservableCollection<RssFeed>();
        }


        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>

        public ObservableCollection<RssFeed> StorySources
        {
            get { return _storySources; }
            set
            {
                _storySources = value;
                NotifyPropertyChanged("StorySources");
                NotifyPropertyChanged("FeedsExists");
            }
        }

        public bool FeedsExists
        {
            get { return _isDataLoaded && (_storySources!=null && _storySources.Count == 0); }
        }

        public bool IsDataLoaded
        {
            get { return _isDataLoaded; }
            set { 
                _isDataLoaded = value;
                NotifyPropertyChanged("IsDataLoaded");
                NotifyPropertyChanged("FeedsExists");
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        private void EndGetResponse(IAsyncResult result)
        {
            try
            {
                var state = result.AsyncState as RequestState;
                WebResponse response = state.Request.EndGetResponse(result);
                Rss.Structure.RssFeed rss = RssHelper.ReadRss(response.GetResponseStream());
                Deployment.Current.Dispatcher.BeginInvoke(() => IsDataLoaded = true);
                Deployment.Current.Dispatcher.BeginInvoke(() => UpdateRssFeed(rss, state.RssFeed));
            } catch (Exception exception) {
                Deployment.Current.Dispatcher.BeginInvoke(() => IsDataLoaded = true);
                Deployment.Current.Dispatcher.BeginInvoke(() => MessageBox.Show(exception.Message));
            }
        }

        private void UpdateRssFeed(Rss.Structure.RssFeed rss, RssFeed rssFeed)
        {
            var miniRss = new RssFeed(rss);
            rssFeed.Stories.Clear();
            foreach (RssStory story in miniRss.Stories){
                rssFeed.Stories.Add(story);
            }
            _applicationSettings.Remove(rssFeed.RssTitle);
            rssFeed.RssTitle = miniRss.RssTitle.ToLower();
            NotifyPropertyChanged("StorySources");
            NotifyPropertyChanged("Stories");
            rssFeed.LastUpdated = DateTime.Now;
            _applicationSettings[RssFeedsKey] = new Collection<RssFeed> (StorySources);
            _applicationSettings.Save();
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            if (!_applicationSettings.Contains("initialized"))
            {
                _applicationSettings["initialized"] = true;
                var hackerNewRssFeed = new RssFeed();
                hackerNewRssFeed.RssTitle = "hacker news";
                hackerNewRssFeed.RssLink = new Uri("https://news.ycombinator.com/rss");
                var nprScienceNewRssFeed = new RssFeed();
                nprScienceNewRssFeed.RssTitle = "science";
                nprScienceNewRssFeed.RssLink = new Uri("https://www.npr.org/rss/rss.php?id=1007");
                _applicationSettings[RssFeedsKey] = new Collection<RssFeed> {hackerNewRssFeed, nprScienceNewRssFeed};
                _applicationSettings.Save();
            }
            StorySources = new ObservableCollection<RssFeed>(_applicationSettings[RssFeedsKey] as Collection<RssFeed>);
            LoadRssFeeds();
            IsDataLoaded = true;
        }

        private void LoadRssFeeds() {
            IsDataLoaded = false;            
            foreach (RssFeed rssFeed in StorySources){
                LoadRssFeed(rssFeed);
            }
        }

        private void LoadRssFeed(RssFeed rssFeed)
        {
            var timeSpan = new TimeSpan(DateTime.Now.Ticks - rssFeed.LastUpdated.Ticks);
            if (timeSpan.Days > 0) {
                RefreshRssFeed(rssFeed);
            }
        }

        private void NotifyPropertyChanged(String propertyName) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void HideRssItem(RssStory rssItem) {
            foreach (RssFeed source in App.ViewModel.StorySources){
                source.Stories.Remove(rssItem);
            }
            StorySources = new ObservableCollection<RssFeed>(StorySources);
        }

        public void RefreshRssFeed(RssFeed rssFeed) {
            if (DeviceNetworkInformation.IsNetworkAvailable) {
                IsDataLoaded = false;
                WebRequest request = WebRequest.Create(rssFeed.RssLink);
                request.BeginGetResponse(EndGetResponse, new RequestState {Request = request, Address = rssFeed.RssLink, RssFeed = rssFeed});
            } else {
                Deployment.Current.Dispatcher.BeginInvoke(() => MessageBox.Show("Unable to connect.  Please check your network settings."));                
            }
        }

        #region Nested type: RequestState

        public class RequestState
        {
            public WebRequest Request { get; set; }
            public Uri Address { get; set; }
            public RssFeed RssFeed { get; set; }
        }

        #endregion
    }
}