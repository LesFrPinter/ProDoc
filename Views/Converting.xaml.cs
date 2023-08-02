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
    public partial class Converting : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

        // The following test data was added to the Features table:
            //update features set
            //  plate_matl      = '10.00',
            //  finish_matl     = '20.00',
            //  press_matl      = '30.00',
            //  conv_matl       = '40.00'
            // where category   = 'CONVERTING'
            //   and press_size = '11'

        #region Properties

        public string ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public SqlConnection? conn;
        public SqlDataAdapter? da;
        public DataTable? dt;
        public SqlCommand? scmd;

        private int    max;         public int    Max       { get { return max;       } set { max       = value; OnPropertyChanged(); } }
        private string pressSize;   public string PressSize { get { return pressSize; } set { pressSize = value; OnPropertyChanged(); } }
        private string quoteNum;    public string QuoteNum  { get { return quoteNum;  } set { quoteNum  = value; OnPropertyChanged(); } }    // Which of these two
//        private string quoteNo;     public string QuoteNo   { get { return quoteNo;   } set { quoteNo   = value; OnPropertyChanged(); } }  //  is no longer used?

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

        #endregion

        public Converting(string PRESSSIZE, string QUOTENUM)
        {
            InitializeComponent();
            this.DataContext = this;

            Title = "Quote #: " + QUOTENUM;
            QuoteNum = QUOTENUM;
            PressSize = PRESSSIZE;

            LoadMaxima();
            LoadBaseValues();
            LoadData();

            PreviewKeyDown += (s, e) => { if (e.Key == Key.Escape) Close(); };
        }

        public void OnLoad(object sender, RoutedEventArgs e)
        {
            this.Height = this.Height *= 1.8;
            this.Width = this.Width *= 1.8;
        }

        private void LoadMaxima()
        {
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

            //#region Old method of summing charges by f_type
            //str = $"SELECT * FROM [ESTIMATING].[dbo].[FEATURES] WHERE Category = 'Converting' AND Press_Size = '{PressSize}' ORDER BY F_TYPE";
            //da.SelectCommand.CommandText = str;
            //DataTable dt2 = new(); da.Fill(dt2);
            //if (dt2.Rows.Count == 0) return;

            //dv = dt2.DefaultView;

            //float t = 0.00F;
            //dv.RowFilter = "F_TYPE='2 CRASH NUMB'";
            //t = 0.00F; float.TryParse(dv[0]["FLAT_CHARGE"].ToString(), out t); Fc11 = t;
            //t = 0.00F; float.TryParse(dv[0]["RUN_CHARGE"].ToString(),  out t); Fc12 = t;
            //t = 0.00F; float.TryParse(dv[0]["PLATE_MATL"].ToString(),  out t); Fc13 = t;
            //t = 0.00F; float.TryParse(dv[0]["FINISH_MATL"].ToString(), out t); Fc14 = t;
            //t = 0.00F; float.TryParse(dv[0]["PRESS_MATL"].ToString(),  out t); Fc15 = t;
            //t = 0.00F; float.TryParse(dv[0]["CONV_MATL"].ToString(),   out t); Fc16 = t;

            //dv.RowFilter = "F_TYPE='BLACK NUMB'";
            //t = 0.00F; float.TryParse(dv[0]["FLAT_CHARGE"].ToString(), out t); Fc21 = t;
            //t = 0.00F; float.TryParse(dv[0]["RUN_CHARGE"].ToString(),  out t); Fc22 = t;
            //t = 0.00F; float.TryParse(dv[0]["PLATE_MATL"].ToString(),  out t); Fc23 = t;
            //t = 0.00F; float.TryParse(dv[0]["FINISH_MATL"].ToString(), out t); Fc24 = t;
            //t = 0.00F; float.TryParse(dv[0]["PRESS_MATL"].ToString(),  out t); Fc25 = t;
            //t = 0.00F; float.TryParse(dv[0]["CONV_MATL"].ToString(),   out t); Fc26 = t;

            //dv.RowFilter = "F_TYPE='SNAP GLUE 2-4'";
            //t = 0.00F; float.TryParse(dv[0]["FLAT_CHARGE"].ToString(), out t); Fc31 = t;
            //t = 0.00F; float.TryParse(dv[0]["RUN_CHARGE"].ToString(),  out t); Fc32 = t;
            //t = 0.00F; float.TryParse(dv[0]["PLATE_MATL"].ToString(),  out t); Fc33 = t;
            //t = 0.00F; float.TryParse(dv[0]["FINISH_MATL"].ToString(), out t); Fc34 = t;
            //t = 0.00F; float.TryParse(dv[0]["PRESS_MATL"].ToString(),  out t); Fc35 = t;
            //t = 0.00F; float.TryParse(dv[0]["CONV_MATL"].ToString(),   out t); Fc36 = t;

            //dv.RowFilter = "F_TYPE='SNAP GLUE 5-12'";
            //t = 0.00F; float.TryParse(dv[0]["FLAT_CHARGE"].ToString(), out t); Fc41 = t;
            //t = 0.00F; float.TryParse(dv[0]["RUN_CHARGE"].ToString(),  out t); Fc42 = t;
            //t = 0.00F; float.TryParse(dv[0]["PLATE_MATL"].ToString(),  out t); Fc43 = t;
            //t = 0.00F; float.TryParse(dv[0]["FINISH_MATL"].ToString(), out t); Fc44 = t;
            //t = 0.00F; float.TryParse(dv[0]["PRESS_MATL"].ToString(),  out t); Fc45 = t;
            //t = 0.00F; float.TryParse(dv[0]["CONV_MATL"].ToString(),   out t); Fc46 = t;

            //dv.RowFilter = "F_TYPE='FOLDING'";
            //t = 0.00F; float.TryParse(dv[0]["FLAT_CHARGE"].ToString(), out t); Fc51 = t;
            //t = 0.00F; float.TryParse(dv[0]["RUN_CHARGE"].ToString(),  out t); Fc52 = t;
            //t = 0.00F; float.TryParse(dv[0]["PLATE_MATL"].ToString(),  out t); Fc53 = t;
            //t = 0.00F; float.TryParse(dv[0]["FINISH_MATL"].ToString(), out t); Fc54 = t;
            //t = 0.00F; float.TryParse(dv[0]["PRESS_MATL"].ToString(),  out t); Fc55 = t;
            //t = 0.00F; float.TryParse(dv[0]["CONV_MATL"].ToString(),   out t); Fc56 = t;

            //dv.RowFilter = "F_TYPE='CONT GLUE 2-4'";
            //t = 0.00F; float.TryParse(dv[0]["FLAT_CHARGE"].ToString(), out t); Fc61 = t;
            //t = 0.00F; float.TryParse(dv[0]["RUN_CHARGE"].ToString(),  out t); Fc62 = t;
            //t = 0.00F; float.TryParse(dv[0]["PLATE_MATL"].ToString(),  out t); Fc63 = t;
            //t = 0.00F; float.TryParse(dv[0]["FINISH_MATL"].ToString(), out t); Fc64 = t;
            //t = 0.00F; float.TryParse(dv[0]["PRESS_MATL"].ToString(),  out t); Fc65 = t;
            //t = 0.00F; float.TryParse(dv[0]["CONV_MATL"].ToString(),   out t); Fc66 = t;

            //dv.RowFilter = "F_TYPE='CONT GLUE 2-4'";
            //t = 0.00F; float.TryParse(dv[0]["FLAT_CHARGE"].ToString(), out t); Fc71 = t;
            //t = 0.00F; float.TryParse(dv[0]["RUN_CHARGE"].ToString(),  out t); Fc72 = t;
            //t = 0.00F; float.TryParse(dv[0]["PLATE_MATL"].ToString(),  out t); Fc73 = t;
            //t = 0.00F; float.TryParse(dv[0]["FINISH_MATL"].ToString(), out t); Fc74 = t;
            //t = 0.00F; float.TryParse(dv[0]["PRESS_MATL"].ToString(),  out t); Fc75 = t;
            //t = 0.00F; float.TryParse(dv[0]["CONV_MATL"].ToString(),   out t); Fc76 = t;

            //dv.RowFilter = "F_TYPE='TRANSFER TAPE'";
            //t = 0.00F; float.TryParse(dv[0]["FLAT_CHARGE"].ToString(), out t); Fc81 = t;
            //t = 0.00F; float.TryParse(dv[0]["RUN_CHARGE"].ToString(),  out t); Fc82 = t;
            //t = 0.00F; float.TryParse(dv[0]["PLATE_MATL"].ToString(),  out t); Fc83 = t;
            //t = 0.00F; float.TryParse(dv[0]["FINISH_MATL"].ToString(), out t); Fc84 = t;
            //t = 0.00F; float.TryParse(dv[0]["PRESS_MATL"].ToString(),  out t); Fc85 = t;
            //t = 0.00F; float.TryParse(dv[0]["CONV_MATL"].ToString(),   out t); Fc86 = t;

            //#endregion

            return;
        }

        private void LoadData()
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
        }

        private void LoadBaseValues()
        {
            BaseFlatCharge = 0.00F;
            BaseRunCharge = 0.00F;
            BasePlateCharge = 0.00F;
            BaseFinishCharge = 0.00F;
            BasePressCharge = 0.00F;
            BaseConvCharge = 0.00F;

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
            {   t = 0.00F; float.TryParse(dv3[i]["FLAT_CHARGE"].ToString(), out t); BaseFlatCharge += t;
                t = 0.00F; float.TryParse(dv3[i]["RUN_CHARGE"].ToString(), out t); BaseRunCharge += t;
                t = 0.00F; float.TryParse(dv3[i]["PLATE_MATL"].ToString(), out t); BasePlateCharge += t;
                t = 0.00F; float.TryParse(dv3[i]["FINISH_MATL"].ToString(), out t); BaseFinishCharge += t;
                t = 0.00F; float.TryParse(dv3[i]["PRESS_MATL"].ToString(), out t); BasePressCharge += t;
                t = 0.00F; float.TryParse(dv3[i]["CONV_MATL"].ToString(), out t); BaseConvCharge += t;
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

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string cmd = "DELETE [ESTIMATING].[dbo].[Quote_Details] WHERE Quote_Num = '" + QuoteNum + "' AND Category = 'Converting'";
            conn = new SqlConnection(ConnectionString); SqlCommand scmd = new SqlCommand(cmd, conn); conn.Open();

            try     { scmd.ExecuteNonQuery(); }
            catch   (Exception ex) { MessageBox.Show(ex.Message); }
            finally { conn.Close(); }

            cmd = "INSERT INTO [ESTIMATING].[dbo].[Quote_Details] ("
                + "   Quote_Num,      Category,                 Sequence,"
                + "   Param1,         Param2,                   Param3,             Param4,           Param5,               Param6,             Param7,    Param8,         "
                + "   Value1,         Value2,                   Value3,             Value4,           Value5,               Value6,             Value7,    Value8, "
                + "   TotalFlatChg,   PerThousandChg,           FlatChargePct,      RunChargePct,     FinishChargePct,      PressChargePct,     ConvertChargePct,   PlateChargePct ) VALUES ( "
                + $"'{QuoteNum}',    'Converting',              9, "
                +  " '2 CRASH NUMB', 'BLACK NUMB',             'SNAP GLUE 2-4',    'SNAP GLUE 5-12', 'CONT GLUE 2-4',      'CONT GLUE 5-12',   'FOLDING', 'TRANSFER TAPE', "
                + $"'{Crash}',      '{Black}',                 '{Snap2}',          '{Snap5}',        '{Cont2}',            '{Cont5}',          '{Fold}',  '{Tape}',"
                + $"'{FlatTotal}',  '{CalculatedRunCharge}',   '{FlatChargePct}',  '{RunChargePct}', '{FinishChargePct}',  '{PressChargePct}', '{ConvChargePct}', '{PlateChargePct}' )";

            scmd.CommandText = cmd;
            conn.Open();
            try     { scmd.ExecuteNonQuery(); }
            catch   (Exception ex) { MessageBox.Show(ex.Message); }
            finally { conn.Close(); }

            this.Close();
        }

        private void Pct_Changed(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        { CalcTotals(); }

        private void ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        { LoadBaseValues(); CalcTotals(); }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        { Close(); }

    }
}