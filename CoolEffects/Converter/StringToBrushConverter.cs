// Cool Image Effects

using System;
using System.Collections.Generic;
using System.Windows.Data;
using System.Windows.Media;

namespace CoolImageEffects {
    /// <summary>
    /// Class converts file path to Image
    /// </summary>
    class StringToBrushConverter : IValueConverter {

        #region Public Methods
        /// <summary>
        /// Converts string to colour
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture) {
            if (value is KeyValuePair<Algorithm.AlgorithmParameter, string>) {
                var algorithmParameter = (KeyValuePair<Algorithm.AlgorithmParameter, string>)value;
                var colourName = algorithmParameter.Value;
                if (colourName == "None") {
                    return new SolidColorBrush(System.Windows.Media.Colors.Silver);
                }

                System.Drawing.Color colour = System.Drawing.Color.FromName(colourName);
                var c = new Color()
                {
                    R = colour.R,
                    G = colour.G,
                    B = colour.B
                };
                var t = Colors.Red;
                var colourNew = (Color)ColorConverter.ConvertFromString(colourName);
                if (c == t) {
                }
                return new SolidColorBrush(colourNew);
            } else {
                return new SolidColorBrush(System.Windows.Media.Colors.Transparent);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture) {
            return null;
        }
        #endregion
    }
}