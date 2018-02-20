// Cool Image Effects

using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace CoolImageEffects {
    public static class FileOperations {

        #region Public Methods
        public static string ShowFileDialogue(ImageSource image, String fileName = "") {
            SaveFileDialog saveFile = new SaveFileDialog();
            try {
                saveFile.Filter = "PNG Images (.png)|*.png|JPG Images (.jpg)|*.jpg|BMP Images (.bmp)|*.bmp";
                saveFile.InitialDirectory = Convert.ToString(Environment.SpecialFolder.MyPictures);
                saveFile.FileName = fileName;
                if (!String.IsNullOrEmpty(fileName)) {
                    var extn = Path.GetExtension(fileName).ToLower();
                    if (extn == ".jpg") {
                        saveFile.FilterIndex = 2;
                    } else if (extn == ".bmp") {
                        saveFile.FilterIndex = 3;
                    }
                }
                if (saveFile.ShowDialog() == false) {
                    saveFile.FileName = string.Empty;
                }
            } catch {
            }
            return saveFile.FileName;
        }

        public static void SaveImageFile(ImageSource image, string fileNameToSave) {
            // Then, save the image
            string extn = Path.GetExtension(fileNameToSave).ToLower();
            using (FileStream fs = new FileStream(fileNameToSave, FileMode.Create)) {
                if (extn == ".png") {
                    // Save as PNG
                    BitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(image as BitmapSource));
                    encoder.Save(fs);
                } else if (extn == ".jpg" || extn == ".jpeg") {
                    // Save as JPG
                    BitmapEncoder encoder = new JpegBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(image as BitmapSource));
                    encoder.Save(fs);
                } else {
                    // Save as BMP
                    BitmapEncoder encoder = new BmpBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(image as BitmapSource));
                    encoder.Save(fs);
                }
                fs.Close();
            }
        }

        public static string SelectFile() {
            var fileName = string.Empty;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All files (*.*)|*.*|PNG Images (.png)|*.png|JPG Images (.jpg)|*.jpg|BMP Images (.bmp)|*.bmp";
            if (openFileDialog.ShowDialog() == true) {
                fileName = openFileDialog.FileName;
                if (!ValidateFile(fileName)) {
                    fileName = string.Empty;
                }
            }
            return fileName;
        }

        /// <summary>
        /// Validate the file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool ValidateFile(string fileName) {
            bool result = false;
            FileInfo fileInfo = new FileInfo(fileName);
            if (fileInfo != null) {
                if (fileInfo.Extension == ".jpg" || fileInfo.Extension == ".png" || fileInfo.Extension == ".bmp" || fileInfo.Extension == ".JPG") {
                    result = true;
                }
            }
            return result;
        }
        #endregion
    }
}