using System.Windows;
using System.Windows.Controls;

namespace NewWpf1.View.UserControls
{
	public partial class MenuBar : UserControl
    {
        public MenuBar()
        { InitializeComponent(); }

		    private void MenuItem_Click(object sender, RoutedEventArgs e)
		    { App.Current.Shutdown(); }
		}
}