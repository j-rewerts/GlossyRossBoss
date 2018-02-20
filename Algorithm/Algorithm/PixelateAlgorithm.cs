// Cool Image Effects

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Algorithm {
    /// <summary>
    /// Pixelate Algorithm
    /// Our own algorithm / code
    /// </summary>
    public class PixelateAlgorithm : AlgorithmBase {
        int noHorizBlocks;
        int noVertBlocks;
        int blockWidth;
        int blockHeight;
        Color borderColour;
        bool noBorder = false;

        List<int> blockWidths;
        List<int> blockHeights;
        List<int> shuffledVertList;
        List<int> shuffledHorizList;

        #region Public Methods
        /// <summary>
        /// Pixelate effect
        /// </summary>
        /// <param name="algorithmParameter"></param>
        /// <param name="isSave"></param>
        /// <returns></returns>
        public override BitmapSource ApplyEffect(List<AlgorithmParameter> algorithmParameter, bool isSave = false) {
            SetData(isSave);
            var userBlockWidth = algorithmParameter.First(x => x.ParameterName == "BlockWidth");
            var userBlockHeight = algorithmParameter.First(x => x.ParameterName == "BlockHeight");
            SetupBlockGeometry(algorithmParameter);
            SetupBorderColour(algorithmParameter);
            CreateBlockyImage();
            return UpdateImage();
        }

        /// <summary>
        /// Get the options for the Pixelate Algorithm
        /// </summary>
        public override IList<AlgorithmOption> GetOptions() {
            Dictionary<AlgorithmParameter, string> blockWidth = new Dictionary<AlgorithmParameter, string>();
            blockWidth.Add(new RangeAlgorithmParameter()
            {
                Value = 0,
                ParameterName = "BlockWidth",
                Minimum = 12,
                Maximum = 150
            }, string.Empty);
            Options.Add(new AlgorithmOption(InputType.SingleInput, blockWidth)
            {
                ParameterName = "BlockWidth"
            });

            Dictionary<AlgorithmParameter, string> blockHeight = new Dictionary<AlgorithmParameter, string>();
            blockHeight.Add(new RangeAlgorithmParameter()
            {
                Value = 0,
                ParameterName = "BlockHeight",
                Minimum = 12,
                Maximum = 150
            }, string.Empty);
            Options.Add(new AlgorithmOption(InputType.SingleInput, blockHeight)
            {
                ParameterName = "BlockHeight"
            });

            Dictionary<AlgorithmParameter, string> colourOption = new Dictionary<AlgorithmParameter, string>();
            colourOption.Add(new AlgorithmParameter()
            {
                Value = 5,
                ParameterName = "BorderColour"
            }, "Black");
            colourOption.Add(new AlgorithmParameter()
            {
                Value = 6,
                ParameterName = "BorderColour"
            }, "White");
            colourOption.Add(new AlgorithmParameter()
            {
                Value = 1,
                ParameterName = "BorderColour"
            }, "Red");
            colourOption.Add(new AlgorithmParameter()
            {
                Value = 2,
                ParameterName = "BorderColour"
            }, "Green");
            colourOption.Add(new AlgorithmParameter()
            {
                Value = 3,
                ParameterName = "BorderColour"
            }, "Blue");
            colourOption.Add(new AlgorithmParameter()
            {
                Value = 4,
                ParameterName = "BorderColour"
            }, "Gray");
            colourOption.Add(new AlgorithmParameter()
            {
                Value = 7,
                ParameterName = "BorderColour"
            }, "None");
            Options.Add(new AlgorithmOption(InputType.MultipleChoice, colourOption)
            {
                ParameterName = "BorderColour"
            });

            return Options;
        }

        public override string GetDisplayInfo() {
            return "Pixelates the image";
        }
        #endregion

        #region Private Methods
        void CreateBlockyImage() {
            /* Loop over number of vertical blocks
             *   Loop over number of horizontal blocks
             *      Form red, green, blue blocks
             *      Compute red, green, blue averages
             *      Set appropriate pixels in red, green, blue blocks to update the image
             */
            long redTotal, greenTotal, blueTotal;
            int bw, bh, index1, index2, area;
            int blockTopLeftX = 0, blockTopLeftY = 0, pixelX, pixelY;
            byte red, green, blue;

            for (int el = 0; el < noVertBlocks; ++el) {
                bh = blockHeights[el];
                blockTopLeftX = 0;

                for (int k = 0; k < noHorizBlocks; ++k) {
                    bw = blockWidths[k];
                    area = bh * bw;

                    // Loop over each block to compute the average
                    redTotal = greenTotal = blueTotal = 0;
                    for (int j1 = 0; j1 < bh; ++j1) {
                        pixelY = blockTopLeftY + j1;
                        index1 = pixelY * CurrentWidth;

                        for (int i1 = 0; i1 < bw; ++i1) {
                            pixelX = blockTopLeftX + i1;
                            index2 = index1 + pixelX;
                            redTotal += Pixels8RedCurrent[index2];
                            greenTotal += Pixels8GreenCurrent[index2];
                            blueTotal += Pixels8BlueCurrent[index2];
                        }
                    }

                    red = Convert.ToByte((1.0 * redTotal) / area);
                    green = Convert.ToByte((1.0 * greenTotal) / area);
                    blue = Convert.ToByte((1.0 * blueTotal) / area);

                    // Now, assign these average values into the modified arrays.
                    // Take the border colour into account while doing so.
                    for (int j1 = 0; j1 < bh; ++j1) {
                        pixelY = blockTopLeftY + j1;
                        index1 = pixelY * CurrentWidth;

                        for (int i1 = 0; i1 < bw; ++i1) {
                            pixelX = blockTopLeftX + i1;
                            index2 = index1 + pixelX;

                            if ((noBorder == false) && ((j1 * i1 == 0) ||
                              (((el == noVertBlocks - 1) && (j1 == bh - 1)) ||
                              ((k == noHorizBlocks - 1) && (i1 == bw - 1))))) {
                                /* This is the code to create a border around each block. 
                                 * The top row and left column of a block gets the 
                                 * border colour. However, some conditions are needed to 
                                 * add the bottom-most row, and right-most column also.
                                 */
                                Pixels8RedResult[index2] = borderColour.R;
                                Pixels8GreenResult[index2] = borderColour.G;
                                Pixels8BlueResult[index2] = borderColour.B;
                            } else {
                                Pixels8RedResult[index2] = red;
                                Pixels8GreenResult[index2] = green;
                                Pixels8BlueResult[index2] = blue;
                            }
                        }
                    }
                    blockTopLeftX += bw;
                }
                blockTopLeftY += bh;
            }
        }

        // Setting up the block widths and heights
        void SetupBlockGeometry(List<AlgorithmParameter> value) {
            var userBlockWidth = value.First(x => x.ParameterName == "BlockWidth");
            var userBlockHeight = value.First(x => x.ParameterName == "BlockHeight");
            noHorizBlocks = userBlockWidth.Value;
            noVertBlocks = userBlockHeight.Value;

            if (shuffledVertList != null)
                shuffledHorizList.Clear();
            if (shuffledVertList != null)
                shuffledVertList.Clear();

            if (noHorizBlocks >= CurrentWidth)
                noHorizBlocks = CurrentWidth / 2;
            if (noVertBlocks >= CurrentHeight)
                noVertBlocks = CurrentHeight / 2;

            blockWidth = CurrentWidth / noHorizBlocks;
            blockHeight = CurrentHeight / noVertBlocks;
            blockWidths = Enumerable.Repeat(blockWidth, noHorizBlocks).ToList();
            blockHeights = Enumerable.Repeat(blockHeight, noVertBlocks).ToList();

            // For the horizontal blocks
            var horizList = Enumerable.Range(0, noHorizBlocks - 1).ToList();
            // Shuffle the list, as suggested on StackOverflow site.
            Random random = new Random(Environment.TickCount);
            shuffledHorizList = horizList.OrderBy(k => random.Next()).ToList();
            int diffH = CurrentWidth - noHorizBlocks * blockWidth;
            if (diffH > 0) {
                ++blockWidths[noHorizBlocks - 1];
                for (int i = 0; i < diffH - 1; ++i) {
                    ++blockWidths[shuffledHorizList[i]];
                }
            }

            // Do the same thing for the vertical blocks
            var vertList = Enumerable.Range(0, noVertBlocks - 1).ToList();
            shuffledVertList = vertList.OrderBy(k => random.Next()).ToList();
            int diffV = CurrentHeight - noVertBlocks * blockHeight;
            if (diffV > 0) {
                ++blockHeights[noVertBlocks - 1];
                for (int i = 0; i < diffV - 1; ++i) {
                    ++blockHeights[shuffledVertList[i]];
                }
            }
        }

        // Setting up the border colour
        void SetupBorderColour(List<AlgorithmParameter> value) {
            noBorder = false;
            var userBorderColour = value.First(x => x.ParameterName == "BorderColour");
            if (userBorderColour.Value < 7) {
                borderColour = colourArray[userBorderColour.Value - 1];
            } else {
                noBorder = true;
            }
        }
        #endregion
    }
}