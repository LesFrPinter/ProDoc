using System.Windows;

namespace ProDocEstimate.Views
{
    public partial class Saved : Window
    {
        public Saved()
        {
            InitializeComponent();
        }

        public void OnLoad(object sender, RoutedEventArgs e)
        {
            this.Height = this.Height *= 1.8;
            this.Width = this.Width *= 1.8;
            Top = 150;
        }

    }
}