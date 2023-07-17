using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace ProDocEstimate.Views
{
    public partial class Security : Window, INotifyPropertyChanged
    {

        #region Property definitions

        // 07/14/2023 LFP Changed Quote_Details to provide storage space for up to 21 F_TYPES:
        //ALTER TABLE[ESTIMATING].[dbo].[Quote_Details] ALTER COLUMN Value9 VarChar(100)   // '1001' means 'F_TYPES 1 and 4 are checked'
        //ALTER TABLE[ESTIMATING].[dbo].[Quote_Details] ALTER COLUMN Value10 VarChar(100)  // '30,,,20' assigns 30% to the first ftype and 20% to the fourth;
        //                                                                                     use 'string pct[] = split(",") to retrieve values into an array.

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

        public string ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public SqlConnection? conn;
        public SqlDataAdapter? da;
        public DataTable? dt;
        public DataView? dv;
        public SqlCommand? scmd;

        private string fieldList; public string FieldList { get { return fieldList; } set { fieldList = value; OnPropertyChanged(); } }

        private int num_F_Types = 21; public int Num_F_Types { get { return num_F_Types = 21; } set { num_F_Types = value; OnPropertyChanged(); } }

        private string   quoteNum;  public string   QuoteNum  { get { return quoteNum;  } set { quoteNum  = value; OnPropertyChanged(); } }
        private string   pressSize; public string   PressSize { get { return pressSize; } set { pressSize = value; OnPropertyChanged(); } }

        private bool[]   chk;    public bool[]   Chk    { get { return chk;     } set { chk     = value; OnPropertyChanged(); } }
        private string[] ftype;  public string[] Ftype  { get { return ftype;   } set { ftype   = value; OnPropertyChanged(); } }
        private string[] flat;   public string[] Flat   { get { return flat;    } set { flat    = value; OnPropertyChanged(); } }
        private string[] run;    public string[] Run    { get { return run;     } set { run     = value; OnPropertyChanged(); } }

        private bool startup = true; public bool Startup { get { return startup; } set { startup = value; OnPropertyChanged(); } }

        //private bool chk01; public bool Chk01 { get { return chk01; } set { chk01 = value; OnPropertyChanged(); } }
        //private bool chk02; public bool Chk02 { get { return chk02; } set { chk02 = value; OnPropertyChanged(); } }
        //private bool chk03; public bool Chk03 { get { return chk03; } set { chk03 = value; OnPropertyChanged(); } }
        //private bool chk04; public bool Chk04 { get { return chk04; } set { chk04 = value; OnPropertyChanged(); } }
        //private bool chk05; public bool Chk05 { get { return chk05; } set { chk05 = value; OnPropertyChanged(); } }
        //private bool chk06; public bool Chk06 { get { return chk06; } set { chk06 = value; OnPropertyChanged(); } }
        //private bool chk07; public bool Chk07 { get { return chk07; } set { chk07 = value; OnPropertyChanged(); } }
        //private bool chk08; public bool Chk08 { get { return chk08; } set { chk08 = value; OnPropertyChanged(); } }
        //private bool chk09; public bool Chk09 { get { return chk09; } set { chk09 = value; OnPropertyChanged(); } }
        //private bool chk10; public bool Chk10 { get { return chk10; } set { chk10 = value; OnPropertyChanged(); } }
        //private bool chk11; public bool Chk11 { get { return chk11; } set { chk11 = value; OnPropertyChanged(); } }
        //private bool chk12; public bool Chk12 { get { return chk12; } set { chk12 = value; OnPropertyChanged(); } }
        //private bool chk13; public bool Chk13 { get { return chk13; } set { chk13 = value; OnPropertyChanged(); } }
        //private bool chk14; public bool Chk14 { get { return chk14; } set { chk14 = value; OnPropertyChanged(); } }
        //private bool chk15; public bool Chk15 { get { return chk15; } set { chk15 = value; OnPropertyChanged(); } }
        //private bool chk16; public bool Chk16 { get { return chk16; } set { chk16 = value; OnPropertyChanged(); } }
        //private bool chk17; public bool Chk17 { get { return chk17; } set { chk17 = value; OnPropertyChanged(); } }
        //private bool chk18; public bool Chk18 { get { return chk18; } set { chk18 = value; OnPropertyChanged(); } }
        //private bool chk19; public bool Chk19 { get { return chk19; } set { chk19 = value; OnPropertyChanged(); } }
        //private bool chk20; public bool Chk20 { get { return chk20; } set { chk20 = value; OnPropertyChanged(); } }
        //private bool chk21; public bool Chk21 { get { return chk21; } set { chk21 = value; OnPropertyChanged(); } }

        private float flatCharges;           public float FlatCharges           { get { return flatCharges;             } set { flatCharges             = value; OnPropertyChanged(); } }
        private float runCharges;            public float RunCharges            { get { return runCharges;              } set { runCharges              = value; OnPropertyChanged(); } }
        private float flatChgPct;            public float FlatChgPct            { get { return flatChgPct;              } set { flatChgPct              = value; OnPropertyChanged(); } }
        private float runChgPct;             public float RunChgPct             { get { return runChgPct;               } set { runChgPct               = value; OnPropertyChanged(); } }

        private float calculatedflatCharges; public float CalculatedFlatCharges { get { return calculatedflatCharges;   } set { calculatedflatCharges   = value; OnPropertyChanged(); } }
        private float calculatedrunCharges;  public float CalculatedRunCharges  { get { return calculatedrunCharges;    } set { calculatedrunCharges    = value; OnPropertyChanged(); } }
        private float flatTotal;             public float FlatTotal             { get { return flatTotal;               } set { flatTotal               = value; OnPropertyChanged(); } }
        private float runTotal;              public float RunTotal              { get { return runTotal;                } set { runTotal                = value; OnPropertyChanged(); } }

        #endregion

        public Security(string PRESSSIZE, string QUOTENUM)
        {
            InitializeComponent();

            this.DataContext = this;

            Title = QUOTENUM;
            QuoteNum  = QUOTENUM.ToString();
            PressSize = PRESSSIZE.ToString();

            Chk     = new bool  [Num_F_Types];
            Ftype   = new string[Num_F_Types];
            Flat    = new string[Num_F_Types];
            Run     = new string[Num_F_Types];

            for ( int j=0; j<Num_F_Types; j++ )       // Initialize all of the arrays.
            {   Chk[j]    = false;
                Ftype[j]  = ""; 
                Flat[j]   = "";
                Run[j]    = "";
            }

            FieldList = "F_TYPE, FLAT_CHARGE, RUN_CHARGE, PLATE_MATL, FINISH_MATL, CONV_MATL, PRESS_MATL";

            LoadFtypes();
            LoadData();

            Startup = false;

            PreviewKeyDown += (s, e) => { if (e.Key == Key.Escape) Close(); };  // Esc to close window
        }

        public void OnLoad(object sender, RoutedEventArgs e) 
        { 
            Startup = false;
            this.Height = this.Height *= 1.8;
            this.Width = this.Width *= 1.8;
        }

        public void LoadFtypes()
        {
            string cmd = $"SELECT {FieldList} FROM [Estimating].[dbo].[Features] WHERE CATEGORY = 'SECURITY' AND PRESS_SIZE = '{PressSize}'";
            conn = new SqlConnection(ConnectionString);
            da = new SqlDataAdapter(cmd, conn); 
            dt = new DataTable("Security"); da.Fill(dt);
            DataView dv = dt.AsDataView();
            int start = 0;
            for (int i = start; i < start + 20; i++)
            {   Ftype[i] = dv[i]["F_TYPE"].ToString();
                Flat[i]  = dv[i]["FLAT_CHARGE"].ToString();
                Run[i]   = dv[i]["RUN_CHARGE"].ToString();
            }
        }

        public void LoadData()
        {
            string cmd = $"SELECT * FROM [ESTIMATING].[dbo].[Quote_Details] WHERE QUOTE_NUM = '{QuoteNum}' AND CATEGORY = 'Security'";
            conn = new SqlConnection(ConnectionString); da = new SqlDataAdapter(cmd, conn);
            DataTable dt2 = new DataTable(); da.Fill(dt2); dv = dt2.DefaultView;

            if(dt2.Rows.Count==0) 
            {   FlatChgPct  = 0.00F;
                RunChgPct   = 0.00F;
                FlatCharges = 0.0F;
                RunCharges  = 0.00F;
                CalculatedFlatCharges = 0.00F;
                CalculatedRunCharges  = 0.00F;
                return; 
            }

            //CalculatedFlatCharges = 0.0F;
            //CalculatedRunCharges = 0.0F;

            FlatChgPct = 0; if (dv[0]["FlatChargePct"].ToString().Length > 0) { FlatChgPct = float.Parse(dv[0]["FlatChargePct"].ToString()); }
            RunChgPct  = 0; if (dv[0]["RunChargePct"].ToString().Length > 0)  { RunChgPct  = float.Parse(dv[0]["RunChargePct"].ToString()); }

            FlatCharges = 0.0F;
            RunCharges = 0.00F;
            for (int i = 0; i < dv[0]["Value9"].ToString().Length; i++)
            {   string v = dv[0]["Value9"].ToString().Substring(i,1); 
                chk[i] = (v=="1") ? true: false;
                if (chk[i] == true) 
                { FlatCharges += float.Parse(Flat[i].ToString());  
                  RunCharges  += float.Parse(Run[i].ToString()); 
                }
            }

            CalculatedFlatCharges = FlatCharges * ( 1.00F + (FlatChgPct / 100.00F) );
            CalculatedRunCharges  = RunCharges  * ( 1.00F + (RunChgPct  / 100.00F) );
        }

        private void TextBox_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        { 
            // This is only for testing; reassigning Ext[i] to Ext{i} fixes the data display problem.
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        { 
            string Checked = ""; for(int i = 0; i < 21;  i++)  { Checked  += (Chk[i] ? "1" : "0"); }
            string cmd = $"DELETE [ESTIMATING].[dbo].[Quote_Details] WHERE QUOTE_NUM = '{QuoteNum}' AND CATEGORY = 'Security'";
            conn = new SqlConnection(ConnectionString); conn.Open();
            scmd = new SqlCommand(cmd, conn); scmd.ExecuteNonQuery();

            string sFlatCharges = CalculatedFlatCharges.ToString();
            string sRunCharges  = CalculatedRunCharges.ToString();

            scmd.CommandText = $"INSERT INTO [ESTIMATING].[dbo].[Quote_Details] ( QUOTE_NUM, SEQUENCE, CATEGORY, VALUE9, FlatChargePct, RunChargePct, TotalFlatChg, PerThousandChg ) VALUES ( '{QuoteNum}', 10, 'Security', '{Checked}', {FlatChgPct}, '{RunChgPct}', '{sFlatCharges}', '{sRunCharges}' )";
            if(conn.State != ConnectionState.Open) conn.Open();
            scmd.ExecuteNonQuery();
            conn.Close();
            this.Close(); 
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        { this.Close(); }

        private void ChkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!Startup)
            { SumCharges(); }
        }

        private void ChkBox_Checked(object sender, RoutedEventArgs e)
        {
            if (!Startup)
            { SumCharges(); }
        }

        private void SumCharges()
        {
            FlatCharges = 0.00F; 
            RunCharges  = 0.00F;

            for(int i=0; i< Num_F_Types-1;  i++)
              { if (Chk[i] == true) 
                {  FlatCharges += float.Parse(Flat[i].ToString()); 
                   RunCharges  += float.Parse(Run[i].ToString()); 
                }
              }

            float percent = 0.00F;
            if (FCPct.Value == null) { percent = 0.00F; } else { percent = float.Parse(FCPct.Value.ToString()) / 100.00F; }
            CalculatedFlatCharges = FlatCharges * (1.00F + percent);

            percent = 0.00F;
            if (RCPct.Value == null) { percent = 0.00F; } else { percent = float.Parse(RCPct.Value.ToString()) / 100.00F; }
            CalculatedRunCharges = RunCharges * (1.00F + percent);
        }

        private void FCPct_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        {
            float percent = float.Parse(FCPct.Value.ToString()) / 100.00F;
            CalculatedFlatCharges = FlatCharges * (1.00F + percent);
        }

        private void RCPct_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        {
            float percent = float.Parse(RCPct.Value.ToString()) / 100.00F;
            CalculatedRunCharges = RunCharges * (1.00F + percent);
        }
    }
}