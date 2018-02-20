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
    class MultipleChoiceViewModel : ViewModelBase {
        ICommand selectMethodCommand;
        AlgorithmParameter lastSelected = null;

        public AlgorithmParameter LastSelected {
            get {
                return lastSelected;
            }
            set {
                lastSelected = value;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="effect"></param>
        public MultipleChoiceViewModel(Effects effect) {
            selectMethodCommand = new DelegateCommand(new Action<object>(SelectMethod));
            ImageProcessingAlgorithm.SetEffects(effect);
            AlgorithmOptions = ImageProcessingAlgorithm.GetOptions(effect);
            var method = AlgorithmOptions.First(x => x.InputType == Algorithm.InputType.MultipleChoice);
            lastSelected = method.Options.First().Key;
        }

        /// <summary>
        /// Select command method
        /// </summary>
        public ICommand SelectMethodCommand {
            get {
                return selectMethodCommand;
            }
        }

        /// <summary>
        /// Options for radio button
        /// </summary>
        public IDictionary<AlgorithmParameter, string> Options {
            get {
                return AlgorithmOptions.First(x => x.InputType == Algorithm.InputType.MultipleChoice).Options;
            }
        }

        /// <summary>
        /// Select method 
        /// </summary>
        /// <param name="methodName"></param>
        protected void SelectMethod(object methodName) {
            lastSelected = methodName as AlgorithmParameter;
            if (InputImage != null) {

                List<AlgorithmParameter> algorithmParameter = new List<AlgorithmParameter>();
                algorithmParameter.Add(lastSelected);
                OutputImage = ImageProcessingAlgorithm.ApplyEffect(algorithmParameter, out Message);
            }
        }

        /// <summary>
        /// Loads the image
        /// </summary>
        /// <param name="fileName"></param>
        public override void LoadImage(string fileName) {
            InputImage = ImageProcessingAlgorithm.LoadInputImage(fileName, out Message);
            if (lastSelected != null) {
                SelectMethod(lastSelected);
            }
        }

        /// <summary>
        /// Get the image to be saved in original size
        /// </summary>
        /// <returns></returns>
        public override ImageSource SaveImage() {
            if (InputImage != null) {
                List<AlgorithmParameter> algorithmParameter = new List<AlgorithmParameter>();
                algorithmParameter.Add(lastSelected);
                var result = ImageProcessingAlgorithm.ApplyEffectOnOriginalDimensions(algorithmParameter, out Message);

                if (!String.IsNullOrEmpty(Message)) {
                    MessageBox.Show("Some error occured \n" + Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                return result;
            }
            return null;
        }
    }
}