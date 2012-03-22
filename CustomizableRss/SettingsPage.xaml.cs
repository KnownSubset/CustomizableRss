using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CustomizableRss.MiniRss;
using CustomizableRss.ViewModels;
using Microsoft.Phone.Controls;

namespace CustomizableRss
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        private readonly SettingsViewModel settingsViewModel = new SettingsViewModel();
        private readonly IsolatedStorageSettings isolatedStorageSettings = IsolatedStorageSettings.ApplicationSettings;

        public SettingsPage()
        {
            DataContext = settingsViewModel;
            settingsViewModel.LoadData();
            InitializeComponent();
        }

        private void EditRss(object sender, RoutedEventArgs e)
        {
            var rssItem = (sender as StackPanel).DataContext as RssFeed;
            isolatedStorageSettings["rssFeed"] = rssItem;
            settingsViewModel.RssFeeds.Remove(rssItem);
            NavigationService.Navigate(new Uri("AddEditPage.xaml", UriKind.Relative));
            NavigationService.Navigated += NavigatedBackToPage;
        }

        private void NavigatedBackToPage(object sender, NavigationEventArgs navigationEventArgs)
        {
            if (isolatedStorageSettings.Contains("rssFeed"))
            {
                var rssFeed = isolatedStorageSettings["rssFeed"] as RssFeed;
                settingsViewModel.RssFeeds.Add(rssFeed);
            }
        }

        private void DeleteRss(object sender, RoutedEventArgs e)
        {
            var rssItem = (sender as StackPanel).DataContext as RssFeed;
            settingsViewModel.RemoveRssFeed(rssItem);
        }

        private void AddRss(object sender, RoutedEventArgs e)
        {
            isolatedStorageSettings.Remove("rssFeed");
            NavigationService.Navigate(new Uri("/AddEditPage.xaml", UriKind.Relative));
            NavigationService.Navigated += NavigatedBackToPage;
        }
    }
}