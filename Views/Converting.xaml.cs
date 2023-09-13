using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace ProDocEstimate.Views
{
    public partial class Converting : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

        #region Properties

        // The following test data was added to the Features table:
        //UPDATE FEATURES SET
        //  PLATE_MATL      = '10.00',
        //  FINISH_MATL     = '20.00',
        //  PRESS_MATL      = '30.00',
        //  CONV_MATL       = '40.00'
        // WHERE CATEGORY   = 'CONVERTING'
        //   AND PRESS_SIZE = '11'

        // Added for LABOR COST testing:
        //UPDATE FEATURES
        //   SET
        //    PRESS_SETUP_TIME = 5,
        //    COLLATOR_SETUP = 10,
        //    BINDERY_SETUP = 15,
        //    PRESS_SLOWDOWN = 20,
        //    COLLATOR_SLOWDOWN = 25,
        //    BINDERY_SLOWDOWN = 30
        // WHERE CATEGORY = 'CONVERTING'
        //   AND PRESS_SIZE = '11'

        public string ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public SqlConnection? conn;
        public SqlDataAdapter? da;
        public DataTable? dt;
        public SqlCommand? scmd;

        private int    max;         public int    Max       { get { return max;       } set { max       = value; OnPropertyChanged(); } }
        private string pressSize;   public string PressSize { get { return pressSize; } set { pressSize = value; OnPropertyChanged(); } }
        private string quoteNum;    public string QuoteNum  { get { return quoteNum;  } set { quoteNum  = value; OnPropertyChanged(); } }    // Which of these two
        public  bool   Starting;

        private int crash; public int Crash { get { return crash; } set { crash = value; OnPropertyChanged(); } }
        private int black; public int Black { get { return black; } set { black = value; OnPropertyChanged(); } }
        private int snap2; public int Snap2 { get { return snap2; } set { snap2 = value; OnPropertyChanged(); } }
        private int snap5; public int Snap5 { get { return snap5; } set { snap5 = value; OnPropertyChanged(); } }
        private int cont2; public int Cont2 { get { return cont2; } set { cont2 = value; OnPropertyChanged(); } }
        private int cont5; public int Cont5 { get { return cont5; } set { cont5 = value; OnPropertyChanged(); } }
        private int fold;  public int Fold  { get { return fold;  } set { fold  = value; OnPropertyChanged(); } }
        private int tape;  public int Tape  { get { return tape;  } set { tape  = value; OnPropertyChanged(); } }

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

        private float pressSetup; public float PressSetup { get { return pressSetup; } set { pressSetup = value; OnPropertyChanged(); } }
        private int collatorSetup; public int CollatorSetup { get { return collatorSetup; } set { collatorSetup = value; OnPropertyChanged(); } }
        private int binderySetup; public int BinderySetup { get { return binderySetup; } set { binderySetup = value; OnPropertyChanged(); } }

        private int basePressSetup; public int BasePressSetup { get { return basePressSetup; } set { basePressSetup = value; OnPropertyChanged(); } }
        private int baseCollatorSetup; public int BaseCollatorSetup { get { return baseCollatorSetup; } set { baseCollatorSetup = value; OnPropertyChanged(); } }
        private int baseBinderySetup; public int BaseBinderySetup { get { return baseBinderySetup; } set { baseBinderySetup = value; OnPropertyChanged(); } }

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

        private int calculatedPS; public int CalculatedPS { get { return calculatedPS; } set { calculatedPS = value; OnPropertyChanged(); } }
        private int calculatedCS; public int CalculatedCS { get { return calculatedCS; } set { calculatedCS = value; OnPropertyChanged(); } }
        private int calculatedBS; public int CalculatedBS { get { return calculatedBS; } set { calculatedBS = value; OnPropertyChanged(); } }

        private int slowdownTotal; public int SlowdownTotal { get { return slowdownTotal; } set { slowdownTotal = value; OnPropertyChanged(); } }
        private int setupTotal; public int SetupTotal { get { return setupTotal; } set { setupTotal = value; OnPropertyChanged(); } }

        #endregion

        public Converting(string PRESSSIZE, string QUOTENUM)
        {
            InitializeComponent();
            this.DataContext = this;

            Title = "Quote #: " + QUOTENUM;
            QuoteNum = QUOTENUM;
            PressSize = PRESSSIZE;

            Starting = true;

            LoadMaxima();
            LoadQuote();
            LoadBaseValues();

            Starting = false;

            PreviewKeyDown += (s, e) => { if (e.Key == Key.Escape) Close(); };
        }

        public void OnLoad(object sender, RoutedEventArgs e)
        {
            this.Height = this.Height *= 1.8;
            this.Width = this.Width *= 1.8;
            Top = 150;
        }

        private void LoadMaxima()
        {
            // This sets the maximum value for each of the F_TYPE NumericUpDowns (1 for most of them)
            string str =  "SELECT F_TYPE, MAX(Number) AS Max FROM [ESTIMATING].[dbo].[FEATURES] " 
                       + $" WHERE Category = 'Converting' AND Press_Size = '{PressSize}' GROUP BY F_TYPE";

            SqlConnection conn = new(ConnectionString);
            SqlDataAdapter da = new(str, conn); DataTable dt = new(); dt.Rows.Clear(); da.Fill(dt);
            if (dt.Rows.Count == 0) return;

            DataView dv  = dt.DefaultView;
            dv.RowFilter = "F_TYPE='2 CRASH NUMB'";   M1.Maximum = int.Parse(dv[0]["Max"].ToString());
            dv.RowFilter = "F_TYPE='BLACK NUMB'";     M2.Maximum = int.Parse(dv[0]["Max"].ToString());
            dv.RowFilter = "F_TYPE='SNAP GLUE 2-4'";  M3.Maximum = int.Parse(dv[0]["Max"].ToString());
            dv.RowFilter = "F_TYPE='SNAP GLUE 5-12'"; M4.Maximum = int.Parse(dv[0]["Max"].ToString());
            dv.RowFilter = "F_TYPE='FOLDING'";        M5.Maximum = int.Parse(dv[0]["Max"].ToString());
            dv.RowFilter = "F_TYPE='CONT GLUE 2-4'";  M6.Maximum = int.Parse(dv[0]["Max"].ToString());
            dv.RowFilter = "F_TYPE='CONT GLUE 5-12'"; M7.Maximum = int.Parse(dv[0]["Max"].ToString());
            dv.RowFilter = "F_TYPE='TRANSFER TAPE'";  M8.Maximum = int.Parse(dv[0]["Max"].ToString());
            dt.Clear();

            return;
        }

        private void LoadQuote()
        {
            string str = $"SELECT * FROM [ESTIMATING].[dbo].[Quote_Details] WHERE QUOTE_NUM = '{QuoteNum}' AND Category = 'Converting'";
            SqlConnection conn = new(ConnectionString);
            SqlDataAdapter da = new(str, conn); DataTable dt = new(); dt.Rows.Clear(); da.Fill(dt);

            if (dt.Rows.Count == 0) return;

            DataView dv = new DataView(dt);

            Crash = int.Parse(dv[0]["Value1"].ToString());
            Black = int.Parse(dv[0]["Value2"].ToString());
            Snap2 = int.Parse(dv[0]["Value3"].ToString());
            Snap5 = int.Parse(dv[0]["Value4"].ToString());
            Cont2 = int.Parse(dv[0]["Value5"].ToString());
            Cont5 = int.Parse(dv[0]["Value6"].ToString());
            Fold  = int.Parse(dv[0]["Value7"].ToString());
            Tape  = int.Parse(dv[0]["Value8"].ToString());

            FlatTotal = float.Parse(dv[0]["TotalFlatChg"].ToString());
            CalculatedRunCharge = float.Parse(dv[0]["PerThousandChg"].ToString());

            int t1 = 0; int.TryParse(dv[0]["FlatChargePct"].ToString(), out t1); FlatChargePct = t1;
            int t2 = 0; int.TryParse(dv[0]["RunChargePct"].ToString(), out t2); RunChargePct = t2;
            int t3 = 0; int.TryParse(dv[0]["FinishChargePct"].ToString(), out t3); FinishChargePct = t3;
            int t4 = 0; int.TryParse(dv[0]["PressChargePct"].ToString(), out t4); PressChargePct = t4;
            int t5 = 0; int.TryParse(dv[0]["ConvertChargePct"].ToString(), out t5); ConvChargePct = t5;
            int t6 = 0; int.TryParse(dv[0]["PlateChargePct"].ToString(), out t6); PlateChargePct = t6;

            LabPS  = int.Parse(dv[0]["PRESS_ADDL_MIN"].ToString());
            LabPSL = int.Parse(dv[0]["PRESS_SLOW_PCT"].ToString());
            LabCS  = int.Parse(dv[0]["COLL_ADDL_MIN"].ToString());
            LabCSL = int.Parse(dv[0]["COLL_SLOW_PCT"].ToString());
            LabBS  = int.Parse(dv[0]["BIND_ADDL_MIN"].ToString());
            LabBSL = int.Parse(dv[0]["BIND_SLOW_PCT"].ToString());
        }

        private void LoadBaseValues()
        {
            // Material base costs:
            BaseFlatCharge = 0.00F;
            BaseRunCharge = 0.00F;
            BasePlateCharge = 0.00F;
            BaseFinishCharge = 0.00F;
            BasePressCharge = 0.00F;
            BaseConvCharge = 0.00F;

            // Labor base minutes:
            BasePressSetup = 0;
            BaseCollatorSetup = 0;
            BaseBinderySetup = 0;
            BasePressSlowdown = 0;
            BaseCollatorSlowdown = 0;
            BaseBinderySlowdown = 0;

            string str = "SELECT * FROM[ESTIMATING].[dbo].[FEATURES]"
                + $" WHERE CATEGORY = 'CONVERTING' AND PRESS_SIZE = '11'"
                + $"   AND ((F_TYPE = '2 CRASH NUMB'   AND NUMBER = {Crash})"
                + $"    OR  (F_TYPE = 'BLACK NUMB'     AND NUMBER = {Black})"
                + $"    OR  (F_TYPE = 'SNAP GLUE 2-4'  AND NUMBER = {Snap2})"
                + $"    OR  (F_TYPE = 'SNAP GLUE 5-12' AND NUMBER = {Snap5})"
                + $"    OR  (F_TYPE = 'CONT GLUE 2-4'  AND NUMBER = {Cont2})"
                + $"    OR  (F_TYPE = 'CONT GLUE 5-12' AND NUMBER = {Cont5})"
                + $"    OR  (F_TYPE = 'FOLDING'        AND NUMBER = {Fold} )"
                + $"    OR  (F_TYPE = 'TRANSFER TAPE'  AND NUMBER = {Tape}))";

            conn = new SqlConnection(ConnectionString); 
            da = new SqlDataAdapter(str, conn);
            DataTable dt3 = new(); dt3.Rows.Clear(); da.Fill(dt3);
            if (dt3.Rows.Count == 0) return;
            DataView dv3 = dt3.DefaultView;

            float t = 0.00F;
            // Add costs for all rows of each f_type that currently have a value greater than zero
            for (int i = 0; i < dv3.Count; i++)
            {   t = 0.00F; float.TryParse(dv3[i]["FLAT_CHARGE"].ToString(), out t); BaseFlatCharge   += t;
                t = 0.00F; float.TryParse(dv3[i]["RUN_CHARGE"].ToString(),  out t); BaseRunCharge    += t;
                t = 0.00F; float.TryParse(dv3[i]["PLATE_MATL"].ToString(),  out t); BasePlateCharge  += t;
                t = 0.00F; float.TryParse(dv3[i]["FINISH_MATL"].ToString(), out t); BaseFinishCharge += t;
                t = 0.00F; float.TryParse(dv3[i]["PRESS_MATL"].ToString(),  out t); BasePressCharge  += t;
                t = 0.00F; float.TryParse(dv3[i]["CONV_MATL"].ToString(),   out t); BaseConvCharge   += t;

            // Load labor costs
                BasePressSetup       += int.Parse(dv3[i]["PRESS_SETUP_TIME"].ToString());
                BaseCollatorSetup    += int.Parse(dv3[i]["COLLATOR_SETUP"].ToString());
                BaseBinderySetup     += int.Parse(dv3[i]["BINDERY_SETUP"].ToString());
                BasePressSlowdown    += int.Parse(dv3[i]["PRESS_SLOWDOWN"].ToString());
                BaseCollatorSlowdown += int.Parse(dv3[i]["COLLATOR_SLOWDOWN"].ToString());
                BaseBinderySlowdown  += int.Parse(dv3[i]["BINDERY_SLOWDOWN"].ToString());
            }

            CalcTotals();
        }

        private void CalcTotals()
        {  CalculatedFlatCharge    = BaseFlatCharge    * ( 1 + FlatChargePct   / 100);
           CalculatedRunCharge     = BaseRunCharge     * ( 1 + RunChargePct    / 100);
           CalculatedPlateCharge   = BasePlateCharge   * ( 1 + PlateChargePct  / 100);
           CalculatedFinishCharge  = BaseFinishCharge  * ( 1 + FinishChargePct / 100);
           CalculatedPressCharge   = BasePressCharge   * ( 1 + PressChargePct  / 100); 
           CalculatedConvCharge    = BaseConvCharge    * ( 1 + ConvChargePct   / 100);
           FlatTotal = CalculatedFlatCharge + CalculatedPlateCharge + CalculatedFinishCharge + CalculatedPressCharge + CalculatedConvCharge;
        }

        private void CalcLabor(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        {
            CalculateLabor();
        }

        private void CalculateLabor()
        {
            if (Starting) return;
            PressSetup = BasePressSetup + LabPS;
            CollatorSetup = BaseCollatorSetup + LabCS;
            BinderySetup = BaseBinderySetup + LabBS;
            SetupTotal = (int)PressSetup + CollatorSetup + BinderySetup;

            PressSlowdown = BasePressSlowdown + LabPSL;
            CollatorSlowdown = BaseCollatorSlowdown + LabCSL;
            BinderySlowdown = BaseBinderySlowdown + LabBSL;
            SlowdownTotal = PressSlowdown + CollatorSlowdown + BinderySlowdown;

        }

        private void Pct_Changed(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        { CalcTotals(); }

        private void ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        { LoadBaseValues(); CalcTotals(); CalculateLabor(); }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string cmd = "DELETE [ESTIMATING].[dbo].[Quote_Details] WHERE Quote_Num = '" + QuoteNum + "' AND Category = 'Converting'";
            conn = new SqlConnection(ConnectionString); SqlCommand scmd = new SqlCommand(cmd, conn); conn.Open();

            try { scmd.ExecuteNonQuery(); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { conn.Close(); }

            cmd = "INSERT INTO [ESTIMATING].[dbo].[Quote_Details] ("
                + "    Quote_Num,       Category,                Sequence,"
                + "    Param1,          Param2,                  Param3,             Param4,            Param5,               Param6,             Param7,            Param8, "
                + "    Value1,          Value2,                  Value3,             Value4,            Value5,               Value6,             Value7,            Value8, "
                + "    TotalFlatChg,    PerThousandChg,          FlatChargePct,      RunChargePct,      FinishChargePct,      PressChargePct,     ConvertChargePct,  PlateChargePct,"
                + "    PRESS_ADDL_MIN,  COLL_ADDL_MIN,           BIND_ADDL_MIN,      PRESS_SLOW_PCT,    COLL_SLOW_PCT,        BIND_SLOW_PCT, "
                + "    PressSetupMin,   PressSlowPct,            CollSetupMin,       CollSlowPct,       BindSetupMin,         BindSlowPct   ) "
                + " VALUES ( "
                + $" '{QuoteNum}',     'Converting',             10, "
                + "  '2 CRASH NUMB',   'BLACK NUMB',            'SNAP GLUE 2-4',    'SNAP GLUE 5-12',  'CONT GLUE 2-4',      'CONT GLUE 5-12',   'FOLDING',          'TRANSFER TAPE', "
                + $" '{Crash}',        '{Black}',               '{Snap2}',          '{Snap5}',         '{Cont2}',            '{Cont5}',          '{Fold}',           '{Tape}', "
                + $" '{FlatTotal}',    '{CalculatedRunCharge}', '{FlatChargePct}',  '{RunChargePct}',  '{FinishChargePct}',  '{PressChargePct}', '{ConvChargePct}',  '{PlateChargePct}', "
                + $"  {LabPS},          {LabCS},                 {LabBS},            {LabPSL},          {LabCSL},             {LabBSL}, "
                + $"  {PressSetup},     {PressSlowdown},         {CollatorSetup},    {CollatorSlowdown},{BinderySetup},       {BinderySlowdown} )";

            //Clipboard.SetText(cmd);
            //Debugger.Break();

            scmd.CommandText = cmd;
            conn.Open();
            try { scmd.ExecuteNonQuery(); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { conn.Close(); }

            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        { Close(); }

    }
}