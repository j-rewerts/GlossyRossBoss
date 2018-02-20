// Cool Image Effects

using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;

namespace Algorithm {
    /// <summary>
    /// Solarize Effect
    /// </summary>
    class SolarizeAlgorithm : AlgorithmBase {
        byte threshold;

        #region Public Methods
        /// <summary>
        /// Solarize effect
        /// </summary>
        /// <param name="algorithmParameter"></param>
        /// <param name="isSave"></param>
        /// <returns></returns>
        public override BitmapSource ApplyEffect(List<AlgorithmParameter> algorithmParameter, bool isSave = false) {
            var thresholdValue = algorithmParameter.First(x => x.ParameterName == "Threshold");
            SetData(isSave);
            int k, el, w1, w2;
            byte r, g, b;

            threshold = (byte)(thresholdValue.Value);

            // Target image
            for (el = 0; el < CurrentHeight; ++el) {
                w2 = CurrentWidth * el;
                for (k = 0; k < CurrentWidth; ++k) {
                    w1 = w2 + k;

                    // Source image
                    r = Pixels8RedCurrent[w1];
                    g = Pixels8GreenCurrent[w1];
                    b = Pixels8BlueCurrent[w1];

                    if (r > threshold)
                        r = (byte)(255 - r);
                    if (g > threshold)
                        g = (byte)(255 - g);
                    if (b > threshold)
                        b = (byte)(255 - b);

                    // Clamp the values
                    if (r < 0)
                        r = 0;
                    if (g < 0)
                        g = 0;
                    if (b < 0)
                        b = 0;
                    if (r > 255)
                        r = 255;
                    if (g > 255)
                        g = 255;
                    if (b > 255)
                        b = 255;

                    Pixels8RedResult[w1] = r;
                    Pixels8BlueResult[w1] = g;
                    Pixels8GreenResult[w1] = b;
                }
            }
            return UpdateImage();
        }

        /// <summary>
        /// Get options supported by this algorithm
        /// </summary>
        /// <returns></returns>
        public override IList<AlgorithmOption> GetOptions() {
            Dictionary<AlgorithmParameter, string> threshold = new Dictionary<AlgorithmParameter, string>();
            threshold.Add(new RangeAlgorithmParameter()
            {
                Value = 0,
                ParameterName = "Threshold",
                Minimum = 0,
                Maximum = 255
            }, string.Empty);
            Options.Add(new AlgorithmOption(InputType.SingleInput, threshold));
            return Options;
        }

        /// <summary>
        /// Get display information for this algorithm
        /// </summary>
        /// <returns></returns>
        public override string GetDisplayInfo() {
            return "Solarize Effect for your image";
        }
        #endregion
    }
}