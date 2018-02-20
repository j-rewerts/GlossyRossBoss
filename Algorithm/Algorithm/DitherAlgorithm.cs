// Cool Image Effects

using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;

namespace Algorithm {
    class DitherAlgorithm : AlgorithmBase {

        #region Dither Matrices
        // Dither Matrices
        int[, ,] ditherMatrix = new int[10, 4, 4]  {
            { {96, 160, 208, 240}, {48, 112, 176, 224}, {16,  64, 128, 192}, { 0,  32,  80, 144} }, // Matrix1
            { {96, 128, 160,  16}, {32, 144, 224,  80}, {64, 208, 128,  48}, { 0, 176, 240, 112} }, // Matrix2
            { { 0, 224,  48, 208}, {176, 80, 128,  96}, {192, 32, 240,  16}, {112, 144, 64, 160} }, // Matrix3
            { { 16,240,  32, 192}, {144, 80, 160,  96}, { 48, 208,  0, 224}, {176, 112, 128, 64} }, // Matrix4
            { {176, 80, 144,  48}, {  0, 240, 208, 96}, {112, 192, 224, 16}, {32, 128,  64, 160} }, // Matrix5
            { { 16, 240, 32, 192}, {144,  80, 160, 96}, { 48, 208,  0, 224}, {176, 112, 128, 64} }, // Matrix6
            { { 240, 224, 208, 192}, {176, 160, 144, 128}, { 96, 112, 80, 64}, {0, 16, 32, 48} }, // Matrix7
            { { 240, 160, 0, 32}, {128, 208, 64, 112}, { 144, 192, 80, 96}, {224, 176, 16, 48} }, // Matrix8
            { { 0, 48, 128, 240}, {32, 112, 224, 176}, { 96, 208, 160, 80}, {192, 144, 64, 16} }, // Matrix9
            { { 192, 96, 32, 0}, {144, 208, 112, 48}, { 64, 160, 224, 128}, {16, 80, 176, 240} }  // Matrix10
        };
        #endregion

        #region Public Methods
        /// <summary>
        /// Gets options suported by algorithm
        /// </summary>
        /// <returns></returns>
        public override IList<AlgorithmOption> GetOptions() {
            Dictionary<AlgorithmParameter, string> option = GetMethods();
            Options.Add(new AlgorithmOption(InputType.MultipleChoice, option)
            {
                ParameterName = "Method"
            });

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
        /// Dither method
        /// 1. Compute the grayscale image
        /// 2. Apply Dither matrix
        /// </summary>
        /// <param name="algorithmParameter"></param>
        /// <param name="isSave"></param>
        /// <returns></returns>
        public override BitmapSource ApplyEffect(List<AlgorithmParameter> algorithmParameter, bool isSave = false) {
            SetData(isSave);
            ComputeGrayscaleImage();
            DitherImage(algorithmParameter, CurrentHeight, CurrentWidth);
            return UpdateImage();
        }

        /// <summary>
        /// Gets information about the algorithm
        /// </summary>
        /// <returns></returns>
        public override string GetDisplayInfo() {
            return "Dither Effect on the image";
        }
        #endregion

        #region Private Methods
        static Dictionary<AlgorithmParameter, string> GetMethods() {
            Dictionary<AlgorithmParameter, string> option = new Dictionary<AlgorithmParameter, string>();
            option.Add(new AlgorithmParameter()
            {
                Value = 1,
                ParameterName = "Method"
            }, "Method 1");
            option.Add(new AlgorithmParameter()
            {
                Value = 2,
                ParameterName = "Method"
            }, "Method 2");
            option.Add(new AlgorithmParameter()
            {
                Value = 3,
                ParameterName = "Method"
            }, "Method 3");
            option.Add(new AlgorithmParameter()
            {
                Value = 4,
                ParameterName = "Method"
            }, "Method 4");
            option.Add(new AlgorithmParameter()
            {
                Value = 5,
                ParameterName = "Method"
            }, "Method 5");
            option.Add(new AlgorithmParameter()
            {
                Value = 6,
                ParameterName = "Method"
            }, "Method 6");
            option.Add(new AlgorithmParameter()
            {
                Value = 7,
                ParameterName = "Method"
            }, "Method 7");
            option.Add(new AlgorithmParameter()
            {
                Value = 8,
                ParameterName = "Method"
            }, "Method 8");
            option.Add(new AlgorithmParameter()
            {
                Value = 9,
                ParameterName = "Method"
            }, "Method 9");
            option.Add(new AlgorithmParameter()
            {
                Value = 10,
                ParameterName = "Method"
            }, "Method 10");
            return option;
        }

        void DitherImage(List<AlgorithmParameter> algorithmParameter, int height, int width, bool isSave = false) {
            var methodValue = algorithmParameter.First(x => x.ParameterName == "Method");
            var colour = algorithmParameter.First(x => x.ParameterName == "Colour");

            int i1, start, n = 4, threshold;
            byte value = 255;

            // This is the code for dithering
            for (int j = 0; j < height; ++j) {
                start = width * j;
                for (int i = 0; i < width; ++i) {
                    i1 = i + start;
                    threshold = ditherMatrix[methodValue.Value - 1, j % n, i % n];

                    if (PixGray[i1] > threshold) {
                        SetBackgroundColour(colour, i1, value);
                    } else {
                        SetBackgroundColour(colour, i1, 0);
                    }
                }
            }
        }
        #endregion
    }
}