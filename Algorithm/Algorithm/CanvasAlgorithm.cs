// Cool Image Effects

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Algorithm {
    // This method is inspired from http://www.pegtop.net/delphi/articles/blendmodes/softlight.htm
    // Peg Top Light effect on that page.
    // Some of the textures are taken from the internet.
    public class CanvasAlgorithm : AlgorithmBase {
        #region Private Fields
        byte[] texturePixels;
        BitmapSource textureImg;
        int textureWidth, textureHeight;
        #endregion

        #region Private Methods
        void ReadTextureImage(int textureType) {
            Uri imageUri;
            if (textureType == 1) {
                imageUri = new Uri("External\\texture_1.png", UriKind.Relative);
            } else if (textureType == 2) {
                imageUri = new Uri("External\\texture_2.png", UriKind.Relative);
            } else if (textureType == 3) {
                imageUri = new Uri("External\\texture_3.png", UriKind.Relative);
            } else if (textureType == 4) {
                imageUri = new Uri("External\\texture_4.png", UriKind.Relative);
            } else {
                imageUri = new Uri("External\\texture_5.png", UriKind.Relative);
            }

            textureImg = new BitmapImage(imageUri);
            int stride = (textureImg.PixelWidth * textureImg.Format.BitsPerPixel + 7) / 8;
            int origStride = stride;
            textureWidth = textureImg.PixelWidth;
            textureHeight = textureImg.PixelHeight;
            int iNumberOfPixels = textureWidth * textureHeight;

            if ((textureImg.Format == PixelFormats.Bgra32) ||
                (textureImg.Format == PixelFormats.Bgr32)) {
                texturePixels = new byte[stride * textureHeight];
                // Read in pixel values from the image
                textureImg.CopyPixels(Int32Rect.Empty, texturePixels, stride, 0);
            }
        }

        static Dictionary<AlgorithmParameter, string> GetMethodOptions() {
            var options = new Dictionary<AlgorithmParameter, string>();
            options.Add(new AlgorithmParameter()
            {
                Value = 1
            }, "Option 1");
            options.Add(new AlgorithmParameter()
            {
                Value = 2
            }, "Option 2");
            options.Add(new AlgorithmParameter()
            {
                Value = 3
            }, "Option 3");
            options.Add(new AlgorithmParameter()
            {
                Value = 4
            }, "Option 4");
            options.Add(new AlgorithmParameter()
            {
                Value = 5
            }, "Option 5");
            return options;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Returns options supported by this algorithm
        /// </summary>
        /// <returns></returns>
        public override IList<AlgorithmOption> GetOptions() {
            var options = GetMethodOptions();
            Options.Add(new AlgorithmOption(InputType.MultipleChoice, options));
            return Options;
        }

        /// <summary>
        /// Canvas effect
        /// </summary>
        /// <param name="algorithmParameter"></param>
        /// <param name="isSave"></param>
        /// <returns></returns>
        public override BitmapSource ApplyEffect(List<AlgorithmParameter> algorithmParameter, bool isSave = false) {
            SetData(isSave);
            ReadTextureImage(algorithmParameter[0].Value);

            int k, el;
            byte r, g, b;
            int w1, w2;
            byte resultRed, resultGreen, resultBlue;

            // Target image
            for (el = 0; el < CurrentHeight; ++el) {
                w2 = CurrentWidth * el;
                for (k = 0; k < CurrentWidth; ++k) {
                    w1 = w2 + k;

                    // Source pixel
                    r = Pixels8RedCurrent[w1];
                    g = Pixels8GreenCurrent[w1];
                    b = Pixels8BlueCurrent[w1];

                    int widMod = k % textureWidth;
                    int heightMod = el % textureHeight;

                    //int index = textureWidth * heightMod * 4 + widMod * 4;
                    int bVal = texturePixels[textureWidth * heightMod * 4 + widMod * 4];

                    if (bVal < 128) {
                        resultRed = (byte)((bVal * r) >> 7);
                        resultGreen = (byte)((bVal * g) >> 7);
                        resultBlue = (byte)((bVal * b) >> 7);
                    } else {
                        resultRed = (byte)(255 - (((255 - bVal) * (255 - r)) >> 7));
                        resultGreen = (byte)(255 - (((255 - bVal) * (255 - g)) >> 7));
                        resultBlue = (byte)(255 - (((255 - bVal) * (255 - b)) >> 7));
                    }

                    Pixels8RedResult[w1] = resultRed;
                    Pixels8GreenResult[w1] = resultGreen;
                    Pixels8BlueResult[w1] = resultBlue;
                }
            }
            return UpdateImage();
        }

        /// <summary>
        /// Gets information about the algorithm
        /// </summary>
        /// <returns></returns>
        public override string GetDisplayInfo() {
            return "Texture for your image";
        }
        #endregion
    }
}