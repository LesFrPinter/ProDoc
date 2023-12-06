using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace ProDocEstimate.Views
{
    public partial class Perfing : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

        #region Properties

        //DATA ADDED FOR TESTING (REMOVE IN PRODUCTION)
        //UPDATE FEATURES
        //   SET
        //     PRESS_SETUP_TIME = '10',
        //     COLLATOR_SETUP = '20',
        //     BINDERY_SETUP = '30',
        //     PRESS_SLOWDOWN = '5',
        //     COLLATOR_SLOWDOWN = '15',
        //     BINDERY_SLOWDOWN = '25'
        // WHERE CATEGORY = 'Perfing'
        //   AND PRESS_SIZE = '11'

        public string ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public SqlConnection? conn;
        public SqlDataAdapter? da;
        public DataTable? dt;
        public SqlCommand? scmd;

        private int    maxPerfs;  public int    MaxPerfs  { get { return maxPerfs;  } set { maxPerfs  = value; OnPropertyChanged(); } }
        private string pressSize; public string PressSize { get { return pressSize; } set { pressSize = value; OnPropertyChanged(); } }
        private string quoteNum;  public string QuoteNum  { get { return quoteNum;  } set { quoteNum  = value; OnPropertyChanged(); } }
        private float  flatTotal; public float  FlatTotal { get { return flatTotal; } set { flatTotal = value; OnPropertyChanged(); } }

        private int   partCross;  public int PartCross { get { return partCross;  } set { partCross   = value; OnPropertyChanged(); } }
        private int   cross;      public int Cross     { get { return cross;      } set { cross       = value; OnPropertyChanged(); } }
        private int   partRun;    public int PartRun   { get { return partRun;    } set { partRun     = value; OnPropertyChanged(); } }
        private int   running;    public int Running   { get { return running;    } set { running     = value; OnPropertyChanged(); } }

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

        private int calculatedPS; public int CalculatedPS { get { return calculatedPS; } set { calculatedPS = value; OnPropertyChanged(); } }
        private int calculatedCS; public int CalculatedCS { get { return calculatedCS; } set { calculatedCS = value; OnPropertyChanged(); } }
        private int calculatedBS; public int CalculatedBS { get { return calculatedBS; } set { calculatedBS = value; OnPropertyChanged(); } }

        private int slowdownTotal; public int SlowdownTotal { get { return slowdownTotal; } set { slowdownTotal = value; OnPropertyChanged(); } }
        private int setupTotal; public int SetupTotal { get { return setupTotal; } set { setupTotal = value; OnPropertyChanged(); } }

        private string FieldList = "F_TYPE, NUMBER, FLAT_CHARGE, RUN_CHARGE, PLATE_MATL, FINISH_MATL, PRESS_MATL, CONV_MATL, "
                                 + " PRESS_SETUP_TIME, COLLATOR_SETUP, BINDERY_SETUP, PRESS_SLOWDOWN, COLLATOR_SLOWDOWN, BINDERY_SLOWDOWN";

        #endregion

        public Perfing(string PRESSSIZE, string QUOTENUM)
        {
            // Test data added by me:
            //

            InitializeComponent();
            this.DataContext = this;

            PressSize = PRESSSIZE;
            QuoteNum  = QUOTENUM;

            GetMaxValues();
            GetSavedValues();
            GetCharges();

            PreviewKeyDown += (s, e) => { if (e.Key == Key.Escape) Close(); };
        }

        public void OnLoad(object sender, RoutedEventArgs e)
        {
            this.Height = this.Height *= (1.0F + MainWindow.FeatureZoom);
            this.Width = this.Width *= (1.0F + MainWindow.FeatureZoom);
            Top = 50;
        }

        private void GetMaxValues()
        {
            string cmd = $"SELECT F_TYPE, MAX(Number) AS MaxPerfs FROM [ESTIMATING].[dbo].[FEATURES] WHERE CATEGORY = 'Perfing' AND PRESS_SIZE = '{PressSize}' GROUP BY F_TYPE";
            conn = new SqlConnection(ConnectionString); da = new SqlDataAdapter(cmd, conn); dt = new DataTable(); da.Fill(dt);

            //Use E-F:
            DataView dv = new DataView(dt);
            dv.RowFilter = "F_TYPE='PART CROSS'";   PC1.Maximum = int.Parse(dv[0]["MaxPerfs"].ToString());
            dv.RowFilter = "F_TYPE='CROSS'";        C1.Maximum  = int.Parse(dv[0]["MaxPerfs"].ToString());
            dv.RowFilter = "F_TYPE='PART RUN'";     PR1.Maximum = int.Parse(dv[0]["MaxPerfs"].ToString());
            dv.RowFilter = "F_TYPE='RUNNING'";      R1.Maximum  = int.Parse(dv[0]["MaxPerfs"].ToString());
        }

        private void GetSavedValues()
        {
            FlatChargePct = 0;
            RunChargePct = 0;
            PlateChargePct = 0;
            FinishChargePct = 0;
            PressChargePct = 0;
            ConvChargePct =0;

            string str = $"SELECT * FROM [ESTIMATING].[dbo].[Quote_Details] WHERE QUOTE_NUM = '{QuoteNum}' AND CATEGORY = 'Perfing'";
            da.SelectCommand.CommandText = str; dt.Rows.Clear(); da.Fill(dt);
            if (dt.Rows.Count > 0) {

                int t1, t2, t3, t4, t5, t6 = 0;
                int l1, l2, l3, l4, l5, l6 = 0;

                PartCross = int.Parse(dt.Rows[0]["Value1"].ToString());
                Cross     = int.Parse(dt.Rows[0]["Value2"].ToString());
                PartRun   = int.Parse(dt.Rows[0]["Value3"].ToString());
                Running   = int.Parse(dt.Rows[0]["Value4"].ToString());

                t1 = 0; int.TryParse(dt.Rows[0]["FlatChargePct"].ToString(), out t1); FlatChargePct = t1;
                t2 = 0; int.TryParse(dt.Rows[0]["RunChargePct"].ToString(),  out t2); RunChargePct = t2;
                t3 = 0; int.TryParse(dt.Rows[0]["PlateChargePct"].ToString(), out t3); PlateChargePct = t3; 
                t4 = 0; int.TryParse(dt.Rows[0]["FinishChargePct"].ToString(), out t4); FinishChargePct = t4;
                t5 = 0; int.TryParse(dt.Rows[0]["PressChargePct"].ToString(), out t5); PressChargePct = t5;
                t6 = 0; int.TryParse(dt.Rows[0]["ConvertChargePct"].ToString(), out t6); ConvChargePct = t6;

                l1 = 0; int.TryParse(dt.Rows[0]["PRESS_ADDL_MIN"].ToString(), out l1); LabPS  += l1;
                l2 = 0; int.TryParse(dt.Rows[0]["COLL_ADDL_MIN"].ToString(), out l2);  LabCS  += l2;
                l3 = 0; int.TryParse(dt.Rows[0]["BIND_ADDL_MIN"].ToString(), out l3);  LabBS  += l3;
                l4 = 0; int.TryParse(dt.Rows[0]["PRESS_SLOW_PCT"].ToString(), out l4); LabPSL += l4;
                l5 = 0; int.TryParse(dt.Rows[0]["COLL_SLOW_PCT"].ToString(), out l5);  LabCSL += l5;
                l6 = 0; int.TryParse(dt.Rows[0]["BIND_SLOW_PCT"].ToString(), out l6);  LabBSL += l6;
            }

            CalcTotal();
        }

        private void GetCharges()
        {
            string cmd = $"SELECT {FieldList}"
                       + $"  FROM [ESTIMATING].[dbo].[FEATURES]"
                       + $" WHERE CATEGORY = 'PERFING'    AND PRESS_SIZE = '{PressSize}'"
                       + $"   AND ((F_TYPE = 'CROSS'      AND Number = {Cross}     )"
                       + $"    OR  (F_TYPE = 'PART CROSS' AND Number = {PartCross} )"
                       + $"    OR  (F_TYPE = 'PART RUN'   AND Number = {PartRun}   )"
                       + $"    OR  (F_TYPE = 'RUNNING'    AND Number = {Running}   ))"
                       + "    ORDER BY F_TYPE, NUMBER";

            conn = new SqlConnection(ConnectionString);
            da = new SqlDataAdapter(cmd, conn); dt = new DataTable(); da.Fill(dt);
            DataView dv = dt.DefaultView;

            BaseFlatCharge = 0;
            BaseRunCharge = 0;
            BaseFinishCharge = 0;
            BaseConvCharge = 0;
            BasePlateCharge = 0;
            BasePressCharge = 0;

            BasePressSetup = 0;
            BaseCollatorSetup = 0;
            BaseBinderySetup = 0;
            BasePressSlowdown = 0;
            BaseCollatorSlowdown = 0;
            BaseBinderySlowdown = 0;

            FlatTotal = 0;

            float t1, t2, t3, t4, t5, t6 = 0.00F;
            int l1, l2, l3, l4, l5, l6 = 0;

            for (int i = 0; i < dv.Count; i++)
            {
                t1 = 0.00F; float.TryParse(dv[i]["FLAT_CHARGE"].ToString(), out t1); BaseFlatCharge       += t1;
                t2 = 0.00F; float.TryParse(dv[i]["RUN_CHARGE"].ToString(),  out t2); BaseRunCharge        += t2;
                t3 = 0.00F; float.TryParse(dv[i]["FINISH_MATL"].ToString(), out t3); BaseFinishCharge     += t3;
                t4 = 0.00F; float.TryParse(dv[i]["CONV_MATL"].ToString(),   out t4); BaseConvCharge       += t4;
                t5 = 0.00F; float.TryParse(dv[i]["PLATE_MATL"].ToString(),  out t5); BasePlateCharge      += t5;
                t6 = 0.00F; float.TryParse(dv[i]["PRESS_MATL"].ToString(),  out t6); BasePressCharge      += t6;

                l1 = 0; int.TryParse(dv[i]["PRESS_SETUP_TIME"] .ToString(), out l1); BasePressSetup       += l1;
                l2 = 0; int.TryParse(dv[i]["COLLATOR_SETUP"]   .ToString(), out l2); BaseCollatorSetup    += l2;
                l3 = 0; int.TryParse(dv[i]["BINDERY_SETUP"]    .ToString(), out l3); BaseBinderySetup     += l3;
                l4 = 0; int.TryParse(dv[i]["PRESS_SLOWDOWN"]   .ToString(), out l4); BasePressSlowdown    += l4;
                l5 = 0; int.TryParse(dv[i]["COLLATOR_SLOWDOWN"].ToString(), out l5); BaseCollatorSlowdown += l5;
                l6 = 0; int.TryParse(dv[i]["BINDERY_SLOWDOWN"] .ToString(), out l6); BaseBinderySlowdown  += l6;
            }

            CalcTotal();
        }

        private void CalcTotal()
        {
            CalculatedFlatCharge = BaseFlatCharge * (1 + FlatChargePct / 100);
            CalculatedRunCharge = BaseRunCharge * (1 + RunChargePct / 100);
            CalculatedPlateCharge = BasePlateCharge * (1 + PlateChargePct / 100);
            CalculatedFinishCharge = BaseFinishCharge * (1 + FinishChargePct / 100);
            CalculatedPressCharge = BasePressCharge * (1 + PressChargePct / 100);
            CalculatedConvCharge = BaseConvCharge * (1 + ConvChargePct / 100);

            FlatTotal = CalculatedFlatCharge + CalculatedPlateCharge + CalculatedFinishCharge + CalculatedPressCharge + CalculatedConvCharge;

            //PressSetup = BasePressSetup + LabPS;
            //BinderySetup = BaseBinderySetup + LabBS;
            //CollatorSetup = BaseCollatorSetup + LabCS;
            //PressSlowdown = BasePressSlowdown + LabPSL;
            //CollatorSlowdown = BaseBinderySlowdown + LabCSL;
            //BinderySlowdown = BaseBinderySlowdown + LabBSL;

            //SetupTotal = LabCS + LabBS + LabCS;
            //SlowdownTotal = LabPSL + LabBSL + LabCSL;

            CalculateLabor();

        }

        private void CalcLabor(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        {
            CalculateLabor();
        }

        private void CalculateLabor()
        {
            PressSetup       = BasePressSetup       + LabPS;
            CollatorSetup    = BaseCollatorSetup    + LabCS;
            BinderySetup     = BaseBinderySetup     + LabBS;
            SetupTotal       = (int)PressSetup      + CollatorSetup    + BinderySetup;

            PressSlowdown    = BasePressSlowdown    + LabPSL;
            CollatorSlowdown = BaseCollatorSlowdown + LabCSL;
            BinderySlowdown  = BaseBinderySlowdown  + LabBSL;
            SlowdownTotal    = PressSlowdown        + CollatorSlowdown + BinderySlowdown;
        }

        private void PctChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        { CalcTotal(); }

        private void ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        { GetCharges(); }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string cmd = "DELETE [ESTIMATING].[dbo].[Quote_Details] WHERE Quote_Num = '" + QuoteNum + "' AND Category = 'Perfing'";
            conn = new SqlConnection(ConnectionString); SqlCommand scmd = new SqlCommand(cmd, conn); conn.Open();

            try { scmd.ExecuteNonQuery(); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { conn.Close(); }

            cmd = "INSERT INTO [ESTIMATING].[dbo].[Quote_Details] ("
                + "   Quote_Num,      Category,          Sequence,"
                + "   Param1,         Param2,            Param3,          Param4,             Value1,              Value2,             Value3,            Value4,"
                + "   FlatCharge,     FlatChargePct,     RunChargePct,    PlateChargePct,     FinishChargePct,     PressChargePct,     ConvertChargePct,  TotalFlatChg,  PerThousandChg,"
                + "   PRESS_ADDL_MIN, COLL_ADDL_MIN,     BIND_ADDL_MIN,   PRESS_SLOW_PCT,     COLL_SLOW_PCT,       BIND_SLOW_PCT,  "
                + "   PressSetupMin,  PressSlowPct,      CollSetupMin,    CollSlowPct,        BindSetupMin,        BindSlowPct   ) " 
                + " VALUES ( "
                + $"'{QuoteNum}',    'Perfing',          3,"
                + $" 'Part Cross',   'Cross',           'Part Run',      'Running',         '{PartCross}',       '{Cross}',          '{PartRun}',       '{Running}', "
                + $"'{FlatCharge}', '{FlatChargePct}', '{RunChargePct}','{PlateChargePct}', '{FinishChargePct}', '{PressChargePct}', '{ConvChargePct}', '{FlatTotal}', '{CalculatedRunCharge}',"
                + $" {LabPS},        {LabCS},           {LabBS},         {LabPSL},           {LabCSL},            {LabBSL}, "
                + $" {PressSetup},  {PressSlowdown},   {CollatorSetup}, {CollatorSlowdown}, {BinderySetup},      {BinderySlowdown} )";

            scmd.CommandText = cmd;
            conn.Open();
            try { scmd.ExecuteNonQuery(); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { conn.Close(); }

            cmd =  "UPDATE [ESTIMATING].[dbo].[Quote_Details]     "
                + $" SET PressSlowdown      = {PressSlowdown},    "
                + $"     ConvertingSlowdown = {CollatorSlowdown}, "
                + $"     FinishingSlowdown  = {BinderySlowdown},  "
                + $"     Press              = {PressSetup},       "
                + $"     Converting         = {CollatorSetup},    "
                + $"     Finishing          = {BinderySetup}      "
                + $" WHERE Quote_Num = '{QuoteNum}'               "
                + "   AND CATEGORY   = 'Perfing'";
            scmd.CommandText = cmd;
            conn.Open();
            try { scmd.ExecuteNonQuery(); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { conn.Close(); }

            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        { this.Close(); }

    }
}