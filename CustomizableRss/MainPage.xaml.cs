using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using CustomizableRss.MiniRss;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace CustomizableRss
{


    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            Loaded += MainPage_Loaded;
        }

        private void GoToStory(object sender, GestureEventArgs gestureEventArgs)
        {
            var rssItem = (sender as StackPanel).DataContext as RssStory;
            var wbt = new WebBrowserTask();
            wbt.Uri = rssItem.StoryLink;
            wbt.Show();
        }

        private void HideClick(object sender, RoutedEventArgs e)
        {
            var rssItem = (sender as MenuItem).DataContext as RssStory;
            App.ViewModel.HideRssItem(rssItem);
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        private void RefreshRssFeed(object sender, EventArgs e)
        {
            var rssFeed = (rssPivot.SelectedItem ?? rssPivot.Items[0]) as RssFeed;
            App.ViewModel.RefreshRssFeed(rssFeed);
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

        private void ViewSettings(object sender, EventArgs e)
        {
            NavigationService.Navigated += ReturnedToPage;
            NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
        }

        private void ReturnedToPage(object sender, NavigationEventArgs navigationEventArgs)
        {
            if (!navigationEventArgs.Uri.OriginalString.Equals("/MainPage.xaml")) { return; }
            var rssFeeds = IsolatedStorageSettings.ApplicationSettings["rssFeeds"] as IEnumerable<RssFeed>;
            App.ViewModel.StorySources = new ObservableCollection<RssFeed>(rssFeeds);

        }
    }

}