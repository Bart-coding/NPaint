using NPaint.Figures;
using NPaint.Iterator_singleton;
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
        public Canvas canvas;
        private List<Figure> FigureList;
        public Figure SelectedFigure;
        private ObservableFigure ObservableFigure;
        
        private readonly RGBIterator Iterator = RGBIterator.getIterator();
        private readonly ShapeFactory shapeFactory = ShapeFactory.GetShapeFactory();

        public bool BorderIteratorSelected = false;
        public bool FillIteratorSelected = false;

        private readonly Caretaker caretaker;
        private readonly Originator originator;
        private readonly string canvasPath = @"..\..\..\Canvases\";
        private readonly string canvasListFilePath = @"..\..\..\Canvases\CanvasList.txt";

        private RGBWindow rgbWindow = null;

        public MainWindow()
        {
            InitializeComponent();
            FigureList = new List<Figure>();
            caretaker = new Caretaker();
            originator = new Originator();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FillrColorLabel.Width = BorderColorLabel.ActualWidth;
            AddCanvas();
            RestoreCaretaker();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            using (StreamWriter sw = new StreamWriter(this.canvasListFilePath, false))
            {
                for (int i = 0; ; i++)
                {
                    CanvasMemento memento = this.caretaker.GetMemento(i);
                    if (memento != null)
                    {
                        String canvasName = this.originator.RestoreFromMemento(memento);
                        sw.WriteLine(canvasName);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        private List<Figure> RestoreFigureList(UIElementCollection CanvasChildren)
        {
            List<Figure> RestoredFigureList = new List<Figure>();
            foreach (UIElement canvasChild in CanvasChildren)
            {
                if (canvasChild.GetType() == typeof(System.Windows.Shapes.Path))
                {
                    System.Windows.Shapes.Path path = canvasChild as System.Windows.Shapes.Path;
                    Figure prototype = (Figure)shapeFactory.GetFigure(path.Tag as String);
                    if(prototype != null)
                    {
                        Figure figure = (Figure)prototype.Clone();
                        figure.SetFields(path);
                        RestoredFigureList.Add(figure);
                    }
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
                canvas.Background = Brushes.Transparent;   // przypisanie tla do canvasa, zeby przechwytywac eventy
            }
            Grid.SetRow(canvas, 1); // przypisanie canvasa do pierwszego wiersza w Gridzie

            // przypisanie eventow do canvasa
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
            {
                if (Mouse.LeftButton == MouseButtonState.Pressed)
                {
                    if (this.Cursor != Cursors.SizeAll)
                        this.Cursor = Cursors.SizeAll;
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
                        // zaleznie od stanu podejmujemy akcje

                        if (this.Cursor != Cursors.Hand) //Cursors.SizeNESW
                        {
                            this.Cursor = Cursors.Hand;
                        }

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
            // zaleznie od stanu podejmujemy akcje
            if (menuState != null)
            {
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

        public void AddObservable(Figure figure)
        {
            // musimy usunac starego obserwowanego z canvasa
            if(ObservableFigure != null)
            {
                canvas.Children.Remove(ObservableFigure.adaptedPath);
            }

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
                SelectedFigure.adaptedPath.StrokeDashArray = null;
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
                // jezeli ostatnio dodana figura do listy to wielokat, ktory nie jest zamkniety
                if (FigureList.Last().GetType() == typeof(NPolygon) && !((NPolygon)FigureList.Last()).PathFigure.IsClosed)
                {
                    ((NPolygon)FigureList.Last()).CloseFigure();    // zamykamy figure
                    SetSelectedFigure(FigureList.Last());
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

        private void FigureMenu_Click(object sender, RoutedEventArgs e)
        {
            AfterClick(sender);
            string Tag = (sender as Button).Tag.ToString();
            Type type = Type.GetType("NPaint.State." + Tag + "State");
            Figure figure = shapeFactory.GetFigure(Tag);
            menuState = (MenuState)Activator.CreateInstance(type, figure);
        }

        private void Tool_Click(object sender, RoutedEventArgs e)
        {
            AfterClick(sender);
            string Tag = (sender as Button).Tag.ToString();
            Type type = Type.GetType("NPaint.State." + Tag + "State");
            menuState = (MenuState)Activator.CreateInstance(type);
        }

        private void PlusSizeButton_Click(object sender, RoutedEventArgs e)
        {
            if(SelectedFigure != null)
            {
                SelectedFigure.IncreaseSize();
                return;
            }
            if(ObservableFigure != null)
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
                ObservableFigure.Notify_ChangeBorderThickness(BorderThicknessySlider.Value);
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
                    CanvasMemento memento = this.caretaker.GetMemento(i);

                    if (memento != null)
                    {
                        String canvasName = this.originator.RestoreFromMemento(memento);
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

        private void SerializeCanvas(string fileName)
        {
            fileName += ".txt";
            string newPath = this.canvasPath + fileName;

            //Przed zapisem Canvasa resetujemy chwilowo to co związane z CursorsState i SelectionState
            if (SelectedFigure != null)
            {
                SelectedFigure.adaptedPath.StrokeDashArray = null;
            }
            if (ObservableFigure != null)
            {
                this.canvas.Children.Remove(ObservableFigure.adaptedPath);
                ObservableFigure.Notify_DeleteSelectionVisualEffect();
            }

            string CanvasXAML = XamlWriter.Save(this.canvas); //Zapisujemy Canvas

            //Po zapisie przywracamy to co chwilowo zresetowaliśmy
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
                this.caretaker.AddMemento(this.originator.CreateMemento(fileName));
            }
            catch (Exception e)
            {
                MessageBox.Show("Wyjatek: " + e.Message);
            }
        }

        private void RestoreCanvas(int index)
        {
            if (SelectedFigure != null)
            {
                SelectedFigure = null;
            }
            if (ObservableFigure != null)
            {
                ObservableFigure = null;
            }

            string oldCanvasFile = this.originator.RestoreFromMemento(this.caretaker.GetMemento(index)); //Odczyt z listy Memento
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
                FigureList = RestoreFigureList(canvas.Children);
            }
            catch (IOException e)
            {
                MessageBox.Show("Nie można odczytać pliku: " + e.Message);
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
                        this.caretaker.AddMemento(this.originator.CreateMemento(line));
                    }
                }
            }
        }

        private void RGB_Click(object sender, RoutedEventArgs e)
        {
            if(rgbWindow == null)
            {
                rgbWindow = new RGBWindow
                {
                    Owner = (MainWindow)Application.Current.MainWindow
                };
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