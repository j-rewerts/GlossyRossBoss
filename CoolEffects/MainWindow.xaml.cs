using System;
using System.Windows;

namespace CoolImageEffects {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            try {
                InitializeComponent();
            } catch(Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
    }
}