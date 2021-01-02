using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
using NPaint.Figures;
using NPaint.Memento;
using NPaint.Observer;
using NPaint.State;

namespace NPaint
{
    public partial class MainWindow : Window
    {
        private MenuState menuState;
        public Canvas canvas;
        private List<Figure> FigureList;
        private Figure SelectedFigure;

        private Caretaker caretaker;
        private Originator originator;
        private readonly String canvasPath = @"..\..\..\Canvases\";

        public MainWindow()
        {
            InitializeComponent();
            caretaker = new Caretaker();
            InitializeCaretakerList();//
            originator = new Originator();
            FigureList = new List<Figure>();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FillrColorLabel.Width = BorderColorLabel.ActualWidth;
            AddCanvas();
            

            // na sztywno, zeby sprawdzic czy mozna rysowac figury
            menuState = new TriangleState();
        }

        private void AddCanvas()
        {
            canvas = new Canvas();
            SetCanvas();
        }

        private void SetCanvas()
        {
            MainGrid.Children.Add(canvas);
            if (canvas.Background == null)
                canvas.Background = Brushes.Transparent;    // przypisanie tla do canvasa, zeby przechwytywac eventy
            Grid.SetRow(canvas, 1); // przypisanie canvasa do pierwszego wiersza w Gridzie

            // przypisanie eventow do canvasa
            canvas.MouseMove += new MouseEventHandler(Canvas_MouseMove);
            canvas.MouseLeftButtonDown += new MouseButtonEventHandler(Canvas_MouseLeftButtonDown);
            canvas.MouseLeftButtonUp += new MouseButtonEventHandler(Canvas_MouseLeftButtonUp);
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            //if(Mouse.Captured == canvas)
            {
                if(Mouse.LeftButton == MouseButtonState.Pressed)
                {
                    if(menuState != null)
                    {
                        // zaleznie od stanu podejmujemy akcje
                        Point pt = e.GetPosition(canvas);   // punkt przechwycony ze zdarzenia myszy
                        if(pt.Y < 0 + BorderThicknessySlider.Value/2)
                        {
                            pt.Y = 0 + BorderThicknessySlider.Value / 2;
                        }
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
                Point pt = e.GetPosition(canvas);

                if(pt.Y < 0 + BorderThicknessySlider.Value/2)
                {
                    pt.Y = 0 + BorderThicknessySlider.Value / 2;
                }
                // np. stan rysowanie figury 
                menuState.MouseLeftButtonDown(pt);

                Figure figure = FigureList.Last();

                figure.ChangeBorderColor(BorderColorButton.Background);
                figure.ChangeFillColor(FillColorButton.Background);
                figure.ChangeTransparency((100 - TransparencySlider.Value) / 100);
                figure.ChangeBorderThickness(BorderThicknessySlider.Value);
            }
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // zwolnienie myszy z Canvasa
            Mouse.Capture(null);
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
                SetCanvas();

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

        public void AddFigure(Figure figure)
        {
            // jezeli mamy juz narysowanego obserwatora to chcemy go usunac
            if (figure.GetType() == typeof(ConcreteObservable))
            {
                ConcreteObservable observable = new ConcreteObservable();
                foreach (Figure f in FigureList)
                {
                    // jezeli natrafilismy na obiekt/figure obserwowanego
                    if (f.GetType() == typeof(ConcreteObservable))
                    {
                        observable = f as ConcreteObservable;
                        break;
                    }
                }
                // usuniecie starego obserwowanego
                FigureList.Remove(observable);
                canvas.Children.Remove(observable.adaptedPath);
            }

            // dodanie nowej figury
            canvas.Children.Add(figure.adaptedPath);
            FigureList.Add(figure);
        }
        private void RemoveFigure(object sender, RoutedEventArgs e)
        {
            if(SelectedFigure != null)
            {
                canvas.Children.Remove(SelectedFigure.adaptedPath);
                FigureList.Remove(SelectedFigure);
                SelectedFigure = null;
            }
        }
        public List<Figure> GetFigureList()
        {
            return FigureList;
        }
        public void ResetSelectedFigure()
        {
            // musimy wylaczyc ramke dla obecnie wybranej figury
            if (SelectedFigure != null)
                SelectedFigure.adaptedPath.StrokeDashArray = null;
            SelectedFigure = null;
        }
        public void SetSelectedFigure(Figure figure)
        {
            // musimy wylaczyc ramke dla poprzednio wybranej figury
            if(SelectedFigure != null)
                SelectedFigure.adaptedPath.StrokeDashArray = null;
            // dodanie ramki dla obecnie wybranej figury
            figure.adaptedPath.StrokeDashArray = new DoubleCollection() { 1 };

            SelectedFigure = figure;

            // ustawienia pod wybrana figure
            BorderColorButton.Background = figure.adaptedPath.Stroke;
            FillColorButton.Background = figure.adaptedPath.Fill;
            TransparencySlider.Value = figure.adaptedPath.Fill.Opacity;
            BorderThicknessySlider.Value = figure.adaptedPath.StrokeThickness;
        }

        private void CircleButton_Click(object sender, RoutedEventArgs e)
        {
            menuState = new CircleState();
        }

        private void SquareButton_Click(object sender, RoutedEventArgs e)
        {
            menuState = new SquareState();
        }

        private void TriangleButton_Click(object sender, RoutedEventArgs e)
        {
            menuState = new TriangleState();
        }

        private void EllipseButton_Click(object sender, RoutedEventArgs e)
        {
            menuState = new EllipseState();
        }

        private void RectangleButton_Click(object sender, RoutedEventArgs e)
        {
            menuState = new RectangleState();
        }

        private void PolygonButton_Click(object sender, RoutedEventArgs e)
        {
            menuState = new PolygonState();
        }

        private void CursorButton_Click(object sender, RoutedEventArgs e)
        {
            menuState = new BasicState();
        }
        private void SelectionButton_Click(object sender, RoutedEventArgs e)
        {
            menuState = new SelectionState();
        }
        private void ChangeColor_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            BorderColorButton.Background = button.Background;
            if (SelectedFigure != null)
            {
                SelectedFigure.ChangeBorderColor(button.Background);
            }
        }

        private void ChangeColor_RightClick(object sender, MouseButtonEventArgs e)
        {
            Button button = sender as Button;
            FillColorButton.Background = button.Background;
            if(SelectedFigure != null)
            {
                SelectedFigure.ChangeFillColor(button.Background);
            }
        }

        private void TestShapeFactory()
        {
            ShapeFactory shapeFactory = ShapeFactory.getShapeFactory();
            NSquare nSquare = (NSquare)shapeFactory.getFigure("Square");
            canvas.Children.Add(nSquare.adaptedPath);
            NEllipse nEllipse = (NEllipse)shapeFactory.getFigure("Ellipse");
            canvas.Children.Add(nEllipse.adaptedPath);
            NCircle nCircle = (NCircle)shapeFactory.getFigure("Circle");
            canvas.Children.Add(nCircle.adaptedPath);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            CanvasNameWindow canvasNameWindow = new CanvasNameWindow();
            string canvasName;
            if (true == canvasNameWindow.ShowDialog())
            {
                canvasName = canvasNameWindow.nameBox.Text;
                this.SerializeCanvas(canvasName);
            }

        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
