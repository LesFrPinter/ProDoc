using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace ProDocEstimate.Views
{
    public partial class Carbon : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

        #region Properties

        public string ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public SqlConnection? conn;
        public SqlDataAdapter? da;
        public DataTable? dt;
        public SqlCommand? scmd;
        public DataView? dv;

        private string quoteNum;      public string QuoteNum      { get { return quoteNum;      } set { quoteNum      = value; OnPropertyChanged(); } }

        private int    stockNum;      public int    StockNum      { get { return stockNum;      } set { stockNum      = value; OnPropertyChanged(); } }
        private int    patternNum;    public int    PatternNum    { get { return patternNum;    } set { patternNum    = value; OnPropertyChanged(); } }
        private float  stockValue;    public float  StockValue    { get { return stockValue;    } set { stockValue    = value; OnPropertyChanged(); } }
        private float  patternValue;  public float  PatternValue  { get { return patternValue;  } set { patternValue  = value; OnPropertyChanged(); } }

        private bool   blackStock;    public bool   BlackStock    { get { return blackStock;    } set { blackStock    = value; OnPropertyChanged(); } }
        private bool   blueStock;     public bool   BlueStock     { get { return blueStock;     } set { blueStock     = value; OnPropertyChanged(); } }
        private bool   blackPattern;  public bool   BlackPattern  { get { return blackPattern;  } set { blackPattern  = value; OnPropertyChanged(); } }
        private bool   bluePattern;   public bool   BluePattern   { get { return bluePattern;   } set { bluePattern   = value; OnPropertyChanged(); } }

        private int    collatorSetup; public int    CollatorSetup { get { return collatorSetup; } set { collatorSetup = value; OnPropertyChanged(); } }
        private int    calcCollSetup; public int    CalcCollSetup { get { return calcCollSetup; } set { calcCollSetup = value; OnPropertyChanged(); } }
        private int    collDelta;     public int    CollDelta     { get { return collDelta;     } set { collDelta     = value; OnPropertyChanged(); } }

        private int    collatorSlow;  public int    CollatorSlow  { get { return collatorSlow;  } set { collatorSlow  = value; OnPropertyChanged(); } }
        private int    calcCollSlow;  public int    CalcCollSlow  { get { return calcCollSlow;  } set { calcCollSlow  = value; OnPropertyChanged(); } }
        private int    collSlowDelta; public int    CollSlowDelta { get { return collSlowDelta; } set { collSlowDelta = value; OnPropertyChanged(); } }

        #endregion

        public Carbon(string QUOTENUM)
        {
            InitializeComponent();
            DataContext = this;
            QuoteNum = QUOTENUM;

            LoadData();

            PreviewKeyDown += (s, e) => { if (e.Key == Key.Escape) Close(); };
        }

        public void OnLoad(object sender, RoutedEventArgs e)
        {
            this.Height = this.Height *= 1.6;
            this.Width = this.Width *= 1.6;
            Top = 50;
        }

        public void LoadParameters() { }

        public void LoadData()
        {
            string cmd = $"SELECT * FROM [ESTIMATING].[dbo].[QUOTE_DETAILS] WHERE QUOTE_NUM = '{QuoteNum}' AND CATEGORY = 'Carbon'";
            conn = new(ConnectionString);
            da = new SqlDataAdapter(cmd, conn); 
            dt = new DataTable("row"); 
            da.Fill(dt); 
            dv = dt.DefaultView;

            if (dv.Count == 0) return;  // There is no CARBON entry in QUOTE_DETAILS   

            string StockColor = dv[0]["Value1"].ToString();
            BlackStock = StockColor == "Black" ? true : false;
            BlueStock = !BlackStock;

            string PatternColor = dv[0]["Value2"].ToString();
            BlackPattern = PatternColor == "Black" ? true : false;
            BluePattern = !BlackPattern;

            StockNum = int.Parse(dv[0]["Value3"].ToString());
            PatternNum = int.Parse(dv[0]["Value4"].ToString());

            StockValue = float.Parse(dv[0]["Value5"].ToString());
            PatternValue = float.Parse(dv[0]["Value6"].ToString());

            CollDelta = int.Parse(dv[0]["Value7"].ToString());
            CollSlowDelta = int.Parse(dv[0]["Value8"].ToString());

            CollatorSetup = int.Parse(dv[0]["CollSetupMin"].ToString());
            CollatorSlow = int.Parse(dv[0]["CollSlowPct"].ToString());
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string StockColor   = BlackStock   ? "Black" : "Blue";
            string PatternColor = BlackPattern ? "Black" : "Blue";

            // Save to Quote_Details with Category="Carbon"
            string cmd = $"DELETE [ESTIMATING].[dbo].[QUOTE_DETAILS] WHERE QUOTE_NUM = '{QuoteNum}' AND Category = 'Carbon'";
            conn = new SqlConnection(ConnectionString); conn.Open();
            scmd= new SqlCommand(cmd, conn); scmd.ExecuteNonQuery();

            cmd = "INSERT INTO [ESTIMATING].[dbo].[QUOTE_DETAILS] "
                + "          (  QUOTE_NUM,     Category,         Sequence,   CollSetupMin,    CollSlowPct,                "
                + "             Param1,        Param2,           Param3,     Param4,          Param5,       Param6,         Param7,      Param8,         "
                + "             Value1,        Value2,           Value3,     Value4,          Value5,       Value6,         Value7,      Value8         )"
                + $" VALUES ( '{QuoteNum}',   'Carbon',          14,        {CollatorSetup}, {CollatorSlow},                                             "
                +  "           'StockColor',  'PatternColor',   'StockNum', 'PatternNum',    'StockValue', 'PatternValue', 'CollDelta', 'CollSlowDelta', "
                + $"          '{StockColor}', '{PatternColor}', {StockNum}, {PatternNum},    {StockValue}, {PatternValue}, {CollDelta}, {CollSlowDelta} )";
            if (conn.State== System.Data.ConnectionState.Closed) { conn.Open(); }
            scmd.Connection = conn; scmd.CommandText = cmd; scmd.ExecuteNonQuery(); conn.Close();
            Close();
        }

        private void btnCanc_Click(object sender, RoutedEventArgs e) { Close(); }

        private void StockRadio1_Checked    (object sender, RoutedEventArgs e) { BlackStock   = true;  BlueStock   = false; }
        private void StockRadio1_Unchecked  (object sender, RoutedEventArgs e) { BlackStock   = false; BlueStock   = true;  }
        private void StockRadio2_Checked    (object sender, RoutedEventArgs e) { BlackStock   = false; BlueStock   = true;  }
        private void StockRadio2_Unchecked  (object sender, RoutedEventArgs e) { BlackStock   = true;  BlueStock   = false; }
        private void PatternRadio1_Checked  (object sender, RoutedEventArgs e) { BlackPattern = true;  BluePattern = false; }
        private void PatternRadio1_Unchecked(object sender, RoutedEventArgs e) { BlackPattern = false; BluePattern = true;  }
        private void PatternRadio2_Checked  (object sender, RoutedEventArgs e) { BlackPattern = false; BluePattern = true;  }
        private void PatternRadio2_Unchecked(object sender, RoutedEventArgs e) { BlackPattern = true;  BluePattern = false; }

        private void CalcLabor(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        {
            //double mult  = (1 + double.Parse(CalcCollDelta.ToString())) / 100.0F;
            //double dblCS = double.Parse(CollatorSetup.ToString());
            //CalcCollSetup = CollatorSetup + int.Parse(Math.Round(mult * dblCS).ToString());

            CalcCollSetup = CollatorSetup + CollDelta;
            CalcCollSlow  = CollatorSlow  + CollSlowDelta;
        }
    }
}