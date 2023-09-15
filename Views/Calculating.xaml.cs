using System.Runtime.CompilerServices;
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
            Top = 350;
            Left = 1300;
        }

    }
}