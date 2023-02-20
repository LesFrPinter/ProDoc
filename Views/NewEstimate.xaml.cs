using System.Windows;
using System.Data;

namespace ProDocEstimate.Views
{
	public partial class NewEstimate : Window
	{
		public NewEstimate()
		{ InitializeComponent();
			DataContext = this;
			customerText = "Customer name here";
			estimateText = "Estimate details here ";
			LoadData();
		}

		private string? message;      public string? Message      { get { return message;      } set { message = value; } }
		private string  customerText; public string? CustomerText { get { return customerText; } set { customerText = value; } }
		private string  estimateText; public string? EstimateText { get { return estimateText; } set { estimateText = value; } }

		private DataTable DgFeatures; public DataTable dgFeatures  { get { return DgFeatures; } set { DgFeatures = value; } }

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
					{ txtCustNo.Text = srch.CustomerCode.ToString(); lblCustName.Content = srch.CustomerName; 
					  lblMessage.Content = "(" + srch.CustomerCode + ") " + srch.CustomerName; }
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

		private void btnLoadTemplate_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("What happens here?", "Load Template", MessageBoxButton.OK, MessageBoxImage.Question);
    }

		private void btnBaseCharges_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("What happens here?", "Apply Base Charges", MessageBoxButton.OK, MessageBoxImage.Question);
		}

		private void txtEstimator_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			EstimatorLookup es = new EstimatorLookup();
			es.ShowDialog();
			txtEstimator.Text = es.EstimatorCode;
			lblEstimatorName.Content = es.Estimator;
			es.Close();
		}

		private void btnOptions_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("What Does this do?", "Not defined", MessageBoxButton.OK, MessageBoxImage.Question);
		}

		private void txtProductCode_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			MessageBox.Show("What happens here?", "Product Code", MessageBoxButton.OK, MessageBoxImage.Question);
		}

		private void txtPlantID_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			MessageBox.Show("What happens here?", "Plant ID", MessageBoxButton.OK, MessageBoxImage.Question);
		}

	  public void LoadData()
		{
			this.dgFeatures = new DataTable("Features");
			this.dgFeatures.Columns.Add("SEQ");
			this.dgFeatures.Columns.Add("FEATURE");
			this.dgFeatures.Columns.Add("FLAT");
			this.dgFeatures.Columns.Add("RUN");

			this.dgFeatures.Rows.Add("14", "PREPR = PRE PRESS",  "35.00", "0.00");
			this.dgFeatures.Rows.Add("16", "610-Trim",           "10.00", "1.00");
			this.dgFeatures.Rows.Add("17", "11-2-BLK-STD",       "45.00", "0.70");
			this.dgFeatures.Rows.Add("18", "11-20 THERMO INK",   "40.00", "2.00");
			this.dgFeatures.Rows.Add("19", "11-8-BACKER",        "40.00", "0.00");
			this.dgFeatures.Rows.Add("20", "11-1-1 STD COLOR",   "20.00", "0.60");
			this.dgFeatures.Rows.Add("21", "11-6-PMS MATCH",     "20.00", "0.50");
			this.dgFeatures.Rows.Add("22", "NANO-NaNoCopy Void", "25.00", "0.00");
			this.dgFeatures.Rows.Add("23", "HPO-1U-HP Digital Print 1 Up", "30.00", "30.00");

			MyGrid.ItemsSource = dgFeatures.DefaultView;
		}
	}
}