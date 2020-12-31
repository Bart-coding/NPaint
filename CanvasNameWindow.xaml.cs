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

namespace NPaint
{
    /// <summary>
    /// Logika interakcji dla klasy CanvasName.xaml
    /// </summary>
    public partial class CanvasNameWindow : Window
    {
        public CanvasNameWindow()
        {
            InitializeComponent();
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
