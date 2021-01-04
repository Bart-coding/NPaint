using NPaint.Memento;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using NPaint.Memento;

namespace NPaint
{
    /// <summary>
    /// Logika interakcji dla klasy RestoreCanvasWindow.xaml
    /// </summary>
    public partial class RestoreCanvasWindow : Window
    {
        
        public RestoreCanvasWindow()
        {
            InitializeComponent();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }


        private void Button_OK(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Button_Reject(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
