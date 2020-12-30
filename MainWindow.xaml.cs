using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using NPaint.Figures;
using NPaint.State;

namespace NPaint
{
    public partial class MainWindow : Window
    {
        //public List<Figure> PrototypePalette;
        private MenuState menuState;
        private Canvas canvas;
        public MainWindow()
        {
            InitializeComponent();
            
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FillrColorLabel.Width = BorderColorLabel.ActualWidth;
            AddCanvas();
            TestShapeFactory();
        }

        private void AddCanvas()
        {
            canvas = new Canvas();
            MainGrid.Children.Add(canvas);
            canvas.Background = Brushes.Transparent;    // przypisanie tla do canvasa, zeby przechwytywac eventy
            Grid.SetRow(canvas, 1); // przypisanie canvasa do pierwszego wiersza w Gridzie

            // przypisanie eventow do canvasa
            canvas.MouseMove += new MouseEventHandler(Canvas_MouseMove);
            canvas.MouseLeftButtonDown += new MouseButtonEventHandler(Canvas_MouseLeftButtonDown);
            canvas.MouseLeftButtonUp += new MouseButtonEventHandler(Canvas_MouseLeftButtonUp);

            // przypisanie przykładowej Path do canvasa dla testów
            /*RectangleGeometry rectangle = new RectangleGeometry();
            Rect rect = new Rect();
            rect.X = 100;
            rect.Y = 100;
            rect.Width = 200;
            rect.Height = 50;
            rectangle.Rect = rect;
            //rectangle.Rect = new Rect(100, 100, 200, 50);
            Path myPath = new Path();
            myPath.Fill = Brushes.LemonChiffon;
            myPath.Stroke = Brushes.Black;
            myPath.StrokeThickness = 2;
            myPath.Data = rectangle;
            canvas.Children.Add(myPath);*/
        }
        private void TestShapeFactory()
        {
            ShapeFactory shapeFactory = ShapeFactory.getShapeFactory();
            NRectangle newRectangle = (NRectangle) shapeFactory.getFigure("Rectangle");
            canvas.Children.Add(newRectangle.adaptedPath);
        }
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.Captured == canvas)
            {
                if(Mouse.LeftButton == MouseButtonState.Pressed)
                {
                    if(menuState != null)
                    {
                        // zaleznie od stanu podejmujemy akcje
                        Point pt = e.GetPosition(canvas);   // punkt przechwycony ze zdarzenia myszy
                        menuState.MouseMove(pt);
                    }
                }
            }
        }
        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // złapanie canvasa, aby umozliwic rysowanie poza ekranem
            Mouse.Capture(canvas);

            // zaleznie od stanu podejmujemy akcje

            // np. stan rysowanie figury 
            // w tym miejscu trzeba przypisac startPoint Figury ??
            // startPoint = e.GetPosition(canvas);
        }
        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // zwolnienie myszy z Canvasa
            Mouse.Capture(null);
        }
        private void ExampleMenuStateButtonClick(object sender, RoutedEventArgs e)
        {
            menuState = new RectangleState();
        }
    }
}
