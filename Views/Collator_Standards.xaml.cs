using System;
using System.Windows;
using Microsoft.OData.Edm;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace ProDocEstimate.Views
{

	public partial class Collator_Standards : Window, INotifyPropertyChanged
	{

		public event PropertyChangedEventHandler? PropertyChanged;
		protected void OnPropertyChanged([CallerMemberName] string? name = null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

		private bool? editing; public bool? Editing { get { return editing; } set { editing = value; NotEditing = !editing; OnPropertyChanged(); } }
		private bool? notediting; public bool? NotEditing { get { return notediting; } set { notediting = value; OnPropertyChanged(); } }

		private string id; public string ID { get { return id; } set { id = value; OnPropertyChanged(); } }
		private DataSet ds; public DataSet DS { get { return ds; } set { ds = value; OnPropertyChanged(); } }

		public string ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
		public SqlConnection cn = new SqlConnection();
		public SqlDataAdapter? da;

		private string? coll_std; public string? COLLATOR_STANDARD { get { return coll_std; } set { coll_std = value; OnPropertyChanged(); } }
		private string? coll_num; public string? COLLATOR_NUM { get { return coll_num; } set { coll_num = value; OnPropertyChanged(); } }
		private Date? created; public Date? CREATED { get { return created; } set { created = value; OnPropertyChanged(); } }
		private Date? changed; public Date? CHANGED { get { return changed; } set { changed = value; OnPropertyChanged(); } }
		private string? collator_description; public string? COLLATOR_DESCRIPTION { get { return collator_description; } set { collator_description = value; OnPropertyChanged(); } }

		private int? cont_speed1; public int? CONT_SPEED1 { get { return cont_speed1; } set { cont_speed1 = value; OnPropertyChanged(); } }
		private int? cont_speed2; public int? CONT_SPEED2 { get { return cont_speed2; } set { cont_speed2 = value; OnPropertyChanged(); } }
		private int? cont_speed3; public int? CONT_SPEED3 { get { return cont_speed3; } set { cont_speed3 = value; OnPropertyChanged(); } }
		private int? cont_speed4; public int? CONT_SPEED4 { get { return cont_speed4; } set { cont_speed4 = value; OnPropertyChanged(); } }
		private int? cont_speed5; public int? CONT_SPEED5 { get { return cont_speed5; } set { cont_speed5 = value; OnPropertyChanged(); } }
		private int? cont_speed6; public int? CONT_SPEED6 { get { return cont_speed6; } set { cont_speed6 = value; OnPropertyChanged(); } }
		private int? cont_speed7; public int? CONT_SPEED7 { get { return cont_speed7; } set { cont_speed7 = value; OnPropertyChanged(); } }
		private int? cont_speed8; public int? CONT_SPEED8 { get { return cont_speed8; } set { cont_speed8 = value; OnPropertyChanged(); } }
		private int? cont_speed9; public int? CONT_SPEED9 { get { return cont_speed9; } set { cont_speed9 = value; OnPropertyChanged(); } }
		private int? cont_speed10; public int? CONT_SPEED10 { get { return cont_speed10; } set { cont_speed10 = value; OnPropertyChanged(); } }

		private int? snap_speed1; public int? SNAP_SPEED1 { get { return snap_speed1; } set { snap_speed1 = value; OnPropertyChanged(); } }
		private int? snap_speed2; public int? SNAP_SPEED2 { get { return snap_speed2; } set { snap_speed2 = value; OnPropertyChanged(); } }
		private int? snap_speed3; public int? SNAP_SPEED3 { get { return snap_speed3; } set { snap_speed3 = value; OnPropertyChanged(); } }
		private int? snap_speed4; public int? SNAP_SPEED4 { get { return snap_speed4; } set { snap_speed4 = value; OnPropertyChanged(); } }
		private int? snap_speed5; public int? SNAP_SPEED5 { get { return snap_speed5; } set { snap_speed5 = value; OnPropertyChanged(); } }
		private int? snap_speed6; public int? SNAP_SPEED6 { get { return snap_speed6; } set { snap_speed6 = value; OnPropertyChanged(); } }
		private int? snap_speed7; public int? SNAP_SPEED7 { get { return snap_speed7; } set { snap_speed7 = value; OnPropertyChanged(); } }
		private int? snap_speed8; public int? SNAP_SPEED8 { get { return snap_speed8; } set { snap_speed8 = value; OnPropertyChanged(); } }
		private int? snap_speed9; public int? SNAP_SPEED9 { get { return snap_speed9; } set { snap_speed9 = value; OnPropertyChanged(); } }
		private int? snap_speed10; public int? SNAP_SPEED10 { get { return snap_speed10; } set { snap_speed10 = value; OnPropertyChanged(); } }

		private int? cont_speed21; public int? CONT_SPEED21 { get { return cont_speed21; } set { cont_speed21 = value; OnPropertyChanged(); } }
		private int? cont_speed22; public int? CONT_SPEED22 { get { return cont_speed22; } set { cont_speed22 = value; OnPropertyChanged(); } }
		private int? cont_speed23; public int? CONT_SPEED23 { get { return cont_speed23; } set { cont_speed23 = value; OnPropertyChanged(); } }
		private int? cont_speed24; public int? CONT_SPEED24 { get { return cont_speed24; } set { cont_speed24 = value; OnPropertyChanged(); } }
		private int? cont_speed25; public int? CONT_SPEED25 { get { return cont_speed25; } set { cont_speed25 = value; OnPropertyChanged(); } }
		private int? cont_speed26; public int? CONT_SPEED26 { get { return cont_speed26; } set { cont_speed26 = value; OnPropertyChanged(); } }
		private int? cont_speed27; public int? CONT_SPEED27 { get { return cont_speed27; } set { cont_speed27 = value; OnPropertyChanged(); } }
		private int? cont_speed28; public int? CONT_SPEED28 { get { return cont_speed28; } set { cont_speed28 = value; OnPropertyChanged(); } }
		private int? cont_speed29; public int? CONT_SPEED29 { get { return cont_speed29; } set { cont_speed29 = value; OnPropertyChanged(); } }
		private int? cont_speed210; public int? CONT_SPEED210 { get { return cont_speed210; } set { cont_speed210 = value; OnPropertyChanged(); } }

		private int? snap_speed21; public int? SNAP_SPEED21 { get { return snap_speed21; } set { snap_speed21 = value; OnPropertyChanged(); } }
		private int? snap_speed22; public int? SNAP_SPEED22 { get { return snap_speed22; } set { snap_speed22 = value; OnPropertyChanged(); } }
		private int? snap_speed23; public int? SNAP_SPEED23 { get { return snap_speed23; } set { snap_speed23 = value; OnPropertyChanged(); } }
		private int? snap_speed24; public int? SNAP_SPEED24 { get { return snap_speed24; } set { snap_speed24 = value; OnPropertyChanged(); } }
		private int? snap_speed25; public int? SNAP_SPEED25 { get { return snap_speed25; } set { snap_speed25 = value; OnPropertyChanged(); } }
		private int? snap_speed26; public int? SNAP_SPEED26 { get { return snap_speed26; } set { snap_speed26 = value; OnPropertyChanged(); } }
		private int? snap_speed27; public int? SNAP_SPEED27 { get { return snap_speed27; } set { snap_speed27 = value; OnPropertyChanged(); } }
		private int? snap_speed28; public int? SNAP_SPEED28 { get { return snap_speed28; } set { snap_speed28 = value; OnPropertyChanged(); } }
		private int? snap_speed29; public int? SNAP_SPEED29 { get { return snap_speed29; } set { snap_speed29 = value; OnPropertyChanged(); } }
		private int? snap_speed210; public int? SNAP_SPEED210 { get { return snap_speed210; } set { snap_speed210 = value; OnPropertyChanged(); } }


		private float? coll_setup_ecl; public float? COLL_SETUP_ECL { get { return coll_setup_ecl; } set { coll_setup_ecl = value; } }
		private float? coll_run_ecl; public float? COLL_RUN_ECL { get { return coll_run_ecl; } set { coll_run_ecl = value; } }
		private float? coll_matl_ecl; public float? COLL_MATL_ECL { get { return coll_matl_ecl; } set { coll_matl_ecl = value; } }

		private float? coll_cost; public float? COLL_COST { get { return coll_cost; } set { coll_cost = value; } }
		private float? coll_sell; public float? COLL_SELL { get { return coll_sell; } set { coll_sell = value; } }

		public Collator_Standards()
		{
			InitializeComponent();
			DataContext = this;

			//COLLATOR_STANDARD = "1";
			//COLLATOR_NUM = "311";
			//COLLATOR_DESCRIPTION = "11 inch collator";

			//Date CREATED = new DateTime(2022, 02, 01);
			//Date CHANGED = new DateTime(2022, 05, 14);

			//CONT_SPEED1 = 177;
			//CONT_SPEED2 = 177;
			//CONT_SPEED3 = 177;
			//CONT_SPEED4 = 177;
			//CONT_SPEED5 = 177;
			//CONT_SPEED6 = 159;
			//CONT_SPEED7 = 159;

			//SNAP_SPEED1 = 135;
			//SNAP_SPEED2 = 135;
			//SNAP_SPEED3 = 135;
			//SNAP_SPEED4 = 135;
			//SNAP_SPEED5 = 135;
			//SNAP_SPEED6 = 125;
			//SNAP_SPEED7 = 117;
			//SNAP_SPEED8 = 117;
			//SNAP_SPEED9 = 117;
			//SNAP_SPEED10 = 117;

			// CONT_SPEED21-210 have no values in the database

			//SNAP_SPEED21 = 117;
			//SNAP_SPEED22 = 117;

			//COLL_SETUP_ECL = 303.10F;
			//COLL_RUN_ECL = 303.20F;
			//COLL_MATL_ECL = 303.20F;
			//COLL_COST = 40.00F;
			//COLL_SELL = 40.00F;

			Editing = false;
		}

		private void mnuExit_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void mnuEdit_Click(object sender, RoutedEventArgs e)
		{
			Editing = true;
		}

		private void mnuNew_Click(object sender, RoutedEventArgs e)
		{
			Editing = true;
		}

		private void mnuSave_Click(object sender, RoutedEventArgs e)
		{
			Editing = false;
		}

		private void mnuCancel_Click(object sender, RoutedEventArgs e)
		{
			Editing = false;
		}

		private void txtPartial_LostFocus(object sender, RoutedEventArgs e)
		{
			cn = new SqlConnection(ConnectionString);
			cn.Open();
			string str = txtSearch.Text.Trim();
			string cmd = "SELECT ID, COLLATOR_STANDARD, COLLATOR_NUM, COLLATOR_DESCRIPTION FROM COLLATOR_STANDARDS" +
									 " WHERE COLLATOR_DESCRIPTION LIKE '%" + str + "%' ORDER BY 1, 2";
			da = new SqlDataAdapter(cmd, cn);
			ds = new DataSet();
			da.Fill(ds);
			dgCollators.ItemsSource = ds.Tables[0].DefaultView; dgCollators.SelectedIndex = 0;
			lblResult.Content = ds.Tables[0].Rows.Count.ToString() + " rows matched.";
		}

		private void dgCollators_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			int selindex = dgCollators.SelectedIndex;
			if (selindex >= 0 && selindex < ds.Tables[0].Rows.Count)
			{ ID = DS.Tables[0].Rows[selindex][0].ToString();
				string cmd = "SELECT * FROM COLLATOR_STANDARDS WHERE ID LIKE '" + ID + "%' ORDER BY 1, 2";
				da = new SqlDataAdapter(cmd, cn);
				DataSet ds = new DataSet();
				da.Fill(ds);
				DataContext = ds.Tables[0].DefaultView;
			}
		}
	}
}
