﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CustomizableRss.Converters {
    public class NullToVisibilityConverter : IValueConverter {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return value == null ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return value.Equals(Visibility.Visible);
        }

        #endregion
    }
}