// Cool Image Effects

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using CoolImageEffects.Command;
using CoolImageEffects.ViewModel;

namespace CoolImageEffects {
    public class ImageProcessingViewModel : INotifyPropertyChanged {
        #region Private Fields

        ICommand openFile;
        ICommand saveImageCommand;
        ICommand loadEffectCommand;
        List<String> effects;
        string inputFileName;
        string currentEffect;
        #endregion

        #region Public Fields and Methods
        /// <summary>
        /// Constructor
        /// </summary>
        public ImageProcessingViewModel() {
            openFile = new DelegateCommand(new Action<object>(SelectFile));
            loadEffectCommand = new DelegateCommand(new Action<object>(LoadEffectControl));
            saveImageCommand = new DelegateCommand(new Action<object>(SaveImage));

            var availableEffects = Enum.GetNames(typeof(Algorithm.Effects)).ToList();
            effects = availableEffects;
            LoadEffectControl(availableEffects[0]);
            CurrentEffect = effects[0];
            InputFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SampleImages", "kempegowda.png");
        }

        public string CurrentEffect {
            get {
                return currentEffect;
            }
            set {
                currentEffect = value;
                NotifyPropertyChanged("CurrentEffect");
                LoadEffectControl(currentEffect);
            }
        }

        /// <summary>
        /// File name for input image
        /// </summary>
        public string InputFileName {
            get {
                return inputFileName;
            }
            set {
                if (inputFileName != value) {
                    inputFileName = value;
                    if (!String.IsNullOrEmpty(inputFileName)) {
                        CurrentViewModel.LoadImage(inputFileName);
                    }
                    NotifyPropertyChanged("InputFileName");
                }
            }
        }

        /// <summary>
        /// Command to open file
        /// </summary>
        public ICommand OpenFile {
            get {
                return openFile;
            }
        }

        /// <summary>
        /// Command to load  the effects
        /// </summary>
        public ICommand LoadEffectCommand {
            get {
                return loadEffectCommand;
            }
        }

        /// <summary>
        /// List of available effects
        /// </summary>
        public List<String> Effects {
            get {
                return effects;
            }
        }

        ImageSource inputImage;
        /// <summary>
        /// Input Image
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

        /// <summary>
        /// Current viewModel
        /// </summary>
        public ViewModelBase CurrentViewModel {
            get {
                return currentViewModel;
            }
            set {
                currentViewModel = value;
                NotifyPropertyChanged("CurrentViewModel");
            }
        }

        /// <summary>
        /// Save image command
        /// </summary>
        public ICommand SaveImageCommand {
            get {
                return saveImageCommand;
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Select file to be loaded
        /// </summary>
        /// <param name="input"></param>
        void SelectFile(object input) {
            var filename = FileOperations.SelectFile();
            if (!String.IsNullOrEmpty(filename)) {
                InputFileName = filename;
            }

        }

        /// <summary>
        /// Validate the file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        bool ValidateFile(string fileName) {
            return FileOperations.ValidateFile(fileName);
        }

        void LoadEffectControl(object effect) {
            GetViewModel(effect);
            if (InputFileName != null) {
                CurrentViewModel.LoadImage(InputFileName);
            }
        }

        void GetViewModel(object effect) {
            CurrentViewModel = null;
            var currentEffect = effect.ToString();
            enumValue = (Algorithm.Effects)Enum.Parse(typeof(Algorithm.Effects), currentEffect);
            if (currentEffect == "Canvas" ||
                currentEffect == "Quantize" ||
                currentEffect == "Edge" ||
                currentEffect == "Parabola" ||
                currentEffect == "Wave" ||
                currentEffect == "OilPaint"
                ) {
                CurrentViewModel = new MultipleChoiceViewModel(enumValue);
            } else if (currentEffect == "Sepia" ||
                currentEffect == "Solarize" ||
                currentEffect == "Outline"
                ) {
                CurrentViewModel = new SliderSelectionViewModel(enumValue);
            } else if (
                currentEffect == "Emboss" ||
                currentEffect == "XRay" ||
                currentEffect == "Dither") {
                CurrentViewModel = new MultipleChoiceColourSelectionViewModel(enumValue);
            } else if (
                currentEffect == "Glass") {
                CurrentViewModel = new MultipleChoiceSliderViewModel(enumValue);
            } else if (currentEffect == "Pixelate") {
                CurrentViewModel = new DoubleSliderViewModel(enumValue);
            }
        }

        ViewModelBase currentViewModel;

        /// <summary>
        /// Save the image
        /// </summary>
        /// <param name="image"></param>
        void SaveImage(object typeOfSave) {
            ImageSource imageToBeSaved = null;
            string proposedFileName = string.Empty;
            if (typeOfSave.ToString() == "Original") {
                proposedFileName = System.IO.Path.GetFileNameWithoutExtension(InputFileName) + "_" + enumValue + System.IO.Path.GetExtension(InputFileName);
            } else {
                imageToBeSaved = CurrentViewModel.OutputImage;
                proposedFileName = System.IO.Path.GetFileNameWithoutExtension(InputFileName) + "_" + enumValue + "_Scaled" + System.IO.Path.GetExtension(InputFileName);
            }
            var fileName = FileOperations.ShowFileDialogue(imageToBeSaved, proposedFileName);
            if (!String.IsNullOrEmpty(fileName)) {
                Mouse.OverrideCursor = Cursors.Wait;
                if (imageToBeSaved == null) {
                    imageToBeSaved = CurrentViewModel.SaveImage();
                }
                FileOperations.SaveImageFile(imageToBeSaved, fileName);
                imageToBeSaved = null;
                Mouse.OverrideCursor = null;
                GC.Collect();
                GC.WaitForFullGCComplete();
            }
        }
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

        Algorithm.Effects enumValue;
    }
}