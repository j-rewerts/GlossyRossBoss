// Cool Image Effects

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Algorithm;

namespace CoolImageEffects.ViewModel {
    class SliderSelectionViewModel : ViewModelBase {
        string parameterName;
        AlgorithmOption slider;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="effect"></param>
        public SliderSelectionViewModel(Effects effect) {
            AlgorithmOptions = ImageProcessingAlgorithm.GetOptions(effect);
            ImageProcessingAlgorithm.SetEffects(effect);
            slider = AlgorithmOptions.First(x => x.InputType == Algorithm.InputType.SingleInput);
            parameterName = (slider.Options.Keys.First() as AlgorithmParameter).ParameterName;
            SliderValue = (Minimum + (Max - Minimum) / 2);
        }

        private Double sliderValue;

        public int StepSize {
            get {
                return (slider.Options.Keys.First() as RangeAlgorithmParameter).StepSize;
            }
        }

        /// <summary>
        /// Slider value
        /// </summary>
        public Double SliderValue {
            get {
                return sliderValue;
            }
            set {
                sliderValue = value;
                NotifyPropertyChanged("SliderValue");
                ApplyEffect();
            }
        }

        /// <summary>
        /// Minimum slider value
        /// </summary>
        public int Minimum {
            get {
                return (slider.Options.Keys.First() as RangeAlgorithmParameter).Minimum;
            }
        }

        /// <summary>
        /// Maximum slider value
        /// </summary>
        public int Max {
            get {
                return (slider.Options.Keys.First() as RangeAlgorithmParameter).Maximum;
            }
        }

        /// <summary>
        /// Name of Slider
        /// </summary>
        public string SliderName {
            get {
                return (slider.Options.Keys.First() as RangeAlgorithmParameter).ParameterName;
            }
        }

        /// <summary>
        /// Algorithm adapted from Imagemagick
        /// </summary>
        void ApplyEffect() {
            if (InputImage != null) {
                var parameter = new AlgorithmParameter()
                {
                    Value = (int)sliderValue,
                    ParameterName = parameterName
                };
                List<AlgorithmParameter> algorithmParameter = new List<AlgorithmParameter>();
                algorithmParameter.Add(parameter);
                OutputImage = ImageProcessingAlgorithm.ApplyEffect(algorithmParameter, out Message);
            }
        }

        /// <summary>
        /// Load Images
        /// </summary>
        /// <param name="fileName"></param>
        public override void LoadImage(string fileName) {
            InputImage = ImageProcessingAlgorithm.LoadInputImage(fileName, out Message);
            ApplyEffect();
        }

        /// <summary>
        /// Gets image to save  in original size
        /// </summary>
        /// <returns></returns>
        public override ImageSource SaveImage() {
            ImageSource result = null;
            if (InputImage != null) {
                var parameter = new AlgorithmParameter()
                {
                    Value = (int)sliderValue,
                    ParameterName = parameterName
                };
                List<AlgorithmParameter> algorithmParameter = new List<AlgorithmParameter>();
                algorithmParameter.Add(parameter);
                result = ImageProcessingAlgorithm.ApplyEffectOnOriginalDimensions(algorithmParameter, out Message);
                if (!String.IsNullOrEmpty(Message)) {
                    MessageBox.Show("Some error occured \n" + Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return result;
        }
    }
}