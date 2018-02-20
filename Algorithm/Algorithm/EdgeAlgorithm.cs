// Cool Image Effects

using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace Algorithm {
    [Serializable]
    public class EdgeAlgorithm : AlgorithmBase {
        // Based on the course Computational Photography, offered on Coursera
        // by Prof. Irfan Essa of Georgia Tech
        #region Private Fields
        List<double> pixGrayAvg;
        List<double> dxImage;
        List<double> dyImage;
        List<double> thetaImage;
        List<double> magnitudeImage;
        int currentSelection;
        double magnitudeMax;
        double magnitudeMin;
        #endregion

        #region Public Methods
        /// <summary>
        /// Constructor
        /// </summary>
        public EdgeAlgorithm() {
            pixGrayAvg = new List<double>();
            dxImage = new List<double>();
            dyImage = new List<double>();
            thetaImage = new List<double>();
            magnitudeImage = new List<double>();
        }

        /// <summary>
        ///Get the options suported by this algorithm
        /// </summary>
        /// <returns></returns>
        public override IList<AlgorithmOption> GetOptions() {
            Dictionary<AlgorithmParameter, string> option = GetOptionsForMethod();
            Dictionary<AlgorithmParameter, string> magnitude = new Dictionary<AlgorithmParameter, string>();
            magnitude.Add(new RangeAlgorithmParameter()
            {
                Value = 0,
                ParameterName = "Threshold",
                Minimum = 150,
                Maximum = 250
            }, string.Empty);
            Options.Add(new AlgorithmOption(InputType.SingleInput, magnitude));
            Options.Add(new AlgorithmOption(InputType.MultipleChoice, option));
            return Options;
        }

        /// <summary>
        /// Edge effect
        /// </summary>
        /// <param name="algorithmParameter"></param>
        /// <param name="isSave"></param>
        /// <returns></returns>
        public override BitmapSource ApplyEffect(List<AlgorithmParameter> algorithmParameter, bool isSave = false) {
            short threshold = 235;

            if (algorithmParameter.Any(x => x.ParameterName == "Threshold")) {
                currentSelection = 4;
                threshold = (short)algorithmParameter.First(x => x.ParameterName == "Threshold").Value;
            }

            BitmapSource result = null;
            currentSelection = algorithmParameter[0].Value;
            SetData(isSave);
            ComputeGrayscaleImage();
            ComputePixGrayAverage(CurrentHeight, CurrentWidth);
            ComputeDxAndDyImages(CurrentHeight, CurrentWidth);
            pixGrayAvg = null;
            pixGrayAvg = new List<double>();

            ComputeThetaAndMagnitudeImages(CurrentHeight, CurrentWidth);
            ComputeMaxAndMinMagnitudeImage();
            CreateFinalImage(CurrentHeight, CurrentWidth, threshold);
            result = UpdateImage();
            PixGray = null;
            pixGrayAvg = null;
            dxImage = null;
            dyImage = null;
            thetaImage = null;
            magnitudeImage = null;
            PixGray = new List<byte>();
            pixGrayAvg = new List<double>();
            dxImage = new List<double>();
            dyImage = new List<double>();
            thetaImage = new List<double>();
            magnitudeImage = new List<double>();
            return result;
        }

        /// <summary>
        /// Get information about the algorithm
        /// </summary>
        /// <returns></returns>
        public override string GetDisplayInfo() {
            return "Edge or Outline Effect";
        }
        #endregion

        #region Private Methods
        void ComputePixGrayAverage(int height, int width) {
            int capacity = height * width;
            int i, j, jm1, jp1, im1, ip1;
            int i00, i01, i02, i10, i11, i12, i20, i21, i22;
            double avg;
            int w1, w2, w3;

            pixGrayAvg.Clear();
            pixGrayAvg.Capacity = capacity;
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

                    // We don't divide by 16, since we're anyway scaling the image at a later stage
                    avg = PixGray[i00] + 2 * PixGray[i01] + PixGray[i02] +
                          2 * PixGray[i10] + 4 * PixGray[i11] + 2 * PixGray[i12] +
                          PixGray[i20] + 2 * PixGray[i21] + PixGray[i22];
                    pixGrayAvg.Add(avg);
                }
            }
        }

        // Gradient Images
        void ComputeDxAndDyImages(int height, int width) {
            int capacity = height * width;
            int i, j, jm1, jp1, im1, ip1, w1, w2, w3;
            int i00, i01, i02, i10, i11, i12, i20, i21, i22;
            double dx, dy;

            double[,] sobelX = { { -1.0, 0.0, 1.0 }, { -2.0, 0.0, 2.0 }, { -1.0, 0.0, 1.0 } };
            double[,] sobelY = { { -1.0, -2.0, -1.0 }, { 0.0, 0.0, 0.0 }, { 1.0, 2.0, 1.0 } };

            dxImage.Clear();
            dyImage.Clear();
            dxImage.Capacity = capacity;
            dyImage.Capacity = capacity;

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

                    dx = pixGrayAvg[i00] * sobelX[0, 0] + pixGrayAvg[i01] * sobelX[0, 1] +
                         pixGrayAvg[i02] * sobelX[0, 2] +
                         pixGrayAvg[i10] * sobelX[1, 0] + pixGrayAvg[i11] * sobelX[1, 1] +
                         pixGrayAvg[i12] * sobelX[1, 2] +
                         pixGrayAvg[i20] * sobelX[2, 0] + pixGrayAvg[i21] * sobelX[2, 1] +
                         pixGrayAvg[i22] * sobelX[2, 2];

                    dy = pixGrayAvg[i00] * sobelY[0, 0] + pixGrayAvg[i01] * sobelY[0, 1] +
                         pixGrayAvg[i02] * sobelY[0, 2] +
                         pixGrayAvg[i10] * sobelY[1, 0] + pixGrayAvg[i11] * sobelY[1, 1] +
                         pixGrayAvg[i12] * sobelY[1, 2] +
                         pixGrayAvg[i20] * sobelY[2, 0] + pixGrayAvg[i21] * sobelY[2, 1] +
                         pixGrayAvg[i22] * sobelY[2, 2];

                    if (Math.Abs(dy) < 0.001) // We don't want this to be zero
                        dy = 0.001;
                    dxImage.Add(dx);
                    dyImage.Add(dy);
                }
            }
        }

        void ComputeThetaAndMagnitudeImages(int height, int width) {
            int capacity = height * width;
            thetaImage.Clear();
            magnitudeImage.Clear();
            thetaImage.Capacity = capacity;
            magnitudeImage.Capacity = capacity;

            double theta, magnitude;
            int index, w2;

            for (int j = 0; j < height; ++j) {
                w2 = j * width;
                for (int i = 0; i < width; ++i) {
                    index = w2 + i;
                    theta = Math.Atan(dxImage[index] / dyImage[index]);
                    magnitude = Math.Sqrt(dxImage[index] * dxImage[index] + dyImage[index] * dyImage[index]);
                    thetaImage.Add(theta);
                    magnitudeImage.Add(magnitude);
                }
            }
        }

        void ComputeMaxAndMinMagnitudeImage() {
            magnitudeMax = magnitudeImage[0];
            magnitudeMin = magnitudeImage[0];

            for (int i = 1; i < magnitudeImage.Count; ++i) {
                if (magnitudeImage[i] < magnitudeMin)
                    magnitudeMin = magnitudeImage[i];
                if (magnitudeImage[i] > magnitudeMax)
                    magnitudeMax = magnitudeImage[i];
            }
        }

        void CreateFinalImage(int height, int width, short thresholdSelected) {
            int i, j, index, w2;
            double diff = magnitudeMax - magnitudeMin;
            if (diff < 1) // What if diff is equal to zero?
                diff = 1;
            double factor = 255.0 / diff;
            double dVal, dVal1;
            double[] boundaries = { 0.375 * Math.PI, 0.125 * Math.PI, -0.125 * Math.PI, -0.375 * Math.PI };
            byte bVal;
            byte threshold = (byte)thresholdSelected;

            for (j = 0; j < height; ++j) {
                w2 = j * width;
                for (i = 0; i < width; ++i) {
                    index = w2 + i;
                    dVal1 = magnitudeImage[index];
                    dVal = (dVal1 - magnitudeMin) * factor;
                    bVal = Convert.ToByte(dVal);

                    Pixels8RedResult[index] = 0;
                    Pixels8GreenResult[index] = 0;
                    Pixels8BlueResult[index] = 0;

                    if (currentSelection == 1) {
                        // Vertical is yellow
                        if (thetaImage[index] > boundaries[0] || thetaImage[index] < boundaries[3]) {
                            Pixels8RedResult[index] = bVal;
                            Pixels8GreenResult[index] = bVal;
                            //Pixels8BlueResult[index] = 0;
                        } else if (thetaImage[index] >= boundaries[3] && thetaImage[index] < boundaries[2]) {
                            // \ is green
                            //Pixels8RedResult[index] = 0;
                            Pixels8GreenResult[index] = bVal;
                            //Pixels8BlueResult[index] = 0;
                        } else if (thetaImage[index] >= boundaries[2] && thetaImage[index] < boundaries[1]) {
                            // Horizontal is blue
                            //Pixels8RedResult[index] = 0;
                            //Pixels8GreenResult[index] = 0;
                            Pixels8BlueResult[index] = bVal;

                        } else { // if (thetaImage[index] >= boundaries[1] && thetaImage[index] < boundaries[0]) {
                            // / is red
                            Pixels8RedResult[index] = bVal;
                            //Pixels8GreenResult[index] = 0;
                            //Pixels8BlueResult[index] = 0;
                        }
                    } else if (currentSelection == 2 || currentSelection == 3) {
                        if (currentSelection == 3) {
                            bVal = (byte)(255 - bVal);
                        }
                        Pixels8RedResult[index] = bVal;
                        Pixels8GreenResult[index] = bVal;
                        Pixels8BlueResult[index] = bVal;
                    } else {
                        // currentSelection = 4;
                        bVal = (byte)(255 - bVal);
                        if (bVal > threshold) {
                            Pixels8RedResult[index] = 255;
                            Pixels8GreenResult[index] = 255;
                            Pixels8BlueResult[index] = 255;
                        }
                    }
                }
            }
        }

        Dictionary<AlgorithmParameter, string> GetOptionsForMethod() {
            Dictionary<AlgorithmParameter, string> option = new Dictionary<AlgorithmParameter, string>();
            option.Add(new AlgorithmParameter()
            {
                Value = 1
            }, "Coloured Edges");
            option.Add(new AlgorithmParameter()
            {
                Value = 2
            }, "Inverted Grey Edges");
            option.Add(new AlgorithmParameter()
            {
                Value = 3
            }, "Gray Edges");
            return option;
        }
        #endregion
    }
}