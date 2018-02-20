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
    class DoubleSliderViewModel : ViewModelBase {

        #region Private Fields
        double sliderOne;
        double sliderTwo;
        AlgorithmOption sliderOneParam;
        AlgorithmOption sliderTwoParam;
        AlgorithmParameter lastSelectedColour = null;
        ICommand selectColour;
        #endregion

        #region Public Methods
        /// <summary>
        /// Slider value one
        /// </summary>
        public double SliderOneValue {
            get {
                return sliderOne;
            }
            set {
                sliderOne = (int)value;
                ApplyEffect();
            }
        }

        /// <summary>
        /// Slider value two
        /// </summary>
        public double SliderTwoValue {
            get {
                return sliderTwo;
            }
            set {
                sliderTwo = (int)value;
                ApplyEffect();
            }
        }

        /// <summary>
        /// Name of slider one
        /// </summary>
        public string SliderNameOne {
            get {
                return (sliderOneParam.Options.Keys.First() as AlgorithmParameter).ParameterName;
            }
        }

        /// <summary>
        /// Name of slider two
        /// </summary>
        public string SliderNameTwo {
            get {
                return (sliderTwoParam.Options.Keys.First() as AlgorithmParameter).ParameterName;
            }
        }

        /// <summary>
        /// Max value of slider one
        /// </summary>
        public int MaxSliderOne {
            get {
                return (sliderOneParam.Options.Keys.First() as RangeAlgorithmParameter).Maximum;
            }
        }

        /// <summary>
        /// Max value for slider two
        /// </summary>
        public int MaxSliderTwo {
            get {
                return (sliderTwoParam.Options.Keys.First() as RangeAlgorithmParameter).Maximum;
            }
        }

        /// <summary>
        /// Minimum value of slider one
        /// </summary>
        public int MinSliderOne {
            get {
                return (sliderOneParam.Options.Keys.First() as RangeAlgorithmParameter).Minimum;
            }
        }

        /// <summary>
        /// Minimum value for slider two
        /// </summary>
        public int MinSliderTwo {
            get {
                return (sliderTwoParam.Options.Keys.First() as RangeAlgorithmParameter).Minimum;
            }
        }

        /// <summary>
        /// Colour options
        /// </summary>
        public IDictionary<AlgorithmParameter, string> ColourSelectionOption {
            get {
                return AlgorithmOptions.FirstOrDefault(x => x.InputType == Algorithm.InputType.MultipleChoice && x.ParameterName == "BorderColour").Options;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="effect"></param>
        public DoubleSliderViewModel(Effects effect) {
            AlgorithmOptions = ImageProcessingAlgorithm.GetOptions(effect);
            ImageProcessingAlgorithm.SetEffects(effect);

            sliderOneParam = AlgorithmOptions.First(x => (x.InputType == Algorithm.InputType.SingleInput) &&
                    (x.ParameterName == "BlockWidth"));
            sliderTwoParam = AlgorithmOptions.First(x => (x.InputType == Algorithm.InputType.SingleInput) &&
                    (x.ParameterName == "BlockHeight"));

            SliderOneValue = ((MaxSliderOne - MinSliderOne) / 2) + MinSliderOne;
            SliderTwoValue = ((MaxSliderTwo - MinSliderTwo) / 2) + MinSliderTwo;
            var colours = AlgorithmOptions.First(x => x.InputType == Algorithm.InputType.MultipleChoice && x.ParameterName == "BorderColour");
            lastSelectedColour = colours.Options.First(x => x.Value == "Gray").Key;
            selectColour = new DelegateCommand(new Action<object>(SelectColour));
        }

        /// <summary>
        /// Command for colour change
        /// </summary>
        public ICommand SelectColourCommand {
            get {
                return selectColour;
            }
        }

        /// <summary>
        /// Loads the image
        /// </summary>
        /// <param name="fileName"></param>
        public override void LoadImage(string fileName) {
            InputImage = ImageProcessingAlgorithm.LoadInputImage(fileName, out Message);
            ApplyEffect();
        }

        public override System.Windows.Media.ImageSource SaveImage() {
            ImageSource result = null;
            var parameter = new AlgorithmParameter()
            {
                Value = (int)SliderOneValue,
                ParameterName = SliderNameOne
            };
            var parameter1 = new AlgorithmParameter()
            {
                Value = (int)SliderTwoValue,
                ParameterName = SliderNameTwo
            };
            List<AlgorithmParameter> algorithmParameter = new List<AlgorithmParameter>();
            algorithmParameter.Add(parameter);
            algorithmParameter.Add(parameter1);
            algorithmParameter.Add(lastSelectedColour);

            result = ImageProcessingAlgorithm.ApplyEffectOnOriginalDimensions(algorithmParameter, out Message);
            if (!String.IsNullOrEmpty(Message)) {
                MessageBox.Show("Some error occured \n" + Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return result;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Selects the colour and applies the effect
        /// </summary>
        /// <param name="colour"></param>
        void SelectColour(object colour) {
            if (InputImage != null) {
                var param = (KeyValuePair<Algorithm.AlgorithmParameter, string>)colour;
                lastSelectedColour = param.Key;
                ApplyEffect();
            }
        }

        /// <summary>
        /// Apply the effect on change of slider value or colour change
        /// </summary>
        void ApplyEffect() {
            if (InputImage != null) {
                var parameter = new AlgorithmParameter()
                {
                    Value = (int)SliderOneValue,
                    ParameterName = SliderNameOne
                };
                var parameter1 = new AlgorithmParameter()
                {
                    Value = (int)SliderTwoValue,
                    ParameterName = SliderNameTwo
                };
                List<AlgorithmParameter> algorithmParameter = new List<AlgorithmParameter>();
                algorithmParameter.Add(parameter);
                algorithmParameter.Add(parameter1);
                algorithmParameter.Add(lastSelectedColour);
                OutputImage = ImageProcessingAlgorithm.ApplyEffect(algorithmParameter, out Message);
            }

        }
        #endregion
    }
}