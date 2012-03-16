using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Net;
using CustomizableRss.Rss;
using CustomizableRss.Rss.Enumerators;
using CustomizableRss.Rss.Structure;
using CustomizableRss.Rss.Structure.Validators;

namespace CustomizableRss.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _sampleProperty = "Sample Runtime Property Value";

        public MainViewModel()
        {
            Items = new ObservableCollection<ItemViewModel>();
            StorySources = new ObservableCollection<Rss.Structure.Rss>();            
        }


        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<ItemViewModel> Items { get; private set; }

        private ObservableCollection<Rss.Structure.Rss> _storySources;
        public ObservableCollection<Rss.Structure.Rss> StorySources
        {
            get { return _storySources; }
            private set { _storySources = value; 
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
                var response = state.Request.EndGetResponse(result);
                var rss = RssHelper.ReadRss(response.GetResponseStream());
                StorySources.Add(rss);
            } catch (Exception) {}
        }

        private void GotResponse(Uri address, HttpStatusCode code)
        {
            //BOOM RECEIVED
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            //LoadRssFeeds();
            var item = GetFullRSS();
            StorySources.Add(item);
            IsDataLoaded = true;
        }

        private void LoadRssFeeds()
        {
            var uri = new Uri("https://www.npr.org/rss/rss.php?id=1007");
            var addresses = new List<Uri> {uri};
            //WebRequest.Create(uri).BeginGetRequestStream(asyncCallback => { StorySources.Add(RssHelper.ReadRss()); })

            foreach (Uri current in addresses)
            {
                WebRequest request = WebRequest.Create(current);
                request.BeginGetResponse(EndGetResponse, new RequestState {Request = request, Address = current});
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

        private static Rss.Structure.Rss GetFullRSS()
        {
            return new Rss.Structure.Rss
            {
                Channel =
                    new RssChannel
                    {
                        AtomLink = new RssLink { Href = new RssUrl("http://atomlink.com"), Rel = Rel.self, Type = "text/plain" },
                        Category = "category",
                        Cloud =
                            new RssCloud
                            {
                                Domain = "domain",
                                Path = "path",
                                Port = 1234,
                                Protocol = Protocol.xmlrpc,
                                RegisterProcedure = "registerProcedure"
                            },
                        Copyright = "copyrignt (c)",
                        Description = "long description",
                        Image =
                            new RssImage
                            {
                                Description = "Image Description",
                                Height = 100,
                                Width = 100,
                                Link = new RssUrl("http://www.birdorable.com/img/bird/box/box-barred-owl.gif"),
                                Title = "title",
                                Url = new RssUrl("http://www.birdorable.com/img/bird/box/box-barred-owl.gif")
                            },
                        Language = new CultureInfo("en"),
                        LastBuildDate = new DateTime(2011, 7, 17, 15, 55, 41),
                        Link = new RssUrl("http://channel.url.com"),
                        ManagingEditor = new RssEmail("managingEditor@mail.com (manager)"),
                        PubDate = new DateTime(2011, 7, 17, 15, 55, 41),
                        Rating = "rating",
                        SkipDays = new List<Day> { Day.Thursday, Day.Wednesday },
                        SkipHours = new List<Hour> { new Hour(22), new Hour(15), new Hour(4) },
                        TextInput =
                            new RssTextInput
                            {
                                Description = "text input desctiption",
                                Link = new RssUrl("http://text.input.link.com"),
                                Name = "text input name",
                                Title = "text input title"
                            },
                        Title = "channel title",
                        TTL = 10,
                        WebMaster = new RssEmail("webmaster@mail.ru (webmaster)"),
                        Item =
                            new List<RssItem>
                                        {
                                            new RssItem
                                                {
                                                    Author = new RssEmail("item.author@mail.ru (author)"),
                                                    Category =
                                                        new RssCategory
                                                            {
                                                                Domain = "category domain value", 
                                                                Text = "category text value"
                                                            },
                                                    Comments = new RssUrl("http://rss.item.comment.url.com"),
                                                    Description = "item description",
                                                    Enclosure =
                                                        new RssEnclosure
                                                            {
                                                                Length = 1234,
                                                                Type = "text/plain",
                                                                Url = new RssUrl("http://rss.item.enclosure.type.url.com")
                                                            },
                                                    Link = new RssUrl("http://rss.item.link.url.com"),
                                                    PubDate = new DateTime(2011, 7, 17, 15, 55, 41),
                                                    Title = "item title",
                                                    Guid = new RssGuid { IsPermaLink = false, Value = "guid value" },
                                                    Source = new RssSource { Url = new RssUrl("http://rss.item.source.url.com") }
                                                }
                                        }
                    }
            };
        }


        #region Nested type: RequestState

        private class RequestState
        {
            public WebRequest Request { get; set; }
            public Uri Address { get; set; }
        }

        #endregion
    }
}