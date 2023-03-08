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

		private DataTable? search;            public DataTable? Search           { get { return search;      } set { search      = value; OnPropertyChanged(); } }
		private bool?      notediting = true; public bool?      NotEditing       { get { return notediting;  } set { notediting  = value; OnPropertyChanged(); } }
		private bool?      editing;           public bool?      Editing          { get { return editing;     } set { editing     = value; NotEditing = !editing; OnPropertyChanged(); } }
		private bool?      newCustomer;       public bool?      NewCustomer      { get { return newCustomer; } set { newCustomer = value; OnPropertyChanged(); } }
		private string?    selcust;           public string?    SelectedCustomer { get { return selcust;     } set { selcust     = value; OnPropertyChanged(); } }

		private string? searchCustomer; public string? SearchCustomer { get { return searchCustomer; } set { searchCustomer = value; OnPropertyChanged(); } }
		private string? searchContact;  public string? SearchContact  { get { return searchContact;  } set { searchContact  = value; OnPropertyChanged(); } }
		private string? searchOther   ; public string? SearchOther    { get { return searchOther   ; } set { searchOther    = value; OnPropertyChanged(); } }

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
			SearchCustomer = ""; SearchContact = ""; SearchOther = ""; // OnPropertyChanged();
		}

		private void cmdSelect_Click(object sender, RoutedEventArgs e)
		{
			SelectedCustomer = "State of Georgia";
			this.Hide();
    }

		private void btnApply_Click(object sender, RoutedEventArgs e) {

		}
	}

}