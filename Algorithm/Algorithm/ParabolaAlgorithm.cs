// Cool Image Effects

using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace Algorithm {
    /// <summary>
    /// Parabola Algorithm
    /// </summary>
    class ParabolaAlgorithm : AlgorithmBase {

        #region Public Methods

        /// <summary>
        /// Parabola effect
        /// </summary>
        /// <param name="algorithmParameter"></param>
        /// <param name="isSave"></param>
        /// <returns></returns>
        public override BitmapSource ApplyEffect(List<AlgorithmParameter> algorithmParameter, bool isSave = false) {
            SetData(isSave);
            int k, el, w1, w2;
            double dRed, dGreen, dBlue, factor1, factor2, factor3;
            int option = algorithmParameter[0].Value;

            // Target image
            for (el = 0; el < CurrentHeight; ++el) {
                w2 = CurrentWidth * el;
                for (k = 0; k < CurrentWidth; ++k) {
                    w1 = w2 + k;
                    factor1 = (Pixels8RedCurrent[w1] / 128.0) - 1.0;
                    factor2 = (Pixels8GreenCurrent[w1] / 128.0) - 1.0;
                    factor3 = (Pixels8BlueCurrent[w1] / 128.0) - 1.0;

                    if (option == 1) { // Option 1
                        dRed = 255.0 * (1.0 - factor1 * factor1);
                        dGreen = 255.0 * (1.0 - factor2 * factor2);
                        dBlue = 255.0 * (1.0 - factor3 * factor3);
                    } else { // Option 2
                        dRed = 255.0 * factor1 * factor1;
                        dGreen = 255.0 * factor2 * factor2;
                        dBlue = 255.0 * factor3 * factor3;
                    }

                    // Clamp the pixel values
                    if (dRed < 0.0)
                        dRed = 0.0;
                    if (dGreen < 0.0)
                        dGreen = 0.0;
                    if (dBlue < 0.0)
                        dBlue = 0.0;

                    if (dRed > 255.0)
                        dRed = 255.0;
                    if (dGreen > 255.0)
                        dGreen = 255.0;
                    if (dBlue > 255.0)
                        dBlue = 255.0;

                    Pixels8RedResult[w1] = (byte)dRed;
                    Pixels8GreenResult[w1] = (byte)dGreen;
                    Pixels8BlueResult[w1] = (byte)dBlue;
                }
            }
            return UpdateImage();
        }

        /// <summary>
        /// Gets options for this algorithm
        /// </summary>
        /// <returns></returns>
        public override IList<AlgorithmOption> GetOptions() {
            Options.Add(new AlgorithmOption(InputType.MultipleChoice, GetMethods()));
            return Options;
        }

        /// <summary>
        /// Gets the display information for this algorithm
        /// </summary>
        /// <returns></returns>
        public override string GetDisplayInfo() {
            return "Parabola transformation on image";
        }
        #endregion

        #region Private Methods
        static Dictionary<AlgorithmParameter, string> GetMethods() {
            var options = new Dictionary<AlgorithmParameter, string>();
            options.Add(new AlgorithmParameter()
            {
                Value = 1,
                ParameterName = "Method"
            }, "Option 1");
            options.Add(new AlgorithmParameter()
            {
                Value = 2,
                ParameterName = "Method"
            }, "Option 2");
            return options;
        }
        #endregion
    }
}