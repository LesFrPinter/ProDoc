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

        #endregion

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

            // Note that the string indicating which f_types were checked should be 21 characters long
            Checker = dv[0]["Value9"].ToString();
            for (int i = 0; i < Checker.Length; i++)
            { string v = Checker.Substring(i, 1); Chk[i] = (v == "1") ? true : false; }

            BaseFlatCharge      = float.Parse(dv[0]["FlatCharge"].ToString());
            FlatTotal           = float.Parse(dv[0]["TotalFlatChg"].ToString());
            CalculatedRunCharge = float.Parse(dv[0]["PerThousandChg"].ToString());
            FlatChargePct       = float.Parse(dv[0]["FlatChargePct"].ToString());
            RunChargePct        = float.Parse(dv[0]["RunChargePct"].ToString());
            FinishChargePct     = float.Parse(dv[0]["FinishChargePct"].ToString());
            PressChargePct      = float.Parse(dv[0]["FinishChargePct"].ToString());
            ConvChargePct       = float.Parse(dv[0]["ConvertChargePct"].ToString());

            GetCharges();
        }

        private void GetCharges()
        {
            //if(Startup) return;

            //NOTE: This could be simplified by generating the SELECT statement to only refer to checked F_TYPEs...
            string[] c = { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
            for (int i = 0; i <= 20; i++) { c[i] = Chk[i] ? "1=1" : "1=0"; }
            string cmd = "SELECT F_TYPE, NUMBER, FLAT_CHARGE, RUN_CHARGE, PLATE_MATL, FINISH_MATL, PRESS_MATL, CONV_MATL"
                        + $"  FROM [ESTIMATING].[dbo].[FEATURES]"
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

            for (int i = 0; i < dv.Count; i++)
            {
                float t1 = 0; float.TryParse(dv[i]["FLAT_CHARGE"].ToString(), out t1); BaseFlatCharge   += t1; FlatTotal += t1;
                float t2 = 0; float.TryParse(dv[i]["RUN_CHARGE"].ToString(),  out t2); BaseRunCharge    += t2;
                float t3 = 0; float.TryParse(dv[i]["FINISH_MATL"].ToString(), out t3); BaseFinishCharge += t3; FlatTotal += t3;
                float t4 = 0; float.TryParse(dv[i]["CONV_MATL"].ToString(),   out t4); BaseConvCharge   += t4; FlatTotal += t4;
                float t5 = 0; float.TryParse(dv[i]["PLATE_MATL"].ToString(),  out t5); BasePlateCharge  += t5; FlatTotal += t5;
                float t6 = 0; float.TryParse(dv[i]["PRESS_MATL"].ToString(),  out t6); BasePressCharge  += t6; FlatTotal += t6;
            }

            CalcTotal();
        }

        private void CalcTotal()
        {
            //if (Startup) return;

            CalculatedFlatCharge = BaseFlatCharge * (1 + FlatChargePct / 100);

            CalculatedRunCharge = BaseRunCharge * (1 + RunChargePct / 100);

            CalculatedPlateCharge = BasePlateCharge * (1 + PlateChargePct / 100);
            CalculatedFinishCharge = BaseFinishCharge * (1 + FinishChargePct / 100);
            CalculatedPressCharge = BasePressCharge * (1 + PressChargePct / 100);
            CalculatedConvCharge = BaseConvCharge * (1 + ConvChargePct / 100);

            FlatTotal = CalculatedFlatCharge + CalculatedPlateCharge + CalculatedFinishCharge + CalculatedPressCharge + CalculatedConvCharge;
        }

        private void ChkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            //if (Startup) return; 
            GetCharges(); 
        }

        private void ChkBox_Checked(object sender, RoutedEventArgs e)
        {
            //if (Startup) return;
            GetCharges(); 
        }

        private void PctChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        {
            //if (Startup) return;
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
                = "INSERT INTO [ESTIMATING].[dbo].[Quote_Details] ( QUOTE_NUM, SEQUENCE, CATEGORY, VALUE9,"
                + "    FlatCharge,     TotalFlatChg,  PerThousandChg,          FlatChargePct,     RunChargePct,     FinishChargePct,     PressChargePct, ConvertChargePct ) "
                + $" VALUES ( '{QuoteNum}', 10, 'Security', '{Checker}',"
                + $" '{FlatCharge}', '{FlatTotal}', '{CalculatedRunCharge}', '{FlatChargePct}', '{RunChargePct}', '{FinishChargePct}', '{PressChargePct}', '{ConvChargePct}' )";
            if (conn.State != ConnectionState.Open) conn.Open();
            scmd.ExecuteNonQuery();
            conn.Close();
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        { this.Close(); }

    }
}