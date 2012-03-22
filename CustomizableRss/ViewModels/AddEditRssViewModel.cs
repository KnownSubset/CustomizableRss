using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Net;
using System.Windows;
using CustomizableRss.MiniRss;
using CustomizableRss.Rss;

namespace CustomizableRss.ViewModels {
    public class AddEditRssViewModel : INotifyPropertyChanged {
        private const string RssFeedKey = "rssFeed";
        private ObservableCollection<RssFeed> _rssFeeds = new ObservableCollection<RssFeed>();
        private readonly IsolatedStorageSettings isolatedStorageSettings = IsolatedStorageSettings.ApplicationSettings;
        private string link = string.Empty;
        private string urlStatus = string.Empty;
        private const string invalidRssFeed = "(ノಠ益ಠ)ノ彡┻━┻ it doesn't work";
        private const string validRssFeed = "ヽ(´▽`)/ you are good!";

        public string Link {
            get { return link; }
            set {
                link = value;
                NotifyPropertyChanged("Link");
            }
        }

        public string UrlStatus {
            get { return urlStatus; }
            private set {
                urlStatus = value;
                NotifyPropertyChanged("UrlStatus");
            }
        }

        public bool IsDataLoaded { get; private set; }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName){
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler){
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData(){
            if (isolatedStorageSettings.Contains(RssFeedKey)){
                Link = (isolatedStorageSettings[RssFeedKey] as RssFeed).RssLink.AbsoluteUri;
            }
            VerifyRssFeed();
            IsDataLoaded = true;
        }

        public void VerifyRssFeed() {
            if (string.IsNullOrWhiteSpace(link)) return;
            try {
                var requestUri = new Uri(link);
                WebRequest request = WebRequest.Create(requestUri);
                request.BeginGetResponse(EndGetResponse, new MainViewModel.RequestState { Request = request, Address = requestUri });
            } catch (NotSupportedException exception) {
                Debug.Assert(exception != null);
                Deployment.Current.Dispatcher.BeginInvoke(() => UrlStatus = invalidRssFeed);
            } catch (UriFormatException exception) {
                Debug.Assert(exception != null);
                Deployment.Current.Dispatcher.BeginInvoke(() => UrlStatus = invalidRssFeed);
            }
        }

        private void EndGetResponse(IAsyncResult result){
            try{
                var state = result.AsyncState as MainViewModel.RequestState;
                WebResponse response = state.Request.EndGetResponse(result);
                Rss.Structure.RssFeed rss = RssHelper.ReadRss(response.GetResponseStream());
                if (rss.Channel == null) {
                    Deployment.Current.Dispatcher.BeginInvoke(() => UrlStatus = invalidRssFeed);
                } else{
                    Deployment.Current.Dispatcher.BeginInvoke(() => UrlStatus = validRssFeed);
                    if (!isolatedStorageSettings.Contains(RssFeedKey)){
                        isolatedStorageSettings[RssFeedKey] = new RssFeed();
                    }
                    var rssFeed =  isolatedStorageSettings[RssFeedKey] as RssFeed;
                    rssFeed.RssLink = state.Address;
                    rssFeed.RssTitle = rss.Channel.Title;
                    rssFeed.LastUpdated = DateTime.Now;
                    rssFeed.Stories = new ObservableCollection<RssStory>(RssFeed.CloneStories(rss));
                }
            } catch (Exception exception) {
                Debug.Assert(exception != null);
                Deployment.Current.Dispatcher.BeginInvoke(() => UrlStatus = invalidRssFeed);
            }
        }

        public void SaveFeed(){
            if (validRssFeed.Equals(UrlStatus)) {
                isolatedStorageSettings.Save();
            }
        }
    }


}