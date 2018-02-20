// Cool Image Effects

using System;
using System.Windows.Data;

namespace CoolImageEffects {

    /// <summary>
    /// Class converts string to bool
    /// </summary>
    public class StringToBoolConverter : IMultiValueConverter {

        #region Public Methods
        /// <summary>
        /// Disable the current selected pagenumber
        /// </summary>
        /// <param name="values"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            var result = false;
            //For index
            if (values.Length > 1 && values[0] is int && values[1] is int) {
                var currentPageIndex = (int)values[0];
                var currentItem = (int)values[1];
                if (currentPageIndex + 1 != currentItem) {
                    result = true;
                }
            }

            //For checking radio button
            if (values.Length > 1 && values[0] is Algorithm.AlgorithmParameter && values[1] is Algorithm.AlgorithmParameter) {
                var currentAlgorithm = (Algorithm.AlgorithmParameter)values[0];
                var currentItem = (Algorithm.AlgorithmParameter)values[1];
                result = (currentAlgorithm.Value == currentItem.Value);
            }
            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
        #endregion

}