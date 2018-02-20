// Cool Image Effects

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;

namespace Algorithm {
    /// <summary>
    /// Glass Algorithm
    /// </summary>
    class GlassAlgorithm : AlgorithmBase {

        double sliderVal;

        #region Public Methods
        /// <summary>
        /// Glass effect
        /// Based on the Lecture Notes by Prof. Onur Guleryuz of Polytechnic University, New York. 
        /// </summary>
        /// <param name="algorithmParameter"></param>
        /// <param name="isSave"></param>
        /// <returns></returns>
        public override BitmapSource ApplyEffect(List<AlgorithmParameter> algorithmParameter, bool isSave = false) {
            SetData(isSave);
            var coarsenessValue = algorithmParameter.First(z => z.ParameterName == "Coarseness");
            var method = algorithmParameter.First(z => z.ParameterName == "Method");

            int x, y = 0, k, el, w1, w2;
            byte r, g, b;

            sliderVal = coarsenessValue.Value;
            double parameter = sliderVal;

            Random rand = new Random(10000);
            double rand1, rand2;

            // Horizontal and Bidirectional
            if (method.Value == 1 || method.Value == 3) {
                for (el = 0; el < CurrentHeight; ++el) {
                    w2 = CurrentWidth * el;

                    if (method.Value == 1) { // Horizontally oriented glass
                        rand2 = rand.NextDouble();
                        y = Convert.ToInt32(el + (rand2 - 0.5) * parameter);
                        y = findYInBound(y);
                    }

                    for (k = 0; k < CurrentWidth; ++k) {
                        rand1 = rand.NextDouble();
                        x = Convert.ToInt32(k + (rand1 - 0.5) * parameter);

                        if (method.Value == 3) { // Generally oriented glass
                            rand2 = rand.NextDouble();
                            y = Convert.ToInt32(el + (rand2 - 0.5) * parameter);
                            y = findYInBound(y);
                        }

                        x = findXInBound(x);

                        // Source pixel
                        w1 = CurrentWidth * y + x;
                        r = Pixels8RedCurrent[w1];
                        g = Pixels8GreenCurrent[w1];
                        b = Pixels8BlueCurrent[w1];

                        // Target pixel
                        w1 = w2 + k;

                        Pixels8RedResult[w1] = r;
                        Pixels8GreenResult[w1] = g;
                        Pixels8BlueResult[w1] = b;
                    }
                }
            } else { // Vertical
                for (k = 0; k < CurrentWidth; ++k) {
                    rand1 = rand.NextDouble();
                    x = Convert.ToInt32(k + (rand1 - 0.5) * parameter);
                    x = findXInBound(x);
                    for (el = 0; el < CurrentHeight; ++el) {
                        rand2 = rand.NextDouble();
                        y = Convert.ToInt32(el + (rand2 - 0.5) * parameter);
                        y = findYInBound(y);

                        // Source pixel
                        w1 = CurrentWidth * y + x;
                        r = Pixels8RedCurrent[w1];
                        g = Pixels8GreenCurrent[w1];
                        b = Pixels8BlueCurrent[w1];

                        // Target pixel
                        w1 = CurrentWidth * el + k;
                        Pixels8RedResult[w1] = r;
                        Pixels8GreenResult[w1] = g;
                        Pixels8BlueResult[w1] = b;
                    }
                }
            }
            return UpdateImage();
        }

        /// <summary>
        /// Get options supported by algorithm
        /// </summary>
        /// <returns></returns>
        public override IList<AlgorithmOption> GetOptions() {
            Dictionary<AlgorithmParameter, string> coarseness = new Dictionary<AlgorithmParameter, string>();
            coarseness.Add(new RangeAlgorithmParameter()
            {
                Value = 0,
                ParameterName = "Coarseness",
                Minimum = 3,
                Maximum = 30
            }, string.Empty);
            Options.Add(new AlgorithmOption(InputType.SingleInput, coarseness));

            Dictionary<AlgorithmParameter, string> options = new Dictionary<AlgorithmParameter, string>();
            options.Add(new AlgorithmParameter()
            {
                Value = 1,
                ParameterName = "Method"
            }, "Horiz");
            options.Add(new AlgorithmParameter()
            {
                Value = 2,
                ParameterName = "Method"
            }, "Vert");
            options.Add(new AlgorithmParameter()
            {
                Value = 3,
                ParameterName = "Method"
            }, "Both");
            Options.Add(new AlgorithmOption(InputType.MultipleChoice, options));
            return Options;
        }

        /// <summary>
        /// Get display information about the algorithm
        /// </summary>
        /// <returns></returns>
        public override string GetDisplayInfo() {
            return "Ground Glass Effect for image";
        }
        #endregion

        #region Private Methods
        int findXInBound(int x) {
            int x1 = x;

            if (x < 0)
                x1 = 0;
            else if (x >= CurrentWidth)
                x1 = CurrentWidth - 1;

            return x1;
        }

        int findYInBound(int y) {
            int y1 = y;

            if (y < 0)
                y1 = 0;
            else if (y >= CurrentHeight)
                y1 = CurrentHeight - 1;

            return y1;
        }

        string SliderName {
            get {
                return "Coarseness:";
            }
        }
        #endregion
    }
}