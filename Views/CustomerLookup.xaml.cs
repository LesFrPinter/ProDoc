using ProDocEstimate.ViewModels;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ProDocEstimate.Views
{
    public partial class CustomerLookup : Window
	{
		public CustomerLookup()
		{
			InitializeComponent();
			this.PreviewKeyDown += (ss, ee) => 
			{ if (ee.Key == Key.Escape) { this.Hide(); this.btnCancel.RaiseEvent(new RoutedEventArgs(Button.ClickEvent)); } 
			};
		}

		LookupViewModel viewModel = new LookupViewModel();
		
		private string? searchType; public  string? SearchType { get { return searchType; } set { searchType = value; } }
		private string? customerName; public string? CustomerName { get { return customerName; } set { customerName = value; } }
		private string? customerCode; public string? CustomerCode { get { return customerCode; } set { customerCode = value; } }

		private void btnOK_Click(object sender, RoutedEventArgs e)
		{
			viewModel.CustName = "Ralph Fiennes"; CustomerName = viewModel.CustName; CustomerCode = "32301";  // Get from the datagrid selected row
			this.DataContext = viewModel;
			this.Hide();
		}

		private void btnCancel_Click(object sender, RoutedEventArgs e)
		{
			viewModel.CustName = ""; CustomerName = ""; CustomerCode = "";
			this.DataContext = viewModel;
			this.Hide();
		}

		private void btnGo_Click(object sender, RoutedEventArgs e)
		{

        }
    }
}