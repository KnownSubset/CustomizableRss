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
    public partial class AddEditPage : PhoneApplicationPage
    {
        private readonly IsolatedStorageSettings isolatedStorageSettings = IsolatedStorageSettings.ApplicationSettings;
        private readonly AddEditRssViewModel addEditRssViewModel =  new AddEditRssViewModel();
        public AddEditPage()
        {
            InitializeComponent();
            DataContext = addEditRssViewModel;
            addEditRssViewModel.LoadData();
        }

        private void VerifyRssFeed(object sender, EventArgs e) {
            addEditRssViewModel.VerifyRssFeed();
        }
    }
}