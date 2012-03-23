using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CustomizableRss.Converters
{
    public class EmptyIsVisibileConverter : IValueConverter {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            var enumerable = value as ICollection;
            return enumerable == null || enumerable.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return value.Equals(Visibility.Visible);
        }

        #endregion
    }
}