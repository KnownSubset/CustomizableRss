using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using CustomizableRss.MiniRss;
using CustomizableRss.Rss.Structure;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace CustomizableRss
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        private void GoToStory(object sender, GestureEventArgs gestureEventArgs)
        {
            var rssItem = (sender as StackPanel).DataContext as RssStory;
            var wbt = new WebBrowserTask();
            wbt.URL = rssItem.StoryLink.AbsoluteUri;
            wbt.Show();
        }

        private void Share(object sender, RoutedEventArgs e)
        {
            var rssItem = (sender as MenuItem).DataContext as RssStory;
            var shareLinkTask = new ShareLinkTask();
            shareLinkTask.LinkUri = rssItem.StoryLink;
            shareLinkTask.Title = rssItem.Title;
            shareLinkTask.Message = rssItem.Description;
            shareLinkTask.Show();   
        }
        private void HideClick(object sender, RoutedEventArgs e)
        {
            var rssItem = (sender as MenuItem).DataContext as MiniRss.RssStory;
            App.ViewModel.HideRssItem(rssItem);

        }

        private void RefreshRssFeed(object sender, EventArgs e)
        {
            var rssFeed = rssPivot.SelectedItem as MiniRss.RssFeed;
            App.ViewModel.RefreshRssFeed(rssFeed);
        }
    }
}