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
    public partial class Punching : Window, INotifyPropertyChanged
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

        private int max; public int Max { get { return max; } set { max = value; OnPropertyChanged(); } }
        private string pressSize; public string PressSize  { get { return pressSize; } set { pressSize = value; OnPropertyChanged(); } }
        private string quoteNum;  public string QuoteNum   { get { return quoteNum;  } set { quoteNum  = value; OnPropertyChanged(); } }
        private float  flatTotal; public float  FlatTotal  { get { return flatTotal; } set { flatTotal = value; OnPropertyChanged(); } }

        private int two;   public int Two   { get { return two; } set { two = value; OnPropertyChanged(); } }
        private int three; public int Three { get { return three; } set { three = value; OnPropertyChanged(); } }
        private int over3; public int Over3 { get { return over3; } set { over3 = value; OnPropertyChanged(); } }

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

        public Punching(string PRESSSIZE, string QUOTENUM)
        {
            // Test data added by me:
            // UPDATE FEATURES SET RUN_CHARGE = '2.00', PLATE_MATL = '10.00', FINISH_MATL = '10.00', PRESS_MATL = '20.00', CONV_MATL = '30.00'
            //  WHERE CATEGORY = 'PUNCHING' AND PRESS_SIZE = '11'

            InitializeComponent();
            this.DataContext = this;

            Title = "Quote #: " + QUOTENUM;
            QuoteNum = QUOTENUM;
            PressSize = PRESSSIZE;

            LoadMaxima();
            LoadData();
            GetCharges();

            PreviewKeyDown += (s, e) => { if (e.Key == Key.Escape) Close(); };
        }

        public void OnLoad(object sender, RoutedEventArgs e)
        {
            this.Height = this.Height *= 1.8;
            this.Width = this.Width *= 1.8;
        }

        private void LoadMaxima()
        {
            // Retrieve value for Backer if one was previously entered 
            string str = $"SELECT F_TYPE, MAX(Number) AS Max"
                + $" FROM [ESTIMATING].[dbo].[FEATURES] WHERE Category = 'Punching' AND Press_Size = '{PressSize}' GROUP BY F_TYPE";
            SqlConnection conn = new(ConnectionString);
            SqlDataAdapter da = new(str, conn); DataTable dt = new(); dt.Rows.Clear(); da.Fill(dt);
            DataView dv = new DataView(dt);
            dv.RowFilter = "F_TYPE='2 HOLES'";   M1.Maximum = int.Parse(dv[0]["Max"].ToString());
            dv.RowFilter = "F_TYPE='3 HOLES'";   M2.Maximum = int.Parse(dv[0]["Max"].ToString());
            dv.RowFilter = "F_TYPE='4-5 HOLES'"; M3.Maximum = int.Parse(dv[0]["Max"].ToString());
        }

        private void LoadData()
        {
            string str = $"SELECT * FROM [ESTIMATING].[dbo].[Quote_Details] WHERE QUOTE_NUM = '{QuoteNum}' AND Category = 'Punching'";
            SqlConnection conn = new(ConnectionString);
            SqlDataAdapter da = new(str, conn); DataTable dt = new(); dt.Rows.Clear(); da.Fill(dt);
            if (dt.Rows.Count == 0) return;
            DataView dv = new DataView(dt);
            Two   = int.Parse(dv[0]["Value1"].ToString());
            Three = int.Parse(dv[0]["Value2"].ToString());
            Over3 = int.Parse(dv[0]["Value3"].ToString());

            // Load percentages and recalculate all displayed property values
            FlatChargePct = float.Parse(dv[0]["FlatChargePct"].ToString());
            RunChargePct = float.Parse(dv[0]["RunChargePct"].ToString());
            PlateChargePct = float.Parse(dv[0]["PlateChargePct"].ToString());
            FinishChargePct = float.Parse(dv[0]["FinishChargePct"].ToString());
            PressChargePct = float.Parse(dv[0]["PressChargePct"].ToString());
            ConvChargePct = float.Parse(dv[0]["ConvertChargePct"].ToString());
            FlatTotal = float.Parse(dv[0]["TotalFlatChg"].ToString());
            CalculatedRunCharge = float.Parse(dv[0]["PerThousandChg"].ToString());

            GetCharges();
        }

        private void GetCharges()
        {
            string cmd = "SELECT F_TYPE, NUMBER, FLAT_CHARGE, RUN_CHARGE, PLATE_MATL, FINISH_MATL, PRESS_MATL, CONV_MATL"
                       + $"  FROM [ESTIMATING].[dbo].[FEATURES]"
                       + $" WHERE CATEGORY = 'PUNCHING' AND PRESS_SIZE = '{PressSize}'"
                       + $"   AND ((F_TYPE = '2 HOLES'     AND Number = {Two}   )"
                       + $"    OR  (F_TYPE = '3 HOLES'     AND Number = {Three} )"
                       + $"    OR  (F_TYPE = '4-5 HOLES'   AND Number = {Over3} ))"
                       + " ORDER BY F_TYPE, NUMBER";

            conn = new SqlConnection(ConnectionString);
            da = new SqlDataAdapter(cmd, conn); dt = new DataTable(); da.Fill(dt);
            DataView dv = dt.DefaultView;

            BaseFlatCharge = 0;
            BaseRunCharge = 0;
            BaseFinishCharge = 0;
            BaseConvCharge = 0;
            BasePlateCharge = 0;
            BasePressCharge = 0;

            FlatTotal = 0;

            for (int i = 0; i < dv.Count; i++)
            {
                float t1 = 0; float.TryParse(dv[i]["FLAT_CHARGE"].ToString(), out t1); BaseFlatCharge += t1;
                float t2 = 0; float.TryParse(dv[i]["RUN_CHARGE"].ToString(), out t2); BaseRunCharge += t2;
                float t3 = 0; float.TryParse(dv[i]["FINISH_MATL"].ToString(), out t3); BaseFinishCharge += t3;
                float t4 = 0; float.TryParse(dv[i]["CONV_MATL"].ToString(), out t4); BaseConvCharge += t4;
                float t5 = 0; float.TryParse(dv[i]["PLATE_MATL"].ToString(), out t5); BasePlateCharge += t5;
                float t6 = 0; float.TryParse(dv[i]["PRESS_MATL"].ToString(), out t6); BasePressCharge += t6;
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
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string cmd = "DELETE [ESTIMATING].[dbo].[Quote_Details] WHERE Quote_Num = '" + QuoteNum + "' AND Category = 'Punching'";
            conn = new SqlConnection(ConnectionString); SqlCommand scmd = new SqlCommand(cmd, conn); conn.Open();

            try { scmd.ExecuteNonQuery(); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { conn.Close(); }

            //TODO: Add percentages to the values that are saved
            cmd = "INSERT INTO [ESTIMATING].[dbo].[Quote_Details] ("
                + " Quote_Num, Category, Sequence,"
                + " Param1, Param2, Param3, "
                + " Value1, Value2, Value3 ) VALUES ( "
                + $"'{QuoteNum}', 'Punching', 5, "
                + "  'Two',   'Three',   'Over3', "
                + $" '{Two}', '{Three}', '{Over3}' )";

            scmd.CommandText = cmd;
            conn.Open();
            try { scmd.ExecuteNonQuery(); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { conn.Close(); }

            this.Close();
        }

        private void ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        { GetCharges(); }

        private void PctChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        { CalcTotal(); }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        { Close(); }

    }
}