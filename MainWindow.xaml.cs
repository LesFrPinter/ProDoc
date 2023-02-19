using ProDocEstimate.Views;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Expressions.Functions;

namespace ProDocEstimate
{
	public partial class MainWindow : Window
	{
		public MainWindow() { InitializeComponent(); }

		private string? custName; public string? CustName { get { return custName; } set { custName = value; } }
		private string? custCode; public string? CustCode { get { return custCode; } set { custCode = value; } }

		private void mnuFileExit_Click(object sender, RoutedEventArgs e)
		{ Application.Current.Shutdown(); }

		private void mnuCustomers_Click(object sender, RoutedEventArgs e)
		{ 
			Window customers = new Customers();
			customers.Owner = this;
			customers.ShowDialog();
		}

		private void mnuQuotations_Click(object sender, RoutedEventArgs e)
		{ 
			Window quotations = new Quotations();
			quotations.Owner = this;
			quotations.ShowDialog();
		}

		private void mnuItems_Click(object sender, RoutedEventArgs e)
		{
			Window items = new Items();
			items.Owner = this;
			items.ShowDialog();
		}

		private void mnuPV01_Click(object sender, RoutedEventArgs e)
		{
			Window pv01 = new Views.PV01();
			pv01.Owner = this;
			pv01.ShowDialog();
		}

		private void mnuPV02_Click(object sender, RoutedEventArgs e)
		{
			Window pv02 = new Views.PV02();
			pv02.Owner = this;
			pv02.ShowDialog();
		}

		private void mnuStandards_Click(object sender, RoutedEventArgs e)
		{
			Window standards = new Views.Standards();
			standards.Owner = this;
			standards.ShowDialog();
		}

		private void mnuNewEstimate_Click(object sender, RoutedEventArgs e)
		{
			Window newEstimate = new Views.NewEstimate();
			newEstimate.Owner = this;
			newEstimate.ShowDialog();
		}

		private void mnuEdit_Click(object sender, RoutedEventArgs e)
		{
			CustomerLookup cl = new CustomerLookup();
			cl.SearchType = "Edit an existing quote";
			cl.lblSearchType.Content = cl.SearchType;
			cl.ShowDialog();
			CustName = cl.CustomerName;
			CustCode = cl.CustomerCode;
			if(CustName?.Length!=0)
			{ NewEstimate ne = new NewEstimate();
				ne.Message = "Editing an existing quote";
				ne.lblCustName.Content = CustName;
				ne.txtCustNo.Text = CustCode;
				ne.ShowDialog();
			}
				else
			{ MessageBox.Show("Lookup canceled", "Edit", MessageBoxButton.OK, MessageBoxImage.Information); }

			cl.Close();
    }

		private void mnuCopy_Click(object sender, RoutedEventArgs e)
		{
			CustomerLookup cl = new CustomerLookup();
			cl.SearchType = "Copy an existing quote";
			cl.lblSearchType.Content = cl.SearchType;
			cl.ShowDialog();
			CustName = cl.CustomerName;
			CustCode = cl.CustomerCode;
			if (CustName?.Length != 0)
			{ NewEstimate ne = new NewEstimate();
				ne.Message = "Copy of Quote # " + cl.CustomerCode + " with a new quote number";
				ne.lblCustName.Content = CustName;
				ne.txtCustNo.Text = CustCode;
				ne.ShowDialog();
			}
			else
			{ MessageBox.Show("Lookup canceled", "Edit", MessageBoxButton.OK, MessageBoxImage.Information); }

			cl.Close();
		}

		private void mnuBlank_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Display a blank quote screen with a new quote number", "New", MessageBoxButton.OK, MessageBoxImage.Information);
		}

		private void mnuCombo_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("What is this?", "New", MessageBoxButton.OK, MessageBoxImage.Question);
		}
	}
}