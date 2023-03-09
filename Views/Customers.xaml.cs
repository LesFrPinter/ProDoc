using System;
using System.Data;
using System.Windows;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Configuration;
using System.Windows.Controls;

namespace ProDocEstimate {
	public partial class Customers : Window, INotifyPropertyChanged {

		public string? ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
		public SqlConnection? cn;
		public SqlDataAdapter? da;

		public event PropertyChangedEventHandler? PropertyChanged;
		protected void OnPropertyChanged([CallerMemberName] string? name = null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

		private DataTable? search;           public DataTable? Search    { get { return search;         } set { search         = value; OnPropertyChanged(); } }

		private bool? noteditingComp = true; public bool? NotEditingComp { get { return noteditingComp; } set { noteditingComp = value; OnPropertyChanged(); } }
		private bool? editingComp;           public bool? EditingComp    { get { return editingComp;    } set { editingComp    = value; NotEditingComp = !EditingComp; OnPropertyChanged(); } }

		private bool? noteditingCont = true; public bool? NotEditingCont { get { return noteditingCont; } set { noteditingCont = value; OnPropertyChanged(); } }
		private bool? editingCont;           public bool? EditingCont    { get { return editingCont;    } set { editingCont    = value; NotEditingCont = !EditingCont; OnPropertyChanged(); } }

		private bool? noteditingLoc  = true; public bool? NotEditingLoc  { get { return noteditingLoc;  } set { noteditingLoc  = value; OnPropertyChanged(); } }
		private bool? editingLoc;            public bool? EditingLoc     { get { return editingLoc;     } set { editingLoc     = value; NotEditingLoc  = !EditingLoc; OnPropertyChanged(); } }

		private bool? newCustomer; public bool?   NewCustomer            { get { return newCustomer;    } set { newCustomer    = value; OnPropertyChanged(); } }
		private string? selcust;   public string? SelectedCustomer       { get { return selcust;        } set { selcust        = value; OnPropertyChanged(); } }

		private string? searchCustomer; public string? SearchCustomer    { get { return searchCustomer; } set { searchCustomer = value; OnPropertyChanged(); } }
		private string? searchContact;  public string? SearchContact     { get { return searchContact;  } set { searchContact  = value; OnPropertyChanged(); } }
		private string? searchOther;    public string? SearchOther       { get { return searchOther;    } set { searchOther    = value; OnPropertyChanged(); } }

		public Customers() {

			InitializeComponent();
			this.DataContext = this;

			this.EditingComp = false;  // should be set to False initially...
			this.EditingCont = false;  // should be set to False initially...
			this.EditingLoc = false;  // should be set to False initially...

		}

		private void LoadCustData() {		}

		private void btnDelete_Click(object sender, RoutedEventArgs e) {
			var confirmResult =
				MessageBox.Show( "Delete?", "Confirm delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
			if (confirmResult == MessageBoxResult.OK) { } else { }
		}

		private void btnClear_Click(object sender, RoutedEventArgs e) {
			SearchCustomer = ""; SearchContact = ""; SearchOther = ""; // OnPropertyChanged();
		}

		private void btnApply_Click(object sender, RoutedEventArgs e) {
			string cmd = "SELECT" +
				" CUSTOMERS.CUST_NAME, LOCATIONS.ADDRESS AS LOCATION, LOCATIONS.CITY, LOCATIONS.STATE, " +
				" CUSTOMERS.PHONE AS CorpPhone, CONTACTS.PHONE AS ContPhone, CONTACTS.ContactName, CONTACTS.Notes," +
				" CUSTOMERS.ID AS CustID, lOCATIONS.ID AS LocID, CONTACTS.ID AS ContID" +
				" FROM CUSTOMERS, LOCATIONS, CONTACTS WHERE" +
				" CUSTOMERS.ID = LOCATIONS.CUSTOMERID   AND" +
				" LOCATIONS.ID = CONTACTS.LOCATIONID    AND" +
				" CUST_NAME LIKE '%" + SearchCustomer + "%' ORDER BY CUST_NAME";
			cn = new(ConnectionString);
			da = new(cmd, cn);
			DataTable dt = new DataTable();
			da.Fill(dt);
			dgSearch.ItemsSource = dt.DefaultView;
		}

		private void dgSearch_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e) {
			DataGrid dg = sender as DataGrid;
			if (dg != null) {
				string CustID = (((dg.SelectedItems[0]) as DataRowView).Row["CustID"]).ToString();
				string LocID = (((dg.SelectedItems[0]) as DataRowView).Row["LocID"]).ToString();
				string ContID = (((dg.SelectedItems[0]) as DataRowView).Row["ContID"]).ToString();

				string cmd = "SELECT * FROM CUSTOMERS WHERE ID = '" + CustID + "'";
				cn = new(ConnectionString); da = new(cmd, cn);
				DataTable dt = new DataTable(); da.Fill(dt); 
				txtCustomerName.Text = dt.Rows[0]["CUST_NAME"].ToString();
				txtAddress.Text = dt.Rows[0]["ADDRESS1"].ToString();
				txtCity.Text = dt.Rows[0]["CITY"].ToString();
				txtState.Text = dt.Rows[0]["STATE"].ToString();
				txtPhone.Text = dt.Rows[0]["PHONE"].ToString();

				cmd = "SELECT * FROM LOCATIONS WHERE ID = '" + LocID + "'";
				cn = new(ConnectionString); da = new(cmd, cn);
				dt.Clear(); da.Fill(dt);
				txtLocationName.Text = dt.Rows[0]["Location"].ToString();
				txtLocationAddress.Text = dt.Rows[0]["Address"].ToString();
				txtLocationCity.Text = dt.Rows[0]["City"].ToString();
				txtLocationState.Text = dt.Rows[0]["State"].ToString();
				txtLocationPhone.Text = dt.Rows[0]["Phone"].ToString();

				cmd = "SELECT * FROM CONTACTS WHERE ID = '" + ContID + "'";
				cn = new(ConnectionString); da = new(cmd, cn);
				dt.Clear(); da.Fill(dt);
				txtContactName.Text = dt.Rows[0]["ContactName"].ToString();
				txtContactPhone.Text = dt.Rows[0]["Phone"].ToString();

				tabPage.SelectedIndex = 0;

			}
		}

		private void btnCompAdd_Click   (object sender, RoutedEventArgs e) { EditingComp = true; txtCustomerName.Focus(); }
		private void btnCompEdit_Click  (object sender, RoutedEventArgs e) { EditingComp = true; txtCustomerName.Focus(); }
		private void btnCompSave_Click  (object sender, RoutedEventArgs e) { EditingComp = false; }
		private void btnCompCancel_Click(object sender, RoutedEventArgs e) { EditingComp = false; }

		private void btnAddCont_Click   (object sender, RoutedEventArgs e) { EditingCont = true; txtContactName.Focus(); }
		private void btnEditCont_Click  (object sender, RoutedEventArgs e) { EditingCont = true; txtContactName.Focus(); }
		private void btnSaveCont_Click  (object sender, RoutedEventArgs e) { EditingCont = false; }
		private void btnCancCont_Click  (object sender, RoutedEventArgs e) { EditingCont = false; }

		private void btnAddLoc_Click    (object sender, RoutedEventArgs e) { EditingLoc = true; txtLocationName.Focus(); }
		private void btnEditLoc_Click   (object sender, RoutedEventArgs e) { EditingLoc = true; txtLocationName.Focus(); }
		private void btnSaveLoc_Click   (object sender, RoutedEventArgs e) { EditingLoc = false; }
		private void btnCancLoc_Click   (object sender, RoutedEventArgs e) { EditingLoc = false; }

	}
}

//public class Record {
//		public string? CUST_NAME { get; set; } 
//		public string? LOCATION { get; set; }
//		public string? CITY { get; set; }
//		public string? STATE { get; set; }
//		public string? CorpPhone { get; set; }
//		public string? ContPhone { get; set; }
//		public string? ContactName { get; set; }
//		public string? Notes { get; set; }
//		public string? CustID { get; set; }
//		public string? LocID { get; set; }
//		public string? ContID { get; set; }
//	}