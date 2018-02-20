// Cool Image Effects

using System;
using System.Windows.Data;

namespace CoolImageEffects {
    /// <summary>
    /// Converts the index to bool
    /// </summary>
    class IndexToBoolConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            if ((value is Algorithm.AlgorithmParameter) && ((Algorithm.AlgorithmParameter)value).Value == 1) {
                return true;
            } else {
                return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
