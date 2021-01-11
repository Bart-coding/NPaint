using NPaint.Observer;
using System;
using System.IO;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;
using System.Xml.Serialization;
using Path = System.Windows.Shapes.Path;

namespace NPaint.Figures
{
    [Serializable]

    public abstract class Figure : FigureBase, ICloneable
    {
        public Path adaptedPath { get; set; }
        public Geometry adaptedGeometry { get; set; }
        protected Point startPoint;
        protected PointCollection PointsList;

        public Figure()
        {
            PointsList = new PointCollection();
        }

        public void ChangeFillColor(Brush brush)
        {
            
            adaptedPath.Fill = brush;
        }

        public void ChangeBorderColor(Brush brush)
        {
            adaptedPath.Stroke = brush;
        }
        
        public void ChangeTransparency(double value)
        {
            Brush brush = new SolidColorBrush(((SolidColorBrush)adaptedPath.Fill).Color);
            brush.Opacity = value;
            adaptedPath.Fill = brush;
        }

        public virtual void SetStartPoint (Point point)
        {
            startPoint = point;
        }

        public Point GetStartPoint()
        {
            return startPoint;
        }

        public PointCollection GetPointCollection()
        {
            return PointsList;
        }

        public double GetBorderThickness()
        {
            return adaptedPath.StrokeThickness;
        }
        protected abstract void SetPointCollection();

        public abstract void IncreaseSize();
        public abstract void DecreaseSize();

        public abstract void MoveBy(Point point);
        public abstract void MoveByInsideGroup(Point point);
        public abstract void Draw(Point point);
        public abstract void SetFields(Path path);
        protected abstract void Repaint();
        public abstract void ChangeBorderThickness(double value);
        public abstract void ChangeBorderThicknessInsideGroup(double value, PointCollection pointCollectionOfSelection);

        public virtual object Clone()
        {
            Figure clonedFigure = this.MemberwiseClone() as Figure;

            clonedFigure.adaptedPath = clonePath();
            clonedFigure.adaptedGeometry = this.adaptedGeometry.Clone();
            clonedFigure.startPoint.X = this.startPoint.X;
            clonedFigure.startPoint.Y = this.startPoint.Y;

            if (this.PointsList != null)
            {
                clonedFigure.PointsList = this.PointsList.Clone();
            }
            else
            {
                clonedFigure.PointsList = null;
            }
            return clonedFigure;
        }

        private Path clonePath()
        {
            string pathXaml = XamlWriter.Save(this.adaptedPath);

            StringReader stringReader = new StringReader(pathXaml);
            XmlReader xmlReader = XmlReader.Create(stringReader);

            return (Path) XamlReader.Load(xmlReader);
        }
    }
}
