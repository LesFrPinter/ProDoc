using System.Data;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace ProDocEstimate.Views
{
	public partial class NewEstimate : Window, INotifyPropertyChanged
	{
		public NewEstimate()
		{ InitializeComponent();
			DataContext = this;
			customerText = "Customer name here";
			estimateText = @"065870 Snap Out   1 Part  11 x 8 1/2""";
			DataContext = this;
			LoadData(); LoadPaper(); LoadFinal();
			PricePerThou = 3.20F;
			Quantity = 20000;
			JobValue = 0.00F;
			AdjPct = 110.00F;
			TotalCost = 191.00F;
			TotalMatl =  30.00F;
			TotalSell = 0.00F;
			Freight = 0.00F;
			GrossProf = 191.00F;
			CostPerThou = 1.23F;
			Comments =
					"Price 1 wide only\n"
				+ "Upcharge 10%\n"
				+ "HP 1U HP Digital Print 1 up - includes barcode and numbering.";
			QuoteText = "Add whatever you want here.";
		}

		private string? message;      public string? Message      { get { return message;      } set { message      = value; } }
		private string? customerText; public string? CustomerText { get { return customerText; } set { customerText = value; } }
		private string? estimateText; public string? EstimateText { get { return estimateText; } set { estimateText = value; } }
		private int?    estimate;     public int?    Estimate     { get { return estimate;     } set { estimate     = value; OnPropertyChanged(); } }
		private bool?   autoGen;      public bool?   AutoGen      { get { return autoGen;      } set { autoGen      = value; OnPropertyChanged(); } }

		private float?  pricePerThou; public float?  PricePerThou { get { return pricePerThou; } set { pricePerThou = value; OnPropertyChanged(); } }
		private int?    quantity;     public int?    Quantity     { get { return quantity;     } set { quantity     = value; OnPropertyChanged(); } }
		private float?  jobValue;     public float?  JobValue     { get { return jobValue;     } set { jobValue     = value; OnPropertyChanged(); } }
		private float?  adjPct;       public float?  AdjPct       { get { return adjPct;       } set { adjPct       = value; OnPropertyChanged(); } }

		private float? totalCost; public float? TotalCost { get { return totalCost; } set { totalCost = value; OnPropertyChanged(); } }
		private float? totalMatl; public float? TotalMatl { get { return totalMatl; } set { totalMatl = value; OnPropertyChanged(); } }
		private float? totalSell; public float? TotalSell { get { return totalSell; } set { totalSell = value; OnPropertyChanged(); } }
		private float? freight;   public float? Freight   { get { return freight;   } set { freight   = value; OnPropertyChanged(); } }
		private float? grossProf; public float? GrossProf { get { return grossProf; } set { grossProf = value; OnPropertyChanged(); } }
		private float? costPerThou; public float? CostPerThou { get { return costPerThou; } set { costPerThou = value; OnPropertyChanged(); } }

		private string? comments; public string? Comments { get { return comments; } set { comments = value; OnPropertyChanged(); } }
		private string? quoteText; public string? QuoteText { get { return quoteText; } set { quoteText = value; OnPropertyChanged(); } }

		private DataTable? dtFeatures; public DataTable? DtFeatures { get { return dtFeatures; } set { dtFeatures = value; OnPropertyChanged(); } }
		private DataTable? dtPaper;    public DataTable? DtPaper    { get { return dtPaper;    } set { dtPaper    = value; OnPropertyChanged(); } }
		private DataTable? dtFinal;    public DataTable? DtFinal    { get { return dtFinal;    } set { dtFinal    = value; OnPropertyChanged(); } }

		public event PropertyChangedEventHandler? PropertyChanged;
		protected void OnPropertyChanged([CallerMemberName] string? name = null)
		{ PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

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
			{ 
				//TreeViewCustSearch srch = new TreeViewCustSearch();
				//srch.ShowDialog();
				//txtCustNo.Text = srch.Custno.ToString();
				//lblCustName.Content = srch.CustName;
				//srch.Close();
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
			this.dtFeatures = new DataTable("Features");
			this.dtFeatures.Columns.Add("SEQ");
			this.dtFeatures.Columns.Add("FEATURE");
			this.dtFeatures.Columns.Add("FLAT");
			this.dtFeatures.Columns.Add("RUN");

			this.dtFeatures.Rows.Add("14", "PREPR = PRE PRESS",  "35.00", "0.00");
			this.dtFeatures.Rows.Add("16", "610-Trim",           "10.00", "1.00");
			this.dtFeatures.Rows.Add("17", "11-2-BLK-STD",       "45.00", "0.70");
			this.dtFeatures.Rows.Add("18", "11-20 THERMO INK",   "40.00", "2.00");
			this.dtFeatures.Rows.Add("19", "11-8-BACKER",        "40.00", "0.00");
			this.dtFeatures.Rows.Add("20", "11-1-1 STD COLOR",   "20.00", "0.60");
			this.dtFeatures.Rows.Add("21", "11-6-PMS MATCH",     "20.00", "0.50");
			this.dtFeatures.Rows.Add("22", "NANO-NaNoCopy Void", "25.00", "0.00");
			this.dtFeatures.Rows.Add("23", "HPO-1U-HP Digital Print 1 Up", "30.00", "30.00");

  		MyGrid.ItemsSource = dtFeatures.DefaultView;
		}

		public void LoadPaper()
		{
			this.DtPaper = new DataTable("Paper");
			this.DtPaper.Columns.Add("PAPER");
			this.DtPaper.Columns.Add("LBSREQ");
			this.DtPaper.Columns.Add("WASTEPCT");
			this.DtPaper.Columns.Add("COSTPERLB");
			this.DtPaper.Columns.Add("EXTCOST");

			this.DtPaper.Rows.Add(@" 9 1/2"" 25.0# WHITE    PR - SEC VFT", "0", "28.00", "1.43", "0.00");

			dgPaper.Items.Clear();
			dgPaper.ItemsSource = DtPaper.DefaultView;
		}

		public void LoadFinal()
		{
			this.DtFinal = new DataTable("Final");
			this.DtFinal.Columns.Add("DEPARTMENT");
			this.DtFinal.Columns.Add("SETUPTIME");
			this.DtFinal.Columns.Add("SETUPCOST");
			this.DtFinal.Columns.Add("RUNTIME");
			this.DtFinal.Columns.Add("RUNCOST");

			this.DtFinal.Rows.Add("PRESS",    "1.61", "104.65", "0.000", "0.00");
			this.DtFinal.Rows.Add("COLLATOR", "0.00",   "0.00", "0.000", "0.00");
			this.DtFinal.Rows.Add("BINDERY",  "0.00",   "0.00", "0.000", "0.00");
			this.DtFinal.Rows.Add("PREP",     "1.61",  "56.35", "", "IMPR: 50");

			dgFinal.Items.Clear();
			dgFinal.ItemsSource = DtFinal.DefaultView;
		}


		private void chkAutoGen_Checked(object sender, RoutedEventArgs e)
		{
			// AutoGen = !AutoGen; 
			if (AutoGen==true) { Estimate = 65870; } else { Estimate = null; }
		}
	}
}