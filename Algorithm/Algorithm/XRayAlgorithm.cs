// Cool Image Effects

using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;

namespace Algorithm {
    /// <summary>
    /// X-ray effect
    /// </summary>
    class XRayAlgorithm : AlgorithmBase {

        // This is a magic number, which works well for most images.
        double factor = 35;

        #region Public Methods
        /// <summary>
        /// X-ray effect
        /// </summary>
        /// <param name="algorithmParameter"></param>
        /// <param name="isSave"></param>
        /// <returns></returns>
        public override BitmapSource ApplyEffect(List<AlgorithmParameter> algorithmParameter, bool isSave = false) {
            SetData(isSave);
            int k, el, index, w2;
            byte r, g, b, byteGray;
            var colour = algorithmParameter.First(x => x.ParameterName == "Colour");
            double dGray;

            // Target image
            for (el = 0; el < CurrentHeight; ++el) {
                w2 = el * CurrentWidth;
                for (k = 0; k < CurrentWidth; ++k) {
                    // Source Pixel
                    index = w2 + k;
                    r = Pixels8RedCurrent[index];
                    g = Pixels8GreenCurrent[index];
                    b = Pixels8BlueCurrent[index];
                    dGray = 0.3 * r + 0.6 * g + 0.1 * b;
                    // Since this is a simple algorithm, we prefer not to use 
                    // the ComputeGrayscaleImage method here.

                    // Compute the negative grayscale value
                    dGray = 255 + factor - dGray;
                    // Clamping the values
                    if (dGray > 255)
                        dGray = 255.0;
                    byteGray = (byte)dGray;
                    SetBackgroundColour(colour, index, byteGray);
                }
            }
            return UpdateImage();
        }

        /// <summary>
        /// Get options supported by this algorithm
        /// </summary>
        /// <returns></returns>
        public override IList<AlgorithmOption> GetOptions() {
            Dictionary<AlgorithmParameter, string> colourOption = new Dictionary<AlgorithmParameter, string>();
            colourOption.Add(new AlgorithmParameter()
            {
                Value = 1,
                ParameterName = "Colour"
            }, "Red");
            colourOption.Add(new AlgorithmParameter()
            {
                Value = 2,
                ParameterName = "Colour"
            }, "Green");
            colourOption.Add(new AlgorithmParameter()
            {
                Value = 3,
                ParameterName = "Colour"
            }, "Blue");
            colourOption.Add(new AlgorithmParameter()
            {
                Value = 4,
                ParameterName = "Colour"
            }, "Cyan");
            colourOption.Add(new AlgorithmParameter()
            {
                Value = 5,
                ParameterName = "Colour"
            }, "Magenta");
            colourOption.Add(new AlgorithmParameter()
            {
                Value = 6,
                ParameterName = "Colour"
            }, "Yellow");
            colourOption.Add(new AlgorithmParameter()
            {
                Value = 7,
                ParameterName = "Colour"
            }, "Gray");
            Options.Add(new AlgorithmOption(InputType.MultipleChoice, colourOption)
            {
                ParameterName = "Colour"
            });
            return Options;
        }

        /// <summary>
        /// Gets the display information for this algorithm
        /// </summary>
        /// <returns></returns>
        public override string GetDisplayInfo() {
            return "X-Ray Effect for your image";
        }
        #endregion
    }
}