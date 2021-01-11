using NPaint.Figures;
using NPaint.Iterator___singleton;
using NPaint.Memento;
using NPaint.Observer;
using NPaint.State;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace NPaint
{
    public partial class MainWindow : Window
    {
        private MenuState menuState;
        private List<Figure> FigureList;
        private ObservableFigure ObservableFigure;
        private RGBIterator Iterator = RGBIterator.getIterator();
        private Caretaker caretaker;
        private Originator originator;
        private RGBWindow rgbWindow = null;
        
        private readonly String canvasPath = @"..\..\..\Canvases\";
        private readonly String canvasListFilePath = @"..\..\..\Canvases\CanvasList.txt";

        public Canvas canvas;
        public FigureListClass FigureListClassObject;
        public Figure SelectedFigure;

        public bool BorderIteratorSelected = false;
        public bool FillIteratorSelected = false;

        public MainWindow()
        {
            InitializeComponent();
            caretaker = new Caretaker();
            originator = new Originator();
            FigureListClassObject = new FigureListClass();
            FigureListClassObject.FigureList = new List<Figure>();
            FigureList = FigureListClassObject.FigureList;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FillrColorLabel.Width = BorderColorLabel.ActualWidth;
            AddCanvas();
            RestoreCaretaker();

            menuState = new SquareState();
        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            using (StreamWriter sw = new StreamWriter(this.canvasListFilePath, false))
            {
                for (int i = 0; ; i++)
                {
                    Memento.Memento memento = this.caretaker.GetMemento(i);
                    if (memento != null)
                    {
                        String canvasName = this.originator.restoreFromMemento(memento);
                        sw.WriteLine(canvasName);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        private List<Figure> RestoreFigureListTest(UIElementCollection CanvasChildren)
        {
            List<Figure> RestoredFigureList = new List<Figure>();
            ShapeFactory shapeFactory = ShapeFactory.getShapeFactory();

            foreach (UIElement canvasChild in CanvasChildren)
            {
                if (canvasChild.GetType() == typeof(System.Windows.Shapes.Path))
                {
                    System.Windows.Shapes.Path path = canvasChild as System.Windows.Shapes.Path;
                    Figure figure = shapeFactory.getFigure(path.Tag as String);
                    figure.SetFields(path);
                    RestoredFigureList.Add(figure);
                }
            }
            return RestoredFigureList;
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
            {
                canvas.Background = Brushes.Transparent;
            }

            Grid.SetRow(canvas, 1);

            canvas.MouseMove += new MouseEventHandler(Canvas_MouseMove);
            canvas.MouseLeftButtonDown += new MouseButtonEventHandler(Canvas_MouseLeftButtonDown);
            canvas.MouseLeftButtonUp += new MouseButtonEventHandler(Canvas_MouseLeftButtonUp);
            canvas.MouseRightButtonDown += new MouseButtonEventHandler(Canvas_MouseRightButtonDown);
        }
        private void ClearCanvas(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Czy na pewno chcesz wyczyścić canvas?", "Wyczyść", System.Windows.MessageBoxButton.YesNo);

            if (messageBoxResult == MessageBoxResult.Yes)
            {
                this.canvas.Children.Clear();
            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                if (this.Cursor != Cursors.SizeAll)
                {
                    this.Cursor = Cursors.SizeAll;
                }
                if (ObservableFigure!=null)
                {
                        
                    Point pt = e.GetPosition(canvas);

                    menuState.MouseMove(pt);
                                                
                    return;
                }
                if (SelectedFigure != null)
                {
                    Point pt = e.GetPosition(canvas);
                    menuState.MouseMove(pt);
                    return;
                }
                if (menuState != null)
                {
                    if (this.Cursor != Cursors.Hand)
                    {
                        this.Cursor = Cursors.Hand;
                    }

                    Point pt = e.GetPosition(canvas);

                    if (pt.Y < 0 + BorderThicknessySlider.Value / 2)
                    {
                        pt.Y = 0 + BorderThicknessySlider.Value / 2;
                    }
                    menuState.MouseMove(pt);//to draw and resize
                }
            }
        }
        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
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

                menuState.MouseLeftButtonDown(pt); 
            }
        }
        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(menuState != null)
            {
                if (this.Cursor != Cursors.Arrow)
                {
                    this.Cursor = Cursors.Arrow;
                }
                menuState.MouseLeftButtonUp(e.GetPosition(canvas));
            }
        }
        private void Canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            ResetSelectedFigure();
            ResetObservableFigure();
            TryClosePolygon();
        }

        private void SerializeCanvas(string fileName)
        {
            fileName += ".txt";

            string newPath = this.canvasPath + fileName;

            if (SelectedFigure!=null)
            {
                SelectedFigure.adaptedPath.StrokeDashArray = null;
            }
            if (ObservableFigure != null)
            {
                this.canvas.Children.Remove(ObservableFigure.adaptedPath);
                ObservableFigure.Notify_DeleteSelectionVisualEffect();
            }

            string CanvasXAML = XamlWriter.Save(this.canvas);

            if (SelectedFigure != null)
            {
                SelectedFigure.adaptedPath.StrokeDashArray = new DoubleCollection() { 1 };
            }
            if (ObservableFigure != null)
            {
                this.canvas.Children.Add(ObservableFigure.adaptedPath);
                ObservableFigure.Notify_AddSelectionVisualEffect();
            }
            try
            {
                using (StreamWriter writer = new StreamWriter(newPath))
                {
                    writer.Write(CanvasXAML);
                }
                this.originator.SetMemento(fileName);
                this.caretaker.AddMemento(this.originator.CreateMemento());    
            }
            catch (Exception e)
            {
                MessageBox.Show("Wyjatek: " + e.Message);
            }
        }
        
        private void RestoreCanvas(int index)
        {
            string oldCanvasFile = this.originator.restoreFromMemento(this.caretaker.GetMemento(index));
            string CanvasString;

            try
            {
                using (var sr = new StreamReader(this.canvasPath + oldCanvasFile))
                {
                    CanvasString = sr.ReadToEnd();

                }

                Canvas oldCanvas = XamlReader.Parse(CanvasString) as Canvas;

                MainGrid.Children.Remove(canvas);
                canvas = null;
                canvas = oldCanvas;
                SetCanvas();
                FigureList = RestoreFigureListTest(canvas.Children);
            }
            catch (IOException e)
            {
                MessageBox.Show("Nie można odczytać pliku: " + e.Message);
            }
        }

        public void AddObservable(Figure figure)
        {
            if(ObservableFigure != null)
            {
                canvas.Children.Remove(ObservableFigure.adaptedPath);
            }
            ObservableFigure = figure as ObservableFigure;

            canvas.Children.Add(figure.adaptedPath);
        }
        public void AddFigure(Figure figure)
        {
            figure.ChangeBorderColor(BorderColorButton.Background);
            figure.ChangeFillColor(FillColorButton.Background);
            figure.ChangeTransparency(TransparencySlider.Value);
            figure.ChangeBorderThickness(BorderThicknessySlider.Value);

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
        }
        public List<Figure> GetFigureList()
        {
            return FigureList;
        }

        public void ResetSelectedFigure()
        {
            if (SelectedFigure != null)
            {
                SelectedFigure.adaptedPath.StrokeDashArray = null;
                SelectedFigure = null;
            }
        }
        public void SetSelectedFigure(Figure figure)
        {
            if (SelectedFigure != null)
            {
                SelectedFigure.adaptedPath.StrokeDashArray = null;
            }
            figure.adaptedPath.StrokeDashArray = new DoubleCollection() { 1 };

            SelectedFigure = figure;

            // ustawienia pod wybrana figure
            BorderColorButton.Background = figure.adaptedPath.Stroke;
            FillColorButton.Background = figure.adaptedPath.Fill;
            TransparencySlider.Value = figure.adaptedPath.Fill.Opacity;
            BorderThicknessySlider.Value = figure.adaptedPath.StrokeThickness;

            if(BorderIteratorSelected || FillIteratorSelected)
            {
                if (!Iterator.isDone())
                {
                    RGB rgb = Iterator.Next();

                    if(BorderIteratorSelected)
                    {
                        BorderColorButton.Background = new SolidColorBrush(Color.FromRgb(Byte.Parse(rgb.R.ToString()), Byte.Parse(rgb.G.ToString()), Byte.Parse(rgb.B.ToString())));
                    }
                    if (FillIteratorSelected)
                    {
                        FillColorButton.Background = new SolidColorBrush(Color.FromRgb(Byte.Parse(rgb.R.ToString()), Byte.Parse(rgb.G.ToString()), Byte.Parse(rgb.B.ToString())));
                    }
                }
            }
        }
        private void ResetObservableFigure()
        {
            if (ObservableFigure != null)
            {
                ObservableFigure.DetachAll();
                canvas.Children.Remove(ObservableFigure.adaptedPath);
                ObservableFigure = null;
            }
        }
        private void TryClosePolygon()
        {
            if(FigureList.Count != 0)
            {
                if (FigureList.Last().GetType() == typeof(NPolygon) && !((NPolygon)FigureList.Last()).PathFigure.IsClosed)
                {
                    ((NPolygon)FigureList.Last()).CloseFigure();
                }
            }
        }
        private void ResetButtonsBackgrounds()
        {
            CircleButton.Background = Brushes.White;
            EllipseButton.Background = Brushes.White;
            SquareButton.Background = Brushes.White;
            RectangleButton.Background = Brushes.White;
            TriangleButton.Background = Brushes.White;
            PolygonButton.Background = Brushes.White;
            CursorButton.Background = Brushes.White;
            MarkingButton.Background = Brushes.White;
            PlusSizeButton.Background = Brushes.White;
            MinusSizeButton.Background = Brushes.White;
            RemoveButton.Background = Brushes.White;
            ClearButton.Background = Brushes.White;
        }
        private void AfterClick(object sender)
        {
            TryClosePolygon();
            ResetSelectedFigure();
            ResetObservableFigure();
            ResetButtonsBackgrounds();
            Button button = sender as Button;
            button.Background = Brushes.Gray;
        }
        private void Tool_Click(object sender, RoutedEventArgs e)
        {
            AfterClick(sender);

            Type type = Type.GetType("NPaint.State." + (sender as Button).Tag.ToString());

            menuState = (MenuState)Activator.CreateInstance(type);
        }
        private void PlusSizeButton_Click(object sender, RoutedEventArgs e)
        {
            if(SelectedFigure != null)
            {
                SelectedFigure.IncreaseSize();
                return;
            }
            if (ObservableFigure != null)
            {
                ObservableFigure.Notify_IncreaseSize();
            }
        }
        private void MinusSizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedFigure != null)
            {
                SelectedFigure.DecreaseSize();
                return;
            }
            if (ObservableFigure != null)
            {
                ObservableFigure.Notify_DecreaseSize();
            }
        }

        private void ChangeColor_Click(object sender, RoutedEventArgs e)
        {
            BorderIteratorSelected = false;

            if(!FillIteratorSelected)
            {
                ColorIterator.Background = Brushes.White;
            }

            Button button = sender as Button;

            BorderColorButton.Background = button.Background;

            if (SelectedFigure != null)
            {
                SelectedFigure.ChangeBorderColor(button.Background);
                return;
            }
            if (ObservableFigure != null)
            {
                ObservableFigure.Notify_ChangeBorderColor(button.Background);
            }
        }
        private void ChangeColor_RightClick(object sender, MouseButtonEventArgs e)
        {
            FillIteratorSelected = false;

            if (!BorderIteratorSelected)
            {
                ColorIterator.Background = Brushes.White;
            }

            Button button = sender as Button;

            FillColorButton.Background = button.Background;

            if (SelectedFigure != null)
            {
                SelectedFigure.ChangeFillColor(button.Background);
                return;
            }
            if (ObservableFigure !=null)
            {
                ObservableFigure.Notify_ChangeFillColor(button.Background);
            }
        }

        private void BorderThicknessySlider_ValueChanged(object sender, RoutedEventArgs e)
        {
            if(SelectedFigure != null)
            {
                SelectedFigure.ChangeBorderThickness(BorderThicknessySlider.Value);
                return;
            }
            if (ObservableFigure != null)
            {
                ObservableFigure.ChangeBorderThickness(BorderThicknessySlider.Value);
            }
        }
        private void TransparencySlider_ValueChanged(object sender, RoutedEventArgs e)
        {
            if (SelectedFigure != null)
            {
                SelectedFigure.ChangeTransparency(TransparencySlider.Value);
                return;
            }
            if (ObservableFigure != null)
            {
                ObservableFigure.Notify_ChangeTransparency(TransparencySlider.Value);
            }
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
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Czy na pewno chcesz kontynuować? Utracisz obecny canvas.", "Wczytaj", System.Windows.MessageBoxButton.YesNo);

            if (messageBoxResult == MessageBoxResult.Yes)
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
                    {
                        break;
                    }
                }

                if (true == restoreCanvasWindow.ShowDialog())
                {
                    this.RestoreCanvas(restoreCanvasWindow.canvasNameListbox.SelectedIndex);
                }
            }
        }

        private void RestoreCaretaker()
        {
            if (!File.Exists(this.canvasListFilePath))
            {
                File.Create(this.canvasListFilePath);

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

        private void RGB_Click(object sender, RoutedEventArgs e)
        {
            if(rgbWindow == null)
            {
                rgbWindow = new RGBWindow();
                rgbWindow.Owner = (MainWindow)Application.Current.MainWindow;
            }
            rgbWindow.Show();
        }
        private void ColorIterator_Click(object sender, RoutedEventArgs e)
        {
            FillIteratorSelected = false;
            BorderIteratorSelected = true;

            ColorIterator.Background = Brushes.Gray;

            if(!Iterator.isDone())
            {
                RGB rgb = Iterator.Next();
                
                BorderColorButton.Background = new SolidColorBrush(Color.FromRgb(Byte.Parse(rgb.R.ToString()), Byte.Parse(rgb.G.ToString()), Byte.Parse(rgb.B.ToString())));
            }
            if (SelectedFigure != null)
            {
                SelectedFigure.ChangeBorderColor(BorderColorButton.Background);
            }
        }
        private void ColorIterator_RightClick(object sender, MouseButtonEventArgs e)
        {
            BorderIteratorSelected = false;
            FillIteratorSelected = true;

            ColorIterator.Background = Brushes.Gray;

            if (!Iterator.isDone())
            {
                RGB rgb = Iterator.Next();

                FillColorButton.Background = new SolidColorBrush(Color.FromRgb(Byte.Parse(rgb.R.ToString()), Byte.Parse(rgb.G.ToString()), Byte.Parse(rgb.B.ToString())));
            }
            if (SelectedFigure != null)
            {
                SelectedFigure.ChangeFillColor(FillColorButton.Background);
            }
        }
    }
}