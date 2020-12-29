using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using NPaint.Figures;

namespace NPaint
{
    public partial class MainWindow : Window
    {
        //public List<Figure> PrototypePalette;
        private Canvas canvas;
        public MainWindow()
        {
            InitializeComponent();
            canvas = new Canvas();
            //MainGrid.Children.Add(canvas);
            TestShapeFactory();//
           
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FillrColorLabel.Width = BorderColorLabel.ActualWidth;
        }

        private void TestShapeFactory()
        {
            ShapeFactory shapeFactory = ShapeFactory.getShapeFactory();
            NRectangle newRectangle = (NRectangle) shapeFactory.getFigure("Rectangle");
        }
    }
}
