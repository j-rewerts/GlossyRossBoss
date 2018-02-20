// Cool Image Effects

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Algorithm;
using CoolImageEffects.Command;

namespace CoolImageEffects.ViewModel {
    public class MultipleChoiceSliderViewModel : ViewModelBase {
        string parameterName;
        ICommand selectMethodCommand;
        AlgorithmParameter lastSelected;
        AlgorithmOption slider;
        public MultipleChoiceSliderViewModel(Effects effect) {
            ImageProcessingAlgorithm.SetEffects(effect);
            AlgorithmOptions = ImageProcessingAlgorithm.GetOptions(effect);
            slider = AlgorithmOptions.First(x => x.InputType == Algorithm.InputType.SingleInput);
            parameterName = (slider.Options.Keys.First() as AlgorithmParameter).ParameterName;
            SliderValue = Max / 2;
            var method = AlgorithmOptions.First(x => x.InputType == Algorithm.InputType.MultipleChoice);
            lastSelected = method.Options.First().Key;
            selectMethodCommand = new DelegateCommand(new Action<object>(SelectMethod));
        }

        private Double sliderValue;

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
        /// Select method command
        /// </summary>
        public ICommand SelectMethodCommand {
            get {
                return selectMethodCommand;
            }
        }

        /// <summary>
        /// Applies the effect by delegating to Imageprocessing 
        /// </summary>
        void ApplyEffect() {
            if (InputImage != null && lastSelected != null) {
                var parameter = new AlgorithmParameter()
                {
                    Value = (int)sliderValue,
                    ParameterName = parameterName
                };
                List<AlgorithmParameter> algorithmParameter = new List<AlgorithmParameter>();
                algorithmParameter.Add(parameter);
                algorithmParameter.Add(lastSelected);
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
            List<AlgorithmParameter> algorithmParameter = new List<AlgorithmParameter>();
            algorithmParameter.Add(lastSelected);
            //PreviewImages = ImageProcessingAlgorithm.GetPreview(algorithmParameter, out Message);
        }

        /// <summary>
        /// Options for slider
        /// </summary>
        public IDictionary<AlgorithmParameter, string> Options {
            get {
                return AlgorithmOptions.First(x => x.InputType == Algorithm.InputType.MultipleChoice).Options;
            }
        }

        /// <summary>
        /// Gets image to save in original size
        /// </summary>
        /// <returns></returns>
        public override ImageSource SaveImage() {
            ImageSource result = null;
            if (lastSelected != null && InputImage != null) {
                List<AlgorithmParameter> algorithmParameter = new List<AlgorithmParameter>();
                var parameter = new AlgorithmParameter()
                {
                    Value = (int)sliderValue,
                    ParameterName = parameterName
                };
                algorithmParameter.Add(parameter);
                algorithmParameter.Add(lastSelected);
                result = ImageProcessingAlgorithm.ApplyEffectOnOriginalDimensions(algorithmParameter, out Message);
                if (!String.IsNullOrEmpty(Message)) {
                    MessageBox.Show("Some error occured \n" + Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return result;
        }

        void SelectMethod(object methodName) {
            lastSelected = methodName as AlgorithmParameter;
            if (InputImage != null) {
                List<AlgorithmParameter> algorithmParameter = new List<AlgorithmParameter>();
                var parameter = new AlgorithmParameter()
                {
                    Value = (int)sliderValue,
                    ParameterName = parameterName
                };
                algorithmParameter.Add(parameter);
                algorithmParameter.Add(lastSelected);
                OutputImage = ImageProcessingAlgorithm.ApplyEffect(algorithmParameter, out Message);
            }
        }
    }
}