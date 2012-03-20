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

namespace CustomizableRss.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _sampleProperty = "Sample Runtime Property Value";
        private ObservableCollection<MiniRss.RssFeed> _storySources;
        private readonly IsolatedStorageSettings _applicationSettings = IsolatedStorageSettings.ApplicationSettings;

        public MainViewModel()
        {
            Items = new ObservableCollection<ItemViewModel>();
            StorySources = new ObservableCollection<MiniRss.RssFeed>();
        }


        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<ItemViewModel> Items { get; private set; }

        public ObservableCollection<MiniRss.RssFeed> StorySources
        {
            get { return _storySources; }
            private set
            {
                _storySources = value;
                NotifyPropertyChanged("StorySources");
            }
        }

        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding
        /// </summary>
        /// <returns></returns>
        public string SampleProperty
        {
            get { return _sampleProperty; }
            set
            {
                if (value != _sampleProperty)
                {
                    _sampleProperty = value;
                    NotifyPropertyChanged("SampleProperty");
                }
            }
        }

        public bool IsDataLoaded { get; private set; }

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
                Deployment.Current.Dispatcher.BeginInvoke(() => UpdateRssFeed(rss, state.RssFeed));
            } catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void UpdateRssFeed(Rss.Structure.RssFeed rss, MiniRss.RssFeed rssFeed)
        {
            var miniRss = new MiniRss.RssFeed(rss);
            rssFeed.Stories.Clear();
            foreach (RssStory story in miniRss.Stories){
                rssFeed.Stories.Add(story);
            }
            _applicationSettings.Remove(rssFeed.RssTitle);
            rssFeed.RssTitle = miniRss.RssTitle;
            NotifyPropertyChanged("StorySources");
            NotifyPropertyChanged("Stories");
            rssFeed.LastUpdated = DateTime.Now;
            _applicationSettings["rssFeeds"] = new Collection<MiniRss.RssFeed> (StorySources);
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
                var hackerNewRssFeed = new MiniRss.RssFeed();
                hackerNewRssFeed.RssTitle = "Hacker News";
                hackerNewRssFeed.RssLink = new Uri("https://news.ycombinator.com/rss");
                var nprScienceNewRssFeed = new MiniRss.RssFeed();
                nprScienceNewRssFeed.RssTitle = "Science";
                nprScienceNewRssFeed.RssLink = new Uri("https://www.npr.org/rss/rss.php?id=1007");
                _applicationSettings["rssFeeds"] = new Collection<MiniRss.RssFeed> {hackerNewRssFeed, nprScienceNewRssFeed};
                _applicationSettings.Save();
            }
            StorySources = new ObservableCollection<MiniRss.RssFeed>(_applicationSettings["rssFeeds"] as Collection<MiniRss.RssFeed>);
            LoadRssFeeds();
            IsDataLoaded = true;
        }

        private void LoadRssFeeds()
        {
            foreach (MiniRss.RssFeed rssFeed in StorySources){
                LoadRssFeed(rssFeed);
            }
        }

        private void LoadRssFeed(MiniRss.RssFeed rssFeed)
        {
            var timeSpan = new TimeSpan(DateTime.Now.Ticks - rssFeed.LastUpdated.Ticks);
            if (timeSpan.Days > 0)
            {
                RefreshRssFeed(rssFeed);
            }
        }

        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void HideRssItem(RssStory rssItem){
            foreach (MiniRss.RssFeed source in App.ViewModel.StorySources){
                source.Stories.Remove(rssItem);
            }
            StorySources = new ObservableCollection<MiniRss.RssFeed>(StorySources);
        }

        public void RefreshRssFeed(MiniRss.RssFeed rssFeed)
        {
            WebRequest request = WebRequest.Create(rssFeed.RssLink);
            request.BeginGetResponse(EndGetResponse, new RequestState {Request = request, Address = rssFeed.RssLink, RssFeed = rssFeed});
        }

        #region Nested type: RequestState

        private class RequestState
        {
            public WebRequest Request { get; set; }
            public Uri Address { get; set; }
            public MiniRss.RssFeed RssFeed { get; set; }
        }

        #endregion
    }
}