using System.Data;
using System.Windows;
using System.ComponentModel;
using System.Windows.Controls;
using System.Diagnostics;

namespace ProDocEstimate.Views
{
	public partial class SalesRepLookup : Window, INotifyPropertyChanged
	{
		public SalesRepLookup()
		{
			InitializeComponent();
			this.DataContext = this;
			LoadData();
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		private DataTable? reps;
		public DataTable? Reps
		{
			get { return reps; }
			set { reps = value; /*OnPropertyChanged();*/ }
		}

		private string salesRepCode; public string SalesRepCode { get { return salesRepCode; } set { salesRepCode = value; } }
		private string salesRep;     public string SalesRep     { get { return salesRep;     } set { salesRep = value;     } }

		private void btnSelect_Click(object sender, RoutedEventArgs e)
		{
#pragma warning disable CS8601 // Possible null reference assignment.
			int RowNum = dgReps.SelectedIndex;
			SalesRep     = Reps?.Rows[RowNum][1] + " " + Reps?.Rows[RowNum][0];
			SalesRepCode = Reps?.Rows[RowNum][2].ToString();

			//			SalesRepCode = dgReps.SelectedValue.ToString();
#pragma warning restore CS8601 // Possible null reference assignment.
			this.Hide(); 
		}

		private void btnCancel_Click(object sender, RoutedEventArgs e)
		{ this.Hide(); }

		private void btnOptions_Click(object sender, RoutedEventArgs e)
		{ }

		private void LoadData()
		{
			this.Reps = new DataTable("Reps");
			this.Reps.Columns.Add("LastName");
			this.Reps.Columns.Add("FirstName");
			this.Reps.Columns.Add("Code");

			this.Reps.Rows.Add("Account",  "House",  "9999");
			this.Reps.Rows.Add("Gonzalez", "Blanca", "004");
			this.Reps.Rows.Add("Highnote", "Cris",   "001");
			this.Reps.Rows.Add("Leiner",   "Joyce",  "006");
			this.Reps.Rows.Add("Phillips", "Nancy",  "002");
			this.Reps.Rows.Add("Phillips", "Laura",  "005");

			this.dgReps.DataContext = this;

		}

		private void dgReps_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			this.btnSelect.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
		}
	}
}

public class OneRep
{
	public string? LastName { get; set; }
	public string? FirstName { get; set; }
	public string? Code { get; set; }
}
