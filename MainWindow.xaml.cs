using System.Windows;

namespace ProDocEstimate
{
	public partial class MainWindow : Window
	{
		public MainWindow() { InitializeComponent(); }

		private void mnuFileExit_Click(object sender, RoutedEventArgs e)
		{ Application.Current.Shutdown(); }

		private void mnuCustomers_Click(object sender, RoutedEventArgs e)
		{ Customers customers = new Customers(); }

		private void mnuQuotations_Click(object sender, RoutedEventArgs e)
		{ Quotations quotations = new Quotations(); }

		private void mnuItems_Click(object sender, RoutedEventArgs e)
		{ Items items = new Items(); }

	}
}