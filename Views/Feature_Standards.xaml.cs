using System;
using System.Data;
using System.Windows;
using System.Configuration;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Windows.Controls;
using System.Runtime.CompilerServices;

namespace ProDocEstimate.Views
{
	public partial class Feature_Standards : Window, INotifyPropertyChanged {

		public event PropertyChangedEventHandler? PropertyChanged;
		protected void OnPropertyChanged([CallerMemberName] string? name = null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

		private string id;  public string  ID { get { return id; } set { id = value; OnPropertyChanged(); } }
		private DataSet ds; public DataSet DS { get { return ds; } set { ds = value; OnPropertyChanged(); } }

		public string ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
		public SqlConnection cn = new SqlConnection();
		public SqlDataAdapter? da;

		private bool? editing; public bool? Editing { get { return editing; } set { editing = value; NotEditing = !editing; OnPropertyChanged(); } }
		private bool? notediting; public bool? NotEditing { get { return notediting; } set { notediting = value; OnPropertyChanged(); } }

		#region Property Declarations

		private string? feature;             public string? FEATURE { get { return feature; } set { feature = value; OnPropertyChanged(); } }
		private string? feature_Num;         public string? FEATURE_NUM    { get { return feature_Num; } set { feature_Num = value; OnPropertyChanged(); } }
		private string? feature_Description; public string? FEATURE_DESCRIPTION { get { return feature_Description; } set { feature_Description = value; OnPropertyChanged(); } }
		private string? sort;                public string? Sort          { get { return sort; } set { sort = value; OnPropertyChanged(); } }

		private string? ask_price_in;       public string? ASK_PRICE_IN  { get { return ask_price_in; } set { ask_price_in = value; OnPropertyChanged(); } }
		private string? ask_price_sq_in;    public string? ASK_PRICE_SQ_IN { get { return ask_price_sq_in; } set { ask_price_sq_in = value; OnPropertyChanged(); } }
		private string? sellDollars;        public string? SellDollars   { get { return sellDollars; } set { sellDollars = value; OnPropertyChanged(); } }
		private string? costDollars;        public string? CostDollars   { get { return costDollars; } set { costDollars = value; OnPropertyChanged(); } }
		private string? costMU1;            public string? CostMU1       { get { return costMU1; } set { costMU1 = value; OnPropertyChanged(); } }
		private string? costMU2;            public string? CostMU2       { get { return costMU2; } set { costMU2 = value; OnPropertyChanged(); } }

		private float? esqsel;							public float? ESQSEL { get { return esqsel; } set { esqsel = value; OnPropertyChanged(); } }
		private float? esqcst;							public float? ESQCST { get { return esqcst; } set { esqcst = value; OnPropertyChanged(); } }

		private string? prep_dept_time;     public string? PREP_DEPT_TIME { get { return prep_dept_time; } set { prep_dept_time = value; OnPropertyChanged(); } }
		private string? prep_dept_matl;     public string? PREP_DEPT_MATL { get { return prep_dept_matl; } set { prep_dept_matl = value; OnPropertyChanged(); } }
		private string? prepSlowdown;       public string? PrepSlowdown   { get { return prepSlowdown; } set { prepSlowdown = value; OnPropertyChanged(); } }

		private string? press_setup_time;   public string? PRESS_SETUP_TIME     { get { return press_setup_time; } set { press_setup_time = value; OnPropertyChanged(); } }
		private string? press_setup_material; public string? PRESS_SETUP_MATERIAL { get { return press_setup_material; } set { press_setup_material = value; OnPropertyChanged(); } }
		private string? press_Slowdown;      public string? PRESS_SLOWDOWN { get { return press_Slowdown; } set { press_Slowdown = value; OnPropertyChanged(); } }

		private string? colorTime;          public string? ColorTime     { get { return colorTime; } set { colorTime = value; OnPropertyChanged(); } }
		private string? colorMaterial;      public string? ColorMaterial { get { return colorMaterial; } set { colorMaterial = value; OnPropertyChanged(); } }
		private string? colorSlowdown;      public string? ColorSlowdown { get { return colorSlowdown; } set { colorSlowdown = value; OnPropertyChanged(); } }
		private string? bindTime;           public string? BindTime      { get { return bindTime; } set { bindTime = value; OnPropertyChanged(); } }

		private string? otherTime;          public string? OtherTime     { get { return otherTime; } set { otherTime = value; OnPropertyChanged(); } }
		private string? otherMaterial;      public string? OtherMaterial { get { return otherMaterial; } set { otherMaterial = value; OnPropertyChanged(); } }
		private string? oneTimeMaterial;    public string? OneTimeMaterial { get { return oneTimeMaterial; } set { oneTimeMaterial = value; OnPropertyChanged(); } }
		private string? whatsThis;          public string? WhatsThis     { get { return whatsThis; } set { whatsThis = value; OnPropertyChanged(); } }

		private string? optNumAround;       public string? OptNumAround  { get { return optNumAround; } set { optNumAround = value; OnPropertyChanged(); } }
		private string? optPart;            public string? OptPart       { get { return optPart; } set { optPart = value; OnPropertyChanged(); } }
		private string? optStream;          public string? OptStream     { get { return optStream; } set { optStream = value; OnPropertyChanged(); } }
		private float? flat_charge;         public float? FLAT_CHARGE    { get { return flat_charge; } set { flat_charge = value; OnPropertyChanged(); } }
		private string? flat_prompt;        public string? FLAT_PROMPT   { get { return flat_prompt; } set { flat_prompt = value; OnPropertyChanged(); } }
		private float? run_charge;          public float? RUN_CHARGE     { get { return run_charge; } set { run_charge = value; OnPropertyChanged(); } }
		private string? run_prompt;         public string? RUN_PROMPT    { get { return run_prompt; } set { run_prompt = value; OnPropertyChanged(); } }
		private string? optPercent;         public string? OptPercent    { get { return optPercent; } set { optPercent = value; OnPropertyChanged(); } }
		private string? optType;            public string? OptType       { get { return optType; } set { optType = value; OnPropertyChanged(); } }
		private string? sets;               public string? Sets          { get { return sets; } set { sets = value; OnPropertyChanged(); } }
		private bool? multStreamOK;         public bool?   MultStreamOK  { get { return multStreamOK; } set { multStreamOK = value; OnPropertyChanged(); } }
		private string? wasteSetup;         public string? WasteSetup    { get { return wasteSetup; } set { wasteSetup = value; OnPropertyChanged(); } }
		private string? wasteRunPct;        public string? WasteRunPct   { get { return wasteRunPct; } set { wasteRunPct = value; OnPropertyChanged(); } }
		private string? mr_impr;						public string? MR_IMPR { get { return mr_impr; } set { mr_impr = value; OnPropertyChanged(); } }
		private string? cartonQty;          public string? CartonQty     { get { return cartonQty; } set { cartonQty = value; OnPropertyChanged(); } }
		private string? calcType;           public string? CalcType      { get { return calcType; } set { calcType = value; OnPropertyChanged(); } }
		private string? alert;              public string? Alert         { get { return alert; } set { alert = value; OnPropertyChanged(); } }
		private float? setupECL;            public float? SETUP_ECL      { get { return setupECL; } set { setupECL = value; OnPropertyChanged(); } }
		private float? runECL;              public float? RUN_ECL        { get { return runECL; } set { runECL = value; OnPropertyChanged(); } }
		private float? matlECL;             public float? MATL_ECL       { get { return matlECL; } set { matlECL = value; OnPropertyChanged(); } }
		private bool slowdown_per_part;     public bool SLOWDOWN_PER_PART { get { return slowdown_per_part; } set { slowdown_per_part = value; OnPropertyChanged(); } }
		private bool multi_strm_ok;         public bool MULTI_STRM_OK    { get { return multi_strm_ok; }      set { multi_strm_ok     = value; OnPropertyChanged(); } }

		//SLOWDOWN_PER_PART = true;
		//MULTI_STRM_OK = true;

		//ESQCST = 0.680000007152557F;
		//ESQSEL = 0.680000007152557F;

		#endregion

		public Feature_Standards() {
			InitializeComponent();
			DataContext = this;
			Editing = false;
		}

		private void mnuExit_Click  (object sender, RoutedEventArgs e) { Close(); }
		private void mnuEdit_Click  (object sender, RoutedEventArgs e) { Editing = true;  }
		private void mnuNew_Click   (object sender, RoutedEventArgs e) { Editing = true;  }
		private void mnuSave_Click  (object sender, RoutedEventArgs e) { Editing = false; }
		private void mnuCancel_Click(object sender, RoutedEventArgs e) { Editing = false; }

		private void SearchTerm_LostFocus(object sender, RoutedEventArgs e)
		{ cn = new SqlConnection(ConnectionString); cn.Open();
			string str = txtSearch.Text.Trim();
			string cmd = "SELECT ID, FEATURE, FEATURE_NUM, FEATURE_DESCRIPTION, FLAT_PROMPT FROM FEATURE_STANDARDS" +
									 " WHERE FEATURE_DESCRIPTION LIKE '%" + str + "%' ORDER BY 1, 2";
			da = new SqlDataAdapter(cmd, cn);
			ds = new DataSet(); da.Fill(ds);
			dgFeatures.ItemsSource = ds.Tables[0].DefaultView; dgFeatures.SelectedIndex = 0;
			lblResult.Content = ds.Tables[0].Rows.Count.ToString() + " rows matched.";
		}

		private void dgFeatures_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e) {
			int selindex = dgFeatures.SelectedIndex;
			if(selindex >= 0 && selindex < ds.Tables[0].Rows.Count) { 
			ID = DS.Tables[0].Rows[selindex][0].ToString();
			string cmd = "SELECT * FROM FEATURE_STANDARDS WHERE ID LIKE '" + ID + "%' ORDER BY 1, 2";
			da = new SqlDataAdapter(cmd, cn);
			DataSet ds = new DataSet(); da.Fill(ds);
			DataContext = ds.Tables[0].DefaultView;
			}
		}

		private void dgFeatures_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{ dgFeatures_MouseDoubleClick(new Object(), null); }

	}
}