using System.Data;
using System.Windows;
using System.Configuration;
using System.Windows.Input;
using System.Data.SqlClient;
using System.Windows.Controls;
//using ProDocEstimate.ViewModels;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ProDocEstimate.Views
{
	public partial class CustomerLookup : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

		private bool auto; 
		public bool Auto 
		{ get { return auto; } 
		  set { auto = value; OnPropertyChanged();
			} }

		private Visibility goButtonVisibility;
        public  Visibility GoButtonVisibility
        { get { goButtonVisibility = (Auto)? Visibility.Hidden: Visibility.Visible; return goButtonVisibility; }
		  set { goButtonVisibility = value; OnPropertyChanged(); } }

		private bool desc; public bool Desc { get { return desc; } set { desc = value; OnPropertyChanged(); } }

		public CustomerLookup()
		{ InitializeComponent();
			this.DataContext = this;
			Auto= true;		// Default to show matches as they type

			this.PreviewKeyDown += (ss, ee) =>
			{ if (ee.Key == Key.Escape) { this.Hide(); this.btnCancel.RaiseEvent(new RoutedEventArgs(Button.ClickEvent)); } };
		}

		private string cust_name; public string Cust_Name { get { return cust_name; } set { cust_name = value; OnPropertyChanged(); } }
        private string cont_name; public string Cont_Name { get { return cont_name; } set { cont_name = value; OnPropertyChanged(); } }

        public string ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
		public SqlConnection cn = new SqlConnection();
		public SqlDataAdapter ?da;

		private string? searchType; public string? SearchType { get { return searchType; } set { searchType = value; } }

		// These two values are returned to the QUOTATIONS form...
		private string? customerName; public string? CustomerName { get { return customerName; } set { customerName = value; OnPropertyChanged(); LookupCustomer(); } }
		private string? customerCode; public string? CustomerCode { get { return customerCode; } set { customerCode = value; } }

		private void btnOK_Click(object sender, RoutedEventArgs e)
		{ DataRowView? row = dgCustomers.SelectedItem as DataRowView;
			CustomerName = row?.Row.ItemArray[0]?.ToString();
			CustomerCode = row?.Row.ItemArray[2]?.ToString();
			this.Hide();
		}

		private void btnCancel_Click(object sender, RoutedEventArgs e)
		{
            //	viewModel.CustName = ""; CustomerName = ""; CustomerCode = ""; this.DataContext = viewModel; 
            CustomerName = ""; CustomerCode = "";
            this.Hide(); 
		}

		private void btnGo_Click(object sender, RoutedEventArgs e)
		{ LookupCustomer(); }

		private void txtCustName_TextChanged(object sender, TextChangedEventArgs e)
        { LookupCustomer(); }

        private void LookupCustomer()
        {
            if (!Auto) { return; }

			string str = "SELECT C.CUST_NAME, CC.CONTACT_NAME, C.CUST_NUMB, CC.ADDRESS1, CC.CITY, CC.STATE, CC.PHONE, CC.E_MAIL "
						+ " FROM [ESTIMATING].[dbo].[CUSTOMER] C, [ESTIMATING].[dbo].[CUSTOMER_CONTACT] CC "
                        + " WHERE C.CUST_NUMB = CC.CUST_NUMB AND ";
	// "Partial" means that what they entered can be anywhere in the name; otherwise it means "starts with"...
			if (cbPartial.IsChecked == true)
				{ str += (txtCustName.Text.TrimEnd().Length > 0) ? " CUST_NAME LIKE '%" + Cust_Name + "%' " : " CONTACT_NAME LIKE '%" + Cont_Name + "%' "; }
			else
	            { str += (txtCustName.Text.TrimEnd().Length > 0) ? " CUST_NAME LIKE '"  + Cust_Name + "%' " : " CONTACT_NAME LIKE '"  + Cont_Name + "%' ";  }
            str += (txtCustName.Text.TrimEnd().Length > 0) ? " ORDER BY CUST_NAME" : " ORDER BY CONTACT_NAME";
            str += (Desc) ? " DESC" : "";

            cn = new SqlConnection(ConnectionString);
            da = new SqlDataAdapter(str, cn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dgCustomers.ItemsSource = ds.Tables[0].DefaultView; dgCustomers.SelectedIndex = 0;
            this.Title = ds.Tables[0].Rows.Count.ToString() + " rows matched.";
        }

        private void dgCustomers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{ this.btnOK.RaiseEvent(new RoutedEventArgs(Button.ClickEvent)); }
	}
}