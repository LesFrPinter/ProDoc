using System;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ProDocEstimate
{
	public partial class Customers : Window, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler? PropertyChanged;

		protected void OnPropertyChanged([CallerMemberName] string? name = null)
		{ PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

		//protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		//{ PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }

		private DataTable? custs;             public DataTable? Custs            { get { return custs;       } set { custs       = value; OnPropertyChanged(); } }
		private bool?      notediting = true; public bool?      NotEditing       { get { return notediting;  } set { notediting  = value; OnPropertyChanged(); } }
		private bool?      editing;           public bool?      Editing          { get { return editing;     } set { editing     = value; NotEditing = !editing; OnPropertyChanged(); } }
		private bool?      newCustomer;       public bool?      NewCustomer      { get { return newCustomer; } set { newCustomer = value; OnPropertyChanged(); } }
		private string?    selcust;           public string?    SelectedCustomer { get { return selcust;     } set { selcust     = value; OnPropertyChanged(); } }

		public Customers()
		{ InitializeComponent();
			this.DataContext = this;
			this.Editing = false;
			if 
				( NewCustomer != null ) { LoadCustData(); }
			else 
				{ this.Editing = true; txtCustomerName.Text = new String(' ', 40); txtAddress.Text = new String(' ', 40); }
		}

		private void LoadCustData()
		{

			//this.Custs = new DataTable("Custs");
			//this.Custs.Columns.Add("Customer");
			//this.Custs.Columns.Add("Address");
			//this.Custs.Columns.Add("City");
			//this.Custs.Columns.Add("State");
			//this.Custs.Columns.Add("ZIP");
			//this.Custs.Columns.Add("Phone");
			//this.Custs.Columns.Add("Contact");

			//this.Custs.Rows.Add("State of Alabama", "202 Main Street", "Mongomery", "AL", "20214", "(605) 761-1207", "George Raft");
			//this.Custs.Rows.Add("State of Alabama", "202 Main Street", "Mongomery", "AL", "20214", "(605) 761-4344", "Samatha Billart");
			//this.Custs.Rows.Add("State of Georgia", "114 Yale Avenue", "Dothan", "GA", "20214", "(706) 321-4033", "Sally");
			//this.Custs.Rows.Add("Chick Fil-A", "1001 Fannin Street", "Houston", "TX", "77024", "(713) 667-4309", "Donald Shellman");

			//this.MyGrid.DataContext = this;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Editing = true;
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			Editing = true;
			txtCustomerName.Focus();
		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			Editing = false;
		}

		private void Button_Click_3(object sender, RoutedEventArgs e)
		{
			Editing = false;
		}

		private void btnDelete_Click(object sender, RoutedEventArgs e)
		{
			var confirmResult =
				MessageBox.Show(
					"Delete?",
					"Confirm delete",
					MessageBoxButton.YesNo, MessageBoxImage.Question);
			if (confirmResult == MessageBoxResult.OK) { } else { }
		}

		private void btnClear_Click(object sender, RoutedEventArgs e)
		{

		}

		private void btnClear2_Click(object sender, RoutedEventArgs e)
		{

		}

		private void cmdSelect_Click(object sender, RoutedEventArgs e)
		{
			SelectedCustomer = "State of Georgia";
			this.Hide();
    }
  }

}