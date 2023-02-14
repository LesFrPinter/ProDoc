using System.Data;
using System.Windows;
using System.ComponentModel;
using System;

namespace ProDocEstimate.Views
{
	public partial class PV02 : Window, INotifyPropertyChanged
    {

		public PV02()
        {
            InitializeComponent();
						LoadData();
				}

		public event PropertyChangedEventHandler? PropertyChanged;

		private DataTable est;
		public DataTable Est
		{
			get { return est; }
			set { est = value; /*OnPropertyChanged(new DependencyPropertyChangedEventArgs());*/ }
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

			this.MyGrid.DataContext = this.Est;
			this.MyGrid.ItemsSource = Est.DefaultView;
		}

	}
}
