using System;
using System.IO;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;
using Path = System.Windows.Shapes.Path;////

namespace NPaint.Figures
{
    public abstract class Figure : FigureBase, ICloneable
    {
        public Path adaptedPath { get; set; } //PROTECTED
        protected Geometry adaptedGeometry;
        protected Point startPoint;
        protected PointCollection PointsList;

        public void ChangeFillColor(Brush brush)
        {
            adaptedPath.Fill = brush;
        }

        public void ChangeBorderColor(Brush brush)
        {
            adaptedPath.Stroke = brush;
        }

        public void ChangeBorderThickness(double value)
        {
            adaptedPath.StrokeThickness = value;
        }

        public void ChangeTransparency(double value)
        {
            Brush brush = new SolidColorBrush(((SolidColorBrush)adaptedPath.Fill).Color);
            brush.Opacity = value;
            adaptedPath.Fill = brush;
        }

        public void SetStartPoint (Point point)
        {
            startPoint = point;
        }

        public abstract void MoveBy(Point point);

        public abstract void Resize(Point point);


        public object Clone()
        {
            //throw new NotImplementedException();
            Figure clonedFigure = this.MemberwiseClone() as Figure;
            clonedFigure.adaptedPath = clonePath();
            clonedFigure.adaptedGeometry = this.adaptedGeometry.Clone(); //lub metoda ala clonePath
            clonedFigure.startPoint.X = this.startPoint.X;//
            clonedFigure.startPoint.Y = this.startPoint.Y;
            clonedFigure.ChangeBorderColor(((MainWindow)System.Windows.Application.Current.MainWindow).BorderColorButton.Background);
            clonedFigure.ChangeFillColor(((MainWindow)System.Windows.Application.Current.MainWindow).FillColorButton.Background);
            clonedFigure.ChangeTransparency((100-((MainWindow)System.Windows.Application.Current.MainWindow).TransparencySlider.Value)/100);
            clonedFigure.ChangeBorderThickness(((MainWindow)Application.Current.MainWindow).BorderThicknessySlider.Value);
            if (this.PointsList != null)
            {
                clonedFigure.PointsList = this.PointsList.Clone();//
            }
            else
            {
                clonedFigure.PointsList = null;
            }
            return clonedFigure;
        }
        private Path clonePath()
        {
            string pathXaml = XamlWriter.Save(this.adaptedPath); //uproscic mozna opcjonalne saveując to jak canvasa
            StringReader stringReader = new StringReader(pathXaml);
            XmlReader xmlReader = XmlReader.Create(stringReader);
            return (Path) XamlReader.Load(xmlReader);
        }
        /*private Geometry cloneGeometry()
        {
            string geometryXaml = XamlWriter.Save(this.adaptedGeometry);
            StringReader stringReader = new StringReader(geometryXaml);
            XmlReader xmlReader = XmlReader.Create(stringReader);
            return (Geometry)XamlReader.Load(xmlReader);
        }*/

    }
}
