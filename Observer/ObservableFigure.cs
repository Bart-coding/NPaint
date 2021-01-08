using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shapes;
using NPaint.Figures;

namespace NPaint.Observer
{
    class ObservableFigure : NRectangle, Observable
    {
        private List<Figure> Observers;
        public ObservableFigure()
        {
            adaptedPath = new Path();
            adaptedPath.Fill = Brushes.Transparent;
            adaptedPath.StrokeThickness = 1;
            adaptedPath.Stroke = Brushes.Gray;
            adaptedPath.StrokeDashArray = new DoubleCollection() { 3 };
            adaptedGeometry = new RectangleGeometry();
            rect = new Rect();
            Observers = new List<Figure>();
        }
        public void Attach(Figure figure)
        {
            Observers.Add(figure); // dodanie do listy obserwujacych

            // efekt wizualny zaznaczenia
            figure.adaptedPath.StrokeDashArray = new DoubleCollection() { 0.5 };
            figure.adaptedPath.Effect = new DropShadowEffect();
        }
        public void DetachAll()
        {
            // usuniecie efektu wizualnego
            foreach(Figure figure in Observers)
            {
                figure.adaptedPath.Effect = null;
                figure.adaptedPath.StrokeDashArray = null;
            }

            Observers.Clear(); // wyczyszczenie listy obserwujacych
        }

        public void Notify(Point point)
        {
            foreach(Figure figure in Observers)
            {
                figure.MoveByInsideGroup(point);
            }
        }

        public void Notify_ChangeFillColor(Brush brush)
        {
            foreach (Figure figure in Observers)
            {
                figure.ChangeFillColor(brush);
            }
        }

        public void Notify_ChangeBorderColor(Brush brush)
        {
            foreach (Figure figure in Observers)
            {
                figure.ChangeBorderColor(brush);
            }
        }

        public void Notify_ChangeTransparency(double value)
        {
            foreach (Figure figure in Observers)
            {
                figure.ChangeTransparency(value);
            }
        }
        public override void MoveBy(Point point)
        {
            double widthShift = this.GetTopLeft().X - point.X;
            double lengthShift = this.GetTopLeft().Y - point.Y;
            Point shiftTmpPoint = new Point(widthShift, lengthShift);

            base.MoveBy(point); //póki co

            Notify(shiftTmpPoint);
        }
        public bool Contains(Figure figure)
        {
            PointCollection points = figure.GetPointCollection();
            // poki wszystkie figury nie maja ustalonej PointCollection
            if (points.Count == 0)
                return false;
            foreach (Point point in points)
            {
                // jezeli zaznaczenie nie zawiera danego punktu figury to zwracamy falsz
                if(!this.rect.Contains(point))
                {
                    return false;
                }
                else
                {
                    continue;
                }
            }
            // jezeli przeszlo po wszystkich punktach i nie zwrocilo falszu tzn. ze wszystkie punkty sie zawieraja w zaznaczeniu
            return true;
        }

        public override void ChangeBorderThickness(double value)
        {
            return;
        }
    }
}
