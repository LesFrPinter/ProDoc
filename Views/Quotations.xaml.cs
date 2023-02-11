using System;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ProDocEstimate
{
	public partial class Quotations : Window, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler? PropertyChanged;

		private string customerName;
		public string CustomerName
		{
			get { return customerName; }
			set { customerName = value;/* OnPropertyChanged(""); */ }
		}

		public Quotations() { InitializeComponent(); }

		private void btnLookup_Click(object sender, RoutedEventArgs e)
		{
			Customers lookup = new Customers();
			lookup.tabPage.TabIndex = 2;
			lookup.tabPageTreeView.IsSelected = true;
			lookup.ShowDialog();
			MessageBox.Show(" Selected: New Prospect " + lookup.SelectedCustomer, "Selected");
			lookup.Close();
		}

		private void btnNew_Click(object sender, RoutedEventArgs e)
		{
			Customers lookup = new Customers();
			lookup.NewCustomer = true;
			lookup.ShowDialog();
			MessageBox.Show(" Selected: New Prospect " + lookup.SelectedCustomer, "Selected");
			lookup.Close();			
		}
	}
}