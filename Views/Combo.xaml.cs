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
    public partial class Combo : Window, INotifyPropertyChanged
    {
        // Data loaded for testing:
        //update features set
        //  flat_charge = 60.00,
        //  run_charge  = 10.00,
        //  plate_matl  = 20.00,
        //  finish_matl = 30.00,
        //  press_matl  = 40.00,
        //  conv_matl   = 60.00
        // where category   = 'COMBO'
        //   and Press_SIZE = '11';

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

        #region Properties

        public string ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public SqlConnection? conn;
        public SqlDataAdapter? da;
        public DataTable? dt;
        public SqlCommand? scmd;

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

        private string quoteNo; public string QuoteNo { get { return quoteNo; } set { quoteNo = value; } }

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

        public Combo(string PRESSSIZE, string QUOTENUM)
        {
            InitializeComponent();
            this.DataContext = this;

            SqlConnection conn = new(ConnectionString);

            Title      = "Quote #: " + QUOTENUM;
            QuoteNum   = QUOTENUM;
            PressSize  = PRESSSIZE;

            FlatCharge = 0.00F;     // Do I need to do this? I don't think so...

            LoadMaximum();
            LoadFeature();
            LoadData();
            CalcTotal();

            PreviewKeyDown += (s, e) => { if (e.Key == Key.Escape) Close(); };
        }

        public void OnLoad(object sender, RoutedEventArgs e)
        {
            this.Height = this.Height *= 1.8;
            this.Width = this.Width *= 1.8;
        }

        private void LoadMaximum()
        {
            // Retrieve maximum number of values
            string str = $"SELECT F_TYPE, MAX(Number) AS Max FROM [ESTIMATING].[dbo].[FEATURES] WHERE Category = 'COMBO' AND Press_Size = '{PressSize}' GROUP BY F_TYPE";
            SqlConnection  conn = new(ConnectionString);
            SqlDataAdapter da   = new(str, conn); DataTable dt = new(); dt.Rows.Clear(); da.Fill(dt);
            DataView dv  = new DataView(dt);
            dv.RowFilter = "F_TYPE='COMBO'"; 
            M1.Maximum   = int.Parse(dv[0]["Max"].ToString());
        }

        private void LoadFeature()
        {
            string str = $"SELECT * FROM [ESTIMATING].[dbo].[FEATURES] WHERE Category = 'COMBO' AND Press_Size = '{PressSize}' ";
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

//            Total = float.Parse(dt.Rows[0]["Amount"].ToString());

            CalcTotal();
        }

        private void CalcTotal()
        {
//            if (Total == -1.00F) { Total = 0.00F; return; } // The first time that it's called is spurious

            CalculatedFlatCharge   = Combo1 * (BaseFlatCharge   * (1 + FlatChargePct   / 100));
            CalculatedRunCharge    = Combo1 * (BaseRunCharge    * (1 + RunChargePct    / 100));
            CalculatedPlateCharge  = Combo1 * (BasePlateCharge  * (1 + PlateChargePct  / 100));
            CalculatedFinishCharge = Combo1 * (BaseFinishCharge * (1 + FinishChargePct / 100));
            CalculatedPressCharge  = Combo1 * (BasePressCharge  * (1 + PressChargePct  / 100));
            CalculatedConvCharge   = Combo1 * (BaseConvCharge   * (1 + ConvChargePct   / 100));

            FlatTotal = CalculatedFlatCharge + CalculatedPlateCharge + CalculatedFinishCharge + CalculatedPressCharge + CalculatedConvCharge;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            DeleteQuoteRow("Combo");

            string cmd = "INSERT INTO [ESTIMATING].[dbo].[Quote_Details] "
                + " (Quote_Num,     Category, Sequence, Param1,    Value1,     FlatChargePct,     RunChargePct,     PlateChargePct,     FinishChargePct,     PressChargePct,     ConvertChargePct,  TotalFlatChg,  PerThousandChg )"
                + " VALUES ( "
                + $"'{QuoteNum}',  'Combo',   8,       'Combo1', '{Combo1}', '{FlatChargePct}', '{RunChargePct}', '{PlateChargePct}', '{FinishChargePct}', '{PressChargePct}', '{ConvChargePct}', '{FlatTotal}', '{CalculatedRunCharge}' )";

            Clipboard.SetText(cmd);     // To test in SSMS

            SqlConnection conn = new(ConnectionString); conn.Open();
            SqlCommand    scmd = new(cmd, conn);

            try { scmd.ExecuteNonQuery();                       }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally              { conn.Close();                }

            this.Hide();
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

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void PctChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        { CalcTotal(); }

    }
}