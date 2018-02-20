// Cool Image Effects

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;

namespace Algorithm {
    public class EmbossAlgorithm : AlgorithmBase {

        #region Private Fields
        List<double> pixDouble;
        double dMax, dMin;
        int currentSelection;

        // Emboss Matrices
        double[, ,] embossMatrix = new double[5, 3, 3]  {
            { { 1.0, 0.0, -1.0 }, { 2.0, 0.0, -2.0 }, { 1.0, 0.0, -1.0 } }, // Matrix1
            { { 1.0, 2.0, 1.0 }, { 0.0, 0.0, 0.0 }, { -1.0, -2.0, -1.0 } }, // Matrix2
            { { 2.0, 1.0, 0.0 }, { 1.0, 0.0, -1.0 }, { 0.0, -1.0, -2.0 } }, // Matrix3
            { { -2.0, -1.0, 0.0 }, { -1.0, 0.0, 1.0 }, { 0.0, 1.0, 2.0 } }, // Matrix4
            { { -2.0, -2.0, 0.0 }, { -2.0, 6.0, 0.0 }, { 0.0, 0.0, 0.0 } }  // Matrix5
         };
        #endregion

        #region Public Methods
        public EmbossAlgorithm() {
            pixDouble = new List<double>();
            currentSelection = 1;
        }

        /// <summary>
        /// Gets options suppported by the algorithm
        /// </summary>
        /// <returns></returns>
        public override IList<AlgorithmOption> GetOptions() {
            Dictionary<AlgorithmParameter, string> option = GetMethods();
            Options.Add(new AlgorithmOption(InputType.MultipleChoice, option)
            {
                ParameterName = "Method"
            });
            Options.Add(new AlgorithmOption(InputType.MultipleChoice, GetColourOptions())
            {
                ParameterName = "Colour"
            });
            return Options;
        }

        /// <summary>
        /// Emboss effect
        /// </summary>
        /// <param name="algorithmParameter"></param>
        /// <param name="isSave"></param>
        /// <returns></returns>
        public override BitmapSource ApplyEffect(List<AlgorithmParameter> algorithmParameter, bool isSave = false) {
            SetData(isSave);
            var methodValue = algorithmParameter.First(x => x.ParameterName == "Method");
            var colour = algorithmParameter.First(x => x.ParameterName == "Colour");
            currentSelection = methodValue.Value;
            ComputeGrayscaleImage();
            ComputeDoubleImage();
            PixGray = null;
            PixGray = new List<Byte>();
            ComputeMaxAndMinDoubleImage();
            CreateFinalImage(colour);
            pixDouble = null;
            pixDouble = new List<double>();
            return UpdateImage();
        }

        /// <summary>
        /// Get the display information about the algorithm
        /// </summary>
        /// <returns></returns>
        public override string GetDisplayInfo() {
            return "Bas-relief Effect for your image";
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Applies the filter and creates an image of type double.
        /// </summary>
        void ComputeDoubleImage() {
            int height = CurrentHeight;
            int width = CurrentWidth;
            int capacity = height * width;
            int i, j, jm1, jp1, im1, ip1, w1, w2, w3, cs1;
            int i00, i01, i02, i10, i11, i12, i20, i21, i22;
            double grayVal;
            cs1 = currentSelection - 1;

            pixDouble.Clear();
            pixDouble.Capacity = capacity;

            for (j = 0; j < height; ++j) {
                jm1 = j - 1;
                if (jm1 < 0)
                    jm1 = 0;
                jp1 = j + 1;
                if (jp1 > height - 1)
                    jp1 = height - 1;

                w1 = jm1 * width;
                w2 = j * width;
                w3 = jp1 * width;

                for (i = 0; i < width; ++i) {
                    im1 = i - 1;
                    if (im1 < 0)
                        im1 = 0;
                    ip1 = i + 1;
                    if (ip1 > width - 1)
                        ip1 = width - 1;

                    i00 = w1 + im1;
                    i01 = w1 + i;
                    i02 = w1 + ip1;

                    i10 = w2 + im1;
                    i11 = w2 + i;
                    i12 = w2 + ip1;

                    i20 = w3 + im1;
                    i21 = w3 + i;
                    i22 = w3 + ip1;

                    grayVal =
                        PixGray[i00] * embossMatrix[cs1, 0, 0] +
                        PixGray[i01] * embossMatrix[cs1, 0, 1] +
                        PixGray[i02] * embossMatrix[cs1, 0, 2] +
                        PixGray[i10] * embossMatrix[cs1, 1, 0] +
                        PixGray[i11] * embossMatrix[cs1, 1, 1] +
                        PixGray[i12] * embossMatrix[cs1, 1, 2] +
                        PixGray[i20] * embossMatrix[cs1, 2, 0] +
                        PixGray[i21] * embossMatrix[cs1, 2, 1] +
                        PixGray[i22] * embossMatrix[cs1, 2, 2];

                    pixDouble.Add(grayVal);
                }
            }
        }

        /// <summary>
        /// Compute the minimum and maximum values of the image.
        /// </summary>
        void ComputeMaxAndMinDoubleImage() {
            dMax = pixDouble.Max();
            dMin = pixDouble.Min();
        }

        /// <summary>
        /// Creates the final image after scaling from 
        ///   the range (Min, Max) to [0,255]
        /// </summary>
        /// <param name="colour">Colour to be applied</param>
        void CreateFinalImage(AlgorithmParameter colour) {
            int height = CurrentHeight;
            int width = CurrentWidth;
            int i, j, w1, w2;
            double diff = dMax - dMin;
            if (diff < 1) // What if diff is equal to zero?
                diff = 1.0;
            double factor = 255.0 / diff;
            double dVal, dVal1;

            byte bVal;

            for (j = 0; j < height; ++j) {
                w2 = j * width;
                for (i = 0; i < width; ++i) {
                    w1 = w2 + i;
                    dVal1 = pixDouble[w1];
                    dVal = (dVal1 - dMin) * factor;
                    bVal = Convert.ToByte(dVal);
                    SetBackgroundColour(colour, w1, bVal);
                }
            }
        }

        Dictionary<AlgorithmParameter, string> GetMethods() {
            Dictionary<AlgorithmParameter, string> option = new Dictionary<AlgorithmParameter, string>();
            option.Add(new AlgorithmParameter()
            {
                Value = 1,
                ParameterName = "Method"
            }, "Emboss 1");
            option.Add(new AlgorithmParameter()
            {
                Value = 2,
                ParameterName = "Method"
            }, "Emboss 2");
            option.Add(new AlgorithmParameter()
            {
                Value = 3,
                ParameterName = "Method"
            }, "Emboss 3");
            option.Add(new AlgorithmParameter()
            {
                Value = 4,
                ParameterName = "Method"
            }, "Emboss 4");
            option.Add(new AlgorithmParameter()
            {
                Value = 5,
                ParameterName = "Method"
            }, "Emboss 5");
            return option;
        }

        Dictionary<AlgorithmParameter, string> GetColourOptions() {
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
            return colourOption;
        }
        #endregion
    }
}