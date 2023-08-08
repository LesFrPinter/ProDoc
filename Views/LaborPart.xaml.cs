using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ProDocEstimate.Views
{
    public partial class LaborPart : Window, INotifyPropertyChanged
    {

        #region Properties

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

        private bool starting; public bool Starting { get { return starting; } set { starting = value; } }
        private bool Removed = false;

        public string ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public SqlConnection? conn;
        public SqlDataAdapter? da;
        public DataTable? dt;
        public SqlCommand? scmd;

        //These don't need change notification!
        //private string pressSize; public string PressSize { get { return pressSize; } set { pressSize = value; OnPropertyChanged(); } }
        //private string quoteNum; public string QuoteNum { get { return quoteNum; } set { quoteNum = value; OnPropertyChanged(); } }
        //private string fieldList; public string FieldList { get { return fieldList; } set { fieldList = value; OnPropertyChanged(); } }

        public string PressSize;
        public string QuoteNum;
        public string FieldList;

        // Labor percentages in NumericUpDowns
        private int labPS;  public int LabPS  { get { return labPS;  } set { labPS  = value; OnPropertyChanged(); } }
        private int labCS;  public int LabCS  { get { return labCS;  } set { labCS  = value; OnPropertyChanged(); } }
        private int labBS;  public int LabBS  { get { return labBS;  } set { labBS  = value; OnPropertyChanged(); } }
        private int labPSL; public int LabPSL { get { return labPSL; } set { labPSL = value; OnPropertyChanged(); } }
        private int labCSL; public int LabCSL { get { return labCSL; } set { labCSL = value; OnPropertyChanged(); } }
        private int labBSL; public int LabBSL { get { return labBSL; } set { labBSL = value; OnPropertyChanged(); } }

        private int basePressSetup;       public int BasePressSetup       { get { return basePressSetup;        } set { basePressSetup      = value; OnPropertyChanged(); } }
        private int baseCollatorSetup;    public int BaseCollatorSetup    { get { return baseCollatorSetup;     } set { baseCollatorSetup   = value; OnPropertyChanged(); } }
        private int baseBinderySetup;     public int BaseBinderySetup     { get { return baseBinderySetup;      } set { baseBinderySetup    = value; OnPropertyChanged(); } }

        private int pressSetup;           public int PressSetup           { get { return pressSetup;            } set { pressSetup          = value; OnPropertyChanged(); } }
        private int collatorSetup;        public int CollatorSetup        { get { return collatorSetup;         } set { collatorSetup       = value; OnPropertyChanged(); } }
        private int binderySetup;         public int BinderySetup         { get { return binderySetup;          } set { binderySetup        = value; OnPropertyChanged(); } }

        private int basepressSlowdown;    public int BasePressSlowdown    { get { return basepressSlowdown;     } set { basepressSlowdown   = value; OnPropertyChanged(); } }
        private int basecollatorSlowdown; public int BaseCollatorSlowdown { get { return basecollatorSlowdown;  } set { basecollatorSlowdown= value; OnPropertyChanged(); } }
        private int basebinderySlowdown;  public int BaseBinderySlowdown  { get { return basebinderySlowdown;   } set { basebinderySlowdown = value; OnPropertyChanged(); } }

        private int pressSlowdown;        public int PressSlowdown        { get { return pressSlowdown;         } set { pressSlowdown       = value; OnPropertyChanged(); } }
        private int collatorSlowdown;     public int CollatorSlowdown     { get { return collatorSlowdown;      } set { collatorSlowdown    = value; OnPropertyChanged(); } }
        private int binderySlowdown;      public int BinderySlowdown      { get { return binderySlowdown;       } set { binderySlowdown     = value; OnPropertyChanged(); } }

        //private int labPSS1Pct; public int LabPSS1Pct { get { return labPSS1Pct; } set { labPSS1Pct = value; OnPropertyChanged(); } }
        //private int labCSS2Pct; public int LabCSS2Pct { get { return labCSS2Pct; } set { labCSS2Pct = value; OnPropertyChanged(); } }
        //private int labBSS2Pct; public int LabBSS2Pct { get { return labBSS2Pct; } set { labBSS2Pct = value; OnPropertyChanged(); } }

        private int calculatedPS; public int CalculatedPS { get { return calculatedPS; } set { calculatedPS = value; OnPropertyChanged(); } }
        private int calculatedCS; public int CalculatedCS { get { return calculatedCS; } set { calculatedCS = value; OnPropertyChanged(); } }
        private int calculatedBS; public int CalculatedBS { get { return calculatedBS; } set { calculatedBS = value; OnPropertyChanged(); } }

        private int slowdownTotal; public int SlowdownTotal { get { return slowdownTotal; } set { slowdownTotal = value; OnPropertyChanged(); } }
        private int setupTotal; public int SetupTotal { get { return setupTotal; } set { setupTotal = value; OnPropertyChanged(); } }

        #endregion

        public LaborPart()
        {
            InitializeComponent();

            Starting = true;

            LoadFeature();
            LoadQuote();
            CalculateLabor();

            Starting = false;
        }

        private void LoadFeature()
        {
            string cmd = $"SELECT {FieldList} FROM [ESTIMATING].[dbo].[FEATURES] WHERE CATEGORY = 'STRIKE IN' AND CATEGORY = '{PressSize}'";
            dt = new DataTable("LaborAdjustments");
            conn = new SqlConnection(ConnectionString);
            da = new SqlDataAdapter(cmd, conn);
            dt.Clear(); da.Fill(dt);

            if (dt.Rows.Count == 0) { MessageBox.Show("No data"); return; }

            DataView dv = dt.DefaultView;

            // The next three calculations assume that the values from the FEATURES table are decimal fractions of an Hour (e.g. .25 for 1/4)
            float fPressSetup       = float.Parse(dv[0]["PRESS_SETUP_TIME"].ToString()) * 60.0F; BasePressSetup     = int.Parse(fPressSetup.ToString());
            float fCollatorSetup    = float.Parse(dv[0]["COLLATOR_SETUP"].ToString())   * 60.0F; BaseCollatorSetup  = int.Parse(fCollatorSetup.ToString());
            float fBinderySetup     = float.Parse(dv[0]["BINDERY_SETUP"].ToString())    * 60.0F; BaseBinderySetup   = int.Parse(fBinderySetup.ToString());

            BasePressSlowdown       = int.Parse(dv[0]["PRESS_SLOWDOWN"].ToString());
            BaseCollatorSlowdown    = int.Parse(dv[0]["COLLATOR_SLOWDOWN"].ToString());
            BaseBinderySlowdown     = int.Parse(dv[0]["BINDERY_SLOWDOWN"].ToString());
        }

        private void LoadQuote()
        {
            string cmd = $"SELECT * FROM [ESTIMATING].[dbo].[QUOTE_DETAILS] WHERE QUOTE_NUM = '{QuoteNum}' AND CATEGORY = 'XXXXXX'";
            dt = new DataTable("Details");
            conn = new SqlConnection(ConnectionString);
            da = new SqlDataAdapter(cmd, conn); da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                int t;
                // LABOR modifiers:
                t = 0; int.TryParse(dt.Rows[0]["Value5"].ToString(), out t); LabPS = t;
                t = 0; int.TryParse(dt.Rows[0]["Value6"].ToString(), out t); LabPSL = t;
                t = 0; int.TryParse(dt.Rows[0]["Value7"].ToString(), out t); LabCS = t;
                t = 0; int.TryParse(dt.Rows[0]["Value"].ToString(), out t); LabCSL = t;
                t = 0; int.TryParse(dt.Rows[0]["Value9"].ToString(), out t); LabBS = t;
                t = 0; int.TryParse(dt.Rows[0]["Value10"].ToString(), out t); LabBSL = t;
                t = 0; int.TryParse(dt.Rows[0]["SETUP_MINUTES"].ToString(), out t); SetupTotal = t;
                t = 0; int.TryParse(dt.Rows[0]["SLOWDOWN_PERCENT"].ToString(), out t); SlowdownTotal = t;
            }

            conn.Close();
        }

        private void CalcLabor(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        { CalculateLabor(); }

        private void CalculateLabor()
        {
            if (Starting) return;   // So the routine won't be called every time a "ValueChanged" method is called on a RadNumericUpDown during initial load.
            PressSetup = BasePressSetup + LabPS;
            CollatorSetup = BaseCollatorSetup + LabCS;
            BinderySetup = BaseBinderySetup + LabBS;
            SetupTotal = PressSetup + CollatorSetup + BinderySetup;

            PressSlowdown = BasePressSlowdown + LabPSL;
            CollatorSlowdown = BaseCollatorSlowdown + LabCSL;
            BinderySlowdown = BaseBinderySlowdown + LabBSL;
            SlowdownTotal = PressSlowdown + CollatorSlowdown + BinderySlowdown;
        }

    }
}
