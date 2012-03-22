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
        private const string RssFeedKey = "rssFeed";
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
            var rssItem = (sender as MenuItem).DataContext as RssFeed;
            if (isolatedStorageSettings.Contains(RssFeedKey)){
                isolatedStorageSettings.Remove(RssFeedKey);
            }
            isolatedStorageSettings[RssFeedKey] = rssItem;
            NavigationService.Navigate(new Uri("/AddEditPage.xaml", UriKind.Relative));
            NavigationService.Navigated += NavigatedBackToPage;
        }

        private void NavigatedBackToPage(object sender, NavigationEventArgs navigationEventArgs)
        {
            if (!navigationEventArgs.Uri.OriginalString.Equals("/SettingsPage.xaml")) {return;}
            settingsViewModel.UpdateRssFeeds();
        }

        private void DeleteRss(object sender, RoutedEventArgs e)
        {
            var rssItem = (sender as MenuItem).DataContext as RssFeed;
            settingsViewModel.RemoveRssFeed(rssItem);
        }

        private void AddRss(object sender, RoutedEventArgs e)
        {
            isolatedStorageSettings.Remove(RssFeedKey);
            NavigationService.Navigate(new Uri("/AddEditPage.xaml", UriKind.Relative));
            NavigationService.Navigated += NavigatedBackToPage;
        }
    }
}