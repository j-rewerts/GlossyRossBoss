// Cool Image Effects

using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace Algorithm {
    /// <summary>
    /// Quantize Algorithm
    /// Our own algorithm / code
    /// </summary>
    public class QuantizeAlgorithm : AlgorithmBase {
        int[] stepSize = { 16, 32, 64, 128, 16, 32, 64, 128 };

        #region Public Methods
        /// <summary>
        /// Quantize effect
        /// </summary>
        /// <param name="algorithmParameter"></param>
        /// <param name="isSave"></param>
        /// <returns></returns>
        public override BitmapSource ApplyEffect(List<AlgorithmParameter> algorithmParameter, bool isSave = false) {
            int val = algorithmParameter[0].Value;
            //int val1 = val % 4;
            int sizeOfStep;
            if (val == 9)
                sizeOfStep = 128;
            else
                sizeOfStep = stepSize[val - 1];
            bool bw = algorithmParameter[0].Value > 4;
            byte bGray;
            SetData(isSave);
            for (int i = 0; i < Pixels8RedCurrent.Count; i++) {
                if (!bw) {
                    Pixels8RedResult[i] = (byte)((Pixels8RedCurrent[i] / sizeOfStep) * sizeOfStep);
                    Pixels8GreenResult[i] = (byte)((Pixels8GreenCurrent[i] / sizeOfStep) * sizeOfStep);
                    Pixels8BlueResult[i] = (byte)((Pixels8BlueCurrent[i] / sizeOfStep) * sizeOfStep);
                } else {
                    // Conversion to grayscale - the next line is repeated code :-), 
                    //  but we feel it simplifies understanding.
                    bGray = (byte)(0.3 * Pixels8RedCurrent[i] + 0.6 * Pixels8GreenCurrent[i]
                        + 0.1 * Pixels8BlueCurrent[i]);
                    Pixels8RedResult[i] = (byte)((bGray / sizeOfStep) * sizeOfStep);
                    // Special handling for the case of binarizing an image
                    if (val > 8) {
                        if (bGray > 128) {
                            Pixels8RedResult[i] = 255;
                        } else {
                            Pixels8RedResult[i] = 0;
                        }
                    }
                    Pixels8GreenResult[i] = Pixels8RedResult[i];
                    Pixels8BlueResult[i] = Pixels8RedResult[i];
                }
            }
            return UpdateImage();
        }

        /// <summary>
        /// Gets options for this algorithm
        /// </summary>
        /// <returns></returns>
        public override IList<AlgorithmOption> GetOptions() {
            Options.Add(new AlgorithmOption(InputType.MultipleChoice, GetMethodOptions()));
            return Options;
        }

        /// <summary>
        /// Gets the display info
        /// </summary>
        /// <returns></returns>
        public override string GetDisplayInfo() {
            return "Quantize the image";
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Get options 
        /// </summary>
        /// <returns></returns>
        static Dictionary<AlgorithmParameter, string> GetMethodOptions() {
            var options = new Dictionary<AlgorithmParameter, string>();
            options.Add(new AlgorithmParameter()
            {
                Value = 1
            }, "C16");
            options.Add(new AlgorithmParameter()
            {
                Value = 2
            }, "C32");
            options.Add(new AlgorithmParameter()
            {
                Value = 3
            }, "C64");
            options.Add(new AlgorithmParameter()
            {
                Value = 4
            }, "C128");
            options.Add(new AlgorithmParameter()
            {
                Value = 5
            }, "BW16");
            options.Add(new AlgorithmParameter()
            {
                Value = 6
            }, "BW32");
            options.Add(new AlgorithmParameter()
            {
                Value = 7
            }, "BW64");
            options.Add(new AlgorithmParameter()
            {
                Value = 8
            }, "BW128");
            options.Add(new AlgorithmParameter()
            {
                Value = 9
            }, "Binarize");
            return options;
        }
        #endregion
    }
}