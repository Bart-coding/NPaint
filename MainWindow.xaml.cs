using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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
            TestShapeFactory();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FillrColorLabel.Width = BorderColorLabel.ActualWidth;
            AddCanvas();
        }

        private void AddCanvas()
        {
            canvas = new Canvas();
            MainGrid.Children.Add(canvas);
            Grid.SetRow(canvas, 1); // przypisanie canvasa do pierwszego wiersza w Gridzie

            // przypisanie eventow do canvasa
            canvas.MouseMove += new MouseEventHandler(Canvas_MouseMove);
            canvas.MouseLeftButtonDown += new MouseButtonEventHandler(Canvas_MouseLeftButtonDown);
            canvas.MouseLeftButtonUp += new MouseButtonEventHandler(Canvas_MouseLeftButtonUp);

            // przypisanie przykładowej Path do canvasa dla testów
            RectangleGeometry rectangle = new RectangleGeometry();
            rectangle.Rect = new Rect(100, 100, 200, 50);
            Path myPath = new Path();
            myPath.Fill = Brushes.LemonChiffon;
            myPath.Stroke = Brushes.Black;
            myPath.StrokeThickness = 2;
            myPath.Data = rectangle;
            canvas.Children.Add(myPath);
        }
        private void TestShapeFactory()
        {
            ShapeFactory shapeFactory = ShapeFactory.getShapeFactory();
            NRectangle newRectangle = (NRectangle) shapeFactory.getFigure("Rectangle");
        }
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.Captured == canvas)
            {
                if(Mouse.LeftButton == MouseButtonState.Pressed)
                {
                    // zaleznie od stanu podejmujemy akcje
                }
            }
        }
        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // złapanie canvasa, aby umozliwic rysowanie poza ekranem
            Mouse.Capture(canvas);
        }
        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // zwolnienie myszy z Canvasa
            Mouse.Capture(null);
        }
    }
}
