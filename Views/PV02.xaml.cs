using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ProDocEstimate.Views
{
	public partial class PV02 : Window, INotifyPropertyChanged
    {

		public PV02()
        {
            InitializeComponent();
						LoadData();
						LoadSummaryData();
						LoadOtherData();
				}

		public event PropertyChangedEventHandler? PropertyChanged;

		protected void OnPropertyChanged([CallerMemberName] string? name = null)
		{ PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

		private DataTable? est;
		public DataTable? Est
		{
			get { return est; }
			set { est = value; OnPropertyChanged(); }
		}

		private DataTable? summary;
		public DataTable? Summary
		{
			get { return summary; }
			set { summary = value; OnPropertyChanged(); }
		}

		private DataTable? otherData;
		public DataTable? OtherData
		{
			get { return otherData; }
			set { otherData = value; OnPropertyChanged(); }
		}

		private void LoadData()
		{
			this.Est = new DataTable("Estimate");
			this.Est.Columns.Add("Seq");
			this.Est.Columns.Add("Feature");
			this.Est.Columns.Add("Flat");
			this.Est.Columns.Add("Run");

			this.Est.Rows.Add("10", "BASE CHARGES",       "41.75", "10.41");
			this.Est.Rows.Add("11", "WHT 0260CB2 9.500",   "0.00", "10.41");
			this.Est.Rows.Add("12", "BLU 0140CFB2 9.500", "41.75", "24.73");
			this.Est.Rows.Add("13", "PNK 0150FB2 9.500",  "41.75", "14.08");
			this.Est.Rows.Add("14", "PACKAGING",          "41.75", " 1.11");
			this.Est.Rows.Add("15", "11-1-1 STD COLOR",   "41.75", " 0.60");

			//this.MyGrid.DataContext = this.Est;
			this.MyGrid.ItemsSource = Est.DefaultView;
		}

		private void LoadSummaryData()
		{
			this.Summary = new DataTable("Summary");
			this.Summary.Columns.Add("Paper");
			this.Summary.Columns.Add("LbsReq");
			this.Summary.Columns.Add("WastePct");
			this.Summary.Columns.Add("CostPerLb");
			this.Summary.Columns.Add("ExtCost");

			this.Summary.Rows.Add("   9 1/2  26.0# WHITE   PV - CB COATED BACK NCR", "159", "9.00", "1.505", "238.36");
			this.Summary.Rows.Add("   9 1/2  14.0# BLUE    PV - CFB COATED FRT/BACK", "83", "8.00", "1.673", "138.68");
			this.Summary.Rows.Add("   9 1/2  15.0# PINK    PV - CF COATED FRONT NCR", "92", "9.00", "0.9455", "86.36");

			//this.dgSummary.DataContext = this.Summary;
			this.dgSummary.ItemsSource = Summary.DefaultView;
		}

		private void LoadOtherData()
		{
			this.OtherData = new DataTable("OtherData");
			this.OtherData.Columns.Add("Department");
			this.OtherData.Columns.Add("SetupTime");
			this.OtherData.Columns.Add("SetupCost");
			this.OtherData.Columns.Add("RunTime");
			this.OtherData.Columns.Add("RunCost");

			this.OtherData.Rows.Add("PRESS",    "0.60", "39.00", "1.410", "91.85");
			this.OtherData.Rows.Add("COLLATOR", "0.35", "14.00", "0.863", "34.52");
			this.OtherData.Rows.Add("BINDERY",  "0.00", "0.00",  "1.410", "0.00");
			this.OtherData.Rows.Add("PREP",     "0.00", "0.00",  "", "IMPR: 15060    ");

			//this.dgDetails.DataContext = this.OtherData;
			this.dgDetails.ItemsSource = OtherData.DefaultView;
		}

		private void mnuExit_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
    }

		private void mnuDelete_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Really?", "Not actually working at this time", MessageBoxButton.YesNo, MessageBoxImage.Question);
		}

		private void mnuClear_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Really?", "Not actually working at this time", MessageBoxButton.YesNo, MessageBoxImage.Information);
		}

		private void mnuPrint_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Printing...", "Not actually working at this time", MessageBoxButton.OK, MessageBoxImage.Information);
		}

		private void mnuCopy_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Copy to where?", "Not actually working at this time", MessageBoxButton.OK, MessageBoxImage.Question);
		}
	}
}