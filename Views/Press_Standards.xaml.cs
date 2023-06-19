using System.Data;
using System.Windows;
using System.Configuration;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ProDocEstimate.Views
{
	public partial class Press_Standards : Window, INotifyPropertyChanged
	{
		private string id;	public string  ID { get { return id; } set { id = value; OnPropertyChanged(); } }
		private DataSet ds; public DataSet DS { get { return ds; } set { ds = value; OnPropertyChanged(); } }

		public string ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
		public SqlConnection cn = new SqlConnection();
		public SqlDataAdapter? da;

		private bool? editing;    public bool? Editing    { get { return editing;    } set { editing    = value; NotEditing = !value; OnPropertyChanged(); } }
		private bool? notediting; public bool? NotEditing { get { return notediting; } set { notediting = value; OnPropertyChanged(); } }

		#region Property Declarations

		private string? pressStandardNum; public string? PRESS_STANDARD			{ get { return pressStandardNum;	} set { pressStandardNum	= value; OnPropertyChanged(); } }
		private string? pressDescription; public string? PRESS_DESCRIPTION	{ get { return pressDescription;	} set { pressDescription	= value; OnPropertyChanged(); } }
		private string? pressNum;					public string? PRESS_NUM					{ get { return pressNum;					} set { pressNum					= value; OnPropertyChanged(); } }

		private string? web;   public string? WEB   { get { return web;   } set { web   = value; OnPropertyChanged(); } }
		private string? web2;  public string? WEB2  { get { return web2;  } set { web2  = value; OnPropertyChanged(); } }
		private string? width; public string? WIDTH { get { return width; } set { width = value; OnPropertyChanged(); } }

		private string? towers; public string? TOWERS { get { return towers; } set { towers = value; OnPropertyChanged(); } }
		private string? variable; public string? VARIABLE { get { return variable; } set { variable = value; OnPropertyChanged(); } }
		private string? restrictTo; public string? RESTRICTD { get { return restrictTo; } set { restrictTo = value; OnPropertyChanged(); } }
		private string? slowdown; public string? Slowdown { get { return slowdown; } set { slowdown = value; OnPropertyChanged(); } }

		private float? press_cost; public float? PRESS_COST { get { return press_cost; } set { press_cost = value; OnPropertyChanged(); } }
		private float? press_sell; public float? PRESS_SELL { get { return press_sell; } set { press_sell = value; OnPropertyChanged(); } }
		private string? baseSpeed; public string? BASESPEED { get { return baseSpeed; } set { baseSpeed = value; OnPropertyChanged(); } }

		private string? speed1; public string? SPEED1 { get { return speed1; } set { speed1 = value; OnPropertyChanged(); } }
		private string? speed2; public string? SPEED2 { get { return speed2; } set { speed2 = value; OnPropertyChanged(); } }
		private string? speed3; public string? SPEED3 { get { return speed3; } set { speed3 = value; OnPropertyChanged(); } }
		private string? speed4; public string? SPEED4 { get { return speed4; } set { speed4 = value; OnPropertyChanged(); } }
		private string? speed5; public string? SPEED5 { get { return speed5; } set { speed5 = value; OnPropertyChanged(); } }
		private string? speed6; public string? SPEED6 { get { return speed6; } set { speed6 = value; OnPropertyChanged(); } }
		private string? speed7; public string? SPEED7 { get { return speed7; } set { speed7 = value; OnPropertyChanged(); } }
		private string? speed8; public string? SPEED8 { get { return speed8; } set { speed8 = value; OnPropertyChanged(); } }

		private string? footage1; public string? FOOTAGE1 { get { return footage1;  } set { footage1 = value; OnPropertyChanged(); } }
		private string? footage2; public string? FOOTAGE2 { get { return footage2;  } set { footage2 = value; OnPropertyChanged(); } }
		private string? footage3; public string? FOOTAGE3 { get { return footage3;  } set { footage3 = value; OnPropertyChanged(); } }
		private string? footage4; public string? FOOTAGE4 { get { return footage4;  } set { footage4 = value; OnPropertyChanged(); } }
		private string? footage5; public string? FOOTAGE5 { get { return footage5;  } set { footage5 = value; OnPropertyChanged(); } }
		private string? footage6; public string? FOOTAGE6 { get { return footage6;  } set { footage6 = value; OnPropertyChanged(); } }
		private string? footage7; public string? FOOTAGE7 { get { return footage7;  } set { footage7 = value; OnPropertyChanged(); } }
		private string? footage8; public string? FOOTAGE8 { get { return footage8;  } set { footage8 = value; OnPropertyChanged(); } }

		private string? setupECL; public string? SETUP_ECL { get { return setupECL; } set { setupECL = value; OnPropertyChanged(); } }
		private string? runECL;   public string? RUN_ECL   { get { return runECL;   } set { runECL   = value; OnPropertyChanged(); } }
		private string? matlECL;  public string? MATL_ECL  { get { return matlECL;  } set { matlECL  = value; OnPropertyChanged(); } }
		#endregion

		public event PropertyChangedEventHandler? PropertyChanged;
		protected void OnPropertyChanged([CallerMemberName] string? name = null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

		public Press_Standards()
		{
			InitializeComponent();
			DataContext = this;
			Editing = false;
		}

		private void mnuExit_Click  (object sender, RoutedEventArgs e) { Close();					}
		private void mnuEdit_Click  (object sender, RoutedEventArgs e) { Editing = true;  }
		private void mnuNew_Click   (object sender, RoutedEventArgs e) { Editing = true;  }
		private void mnuSave_Click  (object sender, RoutedEventArgs e) { Editing = false; }
		private void mnuCancel_Click(object sender, RoutedEventArgs e) { Editing = false; }

		private void txtSearch_LostFocus(object sender, RoutedEventArgs e)
		{ cn = new SqlConnection(ConnectionString);
			cn.Open();
			string str = txtSearch.Text.Trim();
			string cmd = "SELECT ID, STANDARD, PRESS_NUMBER, DESCRIPTION FROM PRESS_STANDARDS" +
									 " WHERE DESCRIPTION LIKE '%" + str + "%' ORDER BY 1, 2";
			da = new SqlDataAdapter(cmd, cn);
			ds = new DataSet();
			da.Fill(ds);
			dgPress.ItemsSource = ds.Tables[0].DefaultView; 
			dgPress.SelectedIndex = 0; 
			lblResult.Content = ds.Tables[0].Rows.Count.ToString() + " rows matched.";
		}

		private void dgPress_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
			{ int selindex = dgPress.SelectedIndex;
				if (selindex >= 0 && selindex < ds.Tables[0].Rows.Count)
				{ ID = DS.Tables[0].Rows[selindex][0].ToString();
					string cmd = "SELECT * FROM PRESS_STANDARDS WHERE ID LIKE '" + ID + "%' ORDER BY 1, 2";
					da = new SqlDataAdapter(cmd, cn);
					DataSet ds = new DataSet();
					da.Fill(ds);
					DataContext = ds.Tables[0].DefaultView;
			}
		}

	}
}