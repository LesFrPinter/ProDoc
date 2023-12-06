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
    public partial class Security : Window, INotifyPropertyChanged
    {
        #region Property definitions

        // 07/14/2023 LFP Changed Quote_Details to provide storage space for up to 21 F_TYPES:
        //ALTER TABLE[ESTIMATING].[dbo].[Quote_Details] ALTER COLUMN Value9 VarChar(100)   // '1001' means 'F_TYPES 1 and 4 are checked'
        //ALTER TABLE[ESTIMATING].[dbo].[Quote_Details] ALTER COLUMN Value10 VarChar(100)  // '30,,,20' assigns 30% to the first ftype and 20% to the fourth;

        // Test data added to FEATURES table:
        //UPDATE FEATURES
        //   SET PLATE_MATL = '10.00',
        //       FINISH_MATL = '20.00',
        //       PRESS_MATL = '30.00',
        //       CONV_MATL = '40.00'
        // WHERE CATEGORY = 'SECURITY'
        //   AND PRESS_SIZE = '11'

        //UPDATE FEATURES
        //   SET RUN_CHARGE = '25.00'
        // WHERE CATEGORY = 'SECURITY'
        //   AND PRESS_SIZE = '11'
        //   AND F_TYPE = 'NaNOcopy Void'

        //UPDATE FEATURES
        // SET PRESS_SETUP_TIME = 10,
        //     COLLATOR_SETUP = 20,
        //     BINDERY_SETUP = 30,
        //     PRESS_SLOWDOWN = 5,
        //     COLLATOR_SLOWDOWN = 15,
        //     BINDERY_SLOWDOWN = 25
        // WHERE Category = 'Security'
        //   AND Press_size = '11'

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

        public string ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public SqlConnection? conn;
        public SqlDataAdapter? da;
        public DataTable? dt;
        public DataView? dv;
        public SqlCommand? scmd;

        private string fieldList; public string FieldList { get { return fieldList; } set { fieldList = value; OnPropertyChanged(); } }
        private int num_F_Types = 21; public int Num_F_Types { get { return num_F_Types; } set { num_F_Types = value; OnPropertyChanged(); } }
        private bool startup = true; public bool Startup { get { return startup; } set { startup = value; OnPropertyChanged(); } }
        private string checker; public string Checker { get { return checker; } set { checker = value; OnPropertyChanged(); } }
        private string quoteNum; public string QuoteNum { get { return quoteNum; } set { quoteNum = value; OnPropertyChanged(); } }
        private string pressSize; public string PressSize { get { return pressSize; } set { pressSize = value; OnPropertyChanged(); } }
        private float flatTotal; public float FlatTotal { get { return flatTotal; } set { flatTotal = value; OnPropertyChanged(); } }

        private bool[] chk; public bool[] Chk { get { return chk; } set { chk = value; OnPropertyChanged(); } }
        private string[] ftype; public string[] Ftype { get { return ftype; } set { ftype = value; OnPropertyChanged(); } }
        private string[] flat; public string[] Flat { get { return flat; } set { flat = value; OnPropertyChanged(); } }
        private string[] run; public string[] Run { get { return run; } set { run = value; OnPropertyChanged(); } }

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

        #endregion

        public Security(string PRESSSIZE, string QUOTENUM)
        {
            InitializeComponent();

            this.DataContext = this;

            Title = QUOTENUM;
            QuoteNum = QUOTENUM.ToString();
            PressSize = PRESSSIZE.ToString();

            Chk = new bool[Num_F_Types];
            Ftype = new string[Num_F_Types];
            Flat = new string[Num_F_Types];
            Run = new string[Num_F_Types];

            for (int j = 0; j < Num_F_Types; j++)       // Initialize all of the arrays.
            {
                Chk[j] = false;
                Ftype[j] = "";
                Flat[j] = "";
                Run[j] = "";
            }

            FieldList = "F_TYPE, FLAT_CHARGE, RUN_CHARGE, PLATE_MATL, FINISH_MATL, CONV_MATL, PRESS_MATL";

            LoadFtypes();
            LoadData();
            GetCharges();

            Startup = false;

            PreviewKeyDown += (s, e) => { if (e.Key == Key.Escape) Close(); };  // Esc to close window
        }

        public void OnLoad(object sender, RoutedEventArgs e)
        {
            Startup = false;
            this.Height = this.Height *= (1.0F + MainWindow.FeatureZoom);
            this.Width = this.Width *= (1.0F + MainWindow.FeatureZoom);
            Top = 50;
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
            {
                Ftype[i] = dv[i]["F_TYPE"].ToString();
                Flat[i] = dv[i]["FLAT_CHARGE"].ToString();
                Run[i] = dv[i]["RUN_CHARGE"].ToString();
            }
        }

        public void LoadData()
        {
            string cmd = $"SELECT * FROM [ESTIMATING].[dbo].[Quote_Details] WHERE QUOTE_NUM = '{QuoteNum}' AND CATEGORY = 'Security'";
            conn = new SqlConnection(ConnectionString); da = new SqlDataAdapter(cmd, conn);
            DataTable dt2 = new DataTable(); da.Fill(dt2); dv = dt2.DefaultView;

            if (dt2.Rows.Count == 0) return;

            // Note that Value10 (the string containing 1 for each f_type that was checked) should be varchar(30), or at least 21 characters long
            Checker = dv[0]["Value10"].ToString();
            for (int i = 0; i < Checker.Length; i++)
            { string v = Checker.Substring(i, 1); Chk[i] = (v == "1") ? true : false; }

            BaseFlatCharge      = float.Parse(dv[0]["FlatCharge"].ToString());
            FlatTotal           = float.Parse(dv[0]["TotalFlatChg"].ToString());
            CalculatedRunCharge = float.Parse(dv[0]["PerThousandChg"].ToString());
            FlatChargePct       = float.Parse(dv[0]["FlatChargePct"].ToString());
            RunChargePct        = float.Parse(dv[0]["RunChargePct"].ToString());
            FinishChargePct     = float.Parse(dv[0]["FinishChargePct"].ToString());
            PressChargePct      = float.Parse(dv[0]["PressChargePct"].ToString());
            PlateChargePct      = float.Parse(dv[0]["PlateChargePct"].ToString());
            ConvChargePct       = float.Parse(dv[0]["ConvertChargePct"].ToString());

            // +$"  {LabPS},        {LabCS},        {LabBS},                 {LabPSL},          {LabCSL},         {LabBSL} )";

            LabPS  = int.Parse(dv[0]["PRESS_ADDL_MIN"].ToString());
            LabPSL = int.Parse(dv[0]["PRESS_SLOW_PCT"].ToString());
            LabCS  = int.Parse(dv[0]["COLL_ADDL_MIN"].ToString());
            LabCSL = int.Parse(dv[0]["COLL_SLOW_PCT"].ToString());
            LabBS  = int.Parse(dv[0]["BIND_ADDL_MIN"].ToString());
            LabBSL = int.Parse(dv[0]["BIND_SLOW_PCT"].ToString());

            GetCharges();
        }

        private void GetCharges()
        {
            //NOTE: This could be simplified by generating the SELECT statement to only refer to checked F_TYPEs...
            string[] c = { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
            for (int i = 0; i <= 20; i++) { c[i] = Chk[i] ? "1=1" : "1=0"; }
            string cmd = "SELECT * FROM [ESTIMATING].[dbo].[FEATURES]"
                        + $" WHERE CATEGORY = 'SECURITY' AND PRESS_SIZE = '{PressSize}'"
                        + $"   AND ((F_TYPE = 'Alter  Safe Continuous'    AND {c[0]} )"  // If "  Safe" is changed to " Safe" in the table, change this
                        + $"    OR  (F_TYPE = 'Alter  Safe Cut Sheet'     AND {c[1]} )"
                        + $"    OR  (F_TYPE = 'Embossing Continuous'      AND {c[2]} )"
                        + $"    OR  (F_TYPE = 'Embossing Cut Sheet'       AND {c[3]} )"
                        + $"    OR  (F_TYPE = 'Flat Foil Continuous'      AND {c[4]} )"
                        + $"    OR  (F_TYPE = 'Flat Foil Cut Sheet'       AND {c[5]} )"
                        + $"    OR  (F_TYPE = 'Metallic Safe Continuous'  AND {c[6]} )"
                        + $"    OR  (F_TYPE = 'Metallic Safe Cut Sheet'   AND {c[7]} )"
                        + $"    OR  (F_TYPE = 'Metallic Safe Snap'        AND {c[8]} )"
                        + $"    OR  (F_TYPE = 'Rainbow Foil Continuous'   AND {c[9]} )"
                        + $"    OR  (F_TYPE = 'Rainbow Foil Cut Sheet'    AND {c[10]} )"
                        + $"    OR  (F_TYPE = 'Refracto Safe Continuous'  AND {c[11]} )"
                        + $"    OR  (F_TYPE = 'Refracto Safe Cut Sheet'   AND {c[12]} )"
                        + $"    OR  (F_TYPE = 'Tamper Safe Continuous'    AND {c[13]} )"
                        + $"    OR  (F_TYPE = 'Tamper Safe Cut Sheets'    AND {c[14]} )"
                        + $"    OR  (F_TYPE = 'THERMO Safe &Hide'         AND {c[15]} )"
                        + $"    OR  (F_TYPE = 'THERMOHIDE'                AND {c[16]} )"
                        + $"    OR  (F_TYPE = 'THERMOSAFE'                AND {c[17]} )"
                        + $"    OR  (F_TYPE = 'TOUCHSAFE'                 AND {c[18]} )"
                        + $"    OR  (F_TYPE = 'NaNOcopy Void'             AND {c[19]} )"
                        + $"    OR  (F_TYPE = 'ISP - ThermoCombo'         AND {c[20]} ))"
                       + " ORDER BY F_TYPE, NUMBER";

            conn = new SqlConnection(ConnectionString);
            da = new SqlDataAdapter(cmd, conn); dt = new DataTable(); da.Fill(dt);
            DataView dv = dt.DefaultView;
            // This DataView contains rows from the FEATURES table whose F_TYPEs are checked 

            BaseFlatCharge = 0;
            BaseRunCharge = 0;
            BaseFinishCharge = 0;
            BaseConvCharge = 0;
            BasePlateCharge = 0;
            BasePressCharge = 0;
            FlatTotal = 0;

            BasePressSetup = 0;
            BaseCollatorSetup = 0;
            BaseBinderySetup = 0;
            BasePressSlowdown = 0;
            BaseCollatorSlowdown = 0;
            BaseBinderySlowdown = 0;

            for (int i = 0; i < dv.Count; i++)
            {
                float t1 = 0; float.TryParse(dv[i]["FLAT_CHARGE"].ToString(),   out t1); BaseFlatCharge   += t1; FlatTotal += t1;
                float t2 = 0; float.TryParse(dv[i]["RUN_CHARGE"].ToString(),    out t2); BaseRunCharge    += t2;
                float t3 = 0; float.TryParse(dv[i]["FINISH_MATL"].ToString(),   out t3); BaseFinishCharge += t3; FlatTotal += t3;
                float t4 = 0; float.TryParse(dv[i]["CONV_MATL"].ToString(),     out t4); BaseConvCharge   += t4; FlatTotal += t4;
                float t5 = 0; float.TryParse(dv[i]["PLATE_MATL"].ToString(),    out t5); BasePlateCharge  += t5; FlatTotal += t5;
                float t6 = 0; float.TryParse(dv[i]["PRESS_MATL"].ToString(),    out t6); BasePressCharge  += t6; FlatTotal += t6;

                int l1 = 0; int.TryParse(dv[i]["PRESS_SETUP_TIME"] .ToString(), out l1); BasePressSetup       += l1;
                int l2 = 0; int.TryParse(dv[i]["COLLATOR_SETUP"]   .ToString(), out l2); BaseCollatorSetup    += l2;
                int l3 = 0; int.TryParse(dv[i]["BINDERY_SETUP"]    .ToString(), out l3); BaseBinderySetup     += l3;
                int l4 = 0; int.TryParse(dv[i]["PRESS_SLOWDOWN"]   .ToString(), out l4); BasePressSlowdown    += l4;
                int l5 = 0; int.TryParse(dv[i]["COLLATOR_SLOWDOWN"].ToString(), out l5); BaseCollatorSlowdown += l5;
                int l6 = 0; int.TryParse(dv[i]["BINDERY_SLOWDOWN"] .ToString(), out l6); BaseBinderySlowdown  += l6;
            }

            CalcTotal();
        }

        private void CalcLabor(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        {
            CalculateLabor();
        }

        private void CalculateLabor()
        {
            PressSetup = BasePressSetup + LabPS;
            CollatorSetup = BaseCollatorSetup + LabCS;
            BinderySetup = BaseBinderySetup + LabBS;
            SetupTotal = (int)PressSetup + CollatorSetup + BinderySetup;

            PressSlowdown = BasePressSlowdown + LabPSL;
            CollatorSlowdown = BaseCollatorSlowdown + LabCSL;
            BinderySlowdown = BaseBinderySlowdown + LabBSL;
            SlowdownTotal = PressSlowdown + CollatorSlowdown + BinderySlowdown;
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

            CalculateLabor();
        }

        private void ChkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            GetCharges(); 
        }

        private void ChkBox_Checked(object sender, RoutedEventArgs e)
        {
            GetCharges(); 
        }

        private void PctChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        {
            CalcTotal(); 
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string sChecked = ""; for (int i = 0; i < 21; i++) { sChecked += (Chk[i] ? "1" : "0"); }
            string cmd = $"DELETE [ESTIMATING].[dbo].[Quote_Details] WHERE QUOTE_NUM = '{QuoteNum}' AND CATEGORY = 'Security'";
            conn = new SqlConnection(ConnectionString); conn.Open();
            scmd = new SqlCommand(cmd, conn); scmd.ExecuteNonQuery();

            Checker = ""; for(int i = 0;i <= Num_F_Types-1; i++) { Checker += (Chk[i] ? "1" : "0"); }

            scmd.CommandText
                = "INSERT INTO [ESTIMATING].[dbo].[Quote_Details]" 
                + "  ( QUOTE_NUM,      SEQUENCE,       CATEGORY,                 VALUE10,"
                + "    FlatCharge,     TotalFlatChg,   PerThousandChg,           FlatChargePct,      RunChargePct,     PlateChargePct,     FinishChargePct,     PressChargePct,     ConvertChargePct,"
                + "    PRESS_ADDL_MIN, COLL_ADDL_MIN,  BIND_ADDL_MIN,            PRESS_SLOW_PCT,     COLL_SLOW_PCT,    BIND_SLOW_PCT,  "
                + "    PressSetupMin,  PressSlowPct,   CollSetupMin,             CollSlowPct,        BindSetupMin,     BindSlowPct   ) "
                + " VALUES ( " 
                + $" '{QuoteNum}',     11,            'Security',              '{Checker}',"
                + $" '{FlatCharge}', '{FlatTotal}',   '{CalculatedRunCharge}', '{FlatChargePct}',  '{RunChargePct}', '{PlateChargePct}', '{FinishChargePct}', '{PressChargePct}', '{ConvChargePct}', "
                + $"  {LabPS},        {LabCS},         {LabBS},                 {LabPSL},           {LabCSL},         {LabBSL}, "
                + $"  {PressSetup},   {PressSlowdown}, {CollatorSetup},         {CollatorSlowdown}, {BinderySetup},   {BinderySlowdown} )";

            if (conn.State != ConnectionState.Open) conn.Open();
            scmd.ExecuteNonQuery();
            conn.Close();

            cmd =  "UPDATE [ESTIMATING].[dbo].[Quote_Details]     "
                + $" SET PressSlowdown      = {PressSlowdown},    "
                + $"     ConvertingSlowdown = {CollatorSlowdown}, "
                + $"     FinishingSlowdown  = {BinderySlowdown},  "
                + $"     Press              = {PressSetup},       "
                + $"     Converting         = {CollatorSetup},    "
                + $"     Finishing          = {BinderySetup}      "
                + $" WHERE Quote_Num = '{QuoteNum}'               "
                + "    AND CATEGORY  = 'Security'";

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