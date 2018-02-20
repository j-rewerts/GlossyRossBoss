using System.Windows.Controls;

namespace CoolImageEffects {
    /// <summary>
    /// Interaction logic for ImageProcessingView.xaml
    /// </summary>
    public partial class ImageProcessingView : UserControl {
        public ImageProcessingView() {
            InitializeComponent();
            DataContext = new ImageProcessingViewModel();
        }

    }
}
