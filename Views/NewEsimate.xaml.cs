using System.Windows;

namespace ProDocEstimate.Views
{
	public partial class NewEsimate : Window
	{
		public NewEsimate()
		{
			InitializeComponent();
		}
		private void mnuExit_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void mnuDelete_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Really?", "Not actually working at this time", MessageBoxButton.YesNo, MessageBoxImage.Question);
		}

		private void mnuClear_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Really?", "Not actually working at this time", MessageBoxButton.YesNo, MessageBoxImage.Information);
		}

		private void mnuPrint_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Printing...", "Not actually working at this time", MessageBoxButton.OK, MessageBoxImage.Information);
		}

		private void mnuCopy_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Copy to where?", "Not actually working at this time", MessageBoxButton.OK, MessageBoxImage.Question);
		}

	}
}