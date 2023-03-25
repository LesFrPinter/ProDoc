using System.Data;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ProDocEstimate
{
	public partial class HistoricalPrices : Window, INotifyPropertyChanged
	{
		private DataTable? prices; public DataTable? Prices { get { return prices; } set { prices = value; OnPropertyChanged(); } }
		private float?     price;  public float?     Price  { get { return price;  } set { price  = value; OnPropertyChanged(); } }

		public HistoricalPrices()
		{
			InitializeComponent();
			DataContext = this;
			LoadPrices();
			Price = null;
		}

		public event PropertyChangedEventHandler? PropertyChanged;
		protected void OnPropertyChanged([CallerMemberName] string? name = null)
		{ PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

		private void LoadPrices()
		{
			this.Prices = new DataTable("Prices");
			this.Prices.Columns.Add("Date");
			this.Prices.Columns.Add("Description");
			this.Prices.Columns.Add("Quantity");
			this.Prices.Columns.Add("Price");

			this.Prices.Rows.Add("2/14/2022", "Wht 8 1/2 X 11 Bond", "5,000", "2.98");
			this.Prices.Rows.Add("3/24/2022", "Wht 8 1/2 X 11 Bond", "12,000", "3.98");
			this.Prices.Rows.Add("5/04/2022", "Wht 8 1/2 X 11 Bond", "7,500", "2.49");

			this.dgPrices.Items.Clear();
			this.dgPrices.ItemsSource = Prices.DefaultView;
		}

		private void btnClose_Click(object sender, RoutedEventArgs e)
		{ this.Hide(); }

		private void dgPrices_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{ int RowNumber = dgPrices.SelectedIndex;
			if (RowNumber > Prices?.Rows.Count - 1) return;
			int ColumnNumber = dgPrices.CurrentCell.Column.DisplayIndex;
			DataRow? dr = Prices?.Rows[RowNumber];
			Price = float.Parse(dr.ItemArray[3].ToString());
		}

		private void btnClear_Click(object sender, RoutedEventArgs e)
		{ Price = null; }

  }
}