using System.Collections.Generic;
using System.Windows;
using NPaint.Figures;

namespace NPaint
{
    public partial class MainWindow : Window
    {
        public List<Figure> PrototypePalette;
        public MainWindow()
        {
            InitializeComponent();
            InitializePrototypePalette();
        }

        private void InitializePrototypePalette()
        {
            // how to initialize prototype palette??

            //PrototypePalette.Add(new Figure());
            //PrototypePalette.Add(new NRectangle());
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FillrColorLabel.Width = BorderColorLabel.ActualWidth;
        }
    }
}
