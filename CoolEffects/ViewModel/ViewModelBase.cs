// Cool Image Effects

using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Algorithm;

namespace CoolImageEffects.ViewModel {
    public abstract class ViewModelBase : INotifyPropertyChanged {

        #region Private Fields
        ImageSource outputImage;
        ImageSource inputImage;
        #endregion

        #region Protected Fields
        protected IImageProcessingAlgorithm ImageProcessingAlgorithm;
        protected string Message;
        protected IList<AlgorithmOption> AlgorithmOptions;
        List<BitmapSource> previewImages = new List<BitmapSource>();
        #endregion

        protected ViewModelBase() {
            ImageProcessingAlgorithm = new ImageProcessingAlgorithm();
        }
        #region Public Fields
        /// <summary>
        /// The resultant image after computation
        /// </summary>
        public ImageSource OutputImage {
            get {
                return outputImage;
            }
            set {
                outputImage = value;
                NotifyPropertyChanged("OutputImage");
            }
        }

        /// <summary>
        /// Input image to be processed
        /// </summary>
        public ImageSource InputImage {
            get {
                return inputImage;
            }
            set {
                inputImage = value;
                NotifyPropertyChanged("InputImage");
            }
        }

        public string DisplayInfo {
            get {
                return ImageProcessingAlgorithm.GetDisplayInfo();
            }
        }

        /// <summary>
        /// Method to be implemented by respective view models
        /// </summary>
        /// <param name="fileName"></param>
        public abstract void LoadImage(string fileName);
        public abstract ImageSource SaveImage();
        #endregion

        #region INotifyPropertyChanged Members
        /// <summary>
        /// Notifies clients that a property value has changed.
        /// </summary>
        /// <param name="propertyName">The name of the property that has changed.</param>
        public void NotifyPropertyChanged(string propertyName) {
            // icon = null;
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Indicates that the property has changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}