// Cool Image Effects

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;

namespace Algorithm {
    /// <summary>
    /// Sepia Effect
    /// Based on ImageMagick code
    /// </summary>
    public class SepiaAlgorithm : AlgorithmBase {
        #region Private Fields
        List<double> pixDoubleRed;
        List<double> pixDoubleGreen;
        List<double> pixDoubleBlue;
        double dMaxRed, dMinRed, dMaxGreen, dMinGreen, dMaxBlue, dMinBlue;
        double dMax, dMin;
        #endregion

        #region Public Methods
        /// <summary>
        /// Constructor
        /// </summary>
        public SepiaAlgorithm() {
            pixDoubleRed = new List<double>();
            pixDoubleGreen = new List<double>();
            pixDoubleBlue = new List<double>();
        }

        public override BitmapSource ApplyEffect(List<AlgorithmParameter> algorithmParameter,
            bool isSave = false) {
            SetData(isSave);
            pixDoubleRed = new List<double>();
            pixDoubleGreen = new List<double>();
            pixDoubleBlue = new List<double>();
            CreateSepiaImage(algorithmParameter, isSave);
            ComputeMaxAndMinDoubleImage();
            CreateFinalImage(CurrentHeight, CurrentWidth);
            pixDoubleRed = null;
            pixDoubleGreen = null;
            pixDoubleBlue = null;
            ;
            return UpdateImage();
        }

        /// <summary>
        /// Get options supported by this algorithm
        /// </summary>
        /// <returns></returns>
        public override IList<AlgorithmOption> GetOptions() {
            Dictionary<AlgorithmParameter, string> magnitude = new Dictionary<AlgorithmParameter, string>();
            magnitude.Add(new RangeAlgorithmParameter()
            {
                Value = 0,
                ParameterName = "Magnitude",
                Minimum = 50,
                Maximum = 99
            }, string.Empty);
            Options.Add(new AlgorithmOption(InputType.SingleInput, magnitude));
            return Options;
        }

        /// <summary>
        /// Get display information for this algorithm
        /// </summary>
        /// <returns></returns>
        public override string GetDisplayInfo() {
            return "Old photograph look.";
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Method to create the sepia toned image
        /// </summary>
        /// <param name="algorithmParameter"></param>
        /// <param name="size"></param>
        /// <param name="isSave"></param>
        void CreateSepiaImage(List<AlgorithmParameter> algorithmParameter,
            bool isSave = false) {
            int capacity = CurrentWidth * CurrentHeight;
            var magnitude = algorithmParameter.First(x => x.ParameterName == "Magnitude");
            int k, el, index;
            byte r, g, b;
            double dRed, dGreen, dBlue, intensity, tone;
            double threshold = magnitude.Value * 255.0 / 100.0;
            double thres6By7 = 7.0 * threshold / 6.0;
            double thres6 = threshold / 6.0;
            double thres7 = threshold / 7.0;

            pixDoubleRed.Clear();
            pixDoubleGreen.Clear();
            pixDoubleBlue.Clear();

            pixDoubleRed.Capacity = capacity;
            pixDoubleGreen.Capacity = capacity;
            pixDoubleBlue.Capacity = capacity;

            // Target image
            for (el = 0; el < CurrentHeight; ++el) {
                for (k = 0; k < CurrentWidth; ++k) {
                    index = el * CurrentWidth + k;
                    r = Pixels8RedCurrent[index];
                    g = Pixels8GreenCurrent[index];
                    b = Pixels8BlueCurrent[index];
                    // Grayscale
                    intensity = 0.3 * r + 0.6 * g + 0.1 * b;

                    // Red
                    if (intensity > threshold)
                        tone = 255.0;
                    else
                        tone = intensity + 255.0 - threshold;
                    dRed = tone;

                    // Green
                    if (intensity > thres6By7)
                        tone = 255.0;
                    else
                        tone = intensity + 255.0 - thres6By7;
                    dGreen = tone;

                    // Blue
                    if (intensity < thres6)
                        tone = 0;
                    else
                        tone = intensity - thres6;
                    dBlue = tone;

                    tone = thres7;
                    if (dGreen < tone)
                        dGreen = tone;
                    if (dBlue < tone)
                        dBlue = tone;

                    pixDoubleRed.Add(dRed);
                    pixDoubleGreen.Add(dGreen);
                    pixDoubleBlue.Add(dBlue);
                }
            }
        }

        void ComputeMaxAndMinDoubleImage() {
            dMaxRed = pixDoubleRed.Max();
            dMinRed = pixDoubleRed.Min();

            dMaxGreen = pixDoubleGreen.Max();
            dMinGreen = pixDoubleGreen.Min();

            dMaxBlue = pixDoubleBlue.Max();
            dMinBlue = pixDoubleBlue.Min();

            dMax = Math.Max(dMaxRed, dMaxGreen);
            dMax = Math.Max(dMax, dMaxBlue);

            dMin = Math.Min(dMinRed, dMinGreen);
            dMin = Math.Min(dMin, dMinBlue);
        }

        void CreateFinalImage(int height, int width) {
            int i, j, w1;
            double diffRed = dMaxRed - dMinRed;
            if (diffRed < 1) // What if diff is equal to zero?
                diffRed = 1;
            double factorRed = 255.0 / diffRed;
            double diffGreen = dMaxGreen - dMinGreen;
            if (diffGreen < 1) // What if diff is equal to zero?
                diffGreen = 1;
            double factorGreen = 255.0 / diffGreen;
            double diffBlue = dMaxBlue - dMinBlue;
            if (diffBlue < 1) // What if diff is equal to zero?
                diffBlue = 1;
            double factorBlue = 255.0 / diffBlue;
            double dValRed, dVal1Red, dValGreen, dVal1Green, dValBlue, dVal1Blue;

            double diff = dMax - dMin;
            if (diff < 1) // What if diff is equal to zero?
                diff = 1;
            double factor = 255.0 / diff;
            byte bValRed, bValGreen, bValBlue;

            for (j = 0; j < height; ++j) {
                for (i = 0; i < width; ++i) {
                    w1 = j * width + i;

                    dVal1Red = pixDoubleRed[w1];
                    dValRed = (dVal1Red - dMin) * factor;
                    bValRed = Convert.ToByte(dValRed);

                    dVal1Green = pixDoubleGreen[w1];
                    dValGreen = (dVal1Green - dMin) * factor;
                    bValGreen = Convert.ToByte(dValGreen);

                    dVal1Blue = pixDoubleBlue[w1];
                    dValBlue = (dVal1Blue - dMin) * factor;
                    bValBlue = Convert.ToByte(dValBlue);

                    Pixels8RedResult[w1] = bValRed;
                    Pixels8GreenResult[w1] = bValGreen;
                    Pixels8BlueResult[w1] = bValBlue;
                }
            }
        }
        #endregion
    }
}