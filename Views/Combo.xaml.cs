using ProDocEstimate.Editors;
using System;
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
    public partial class Combo : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

        #region Properties

        // Data loaded for testing:
        //update features set
        //  flat_charge = 60.00,
        //  run_charge  = 10.00,
        //  plate_matl  = 20.00,
        //  finish_matl = 30.00,
        //  press_matl  = 40.00,
        //  conv_matl   = 60.00,
        // where category   = 'COMBO'
        //   and Press_SIZE = '11';

        //TestData:
        //UPDATE[ESTIMATING].[dbo].[FEATURES]
        // SET
        //   PRESS_SETUP_TIME  = 10,
        //   COLLATOR_SETUP    = 20,
        //   BINDERY_SETUP     = 30,
        //   PRESS_SLOWDOWN    = 40,
        //   COLLATOR_SLOWDOWN = 50,
        //   SLOWDOWN_PER_PART = 60
        //  WHERE CATEGORY     = 'COMBO'
        //    AND PRESS_SIZE   = '11'

        public bool Starting;

        public string ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public SqlConnection conn;
        public SqlDataAdapter da;
        public DataTable dt;
        public SqlCommand scmd;

        private int max; public int Max { get { return max; } set { max = value; OnPropertyChanged(); } }
        private string pressSize; public string PressSize { get { return pressSize; } set { pressSize = value; OnPropertyChanged(); } }
        private string quoteNum; public string QuoteNum { get { return quoteNum; } set { quoteNum = value; OnPropertyChanged(); } }

        private int combo1; public int Combo1 { get { return combo1; } set { combo1 = value; OnPropertyChanged(); } }

        public bool Removed = false;

        private float flat; public float Flat { get { return flat; } set { flat = value; OnPropertyChanged(); } }
        private float chng; public float Chng { get { return chng; } set { chng = value; OnPropertyChanged(); } }
        private int changes; public int Changes { get { return changes; } set { changes = value; OnPropertyChanged(); } }
        private float? total; public float? Total { get { return total; } set { total = value; OnPropertyChanged(); } }
        private bool backer; public bool Backer { get { return backer; } set { backer = value; OnPropertyChanged(); } }

        //private string quoteNo; public string QuoteNo { get { return quoteNo; } set { quoteNo = value; } }

        private float flatCharge; public float FlatCharge { get { return flatCharge; } set { flatCharge = value; OnPropertyChanged(); } }
        private float baseflatCharge; public float BaseFlatCharge { get { return baseflatCharge; } set { baseflatCharge = value; OnPropertyChanged(); } }
        private float flatChargePct; public float FlatChargePct { get { return flatChargePct; } set { flatChargePct = value; OnPropertyChanged(); } }
        private float calculatedflatCharge; public float CalculatedFlatCharge { get { return calculatedflatCharge; } set { calculatedflatCharge = value; OnPropertyChanged(); } }

        private float runCharge; public float RunCharge { get { return runCharge; } set { runCharge = value; OnPropertyChanged(); } }
        private float baserunCharge; public float BaseRunCharge { get { return baserunCharge; } set { baserunCharge = value; OnPropertyChanged(); } }
        private float runChargePct; public float RunChargePct { get { return runChargePct; } set { runChargePct = value; OnPropertyChanged(); } }
        private float calculatedrunCharge; public float CalculatedRunCharge { get { return calculatedrunCharge; } set { calculatedrunCharge = value; OnPropertyChanged(); } }

        private float plateCharge; public float PlateCharge { get { return plateCharge; } set { plateCharge = value; OnPropertyChanged(); } }
        private float baseplateCharge; public float BasePlateCharge { get { return baseplateCharge; } set { baseplateCharge = value; OnPropertyChanged(); } }
        private float plateChargePct; public float PlateChargePct { get { return plateChargePct; } set { plateChargePct = value; OnPropertyChanged(); } }
        private float calculatedplateCharge; public float CalculatedPlateCharge { get { return calculatedplateCharge; } set { calculatedplateCharge = value; OnPropertyChanged(); } }

        private float finishCharge; public float FinishCharge { get { return finishCharge; } set { finishCharge = value; OnPropertyChanged(); } }
        private float basefinishCharge; public float BaseFinishCharge { get { return basefinishCharge; } set { basefinishCharge = value; OnPropertyChanged(); } }
        private float finishChargePct; public float FinishChargePct { get { return finishChargePct; } set { finishChargePct = value; OnPropertyChanged(); } }
        private float calculatedfinishCharge; public float CalculatedFinishCharge { get { return calculatedfinishCharge; } set { calculatedfinishCharge = value; OnPropertyChanged(); } }

        private float pressCharge; public float PressCharge { get { return pressCharge; } set { pressCharge = value; OnPropertyChanged(); } }
        private float basepressCharge; public float BasePressCharge { get { return basepressCharge; } set { basepressCharge = value; OnPropertyChanged(); } }
        private float pressChargePct; public float PressChargePct { get { return pressChargePct; } set { pressChargePct = value; OnPropertyChanged(); } }
        private float calculatedpressCharge; public float CalculatedPressCharge { get { return calculatedpressCharge; } set { calculatedpressCharge = value; OnPropertyChanged(); } }

        private float convCharge; public float ConvCharge { get { return convCharge; } set { convCharge = value; OnPropertyChanged(); } }
        private float baseconvCharge; public float BaseConvCharge { get { return baseconvCharge; } set { baseconvCharge = value; OnPropertyChanged(); } }
        private float convChargePct; public float ConvChargePct { get { return convChargePct; } set { convChargePct = value; OnPropertyChanged(); } }
        private float calculatedconvCharge; public float CalculatedConvCharge { get { return calculatedconvCharge; } set { calculatedconvCharge = value; OnPropertyChanged(); } }

        private float flatTotal; public float FlatTotal { get { return flatTotal; } set { flatTotal = value; OnPropertyChanged(); } }
        private string fieldList; public string FieldList { get { return fieldList; } set { fieldList = value; OnPropertyChanged(); } }

        // Labor percentages in NumericUpDowns
        private int labPS; public int LabPS { get { return labPS; } set { labPS = value; OnPropertyChanged(); } }
        private int labCS; public int LabCS { get { return labCS; } set { labCS = value; OnPropertyChanged(); } }
        private int labBS; public int LabBS { get { return labBS; } set { labBS = value; OnPropertyChanged(); } }
        private int labPSL; public int LabPSL { get { return labPSL; } set { labPSL = value; OnPropertyChanged(); } }
        private int labCSL; public int LabCSL { get { return labCSL; } set { labCSL = value; OnPropertyChanged(); } }
        private int labBSL; public int LabBSL { get { return labBSL; } set { labBSL = value; OnPropertyChanged(); } }

        private float pressSetup; public float PressSetup { get { return pressSetup; } set { pressSetup = value; OnPropertyChanged(); } }
        private int collatorSetup; public int CollatorSetup { get { return collatorSetup; } set { collatorSetup = value; OnPropertyChanged(); } }
        private int binderySetup; public int BinderySetup { get { return binderySetup; } set { binderySetup = value; OnPropertyChanged(); } }

        private int basePressSetup; public int BasePressSetup { get { return basePressSetup; } set { basePressSetup = value; OnPropertyChanged(); } }
        private int baseCollatorSetup; public int BaseCollatorSetup { get { return baseCollatorSetup; } set { baseCollatorSetup = value; OnPropertyChanged(); } }
        private int baseBinderySetup; public int BaseBinderySetup { get { return baseBinderySetup; } set { baseBinderySetup = value; OnPropertyChanged(); } }

        private int pressSlowdown; public int PressSlowdown { get { return pressSlowdown; } set { pressSlowdown = value; OnPropertyChanged(); } }
        private int collatorSlowdown; public int CollatorSlowdown { get { return collatorSlowdown; } set { collatorSlowdown = value; OnPropertyChanged(); } }
        private int binderySlowdown; public int BinderySlowdown { get { return binderySlowdown; } set { binderySlowdown = value; OnPropertyChanged(); } }

        private int basepressSlowdown; public int BasePressSlowdown { get { return basepressSlowdown; } set { basepressSlowdown = value; OnPropertyChanged(); } }
        private int basecollatorSlowdown; public int BaseCollatorSlowdown { get { return basecollatorSlowdown; } set { basecollatorSlowdown = value; OnPropertyChanged(); } }
        private int basebinderySlowdown; public int BaseBinderySlowdown { get { return basebinderySlowdown; } set { basebinderySlowdown = value; OnPropertyChanged(); } }

        //private int labPSS1Pct; public int LabPSS1Pct { get { return labPSS1Pct; } set { labPSS1Pct = value; OnPropertyChanged(); } }
        //private int labCSS2Pct; public int LabCSS2Pct { get { return labCSS2Pct; } set { labCSS2Pct = value; OnPropertyChanged(); } }
        //private int labBSS2Pct; public int LabBSS2Pct { get { return labBSS2Pct; } set { labBSS2Pct = value; OnPropertyChanged(); } }

        private int calculatedPS; public int CalculatedPS { get { return calculatedPS; } set { calculatedPS = value; OnPropertyChanged(); } }
        private int calculatedCS; public int CalculatedCS { get { return calculatedCS; } set { calculatedCS = value; OnPropertyChanged(); } }
        private int calculatedBS; public int CalculatedBS { get { return calculatedBS; } set { calculatedBS = value; OnPropertyChanged(); } }

        private int slowdownTotal; public int SlowdownTotal { get { return slowdownTotal; } set { slowdownTotal = value; OnPropertyChanged(); } }
        private int setupTotal; public int SetupTotal { get { return setupTotal; } set { setupTotal = value; OnPropertyChanged(); } }

        #endregion

        public Combo(string PRESSSIZE, string QUOTENUM)
        {
            InitializeComponent();
            this.DataContext = this;

            SqlConnection conn = new(ConnectionString);

            Title      = "Quote #: " + QUOTENUM;
            QuoteNum   = QUOTENUM;
            PressSize  = PRESSSIZE;

            Starting = true;

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

            LoadMaximum();
            LoadFeature();
            LoadData();
            CalcTotal();

            Starting = false;

            PreviewKeyDown += (s, e) => { if (e.Key == Key.Escape) Close(); };
        }

        public void OnLoad(object sender, RoutedEventArgs e)
        {
            this.Height = this.Height *= (1.0F + MainWindow.FeatureZoom);
            this.Width = this.Width *= (1.0F + MainWindow.FeatureZoom);
            Top = 50;
        }

        private void LoadMaximum()
        {
            // Retrieve maximum number of values that can be entered in the M1 spinner
            string str = $"SELECT F_TYPE, MAX(Number) AS Max FROM [ESTIMATING].[dbo].[FEATURES] WHERE Category = 'COMBO' AND Press_Size = '{PressSize}' GROUP BY F_TYPE";
            SqlConnection  conn = new(ConnectionString);
            SqlDataAdapter da   = new(str, conn); DataTable dt = new(); dt.Rows.Clear(); da.Fill(dt);
            DataView dv  = new DataView(dt);
            dv.RowFilter = "F_TYPE='COMBO'"; 
            M1.Maximum   = int.Parse(dv[0]["Max"].ToString());
        }

        private void LoadFeature()
        {
            if ( Starting ) return;

            BaseFlatCharge = 0.00F;
            BaseRunCharge = 0.00F;
            BasePlateCharge = 0.00F;
            BaseFinishCharge = 0.00F;
            BasePressCharge = 0.00F;
            BaseConvCharge = 0.00F;

            //FlatChargePct = 0.00F;
            //RunChargePct = 0.00F;
            //PlateChargePct = 0.00F;
            //FinishChargePct = 0.00F;
            //PressChargePct = 0.00F;
            //ConvChargePct = 0.00F;

            BasePressSetup = 0;
            BaseCollatorSetup = 0;
            BaseBinderySetup = 0;
            BasePressSlowdown = 0;
            BaseCollatorSlowdown = 0;
            BaseBinderySlowdown = 0;

            string str = $"SELECT {FieldList} FROM [ESTIMATING].[dbo].[FEATURES] WHERE Category = 'COMBO' AND Press_Size = '{PressSize}' AND NUMBER = '{Combo1}'";

//Standard test is this:
//SELECT F_TYPE, PRESS_SIZE, NUMBER,
//       FLAT_CHARGE, RUN_CHARGE, PLATE_MATL, ADDTL_BIND_TIME,
//       FINISH_MATL, CONV_MATL, PRESS_MATL,
//       PRESS_SETUP_TIME  AS PRESS_SETUP, COLLATOR_SETUP, BINDERY_SETUP,
//       PRESS_SLOWDOWN, COLLATOR_SLOWDOWN, BINDERY_SLOWDOWN --SLOWDOWN_PER_PART AS BINDERY_SLOWDOWN  -- why is this duplicated?
//  FROM[ESTIMATING].[dbo].[FEATURES]
// WHERE Category   = 'COMBO'
//   AND Press_Size = '11'
//   AND NUMBER     = '1'

            SqlConnection conn = new(ConnectionString);
            SqlDataAdapter da = new(str, conn); DataTable dt = new(); dt.Rows.Clear();
            try { da.Fill(dt); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            
            if (dt.Rows.Count == 0) return;

            DataView dv = dt.DefaultView;

            float fc = 0.00F; float.TryParse(dv[0]["Flat_Charge"].ToString(), out fc); BaseFlatCharge   = fc;
            float rc = 0.00F; float.TryParse(dv[0]["Run_Charge"].ToString(),  out rc); BaseRunCharge    = rc;
            float pm = 0.00F; float.TryParse(dv[0]["Plate_Matl"].ToString(),  out pm); BasePlateCharge  = pm;
            float fm = 0.00F; float.TryParse(dv[0]["Finish_Matl"].ToString(), out fm); BaseFinishCharge = fm;
            float rm = 0.00F; float.TryParse(dv[0]["Press_Matl"].ToString(),  out rm); BasePressCharge  = rm;
            float cm = 0.00F; float.TryParse(dv[0]["Conv_Matl"].ToString(),   out cm); BaseConvCharge   = cm;

            // Base Labor Charges
            // The next three calculations assume that the values from the FEATURES table are minutes
            int i = 0;
            i = 0; int.TryParse(dv[0]["PRESS_SETUP"].ToString(), out i); BasePressSetup = i;
            i = 0; int.TryParse(dv[0]["COLLATOR_SETUP"].ToString(), out i); BaseCollatorSetup = i;
            i = 0; int.TryParse(dv[0]["BINDERY_SETUP"].ToString(), out i); BaseBinderySetup = i;

            i = 0; int.TryParse(dv[0]["PRESS_SLOWDOWN"].ToString(), out i); BasePressSlowdown = i;
            i = 0; int.TryParse(dv[0]["COLLATOR_SLOWDOWN"].ToString(), out i); BaseCollatorSlowdown = i;
            i = 0; int.TryParse(dv[0]["BINDERY_SLOWDOWN"].ToString(), out i); BaseBinderySlowdown = i;
        }

        private void LoadData()
        {
            FlatChargePct = 0.00F;
            RunChargePct = 0.00F;
            PlateChargePct = 0.00F;
            FinishChargePct = 0.00F;
            PressChargePct = 0.00F;
            ConvChargePct = 0.00F;

            string str = $"SELECT * FROM [ESTIMATING].[dbo].[Quote_Details] WHERE QUOTE_NUM = '{QuoteNum}' AND Category = 'COMBO'";
            SqlConnection conn = new(ConnectionString);
            SqlDataAdapter da = new(str, conn); DataTable dt = new(); dt.Rows.Clear();
            try   { da.Fill(dt); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            if    (dt.Rows.Count == 0) return;
            DataView dv = dt.DefaultView;

            Combo1 = int.Parse(dv[0]["Value1"].ToString());

            float fc = 0.00F; float.TryParse(dv[0]["FlatChargePct"].ToString(),    out fc); FlatChargePct   = fc;
            float rc = 0.00F; float.TryParse(dv[0]["RunChargePct"].ToString(),     out rc); RunChargePct    = rc;
            float bp = 0.00F; float.TryParse(dv[0]["PlateChargePct"].ToString(),   out bp); PlateChargePct  = bp;
            float bf = 0.00F; float.TryParse(dv[0]["FinishChargePct"].ToString(),  out bf); FinishChargePct = bf;
            float pr = 0.00F; float.TryParse(dv[0]["PressChargePct"].ToString(),   out pr); PressChargePct  = pr;
            float co = 0.00F; float.TryParse(dv[0]["ConvertChargePct"].ToString(), out co); ConvChargePct   = co;

            float tfc = 0.00F; float.TryParse(dv[0]["TotalFlatChg"].ToString(),    out tfc); Total          = tfc;

            //Load Labor adjustments
            int t;
            t = 0; int.TryParse(dt.Rows[0]["PRESS_ADDL_MIN"].ToString(), out t); LabPS  = t;
            t = 0; int.TryParse(dt.Rows[0]["PRESS_SLOW_PCT"].ToString(), out t); LabPSL = t;
            t = 0; int.TryParse(dt.Rows[0]["COLL_ADDL_MIN"].ToString(),  out t); LabCS  = t;
            t = 0; int.TryParse(dt.Rows[0]["COLL_SLOW_PCT"].ToString(),  out t); LabCSL = t;
            t = 0; int.TryParse(dt.Rows[0]["BIND_ADDL_MIN"].ToString(),  out t); LabBS  = t;
            t = 0; int.TryParse(dt.Rows[0]["BIND_SLOW_PCT"].ToString(),  out t); LabBSL = t;
        }

        private void CalcTotal()
        {
            CalculatedFlatCharge   = BaseFlatCharge   * (1 + FlatChargePct   / 100);
            CalculatedRunCharge    = BaseRunCharge    * (1 + RunChargePct    / 100);
            CalculatedPlateCharge  = BasePlateCharge  * (1 + PlateChargePct  / 100);
            CalculatedFinishCharge = BaseFinishCharge * (1 + FinishChargePct / 100);
            CalculatedPressCharge  = BasePressCharge  * (1 + PressChargePct  / 100);
            CalculatedConvCharge   = BaseConvCharge   * (1 + ConvChargePct   / 100);

            FlatTotal = CalculatedFlatCharge + CalculatedPlateCharge + CalculatedFinishCharge + CalculatedPressCharge + CalculatedConvCharge;

            CalculateLabor();

        }

        private void CalcLabor(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        {
            CalculateLabor();
        }

        private void CalculateLabor()
        {
            if ( Starting ) return;
            PressSetup = BasePressSetup + LabPS;
            CollatorSetup = BaseCollatorSetup + LabCS;
            BinderySetup = BaseBinderySetup + LabBS;
            SetupTotal = (int)PressSetup + CollatorSetup + BinderySetup;

            PressSlowdown = BasePressSlowdown + LabPSL;
            CollatorSlowdown = BaseCollatorSlowdown + LabCSL;
            BinderySlowdown = BaseBinderySlowdown + LabBSL;
            SlowdownTotal = PressSlowdown + CollatorSlowdown + BinderySlowdown;
        }

        private void DeleteQuoteRow(string cat)
        {
            string cmd = $"DELETE [ESTIMATING].[dbo].[Quote_Details] WHERE Quote_Num = '{QuoteNum}' AND Category = '{cat}'";
            conn = new SqlConnection(ConnectionString); SqlCommand scmd = new SqlCommand(cmd, conn);
            conn.Open();
            try     { scmd.ExecuteNonQuery(); }
            catch   (Exception ex) { MessageBox.Show(ex.Message); }
            finally { conn.Close(); }
        }

        private void PctChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        {
            CalcTotal(); 
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            DeleteQuoteRow("Combo");

            string cmd = "INSERT INTO [ESTIMATING].[dbo].[Quote_Details] "
                + " ( Quote_Num,      Category,          Sequence,"
                + "   Param1,         Value1,            FlatChargePct,      RunChargePct,       PlateChargePct,     FinishChargePct,     PressChargePct,     ConvertChargePct,"
                + "   TotalFlatChg,   PerThousandChg,    "
                + "   PRESS_ADDL_MIN, PRESS_SLOW_PCT,    COLL_ADDL_MIN,      COLL_SLOW_PCT,      BIND_ADDL_MIN,      BIND_SLOW_PCT,       SETUP_MINUTES,      SLOWDOWN_PERCENT, "
                + "   PressSetupMin,  PressSlowPct,      CollSetupMin,       CollSlowPct,        BindSetupMin,       BindSlowPct   ) "
                + " VALUES ( "
                + $" '{QuoteNum}',   'Combo',            9,"
                + $"  'Combo1',     '{Combo1}',        '{FlatChargePct}',  '{RunChargePct}',   '{PlateChargePct}', '{FinishChargePct}', '{PressChargePct}', '{ConvChargePct}',"
                + $" '{FlatTotal}', '{CalculatedRunCharge}',"
                + $"  {LabPS},       {LabPSL},          {LabCS},            {LabCSL} ,          {LabBS},            {LabBSL},            {SetupTotal},       {SlowdownTotal}, "
                + $"  {PressSetup},  {PressSlowdown},   {CollatorSetup},    {CollatorSlowdown}, {BinderySetup},     {BinderySlowdown} )";

            SqlCommand scmd = new();
            scmd.CommandText = cmd;
            SqlConnection conn = new(ConnectionString);
            conn.Open();
            scmd.Connection = conn;
            scmd.ExecuteNonQuery();
            conn.Close();

            cmd =  "UPDATE [ESTIMATING].[dbo].[Quote_Details]     "
                + $" SET PressSlowdown      = {PressSlowdown},    "
                + $"     ConvertingSlowdown = {CollatorSlowdown}, "
                + $"     FinishingSlowdown  = {BinderySlowdown},  "
                + $"     Press              = {PressSetup},       "
                + $"     Converting         = {CollatorSetup},    "
                + $"     Finishing          = {BinderySetup}      "
                + $" WHERE Quote_Num = '{QuoteNum}' "
                + "   AND CATEGORY  = 'Combo'";
            scmd.CommandText = cmd;
            conn.Open();
            try { scmd.ExecuteNonQuery(); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { conn.Close(); }

            this.Hide();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        { this.Hide(); }

        private void M1_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        {
            LoadFeature();  // use the selected value of 'Combo1' to retrieve base values
            CalcTotal();
        }

    }
}