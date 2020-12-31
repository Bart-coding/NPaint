using System;
using System.CodeDom;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
using NPaint.Figures;
using NPaint.Memento;
using NPaint.State;

namespace NPaint
{
    public partial class MainWindow : Window
    {
        //public List<Figure> PrototypePalette;
        private MenuState menuState;
        public Canvas canvas;

        private Caretaker caretaker;
        private Originator originator;
        private readonly String canvasPath = @"..\..\..\Canvases\";
        public MainWindow()
        {
            InitializeComponent();
            caretaker = new Caretaker();
            InitializeCaretakerList();//
            originator = new Originator();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FillrColorLabel.Width = BorderColorLabel.ActualWidth;
            AddCanvas();
            

            // na sztywno, zeby sprawdzic czy mozna rysowac prostokaty
            menuState = new EllipseState();
            TestShapeFactory(); // przestawione, może dlatego przy próbie rysowania na tych prototypowych wywalało wyjątek przy MouseMove
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
        }
        private void TestShapeFactory()
        {
            ShapeFactory shapeFactory = ShapeFactory.getShapeFactory();
            NSquare nSquare = (NSquare)shapeFactory.getFigure("Square");
            canvas.Children.Add(nSquare.adaptedPath);
            NEllipse nEllipse = (NEllipse)shapeFactory.getFigure("Ellipse");
            canvas.Children.Add(nEllipse.adaptedPath);
            NCircle nCircle = (NCircle) shapeFactory.getFigure("Circle");
            canvas.Children.Add(nCircle.adaptedPath);
        }
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            //if (Mouse.Captured == canvas)
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
            //Mouse.Capture(canvas);

            // zaleznie od stanu podejmujemy akcje
            if (menuState != null)
            {
                // np. stan rysowanie figury 
                // w tym miejscu trzeba przypisac startPoint Figury ??
                Point point = e.GetPosition(canvas);
                menuState.MouseLeftButtonDown(point);
            }
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

        private void ClearCanvas(object sender, RoutedEventArgs e)
        {

            this.SerializeCanvas("FirstCanvas");//Zapis do pliku i do listy Memento przed usunieciem

            this.canvas.Children.Clear();

            MessageBox.Show("Wyczyszczono Canvas");

            this.RestoreLastCanvas();


        }

        private void SerializeCanvas(string fileName) //możliwe, że bardziej wzorcowo trzymać w ramie całe canvasy, zależy ile by to żarło
        {
            fileName += ".txt";
            //string path = System.IO.Path.GetFullPath(fileName);
            //string newPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(fileName, @"..\..\..\..\Canvases\")) + fileName;
            string newPath = this.canvasPath + fileName;
            string CanvasXAML = XamlWriter.Save(this.canvas);
            try
            {
                using (StreamWriter writer = new StreamWriter(newPath))
                {
                    writer.Write(CanvasXAML);
                }
                this.originator.SetMemento(fileName);
                this.caretaker.AddMemento(this.originator.CreateMemento());
                MessageBox.Show("Zapisano Canvas :)");


            }
            catch (Exception e)
            {
                MessageBox.Show("Wyjatek: "+e.Message);
            }

            



        }


        private void RestoreLastCanvas()
        {


            string oldCanvasFile = this.originator.restoreFromMemento(this.caretaker.GetLastMemento()); //Odczyt z listy Memento
            string CanvasString;
            try //odczyt  z pliku
               
            {
                // Open the text file using a stream reader.
                using (var sr = new StreamReader(this.canvasPath+oldCanvasFile))
                {
                    CanvasString = sr.ReadToEnd();
                    
                }

                Canvas oldCanvas = XamlReader.Parse(CanvasString) as Canvas;

                MainGrid.Children.Remove(canvas);
                canvas = null;
                canvas = oldCanvas;
                MainGrid.Children.Add(canvas);

                MessageBox.Show("Przywrócono poprzedni Canvas :)");

            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            

        }
        private void InitializeCaretakerList()
        {
            //pobieranie nazw plików z folderu Canvases (ich lista może siedzieć w innym txt)
        }
    }
}
