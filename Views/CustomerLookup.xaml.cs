using System.Data;
using System.Windows;
using System.Configuration;
using System.Windows.Input;
using System.Data.SqlClient;
using System.Windows.Controls;
using ProDocEstimate.ViewModels;

namespace ProDocEstimate.Views
{
	public partial class CustomerLookup : Window
	{
		public CustomerLookup()
		{ InitializeComponent();
			this.PreviewKeyDown += (ss, ee) =>
			{ if (ee.Key == Key.Escape) { this.Hide(); this.btnCancel.RaiseEvent(new RoutedEventArgs(Button.ClickEvent)); } };
		}

		public string ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
		public SqlConnection cn = new SqlConnection();
		public SqlDataAdapter ?da;

		LookupViewModel viewModel = new LookupViewModel();

		private string? searchType; public string? SearchType { get { return searchType; } set { searchType = value; } }
		private string? customerName; public string? CustomerName { get { return customerName; } set { customerName = value; } }
		private string? customerCode; public string? CustomerCode { get { return customerCode; } set { customerCode = value; } }

		private void btnOK_Click(object sender, RoutedEventArgs e)
		{ DataRowView? row = dgCustomers.SelectedItem as DataRowView;
			CustomerCode = row?.Row.ItemArray[1]?.ToString();
			CustomerName = row?.Row.ItemArray[2]?.ToString();
			this.Hide();
		}

		private void btnCancel_Click(object sender, RoutedEventArgs e)
		{ viewModel.CustName = ""; CustomerName = ""; CustomerCode = ""; this.DataContext = viewModel; this.Hide(); }

		private void btnGo_Click(object sender, RoutedEventArgs e)
		{ cn = new SqlConnection(ConnectionString);
			cn.Open();
			string prefix = (cbPartial.IsChecked == true) ? "Partial" : "Cust";
			string procname = (cbDesc.IsChecked == true) ? prefix + "NameDESC " : prefix + "NameASC ";
			string str = txtCustName.Text.Trim().Length > 0 ? txtCustName.Text : "''";
			da = new SqlDataAdapter(procname + str, cn);
			DataSet ds = new DataSet();
			da.Fill(ds);
			dgCustomers.ItemsSource = ds.Tables[0].DefaultView; dgCustomers.SelectedIndex = 0; 
			this.Title = ds.Tables[0].Rows.Count.ToString() + " rows matched.";
		}

		private void txtCustName_TextChanged(object sender, TextChangedEventArgs e)
		{ if(cbAuto.IsChecked != true) { return; }
			cn = new SqlConnection(ConnectionString);
			cn.Open();
			string prefix = (cbPartial.IsChecked == true) ? "Partial" : "Cust";
			string procname = (cbDesc.IsChecked == true) ? prefix + "NameDESC " : prefix + "NameASC ";
			string str = txtCustName.Text.Trim().Length > 0 ? txtCustName.Text : "''";
			da = new SqlDataAdapter(procname + str, cn);
			DataSet ds = new DataSet();
			da.Fill(ds);
			dgCustomers.ItemsSource = ds.Tables[0].DefaultView; dgCustomers.SelectedIndex = 0;
			this.Title = ds.Tables[0].Rows.Count.ToString() + " rows matched.";
		}

		private void dgCustomers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{ this.btnOK.RaiseEvent(new RoutedEventArgs(Button.ClickEvent)); }
	}
}