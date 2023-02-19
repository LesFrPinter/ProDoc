using System.Windows;

namespace ProDocEstimate.Views
{
	public partial class NewEstimate : Window
	{
		public NewEstimate()
		{ InitializeComponent();
			DataContext = this;
			customerText = "Customer name here";
			estimateText = "Estimate details here ";
		}

		private string? message;      public string? Message      { get { return message;      } set { message = value; } }
		private string  customerText; public string? CustomerText { get { return customerText; } set { customerText = value; } }
		private string  estimateText; public string? EstimateText { get { return estimateText; } set { estimateText = value; } }

		private void mnuExit_Click(object sender, RoutedEventArgs e)
		{ this.Close(); }

		private void mnuDelete_Click(object sender, RoutedEventArgs e)
		{ MessageBox.Show("Really?", "Not actually working at this time", MessageBoxButton.YesNo, MessageBoxImage.Question); }

		private void mnuClear_Click(object sender, RoutedEventArgs e)
		{ MessageBox.Show("Really?", "Not actually working at this time", MessageBoxButton.YesNo, MessageBoxImage.Information); }

		private void mnuPrint_Click(object sender, RoutedEventArgs e)
		{ MessageBox.Show("Printing...", "Not actually working at this time", MessageBoxButton.OK, MessageBoxImage.Information); }

		private void mnuCopy_Click(object sender, RoutedEventArgs e)
		{ MessageBox.Show("Copy to where?", "Not actually working at this time", MessageBoxButton.OK, MessageBoxImage.Question); }

		private void TextBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			if (rbGrid.IsChecked == true)
			{ CustomerLookup srch = new CustomerLookup();
				srch.ShowDialog();
				if(srch.CustomerCode != null)
					{ txtCustNo.Text = srch.CustomerCode.ToString(); lblCustName.Content = srch.CustomerName; }
				srch.Close();
			}
			else
			{ TreeViewCustSearch srch = new TreeViewCustSearch();
				srch.ShowDialog();
				txtCustNo.Text = srch.Custno.ToString();
				lblCustName.Content = srch.CustName;
				srch.Close();
			}
		}

		private void txtSalesRep_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{ SalesRepLookup sr = new SalesRepLookup();
			sr.ShowDialog();
			txtSalesRep.Text = sr.SalesRepCode;
			lblSalesRep.Content = sr.SalesRep;
			sr.Close();
		}
	}
}