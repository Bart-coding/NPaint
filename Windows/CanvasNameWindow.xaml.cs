using System.Windows;

namespace NPaint
{
    public partial class CanvasNameWindow : Window
    {
        public CanvasNameWindow()
        {
            InitializeComponent();
            nameBox.Focus();
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
