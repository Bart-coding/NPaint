using System;
using System.Collections.Generic;
using System.Linq;
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
            Notify_DeleteSelectionVisualEffect();
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

        public void Notify_ChangeBorderThickness(double value)
        {
            foreach (Figure figure in Observers)
            {
                figure.ChangeBorderThicknessInsideGroup(value, this.GetPointCollection());
            }
        }
        
        public void Notify_IncreaseSize()
        {
            Point[] bounds = new Point[PointsList.Count];
            PointsList.CopyTo(bounds, 0);
            foreach (Figure figure in Observers)
            {
                double figureThicknessHalf = figure.GetBorderThickness()/2;
                bounds[0].X += figureThicknessHalf;//
                bounds[1].X -= figureThicknessHalf;
                bounds[0].Y += figureThicknessHalf;
                bounds[1].Y -= figureThicknessHalf;


                if (figure.GetPointCollection().Any(point => point.X < bounds[0].X
                || point.X > bounds[1].X
                || point.Y < bounds[0].Y
                || point.Y > bounds[1].Y))
                    continue;


                figure.IncreaseSize();
            }
        }

        public void Notify_DecreaseSize()
        {
            foreach (Figure figure in Observers)
            {
                figure.DecreaseSize();
            }
        }

        public void Notify_AddSelectionVisualEffect()
        {
            foreach (Figure figure in Observers)
            {
                figure.adaptedPath.StrokeDashArray = new DoubleCollection() { 0.5 };
                figure.adaptedPath.Effect = new DropShadowEffect();
            }
            
        }
        public void Notify_DeleteSelectionVisualEffect()
        {
            foreach (Figure figure in Observers)
            {
                figure.adaptedPath.StrokeDashArray = null;
                figure.adaptedPath.Effect = null;
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
            Notify_ChangeBorderThickness(value);
        }
    }
}
