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

		private decimal qty1;

		public decimal Qty1
		{
			get { return qty1; }
			set { qty1 = value; /*OnPropertyChanged();*/ }
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

		private void btnBaseCharge_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("What is this for?", "Purpose unclear");
        }
    }
}