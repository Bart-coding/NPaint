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
        protected PointCollection PointsList;

        public Figure()
        {
            PointsList = new PointCollection();
        }

        public abstract void SetFields(Path path);
        public void ChangeFillColor(Brush brush)
        {
            adaptedPath.Fill = brush;
        }
        public void ChangeBorderColor(Brush brush)
        {
            adaptedPath.Stroke = brush;
        }
        public abstract void ChangeBorderThickness(double value);
        public abstract void ChangeBorderThicknessInsideGroup(double value, PointCollection pointCollectionOfSelection);
        public void ChangeTransparency(double value)
        {
            Brush brush = new SolidColorBrush(((SolidColorBrush)adaptedPath.Fill).Color)
            {
                Opacity = value
            };
            adaptedPath.Fill = brush;
        }
        public abstract void Draw(Point startPoint, Point currentPoint);
        public abstract void MoveBy(Point point);
        public abstract void MoveByInsideGroup(Point point);
        public abstract void IncreaseSize();
        public abstract void DecreaseSize();

        protected abstract void Repaint();

        public double GetBorderThickness()
        {
            return adaptedPath.StrokeThickness;
        }

        public PointCollection GetPointCollection()
        {
            return PointsList;
        }
        protected abstract void SetPointCollection();

        public virtual object Clone()
        {
            Figure clonedFigure = this.MemberwiseClone() as Figure;

            clonedFigure.adaptedPath = clonePath();
            clonedFigure.adaptedGeometry = this.adaptedGeometry.Clone();

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
