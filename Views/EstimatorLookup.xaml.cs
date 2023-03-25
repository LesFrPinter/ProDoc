using System.Windows;
using System.ComponentModel;
using System.Data;
using Telerik.Windows.Controls.DataServices;
using System.Windows.Controls;

namespace ProDocEstimate.Views
{
	public partial class EstimatorLookup : Window
	{
		private string? estimator; public string? Estimator { get { return estimator; } set { estimator = value; } }
		private string? estimatorCode; public string? EstimatorCode { get { return estimatorCode; } set { estimatorCode = value; } }

		private DataTable? dt { get; set; }

		public EstimatorLookup()
		{
			InitializeComponent();
			Estimator = "Selected Name";
			EstimatorCode = "12";

			LoadData();
		}

		private DataTable? reps; public DataTable? Reps { get { return reps; } set { reps = value; } }

		private void btnSelect_Click(object sender, RoutedEventArgs e)
		{
			int RowNum = dgReps.SelectedIndex;
			Estimator = Reps?.Rows[RowNum][1] + " " + Reps?.Rows[RowNum][0];
			EstimatorCode = Reps?.Rows[RowNum][7].ToString();
			this.Hide();
		}

		private void btnCancel_Click(object sender, RoutedEventArgs e)
		{
			Estimator = ""; EstimatorCode = ""; this.Hide();
		}

		private void btnOptions_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("What happens here?", "Options button", MessageBoxButton.OK, MessageBoxImage.Question);
		}

		private void LoadData()
		{
			this.Reps = new DataTable("Reps");

			this.Reps.Columns.Add("LASTNAME");
			this.Reps.Columns.Add("FIRSTNAME");
			this.Reps.Columns.Add("ADDRESS");
			this.Reps.Columns.Add("CITY");
			this.Reps.Columns.Add("STATE");
			this.Reps.Columns.Add("ZIP");
			this.Reps.Columns.Add("INITIALS");
			this.Reps.Columns.Add("CODE");

			this.Reps.Rows.Add("Gonzalez", "Blanca", "", "", "", "", "", "03");
			this.Reps.Rows.Add("Highnote", "Cris",   "", "", "", "", "", "01");
			this.Reps.Rows.Add("Leiner",   "Joyce",  "", "", "", "", "", "02");
			this.Reps.Rows.Add("Phillips", "Nancy",  "", "", "", "", "", "05");
			this.Reps.Rows.Add("Phillips", "Laura",  "", "", "", "", "", "04");

			this.dgReps.DataContext = this;

		}

		private void dgReps_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			this.btnSelect.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
		}
	}
}
