using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for RGBWindow.xaml
    /// </summary>
    public partial class RGBWindow : Window
    {
        public RGBWindow()
        {
            InitializeComponent();
            R_TextBox.Focus();
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(R_TextBox != null && G_TextBox != null && B_TextBox != null && RGBButton != null && R_TextBox.Text != "" && G_TextBox.Text != "" && B_TextBox.Text != "")
            {
                if (int.Parse(R_TextBox.Text) <= 255 && int.Parse(G_TextBox.Text) <= 255 && int.Parse(B_TextBox.Text) <= 255)
                {
                    SlideR.Value = int.Parse(R_TextBox.Text);
                    SlideG.Value = int.Parse(G_TextBox.Text);
                    SlideB.Value = int.Parse(B_TextBox.Text);

                    RGBButton.Background = new SolidColorBrush(Color.FromRgb(Byte.Parse(R_TextBox.Text), Byte.Parse(G_TextBox.Text), Byte.Parse(B_TextBox.Text)));
                    return;
                }
                MessageBox.Show("Wpisz wartości dla RGB z przedziału 0-255", "Błędna wartość", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (R_TextBox != null && G_TextBox != null && B_TextBox != null && SlideR != null && SlideG != null && SlideB != null)
            {
                R_TextBox.Text = ((int)SlideR.Value).ToString();
                G_TextBox.Text = ((int)SlideG.Value).ToString();
                B_TextBox.Text = ((int)SlideB.Value).ToString();
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private static readonly Regex _regex = new Regex("[^0-9]");

        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        private void SelectAddress(object sender, RoutedEventArgs e)
        {
            TextBox tb = (sender as TextBox);
            if (tb != null)
            {
                tb.SelectAll();
            }
        }

        private void ChangeColor_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).BorderIteratorSelected = false;

            Button button = sender as Button;

            ((MainWindow)Application.Current.MainWindow).BorderColorButton.Background = button.Background;

            if (((MainWindow)Application.Current.MainWindow).SelectedFigure != null)
            {
                ((MainWindow)Application.Current.MainWindow).SelectedFigure.ChangeBorderColor(button.Background);
            }
        }

        private void ChangeColor_RightClick(object sender, MouseButtonEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).FillIteratorSelected = false;

            Button button = sender as Button;

            ((MainWindow)Application.Current.MainWindow).FillColorButton.Background = button.Background;

            if (((MainWindow)Application.Current.MainWindow).SelectedFigure != null)
            {
                ((MainWindow)Application.Current.MainWindow).SelectedFigure.ChangeFillColor(button.Background);
            }
        }
    }
}
