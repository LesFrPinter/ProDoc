using System.Windows;

namespace ProDocEstimate.Views
{
	public partial class CustomerLookup : Window
	{
		public CustomerLookup()
		{
			InitializeComponent();
		}

		private string? searchType; public  string? SearchType { get { return searchType; } set { searchType = value; } }
		private string? customerName; public string? CustomerName { get { return customerName; } set { customerName = value; } }
		private string? customerCode; public string? CustomerCode { get { return customerCode; } set { customerCode = value; } }

		private void btnOK_Click(object sender, RoutedEventArgs e)
		{
			CustomerName = "Ralph Fiennes"; CustomerCode = "32301";  // Get from the datagrid selected row
			this.Hide();
		}

		private void btnCancel_Click(object sender, RoutedEventArgs e)
		{
			CustomerName = ""; CustomerCode = "";
			this.Hide();
		}
	}
}
