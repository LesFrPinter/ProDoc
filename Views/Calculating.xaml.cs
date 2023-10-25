using System.Windows;

namespace ProDocEstimate.Views
{
    public partial class Calculating : Window
    {
        public Calculating()
        {
            InitializeComponent();
        }

        public void OnLoad(object sender, RoutedEventArgs e)
        {
            this.Left = this.Owner.Left + 50;
            this.Top  = this.Owner.Top + 262;
        }

    }
}