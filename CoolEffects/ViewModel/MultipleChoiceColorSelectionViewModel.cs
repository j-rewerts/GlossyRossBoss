// Cool Image Effects

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using Algorithm;
using CoolImageEffects.Command;

namespace CoolImageEffects.ViewModel {
    class MultipleChoiceColourSelectionViewModel : ViewModelBase {
        ICommand selectMethodCommand, selectColour;
        AlgorithmParameter lastSelected = null;

        public AlgorithmParameter LastSelected {
            get {
                return lastSelected;
            }
            set {
                lastSelected = value;
            }
        }
        AlgorithmParameter lastSelectedColour = null;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="effect"></param>
        public MultipleChoiceColourSelectionViewModel(Algorithm.Effects effect) {
            selectMethodCommand = new DelegateCommand(new Action<object>(SelectMethod));
            selectColour = new DelegateCommand(new Action<object>(SelectColour));
            ImageProcessingAlgorithm.SetEffects(effect);
            AlgorithmOptions = ImageProcessingAlgorithm.GetOptions(effect);
            if (effect == Effects.XRay) {
                lastSelected = new AlgorithmParameter();
            } else {
                var method = AlgorithmOptions.First(x => x.InputType == Algorithm.InputType.MultipleChoice && x.ParameterName == "Method");
                lastSelected = method.Options.First().Key;
            }
            var colours = AlgorithmOptions.First(x => x.InputType == Algorithm.InputType.MultipleChoice && x.ParameterName == "Colour");
            lastSelectedColour = colours.Options.First(x => x.Value == "Gray").Key;
        }

        /// <summary>
        /// Radio button options
        /// </summary>
        public IDictionary<AlgorithmParameter, string> Options {
            get {
                var result = AlgorithmOptions.Where(x => x.InputType == Algorithm.InputType.MultipleChoice && x.ParameterName == "Method").ToList();
                if (result.Count > 0) {
                    return result.First().Options;
                }
                return null;
            }
        }

        /// <summary>
        /// Colour options
        /// </summary>
        public IDictionary<AlgorithmParameter, string> ColourSelectionOption {
            get {
                return AlgorithmOptions.FirstOrDefault(x => x.InputType == Algorithm.InputType.MultipleChoice && x.ParameterName == "Colour").Options;
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
        /// Select Colour method
        /// </summary>
        public ICommand SelectColourCommand {
            get {
                return selectColour;
            }
        }

        /// <summary>
        /// Load Image
        /// </summary>
        /// <param name="fileName"></param>
        public override void LoadImage(string fileName) {
            InputImage = ImageProcessingAlgorithm.LoadInputImage(fileName, out Message);
            if (lastSelected != null) {
                SelectMethod(lastSelected);
            }
        }

        /// <summary>
        /// Gets the image to be saved in original size
        /// </summary>
        /// <returns></returns>
        public override ImageSource SaveImage() {
            ImageSource result = null;
            if (lastSelected != null && lastSelectedColour != null && InputImage != null) {
                List<AlgorithmParameter> algorithmParameter = new List<AlgorithmParameter>();
                algorithmParameter.Add(lastSelectedColour);
                algorithmParameter.Add(lastSelected);
                result = ImageProcessingAlgorithm.ApplyEffectOnOriginalDimensions(algorithmParameter, out Message);
            }
            return result;
        }

        void SelectMethod(object methodName) {
            lastSelected = methodName as AlgorithmParameter;
            if (InputImage != null && lastSelectedColour != null) {
                List<AlgorithmParameter> algorithmParameter = new List<AlgorithmParameter>();
                algorithmParameter.Add(lastSelectedColour);
                algorithmParameter.Add(lastSelected);
                OutputImage = ImageProcessingAlgorithm.ApplyEffect(algorithmParameter, out Message);
            }
        }

        void SelectColour(object colour) {
            if (InputImage != null && lastSelected != null) {
                var param = (KeyValuePair<Algorithm.AlgorithmParameter, string>)colour;
                lastSelectedColour = param.Key;
                List<AlgorithmParameter> algorithmParameter = new List<AlgorithmParameter>();
                algorithmParameter.Add(lastSelectedColour);
                algorithmParameter.Add(lastSelected);
                OutputImage = ImageProcessingAlgorithm.ApplyEffect(algorithmParameter, out Message);
            }
        }
    }
}