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
using CustomizableRss.ViewModels;
using Microsoft.Phone.Controls;

namespace CustomizableRss
{
    public partial class AddEditPage : PhoneApplicationPage
    {
        private readonly AddEditRssViewModel addEditRssViewModel = new AddEditRssViewModel();
        public AddEditPage()
        {
            InitializeComponent();
            DataContext = addEditRssViewModel;
            Loaded += PageLoaded;
        }

        private void PageLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            if (!addEditRssViewModel.IsDataLoaded)
            {
                addEditRssViewModel.LoadData();
            }
        }

        private void VerifyRssFeed(object sender, EventArgs e)
        {
            addEditRssViewModel.VerifyRssFeed();
        }

        private void UrlTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            textBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            addEditRssViewModel.VerifyRssFeed();
        }

        private void SaveFeed(object sender, EventArgs e)
        {
            addEditRssViewModel.SaveFeed();
            NavigationService.GoBack();
        }
    }
}