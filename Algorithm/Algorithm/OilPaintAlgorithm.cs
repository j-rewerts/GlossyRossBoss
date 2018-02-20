// Cool Image Effects

using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Algorithm {
    /// <summary>
    /// Oil Paint Algorithm
    /// </summary>
    class OilPaintAlgorithm : AlgorithmBase {
        // Based on the Lecture Notes by Prof. Onur Guleryuz of Polytechnic University, New York. 

        #region Private Methods
        // Here, the word "mode" means "Statistical Mode", as defined in
        //   http://en.wikipedia.org/wiki/Mode_(statistics)
        // We've adapted this method from here:
        //  http://www.dreamincode.net/forums/topic/70023-statistical-mode-algorithm-for-c%23/
        ArrayList CalculateMode(ref int[] array) {
            // Use an arraylist to return mode values as strings! This way we can            
            // return 3 kinds if information:            
            //    1) if there is no mode, return NONE            
            //    2) lets us return any integer for the mode including 0 (ZERO)            
            //    3) lets us return more than one mode value (BIMODAL)            

            ArrayList mode = new ArrayList();
            // We need a copy of input array because we are going to SORT it.            
            int[] sortArray = new int[array.Length];

            // Array to hold frequency counts. Must be sized to the input array            
            // size as the input array may have totally unique values (no mode 8-).            
            // array contains value, count            
            int[,] frequency = new int[array.Length, 2];

            // Copy the input array to our temporary so we can sort it.            
            Array.Copy(array, sortArray, array.Length);

            // Sort the array to put it in ascending sequence.            
            Array.Sort(sortArray);

            // Now we can examine the sorted array keeping track of the            
            // number of times each value occurs.            
            int tmp = sortArray[0]; // beginning value            
            int index = 0; // index into frequency array            
            int i = 0;
            while (i < sortArray.Length) {
                frequency[index, 0] = tmp; // copy value                
                while (tmp.Equals(sortArray[i])) {
                    frequency[index, 1]++;  // count occurences                    
                    i++;
                    if (i > sortArray.Length - 1) // don't overrun array!                        
                        break;
                }
                if (i < sortArray.Length) {
                    tmp = sortArray[i]; // move to next value                    
                    index++;
                }
            }            // next we copy and sort the frequencies            

            Array.Clear(sortArray, 0, sortArray.Length); // zero it out            

            for (i = 0; i < sortArray.Length; i++)
                sortArray[i] = frequency[i, 1]; // copy the occurance values            

            Array.Sort(sortArray); // ascending order            
            index = sortArray.Length - 1;
            switch (sortArray[index]) {
                case 0:
                    mode.Add("Nil");
                    break;
                case 1:
                    mode.Add("Nil");
                    break;
                default:
                    for (i = 0; i < frequency.Length / frequency.Rank; i++)
                        if (frequency[i, 1].Equals(sortArray[index]))
                            mode.Add(frequency[i, 0].ToString()); // convert mode integer to a string                    
                    break;
            }
            return mode;
        }

        /// <summary>
        /// Get options 
        /// </summary>
        /// <returns></returns>
        static Dictionary<AlgorithmParameter, string> GetMethods() {
            var options = new Dictionary<AlgorithmParameter, string>();
            options.Add(new AlgorithmParameter()
            {
                Value = 1,
                ParameterName = "Method"
            }, "3 X 3");
            options.Add(new AlgorithmParameter()
            {
                Value = 2,
                ParameterName = "Method"
            }, "5 X 5");
            return options;
        }

        // 3x3 Oil Paint Filter
        void OilPaint3x3() {
            // Using a 3 x 3 filter
            int[] pixRed = new int[9];
            int[] pixGreen = new int[9];
            int[] pixBlue = new int[9];
            int im1, i1, ip1, jm1, j1, jp1, jw1, jw2, jw3, i, j;
            int p11, p12, p13,
                p21, p22, p23,
                p31, p32, p33;
            ArrayList modeRed = new ArrayList();
            ArrayList modeGreen = new ArrayList();
            ArrayList modeBlue = new ArrayList();
            int modeValRed, modeValGreen, modeValBlue;

            for (j = 0; j < CurrentHeight; ++j) {
                jm1 = j - 1;
                j1 = j;
                jp1 = j + 1;

                if (jm1 < 0)
                    jm1 = 0;
                if (jp1 >= CurrentHeight)
                    jp1 = CurrentHeight - 1;
                jw1 = jm1 * CurrentWidth;
                jw2 = j1 * CurrentWidth;
                jw3 = jp1 * CurrentWidth;

                for (i = 0; i < CurrentWidth; ++i) {
                    im1 = i - 1;
                    i1 = i;
                    ip1 = i + 1;

                    if (im1 < 0)
                        im1 = 0;
                    if (ip1 >= CurrentWidth)
                        ip1 = CurrentWidth - 1;

                    p11 = jw1 + im1;
                    p12 = jw1 + i1;
                    p13 = jw1 + ip1;

                    p21 = jw2 + im1;
                    p22 = jw2 + i1;
                    p23 = jw2 + ip1;

                    p31 = jw3 + im1;
                    p32 = jw3 + i1;
                    p33 = jw3 + ip1;

                    pixRed[0] = Pixels8RedCurrent[p11];
                    pixRed[1] = Pixels8RedCurrent[p12];
                    pixRed[2] = Pixels8RedCurrent[p13];
                    pixRed[3] = Pixels8RedCurrent[p21];
                    pixRed[4] = Pixels8RedCurrent[p22];
                    pixRed[5] = Pixels8RedCurrent[p23];
                    pixRed[6] = Pixels8RedCurrent[p31];
                    pixRed[7] = Pixels8RedCurrent[p32];
                    pixRed[8] = Pixels8RedCurrent[p33];

                    pixGreen[0] = Pixels8GreenCurrent[p11];
                    pixGreen[1] = Pixels8GreenCurrent[p12];
                    pixGreen[2] = Pixels8GreenCurrent[p13];
                    pixGreen[3] = Pixels8GreenCurrent[p21];
                    pixGreen[4] = Pixels8GreenCurrent[p22];
                    pixGreen[5] = Pixels8GreenCurrent[p23];
                    pixGreen[6] = Pixels8GreenCurrent[p31];
                    pixGreen[7] = Pixels8GreenCurrent[p32];
                    pixGreen[8] = Pixels8GreenCurrent[p33];

                    pixBlue[0] = Pixels8BlueCurrent[p11];
                    pixBlue[1] = Pixels8BlueCurrent[p12];
                    pixBlue[2] = Pixels8BlueCurrent[p13];
                    pixBlue[3] = Pixels8BlueCurrent[p21];
                    pixBlue[4] = Pixels8BlueCurrent[p22];
                    pixBlue[5] = Pixels8BlueCurrent[p23];
                    pixBlue[6] = Pixels8BlueCurrent[p31];
                    pixBlue[7] = Pixels8BlueCurrent[p32];
                    pixBlue[8] = Pixels8BlueCurrent[p33];

                    modeRed = CalculateMode(ref pixRed);
                    modeGreen = CalculateMode(ref pixGreen);
                    modeBlue = CalculateMode(ref pixBlue);

                    if (modeRed[0].ToString() == "Nil")
                        modeValRed = pixRed[4]; // Centre pixel
                    else
                        modeValRed = Convert.ToInt32(modeRed[0].ToString());

                    if (modeGreen[0].ToString() == "Nil")
                        modeValGreen = pixGreen[4]; // Centre pixel
                    else
                        modeValGreen = Convert.ToInt32(modeGreen[0].ToString());

                    if (modeBlue[0].ToString() == "Nil")
                        modeValBlue = pixBlue[4]; // Centre pixel
                    else
                        modeValBlue = Convert.ToInt32(modeBlue[0].ToString());

                    // Target pixel                    
                    Pixels8RedResult[p22] = (byte)(modeValRed);
                    Pixels8GreenResult[p22] = (byte)(modeValGreen);
                    Pixels8BlueResult[p22] = (byte)(modeValBlue);

                }
            }
            modeRed.Clear();
            modeGreen.Clear();
            modeBlue.Clear();
        }

        // 5x5 Oil Paint Filter
        void OilPaint5x5() {
            // Using a 5 x 5 filter
            int[] pixRed = new int[25];
            int[] pixGreen = new int[25];
            int[] pixBlue = new int[25];
            int im2, im1, i1, ip1, ip2, jm2, jm1, j1, jp1, jp2, jw1, jw2, jw3, jw4, jw5, i, j;
            int p11, p12, p13, p14, p15,
                p21, p22, p23, p24, p25,
                p31, p32, p33, p34, p35,
                p41, p42, p43, p44, p45,
                p51, p52, p53, p54, p55;
            ArrayList modeRed = new ArrayList();
            ArrayList modeGreen = new ArrayList();
            ArrayList modeBlue = new ArrayList();
            int modeValRed, modeValGreen, modeValBlue;

            for (j = 0; j < CurrentHeight; ++j) {
                jm2 = j - 2;
                jm1 = j - 1;
                j1 = j;
                jp1 = j + 1;
                jp2 = j + 2;

                if (jm2 < 0)
                    jm2 = 0;
                if (jm1 < 0)
                    jm1 = 0;
                if (jp1 >= CurrentHeight)
                    jp1 = CurrentHeight - 1;
                if (jp2 >= CurrentHeight)
                    jp2 = CurrentHeight - 1;
                jw1 = jm2 * CurrentWidth;
                jw2 = jm1 * CurrentWidth;
                jw3 = j1 * CurrentWidth;
                jw4 = jp1 * CurrentWidth;
                jw5 = jp2 * CurrentWidth;

                for (i = 0; i < CurrentWidth; ++i) {
                    im2 = i - 2;
                    im1 = i - 1;
                    i1 = i;
                    ip1 = i + 1;
                    ip2 = i + 2;

                    if (im2 < 0)
                        im2 = 0;
                    if (im1 < 0)
                        im1 = 0;
                    if (ip1 >= CurrentWidth)
                        ip1 = CurrentWidth - 1;
                    if (ip2 >= CurrentWidth)
                        ip2 = CurrentWidth - 1;

                    p11 = jw1 + im2;
                    p12 = jw1 + im1;
                    p13 = jw1 + i1;
                    p14 = jw1 + ip1;
                    p15 = jw1 + ip2;

                    p21 = jw2 + im2;
                    p22 = jw2 + im1;
                    p23 = jw2 + i1;
                    p24 = jw2 + ip1;
                    p25 = jw2 + ip2;

                    p31 = jw3 + im2;
                    p32 = jw3 + im1;
                    p33 = jw3 + i1;
                    p34 = jw3 + ip1;
                    p35 = jw3 + ip2;

                    p41 = jw4 + im2;
                    p42 = jw4 + im1;
                    p43 = jw4 + i1;
                    p44 = jw4 + ip1;
                    p45 = jw4 + ip2;

                    p51 = jw5 + im2;
                    p52 = jw5 + im1;
                    p53 = jw5 + i1;
                    p54 = jw5 + ip1;
                    p55 = jw5 + ip2;

                    pixRed[0] = Pixels8RedCurrent[p11];
                    pixRed[1] = Pixels8RedCurrent[p12];
                    pixRed[2] = Pixels8RedCurrent[p13];
                    pixRed[3] = Pixels8RedCurrent[p14];
                    pixRed[4] = Pixels8RedCurrent[p15];
                    pixRed[5] = Pixels8RedCurrent[p21];
                    pixRed[6] = Pixels8RedCurrent[p22];
                    pixRed[7] = Pixels8RedCurrent[p23];
                    pixRed[8] = Pixels8RedCurrent[p24];
                    pixRed[9] = Pixels8RedCurrent[p25];
                    pixRed[10] = Pixels8RedCurrent[p31];
                    pixRed[11] = Pixels8RedCurrent[p32];
                    pixRed[12] = Pixels8RedCurrent[p33];
                    pixRed[13] = Pixels8RedCurrent[p34];
                    pixRed[14] = Pixels8RedCurrent[p35];
                    pixRed[15] = Pixels8RedCurrent[p41];
                    pixRed[16] = Pixels8RedCurrent[p42];
                    pixRed[17] = Pixels8RedCurrent[p43];
                    pixRed[18] = Pixels8RedCurrent[p44];
                    pixRed[19] = Pixels8RedCurrent[p45];
                    pixRed[20] = Pixels8RedCurrent[p51];
                    pixRed[21] = Pixels8RedCurrent[p52];
                    pixRed[22] = Pixels8RedCurrent[p53];
                    pixRed[23] = Pixels8RedCurrent[p54];
                    pixRed[24] = Pixels8RedCurrent[p55];

                    pixGreen[0] = Pixels8GreenCurrent[p11];
                    pixGreen[1] = Pixels8GreenCurrent[p12];
                    pixGreen[2] = Pixels8GreenCurrent[p13];
                    pixGreen[3] = Pixels8GreenCurrent[p14];
                    pixGreen[4] = Pixels8GreenCurrent[p15];
                    pixGreen[5] = Pixels8GreenCurrent[p21];
                    pixGreen[6] = Pixels8GreenCurrent[p22];
                    pixGreen[7] = Pixels8GreenCurrent[p23];
                    pixGreen[8] = Pixels8GreenCurrent[p24];
                    pixGreen[9] = Pixels8GreenCurrent[p25];
                    pixGreen[10] = Pixels8GreenCurrent[p31];
                    pixGreen[11] = Pixels8GreenCurrent[p32];
                    pixGreen[12] = Pixels8GreenCurrent[p33];
                    pixGreen[13] = Pixels8GreenCurrent[p34];
                    pixGreen[14] = Pixels8GreenCurrent[p35];
                    pixGreen[15] = Pixels8GreenCurrent[p41];
                    pixGreen[16] = Pixels8GreenCurrent[p42];
                    pixGreen[17] = Pixels8GreenCurrent[p43];
                    pixGreen[18] = Pixels8GreenCurrent[p44];
                    pixGreen[19] = Pixels8GreenCurrent[p45];
                    pixGreen[20] = Pixels8GreenCurrent[p51];
                    pixGreen[21] = Pixels8GreenCurrent[p52];
                    pixGreen[22] = Pixels8GreenCurrent[p53];
                    pixGreen[23] = Pixels8GreenCurrent[p54];
                    pixGreen[24] = Pixels8GreenCurrent[p55];

                    pixBlue[0] = Pixels8BlueCurrent[p11];
                    pixBlue[1] = Pixels8BlueCurrent[p12];
                    pixBlue[2] = Pixels8BlueCurrent[p13];
                    pixBlue[3] = Pixels8BlueCurrent[p14];
                    pixBlue[4] = Pixels8BlueCurrent[p15];
                    pixBlue[5] = Pixels8BlueCurrent[p21];
                    pixBlue[6] = Pixels8BlueCurrent[p22];
                    pixBlue[7] = Pixels8BlueCurrent[p23];
                    pixBlue[8] = Pixels8BlueCurrent[p24];
                    pixBlue[9] = Pixels8BlueCurrent[p25];
                    pixBlue[10] = Pixels8BlueCurrent[p31];
                    pixBlue[11] = Pixels8BlueCurrent[p32];
                    pixBlue[12] = Pixels8BlueCurrent[p33];
                    pixBlue[13] = Pixels8BlueCurrent[p34];
                    pixBlue[14] = Pixels8BlueCurrent[p35];
                    pixBlue[15] = Pixels8BlueCurrent[p41];
                    pixBlue[16] = Pixels8BlueCurrent[p42];
                    pixBlue[17] = Pixels8BlueCurrent[p43];
                    pixBlue[18] = Pixels8BlueCurrent[p44];
                    pixBlue[19] = Pixels8BlueCurrent[p45];
                    pixBlue[20] = Pixels8BlueCurrent[p51];
                    pixBlue[21] = Pixels8BlueCurrent[p52];
                    pixBlue[22] = Pixels8BlueCurrent[p53];
                    pixBlue[23] = Pixels8BlueCurrent[p54];
                    pixBlue[24] = Pixels8BlueCurrent[p55];

                    modeRed = CalculateMode(ref pixRed);
                    modeGreen = CalculateMode(ref pixGreen);
                    modeBlue = CalculateMode(ref pixBlue);

                    if (modeRed[0].ToString() == "Nil")
                        modeValRed = pixRed[12]; // Centre pixel
                    else
                        modeValRed = Convert.ToInt32(modeRed[0].ToString());

                    if (modeGreen[0].ToString() == "Nil")
                        modeValGreen = pixGreen[12]; // Centre pixel
                    else
                        modeValGreen = Convert.ToInt32(modeGreen[0].ToString());

                    if (modeBlue[0].ToString() == "Nil")
                        modeValBlue = pixBlue[12]; // Centre pixel
                    else
                        modeValBlue = Convert.ToInt32(modeBlue[0].ToString());

                    // Target pixel                    
                    Pixels8RedResult[p33] = (byte)(modeValRed);
                    Pixels8GreenResult[p33] = (byte)(modeValGreen);
                    Pixels8BlueResult[p33] = (byte)(modeValBlue);
                }
            }
            modeRed.Clear();
            modeGreen.Clear();
            modeBlue.Clear();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Gets options for this algorithm
        /// </summary>
        /// <returns></returns>
        public override IList<AlgorithmOption> GetOptions() {
            Options.Add(new AlgorithmOption(InputType.MultipleChoice, GetMethods()));
            return Options;
        }



        /// <summary>
        /// Gets the display info for this algorithm
        /// </summary>
        /// <returns></returns>
        public override string GetDisplayInfo() {
            return "Oil Paint effect on image";
        }

        /// <summary>
        /// Apply oil paint effect on image
        /// </summary>
        /// <param name="algorithmParameter"></param>
        /// <returns></returns>
        public override BitmapSource ApplyEffect(List<AlgorithmParameter> algorithmParameter, bool isSave = false) {
            SetData(isSave);
            Mouse.OverrideCursor = Cursors.Wait;
            int option = algorithmParameter[0].Value;
            if (option == 1) {
                OilPaint3x3();
            } else {
                OilPaint5x5();
            }

            Mouse.OverrideCursor = null;
            return UpdateImage();
        }
        #endregion
    }
}