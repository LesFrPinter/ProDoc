﻿using System.Windows;

namespace ProDocEstimate
{
	public partial class MainWindow : Window
	{
		public MainWindow() { InitializeComponent(); }

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
//			Quotations quotations = new Quotations();
			Window quotations = new Quotations();
			quotations.Owner = this;
			quotations.ShowDialog();
		}

		private void mnuItems_Click(object sender, RoutedEventArgs e)
		{
//			Items items = new Items();
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
	}
}