using System.Windows;

namespace NPaint
{
    public partial class RestoreCanvasWindow : Window
    {
        
        public RestoreCanvasWindow()
        {
            InitializeComponent();
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
