using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml.Serialization;
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
        private List<Figure> FigureList;///
        public FigureListClass FigureListClassObject;
        private Figure SelectedFigure;
        private ObservableFigure ObservableFigure;

        private Caretaker caretaker;
        private Originator originator;
        private readonly String canvasPath = @"..\..\..\Canvases\";
        private readonly String canvasListFilePath = @"..\..\..\Canvases\CanvasList.txt";
        //public Point firstStartPointWhileMoving; -> do przemyślenia

        public MainWindow()
        {
            InitializeComponent();
            caretaker = new Caretaker();
            originator = new Originator();
            //FigureList = new List<Figure>();
            FigureListClassObject = new FigureListClass();///
            FigureListClassObject.FigureList = new List<Figure>();//
            FigureList = FigureListClassObject.FigureList;///
            //SaveFigureListTest();/////
            //ReadFigureListTest();
            //SaveFigureListXmlTest();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FillrColorLabel.Width = BorderColorLabel.ActualWidth;
            AddCanvas();
            RestoreCaretaker();


            // na sztywno, zeby sprawdzic czy mozna rysowac figury
            menuState = new SquareState();
        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            //List<String> CanvasNamesList = new List<String>();
            using (StreamWriter sw = new StreamWriter(this.canvasListFilePath, false))
            {
                for (int i = 0; ; i++)
                {
                    Memento.Memento memento = this.caretaker.GetMemento(i);
                    if (memento != null)
                    {
                        String canvasName = this.originator.restoreFromMemento(memento);
                        //CanvasNamesList.Add(canvasName);
                        sw.WriteLine(canvasName);

                    }
                    else
                        break;
                }
            }
            //sw.Close();
        }

        private List<Figure> RestoreFigureListTest(UIElementCollection CanvasChildren) //*I METODA ODZYSKANIA FIGUR*//
        {
            List<Figure> RestoredFigureList = new List<Figure>();
            ShapeFactory shapeFactory = ShapeFactory.getShapeFactory();
            foreach (UIElement canvasChild in CanvasChildren)
            {
                if (canvasChild.GetType() == typeof(System.Windows.Shapes.Path))
                {
                    System.Windows.Shapes.Path tmp = canvasChild as System.Windows.Shapes.Path;
                    Figure figure = shapeFactory.getFigure(tmp.Tag as String);
                    figure.adaptedPath = tmp;
                    figure.adaptedGeometry = tmp.Data;
                    //brak startpointa
                    RestoredFigureList.Add(figure);
                }
            }
            return RestoredFigureList;
        }
        private void SaveFigureListTest()////*II METODA ZAPISU I ODZYSKANIA FIGUR (TA I PONIŻSZA)*//
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(this.canvasPath + "Test.bin", FileMode.Create, FileAccess.Write, FileShare.None);
            try
            {
                formatter.Serialize(stream, this.FigureList);
            }
            catch (SerializationException e)
            {
                MessageBox.Show("Wyjątek związany z serializacją: " + e.Message);
                throw;
            }
            stream.Close();
        }
        private void ReadFigureListTest()//
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(this.canvasPath + "Test.bin", FileMode.Open, FileAccess.Read, FileShare.Read);
            try
            {
                this.FigureList = formatter.Deserialize(stream) as List<Figure>;
            }
            catch (SerializationException e)
            {
                MessageBox.Show("Wyjątek związany z serializacją: " + e.Message);
                throw;
            }
            stream.Close();
        }
        private void SaveFigureListXmlTest()//*III METODA*//
        {
            XmlSerializer serializer = new XmlSerializer(typeof(FigureListClass));
            FileStream fs = new FileStream(this.canvasPath + "TestZapisFigur.xml", FileMode.Create);
            serializer.Serialize(fs, this.FigureListClassObject);
            fs.Close();/////gdzies indziej moze tez go brakuje
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
            canvas.MouseRightButtonDown += new MouseButtonEventHandler(Canvas_MouseRightButtonDown);
        }
        private void ClearCanvas(object sender, RoutedEventArgs e)
        {

            //this.SerializeCanvas("FirstCanvas");//Zapis do pliku i do listy Memento przed usunieciem

            this.canvas.Children.Clear();

            MessageBox.Show("Wyczyszczono Canvas");

            //this.RestoreLastCanvas();
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            //if(Mouse.Captured == canvas)
            {
                if (Mouse.LeftButton == MouseButtonState.Pressed)
                {
                    if (this.Cursor != Cursors.SizeAll)
                        this.Cursor = Cursors.SizeAll;
                    if (SelectedFigure != null)
                    {
                        Point pt = e.GetPosition(canvas);
                        if (pt.Y < 0 + (pt.Y - SelectedFigure.GetStartPoint().Y))//nie wiem dlaczego nie działa
                        {
                            pt.Y = 0 + (pt.Y - SelectedFigure.GetStartPoint().Y);
                        }
                        
                       menuState.MouseMove(pt); //nie działało, bo menuState to BasicState
                       // SelectedFigure.MoveBy(pt);
                        return;
                    }
                    if (menuState != null)
                    {
                        // zaleznie od stanu podejmujemy akcje

                        if (this.Cursor != Cursors.Hand) //Cursors.SizeNESW
                            this.Cursor = Cursors.Hand;

                        Point pt = e.GetPosition(canvas);   // punkt przechwycony ze zdarzenia myszy
                        if (pt.Y < 0 + BorderThicknessySlider.Value / 2)
                        {
                            pt.Y = 0 + BorderThicknessySlider.Value / 2;
                        }
                        menuState.MouseMove(pt);//to draw and resize
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
                if(menuState.GetType() != typeof(CursorState))
                {
                    ResetSelectedFigure();
                }
                Point pt = e.GetPosition(canvas);

                if (pt.Y < 0 + BorderThicknessySlider.Value / 2)
                {
                    pt.Y = 0 + BorderThicknessySlider.Value / 2;
                }
                // np. stan rysowanie figury 
                menuState.MouseLeftButtonDown(pt); 
            }
        }
        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(menuState != null)
            {
                if (this.Cursor != Cursors.Arrow)
                    this.Cursor = Cursors.Arrow;
                menuState.MouseLeftButtonUp(e.GetPosition(canvas));
            }
            //if (selectedFigureState != null)
            //{
            //    //selectedFigureState = null;
            //    selectedFigureState.MouseLeftButtonUp(e.GetPosition(canvas));
            //}
            // zwolnienie myszy z Canvasa
            //Mouse.Capture(null);
        }
        private void Canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            ResetSelectedFigure();
            ResetObservableFigure();
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
                MessageBox.Show("Wyjatek: " + e.Message);
            }
        }
        private void RestoreLastCanvas()
        {
            string oldCanvasFile = this.originator.restoreFromMemento(this.caretaker.GetLastMemento()); //Odczyt z listy Memento
            string CanvasString;
            try //odczyt  z pliku
            {
                // Open the text file using a stream reader.
                using (var sr = new StreamReader(this.canvasPath + oldCanvasFile))
                {
                    CanvasString = sr.ReadToEnd();

                }

                Canvas oldCanvas = XamlReader.Parse(CanvasString) as Canvas;

                MainGrid.Children.Remove(canvas);
                canvas = null;
                canvas = oldCanvas;
                SetCanvas();
                FigureList = RestoreFigureListTest(canvas.Children);///////////

                MessageBox.Show("Przywrócono poprzedni Canvas :)");
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }
        private void RestoreCanvas(int index)
        {
            string oldCanvasFile = this.originator.restoreFromMemento(this.caretaker.GetMemento(index)); //Odczyt z listy Memento
            string CanvasString;
            try //odczyt  z pliku
            {
                // Open the text file using a stream reader.
                using (var sr = new StreamReader(this.canvasPath + oldCanvasFile))
                {
                    CanvasString = sr.ReadToEnd();

                }

                Canvas oldCanvas = XamlReader.Parse(CanvasString) as Canvas;

                MainGrid.Children.Remove(canvas);
                canvas = null;
                canvas = oldCanvas;
                SetCanvas();
                FigureList = RestoreFigureListTest(canvas.Children);///////////

                MessageBox.Show("Przywrócono poprzedni Canvas :)");
            }
            catch (IOException e)
            {
                MessageBox.Show("The file could not be read: " + e.Message);
            }
        }
        private void InitializeCaretakerList()
        {
            //pobieranie nazw plików z folderu Canvases (ich lista może siedzieć w innym txt)
        }

        public void AddObservable(Figure figure)
        {
            // musimy usunac starego obserwowanego z canvasa
            if(ObservableFigure != null)
                canvas.Children.Remove(ObservableFigure.adaptedPath);

            ObservableFigure = figure as ObservableFigure;

            // dodanie go do canvasa, zeby byl widoczny
            canvas.Children.Add(figure.adaptedPath);
        }
        public void AddFigure(Figure figure)
        {
            // zmieniamy jej wlasciwosci na te wybrane
            figure.ChangeBorderColor(BorderColorButton.Background);
            figure.ChangeFillColor(FillColorButton.Background);
            figure.ChangeTransparency(TransparencySlider.Value);
            figure.ChangeBorderThickness(BorderThicknessySlider.Value);

            // dodanie nowej figury
            canvas.Children.Add(figure.adaptedPath);
            FigureList.Add(figure);
        }
        private void RemoveFigure(object sender, RoutedEventArgs e)
        {
            if (SelectedFigure != null)
            {
                canvas.Children.Remove(SelectedFigure.adaptedPath);
                FigureList.Remove(SelectedFigure);
                SelectedFigure = null;
            }
            //SaveFigureListTest();////////////////////////////////USUN
        }
        public List<Figure> GetFigureList()
        {
            return FigureList;
        }
        public void ResetSelectedFigure()
        {
            // musimy wylaczyc ramke dla obecnie wybranej figury
            if (SelectedFigure != null)
            {
                SelectedFigure.adaptedPath.StrokeDashArray = null;
                SelectedFigure = null;
            }
        }
        public void SetSelectedFigure(Figure figure)
        {
            // musimy wylaczyc ramke dla poprzednio wybranej figury
            if (SelectedFigure != null)
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
        private void ResetObservableFigure()
        {
            if (ObservableFigure != null)
            {
                canvas.Children.Remove(ObservableFigure.adaptedPath);
                ObservableFigure = null;
            }
        }

        private void CircleButton_Click(object sender, RoutedEventArgs e)
        {
            ResetSelectedFigure();
            ResetObservableFigure();
            menuState = new CircleState();
        }
        private void SquareButton_Click(object sender, RoutedEventArgs e)
        {
            ResetSelectedFigure();
            ResetObservableFigure();
            menuState = new SquareState();
        }
        private void TriangleButton_Click(object sender, RoutedEventArgs e)
        {
            ResetSelectedFigure();
            ResetObservableFigure();
            menuState = new TriangleState();
        }
        private void EllipseButton_Click(object sender, RoutedEventArgs e)
        {
            ResetSelectedFigure();
            ResetObservableFigure();
            menuState = new EllipseState();
        }
        private void RectangleButton_Click(object sender, RoutedEventArgs e)
        {
            ResetSelectedFigure();
            ResetObservableFigure();
            menuState = new RectangleState();
        }
        private void PolygonButton_Click(object sender, RoutedEventArgs e)
        {
            ResetSelectedFigure();
            ResetObservableFigure();
            menuState = new PolygonState();
        }
        private void CursorButton_Click(object sender, RoutedEventArgs e)
        {
            ResetObservableFigure();
            menuState = new CursorState();
        }
        private void SelectionButton_Click(object sender, RoutedEventArgs e)
        {
            ResetSelectedFigure();
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
            if (SelectedFigure != null)
            {
                SelectedFigure.ChangeFillColor(button.Background);
            }
        }
        private void BorderThicknessySlider_ValueChanged(object sender, RoutedEventArgs e)
        {
            if(SelectedFigure != null)
            {
                SelectedFigure.ChangeBorderThickness(BorderThicknessySlider.Value);
            }
        }
        private void TransparencySlider_ValueChanged(object sender, RoutedEventArgs e)
        {
            if (SelectedFigure != null)
            {
                SelectedFigure.ChangeTransparency(TransparencySlider.Value);
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
            RestoreCanvasWindow restoreCanvasWindow = new RestoreCanvasWindow();
            for (int i = 0; ; i++)
            {
                Memento.Memento memento = this.caretaker.GetMemento(i);
                if (memento != null)
                {
                    String canvasName = this.originator.restoreFromMemento(memento);
                    restoreCanvasWindow.canvasNameListbox.Items.Add(canvasName);

                }
                else
                    break;
                
            }
            if (true == restoreCanvasWindow.ShowDialog())
            {
                this.RestoreCanvas(restoreCanvasWindow.canvasNameListbox.SelectedIndex);
            }
        }
        private void RestoreCaretaker()
        {
            if (!File.Exists(this.canvasListFilePath))
            {
                FileStream fs = File.Create(this.canvasListFilePath);
                return;
            }
            if (new FileInfo(this.canvasListFilePath).Length != 0)
            {
                using (StreamReader reader = new StreamReader(this.canvasListFilePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        this.originator.SetMemento(line);
                        this.caretaker.AddMemento(this.originator.CreateMemento());
                    }
                }
            }
        }
    }
}
