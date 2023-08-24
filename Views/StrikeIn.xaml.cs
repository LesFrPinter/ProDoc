using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Documents.Spreadsheet.Expressions.Functions;

namespace ProDocEstimate.Views
{
    public partial class StrikeIn : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

        #region Properties

        //TEST DATA ADDED:
        //UPDATE FEATURES
        //   SET PRESS_SETUP_TIME =  3,
        //       ADDTL_BIND_TIME  = 20
        // WHERE PRESS_SIZE = '11'
        //   AND CATEGORY = 'STRIKE IN'

        //NOTE: PRESS_SETUP_TIME and ADDTL_BIND_TIME should contain minutes, not fractions of an hour

        private bool starting;  public bool Starting { get { return starting; } set { starting = value; } }
        public bool Removed = false;

        private float flatCharge;     public float FlatCharge     { get { return flatCharge;     } set { flatCharge     = value; OnPropertyChanged(); } } // Dollars
        private float setupMinutes;   public float SetupMinutes   { get { return setupMinutes;   } set { setupMinutes   = value; OnPropertyChanged(); } } // Fraction of an hour
        private float bindingTime;    public float   BindingTime  { get { return bindingTime;    } set { bindingTime    = value; OnPropertyChanged(); } } // Fraction of an hour
        private float finishMaterial; public float FinishMaterial { get { return finishMaterial; } set { finishMaterial = value; OnPropertyChanged(); } } // Dollars

        private float baseFlatCharge; public float BaseFlatCharge { get { return baseFlatCharge; } set { baseFlatCharge = value; OnPropertyChanged(); } }
        private int   baseSetupTime;  public int   BaseSetupTime  { get { return baseSetupTime;  } set { baseSetupTime  = value; OnPropertyChanged(); } }
        private int   baseBindTime;   public int   BaseBindTime   { get { return baseBindTime;   } set { baseBindTime   = value; OnPropertyChanged(); } }
        private float baseFinishMatl; public float BaseFinishMatl { get { return baseFinishMatl; } set { baseFinishMatl = value; OnPropertyChanged(); } }

        public string ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public SqlConnection? conn;
        public SqlDataAdapter? da;
        public DataTable?  dt;
        public SqlCommand? scmd;

        //These don't need change notification!
        //private string pressSize; public string PressSize { get { return pressSize; } set { pressSize = value; OnPropertyChanged(); } }
        //private string quoteNum; public string QuoteNum { get { return quoteNum; } set { quoteNum = value; OnPropertyChanged(); } }
        //private string fieldList; public string FieldList { get { return fieldList; } set { fieldList = value; OnPropertyChanged(); } }

        public string PressSize;
        public string QuoteNum;
        public string FieldList;

        private float pressSetup;      public float PressSetup    { get { return pressSetup;     } set { pressSetup      = value; OnPropertyChanged(); } }
        private int collatorSetup;     public int CollatorSetup { get { return collatorSetup;  } set { collatorSetup   = value; OnPropertyChanged(); } }
        private int binderySetup;      public int BinderySetup  { get { return binderySetup;   } set { binderySetup    = value; OnPropertyChanged(); } }

        private int basePressSetup;    public int BasePressSetup { get { return basePressSetup; } set { basePressSetup = value; OnPropertyChanged(); } }
        private int baseCollatorSetup; public int BaseCollatorSetup { get { return baseCollatorSetup; } set { baseCollatorSetup = value; OnPropertyChanged(); } }
        private int baseBinderySetup;  public int BaseBinderySetup { get { return baseBinderySetup; } set { baseBinderySetup = value; OnPropertyChanged(); } }

        // Material percentages in NumericUpDowns
        private int fcPct; public int FCPct { get { return fcPct; } set { fcPct = value; OnPropertyChanged(); } }
        private int psPct; public int PSPct { get { return psPct; } set { psPct = value; OnPropertyChanged(); } }
        private int btPct; public int BTPct { get { return btPct; } set { btPct = value; OnPropertyChanged(); } }
        private int fmPct; public int FMPct { get { return fmPct; } set { fmPct = value; OnPropertyChanged(); } }

        // Labor percentages in NumericUpDowns
        private int labPS; public int LabPS { get { return labPS; } set { labPS = value; OnPropertyChanged(); } }
        private int labCS; public int LabCS { get { return labCS; } set { labCS = value; OnPropertyChanged(); } }
        private int labBS; public int LabBS { get { return labBS; } set { labBS = value; OnPropertyChanged(); } }
        private int labPSL; public int LabPSL { get { return labPSL; } set { labPSL = value; OnPropertyChanged(); } }
        private int labCSL; public int LabCSL { get { return labCSL; } set { labCSL = value; OnPropertyChanged(); } }
        private int labBSL; public int LabBSL { get { return labBSL; } set { labBSL = value; OnPropertyChanged(); } }

        private int pressSlowdown; public int PressSlowdown { get { return pressSlowdown; } set { pressSlowdown = value; OnPropertyChanged(); } }
        private int collatorSlowdown; public int CollatorSlowdown { get { return collatorSlowdown; } set { collatorSlowdown = value; OnPropertyChanged(); } }
        private int binderySlowdown; public int BinderySlowdown { get { return binderySlowdown; } set { binderySlowdown = value; OnPropertyChanged(); } }

        private int basepressSlowdown; public int BasePressSlowdown { get { return basepressSlowdown; } set { basepressSlowdown = value; OnPropertyChanged(); } }
        private int basecollatorSlowdown; public int BaseCollatorSlowdown { get { return basecollatorSlowdown; } set { basecollatorSlowdown = value; OnPropertyChanged(); } }
        private int basebinderySlowdown; public int BaseBinderySlowdown { get { return basebinderySlowdown; } set { basebinderySlowdown = value; OnPropertyChanged(); } }

        private int labPSS1Pct; public int LabPSS1Pct { get { return labPSS1Pct; } set { labPSS1Pct = value; OnPropertyChanged(); } }
        private int labCSS2Pct; public int LabCSS2Pct { get { return labCSS2Pct; } set { labCSS2Pct = value; OnPropertyChanged(); } }
        private int labBSS2Pct; public int LabBSS2Pct { get { return labBSS2Pct; } set { labBSS2Pct = value; OnPropertyChanged(); } }

        private int calculatedPS; public int CalculatedPS { get { return calculatedPS; } set { calculatedPS = value; OnPropertyChanged(); } }
        private int calculatedCS; public int CalculatedCS { get { return calculatedCS; } set { calculatedCS = value; OnPropertyChanged(); } }
        private int calculatedBS; public int CalculatedBS { get { return calculatedBS; } set { calculatedBS = value; OnPropertyChanged(); } }

        private int slowdownTotal; public int SlowdownTotal { get { return slowdownTotal; } set { slowdownTotal = value; OnPropertyChanged(); } }
        private int setupTotal; public int SetupTotal { get { return setupTotal; } set { setupTotal = value; OnPropertyChanged(); } }

        #endregion

        public StrikeIn(string PRESSSIZE, string QUOTENUM)
        {
            //TestData:
            //UPDATE[ESTIMATING].[dbo].[FEATURES]
            // SET  (What was changed here?)

            InitializeComponent();
            Removed = true; // Will be "automatically" removed from the "lstSelected" collection when the screen is closed unless there are some saved parameters

            PressSize = PRESSSIZE; 
            QuoteNum = QUOTENUM;

            // List of fields to be retrieved from the FEATURES table:
            FieldList =
               " F_TYPE,"
             + " FLAT_CHARGE,"
             + " RUN_CHARGE,"
             + " PLATE_MATL,"
             + " ADDTL_BIND_TIME,"
             + " FINISH_MATL,"
             + " CONV_MATL,"
             + " PRESS_MATL,"
             + " PRESS_SETUP_TIME  AS PRESS_SETUP,"
             + " COLLATOR_SETUP,"
             + " BINDERY_SETUP,"
             + " PRESS_SLOWDOWN,"
             + " COLLATOR_SLOWDOWN,"
             + " BINDERY_SLOWDOWN,"
             + " SLOWDOWN_PER_PART AS BINDERY_SLOWDOWN";  // from the FEATURES table

            DataContext = this;

            Starting = true;    // Don't recalculate during data loading

            LoadFeature();      // This calls CalcTotal
            LoadQuote();        // The first line of CalcTotal says "If (Starting) return...

            Starting = false;   // Now you can recalculate

            CalcTotal();

            PreviewKeyDown += (s, e) => { if (e.Key == Key.Escape) Close(); };
        }

        public void OnLoad(object sender, RoutedEventArgs e)
        { 
            this.Height = this.Height *= 1.8; 
            this.Width = this.Width *= 1.8; 
            Top = 50; 
        }

        private void LoadFeature()
        {
            string cmd = $"SELECT {FieldList} FROM [ESTIMATING].[dbo].[FEATURES] "
                       + $" WHERE CATEGORY = 'STRIKE IN' AND PRESS_SIZE = '{PressSize}'";

            dt = new DataTable("Features");
            conn = new SqlConnection(ConnectionString);
            da = new SqlDataAdapter(cmd, conn); da.Fill(dt);
            DataView dv = dt.DefaultView;

            // Base material charges
            float t;
            t = 0.0F; float.TryParse(dv[0]["FLAT_CHARGE"].ToString(), out t);                     BaseFlatCharge = t;
            t = 0.0F; float.TryParse(dv[0]["PRESS_SETUP"].ToString(), out t); t = 60.00F * t;     BasePressSetup = (int)t;
            t = 0.0F; float.TryParse(dv[0]["ADDTL_BIND_TIME"].ToString(), out t); t = 60.00F * t; BaseBindTime = (int)t;
            t = 0.0F; float.TryParse(dv[0]["FINISH_MATL"].ToString(), out t);                     BaseFinishMatl = t;

            // Base Labor Charges
            // The next three calculations assume that the values from the FEATURES table are decimal fractions of an hour (e.g. .25 for 1/4)
            float fPressSetup    = float.Parse(dv[0]["PRESS_SETUP"].ToString())    * 60.0F; BasePressSetup    = (int)Math.Round(fPressSetup); 
            float fCollatorSetup = float.Parse(dv[0]["COLLATOR_SETUP"].ToString()) * 60.0F; BaseCollatorSetup = (int)Math.Round(fCollatorSetup);
            float fBinderySetup  = float.Parse(dv[0]["BINDERY_SETUP"].ToString())  * 60.0F; BaseBinderySetup  = (int)Math.Round(fBinderySetup);

            BasePressSlowdown    = int.Parse(dv[0]["PRESS_SLOWDOWN"].ToString());       // Minutes of slowdown to add to the base amount...
            BaseCollatorSlowdown = int.Parse(dv[0]["COLLATOR_SLOWDOWN"].ToString());    //              "
            BaseBinderySlowdown  = int.Parse(dv[0]["BINDERY_SLOWDOWN"].ToString());     //

            BaseSetupTime = int.Parse(dv[0]["PRESS_SETUP"].ToString());    // RENAMED FROM "PRESS_SETUP_TIME" IN FieldList
            BaseBindTime  = int.Parse(dv[0]["ADDTL_BIND_TIME"].ToString());
        }

        private void LoadQuote()
        {
            string cmd = "SELECT * FROM [ESTIMATING].[dbo].[QUOTE_DETAILS] WHERE QUOTE_NUM = '" + QuoteNum + "' AND CATEGORY = 'Strike In'";
            dt = new DataTable("Details");
            conn = new SqlConnection(ConnectionString);
            da = new SqlDataAdapter(cmd, conn); 
            da.Fill(dt);        // dt is declared globally, so the LoadAdjustments routines will be able to see it

            if (dt.Rows.Count > 0) { LoadMaterialsAdjustments(); LoadLaborAdjustments(); }

            conn.Close();
            CalcTotal();
            Removed = false;
        }

        private void MatValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        { CalcMaterial(); }

        private void LoadMaterialsAdjustments()
        {   // Load percentage markups for the four Materials base values
            int t;
            t = 0; int.TryParse(dt.Rows[0]["Value1"].ToString(), out t); FCPct = t;
            t = 0; int.TryParse(dt.Rows[0]["Value2"].ToString(), out t); PSPct = t;
            t = 0; int.TryParse(dt.Rows[0]["Value3"].ToString(), out t); BTPct = t;
            t = 0; int.TryParse(dt.Rows[0]["Value4"].ToString(), out t); FMPct = t;
        }

        private void LoadLaborAdjustments()
        {   // Load minutes of setup and slowdown times to the base values read from the FEATURES table
            int t;
            t = 0; int.TryParse(dt.Rows[0]["Value5"].ToString(), out t); LabPS = t;
            t = 0; int.TryParse(dt.Rows[0]["Value6"].ToString(), out t); LabPSL = t;
            t = 0; int.TryParse(dt.Rows[0]["Value7"].ToString(), out t); LabCS = t;
            t = 0; int.TryParse(dt.Rows[0]["Value8"].ToString(), out t); LabCSL = t;
            t = 0; int.TryParse(dt.Rows[0]["Value9"].ToString(), out t); LabBS = t;
            t = 0; int.TryParse(dt.Rows[0]["Value10"].ToString(), out t); LabBSL = t;
        }

        private void CalcMaterial()
        {
            if (Starting) return;

            FlatCharge     = (1.00F + ((float)FCPct / 100.00F)) * BaseFlatCharge;
            SetupMinutes   = (1.00F + ((float)PSPct / 100.00F)) * float.Parse(BaseSetupTime.ToString());
            BindingTime    = (1.00F + ((float)BTPct / 100.00F)) * float.Parse(BaseBindTime.ToString());
            FinishMaterial = (1.00F + ((float)FMPct / 100.00F)) * BaseFinishMatl;
        }

        private void CalcTotal()
        {
            CalcMaterial(); CalculateLabor();
        }

        private void CalcLabor(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        {
            CalculateLabor();
        }

        private void CalculateLabor()
        {
            if (Starting) return;
            PressSetup      = BasePressSetup + LabPS;
            CollatorSetup   = BaseCollatorSetup + LabCS;
            BinderySetup    = BaseBinderySetup + LabBS;
            SetupTotal      = (int)PressSetup + CollatorSetup + BinderySetup;

            PressSlowdown   = BasePressSlowdown + LabPSL;
            CollatorSlowdown= BaseCollatorSlowdown + LabCSL;
            BinderySlowdown = BaseBinderySlowdown + LabBSL;
            SlowdownTotal   = PressSlowdown + CollatorSlowdown + BinderySlowdown;

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string str = $"DELETE [ESTIMATING].[dbo].[QUOTE_DETAILS] WHERE QUOTE_NUM = '{QuoteNum}' AND Category = 'Strike In'";
            conn = new SqlConnection(ConnectionString);
            conn.Open();
            scmd = new SqlCommand(str, conn); scmd.ExecuteNonQuery(); if(conn.State==ConnectionState.Open) { conn.Close(); }

            str = "INSERT INTO [ESTIMATING].[dbo].[QUOTE_DETAILS] "
                +  "(   QUOTE_NUM,     SEQUENCE,       CATEGORY," 
                +  "   Param1,         Param2,         Param3,          Param4,"
                +  "   Value1,         Value2,         Value3,          Value4, "
                +  "   Param5,         Param6,         Param7,          Param8,         Param9,     Param10," 
                +  "   Value5,         Value6,         Value7,          Value8,         Value9,     Value10, "
                +  "   SETUP_MINUTES,  SLOWDOWN_PERCENT ) "
                +  "VALUES ( "
                + $"  '{QuoteNum}',    11,            'Strike In',"
                + $"  'Flat Charge%', 'Press Setup%', 'Binding Time%', 'Finish Matl%',"
                + $"  '{FCPct}',      '{PSPct}',      '{BTPct}',       '{FMPct}',"
                +  "  'PressMins',    'PressSlow',    'CollMins',      'CollSlow',     'BindMins', 'BindSlow'," 
                + $"  '{LabPS}',      '{LabPSL}',     '{LabCS}',       '{LabCSL}',     '{LabBS}',  '{LabBSL}',"
                + $"   {SetupTotal},   {SlowdownTotal} )" ;

            scmd.CommandText = str; 
            conn.Open(); 
            scmd.Connection = conn;
            scmd.ExecuteNonQuery() ; 
            conn.Close();

            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        { Close(); }

    }
}