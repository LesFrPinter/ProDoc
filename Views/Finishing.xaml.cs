using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace ProDocEstimate.Views
{
    public partial class Finishing : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

        // The following test data was added to the Features table:

        //UPDATE FEATURES SET PLATE_MATL = 10, PRESS_MATL = 30, CONV_MATL = 40 WHERE CATEGORY = 'FINISHING' AND PRESS_SIZE = '11'
        //UPDATE FEATURES SET FINISH_MATL = 20 WHERE CATEGORY = 'FINISHING' AND PRESS_SIZE = '11' AND FINISH_MATL = ''
        // NOTE: Where FINISH_MATL had some values already, they were left in place. So if FINISH_MATL contains '20', it can be cleared.

        #region Properties

        public string ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public SqlConnection? conn;
        public SqlDataAdapter? da;
        public DataTable? dt;
        public SqlCommand? scmd;
        public DataView? dv;

        private int    max;         public int    Max        { get { return max;        } set { max         = value; OnPropertyChanged(); } }
        private string pressSize;   public string PressSize  { get { return pressSize;  } set { pressSize   = value; OnPropertyChanged(); } }

        private string quoteNum;    public string QuoteNum   { get { return quoteNum;   } set { quoteNum    = value; OnPropertyChanged(); } }
        private string quoteNo; public string QuoteNo { get { return quoteNo; } set { quoteNo = value; } }

        private string book;        public string Book       { get { return book;       } set { book        = value; OnPropertyChanged(); } }
        private string cello;       public string Cello      { get { return cello;      } set { cello       = value; OnPropertyChanged(); } }
        private string drillHoles;  public string DrillHoles { get { return drillHoles; } set { drillHoles  = value; OnPropertyChanged(); } }
        private string pad;         public string Pad        { get { return pad;        } set { pad         = value; OnPropertyChanged(); } }
        private string trim;        public string Trim       { get { return trim;       } set { trim        = value; OnPropertyChanged(); } }

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

        public Finishing(string PRESSSIZE, string QUOTENUM)
        {
            InitializeComponent();
            this.DataContext = this;

            Title     = "Quote #: " + QUOTENUM;
            QuoteNum  = QUOTENUM;
            PressSize = PRESSSIZE;

            LoadDropDowns();
            LoadData();

            PreviewKeyDown += (s, e) => { if (e.Key == Key.Escape) Close(); };
        }

        public void OnLoad(object sender, RoutedEventArgs e)
        {
            this.Height = this.Height *= 1.5;
            this.Width = this.Width *= 1.5;
            this.Top = 25;
        }

        private void LoadDropDowns()
        {
            M1.Items.Add(""); M2.Items.Add(""); M3.Items.Add(""); M4.Items.Add(""); M5.Items.Add("");   // allow them to leave any selection blank

            // Make the '4-5' entry the last one in the list. Sorting in alphabetical order doesn't work here.
            string str = $"SELECT F_TYPE, CONVERT(VARCHAR(10),CONVERT(INT,NUMBER)) AS NUMBER FROM [ESTIMATING].[dbo].[FEATURES] WHERE CATEGORY = 'FINISHING' AND PRESS_SIZE = '{PressSize}' AND NUMBER NOT LIKE '%-%'"
                       + $" UNION ALL SELECT F_TYPE,                                  NUMBER FROM [ESTIMATING].[dbo].[FEATURES] WHERE CATEGORY = 'FINISHING' AND PRESS_SIZE = '{PressSize}' AND (NUMBER LIKE '%-%') ORDER BY F_TYPE";

//            Clipboard.SetText(str);

            SqlConnection conn = new(ConnectionString);
            SqlDataAdapter da = new(str, conn); dt = new(); da.Fill(dt);
            dv = dt.DefaultView;

            //NOTE: If they need the ability to blank a value that was previously entered, add "" to the top of each ItemsList.
            dv.RowFilter = "F_TYPE='BOOK'";         for (int i = 0; i < dv.Count; i++) { M1.Items.Add(dv[i]["number"].ToString()); }
            dv.RowFilter = "F_TYPE='CELLO'";        for (int i = 0; i < dv.Count; i++) { M2.Items.Add(dv[i]["number"].ToString()); }
            dv.RowFilter = "F_TYPE='DRILL HOLES'";  for (int i = 0; i < dv.Count; i++) { M3.Items.Add(dv[i]["number"].ToString()); }
            dv.RowFilter = "F_TYPE='PAD'";          for (int i = 0; i < dv.Count; i++) { M4.Items.Add(dv[i]["number"].ToString()); }
            dv.RowFilter = "F_TYPE='TRIM'";         for (int i = 0; i < dv.Count; i++) { M5.Items.Add(dv[i]["number"].ToString()); }
        }

        private void LoadData()
        {
            string str = $"SELECT * FROM [ESTIMATING].[dbo].[Quote_Details] WHERE QUOTE_NUM = '{QuoteNum}' AND Category = 'Finishing'";
            SqlConnection conn = new(ConnectionString);
            SqlDataAdapter da = new(str, conn); DataTable dt = new(); dt.Rows.Clear(); da.Fill(dt);
            if (dt.Rows.Count == 0) return;
            dv = dt.DefaultView;
            Book        = dv[0]["Value1"].ToString();
            Cello       = dv[0]["Value2"].ToString();
            DrillHoles  = dv[0]["Value3"].ToString();
            Pad         = dv[0]["Value4"].ToString();
            Trim        = dv[0]["Value5"].ToString();

            float t = 0.00F;
            t = 0.00F; float.TryParse(dv[0]["FlatChargePct"].ToString(),    out t); FlatChargePct = t;
            t = 0.00F; float.TryParse(dv[0]["RunChargePct"].ToString(),     out t); RunChargePct = t;
            t = 0.00F; float.TryParse(dv[0]["ConvertChargePct"].ToString(), out t); ConvChargePct = t;
            t = 0.00F; float.TryParse(dv[0]["PlateChargePct"].ToString(),   out t); PlateChargePct = t;
            t = 0.00F; float.TryParse(dv[0]["PressChargePct"].ToString(),   out t); PressChargePct = t;
            t = 0.00F; float.TryParse(dv[0]["FinishChargePct"].ToString(),  out t); FinishChargePct = t;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // Validate inputs
            // e.g., if ((Button1 + Button2 + Button3) == 0) { MessageBox.Show("Please select an option."); return; }

            // Delete 'Finishing' quote_detail line, then re-insert one.
            string cmd = "DELETE [ESTIMATING].[dbo].[Quote_Details] WHERE Quote_Num = '" + QuoteNum + "' AND Category = 'Finishing'";
            conn = new SqlConnection(ConnectionString); SqlCommand scmd = new SqlCommand(cmd, conn); conn.Open();

            try { scmd.ExecuteNonQuery(); }
            catch (System.Exception ex) { MessageBox.Show(ex.Message); }
            finally { conn.Close(); }

            // Store in Quote_Detail table:
            cmd = "INSERT INTO [ESTIMATING].[dbo].[Quote_Details] ("
                + "   Quote_Num,   Category,   Sequence, Param1, Param2, Param3,       Param4, Param5,  Value1,   Value2,    Value3,         Value4,  Value5,   FlatChargePct,     RunChargePct,     PlateChargePct,     FinishChargePct,     PressChargePct,     ConvertChargePct ) VALUES ( "
                + $"'{QuoteNum}', 'Finishing', 7,       'Book', 'Cello','Drill Holes','Pad',  'Trim', '{Book}', '{Cello}', '{DrillHoles}', '{Pad}', '{Trim}', '{FlatChargePct}', '{RunChargePct}', '{PlateChargePct}', '{FinishChargePct}', '{PressChargePct}', '{ConvChargePct}'  )";

            scmd.CommandText = cmd;
            conn.Open();
            try { scmd.ExecuteNonQuery(); }
            catch (System.Exception ex) { MessageBox.Show(ex.Message); }
            finally { conn.Close(); }

            this.Close();
        }

        private void GrandTotal()
        {
            BaseFlatCharge = 0;
            BaseRunCharge = 0;
            BaseFinishCharge = 0;
            BaseConvCharge = 0;
            BasePlateCharge = 0;
            BasePressCharge = 0;

            SqlConnection conn = new(ConnectionString);
            string str = "SELECT * FROM [ESTIMATING].[dbo].[FEATURES]"
                        + " WHERE CATEGORY    = 'FINISHING'"
                        + $"   AND PRESS_SIZE = '{PressSize}'"
                        + $"   AND ((F_TYPE = 'BOOK'  AND Number = '{Book}')"
                        + $"    OR  (F_TYPE = 'CELLO' AND Number = '{Cello}')"
                        + $"    OR  (F_TYPE = 'DRILL HOLES' AND Number = '{DrillHoles}')"
                        + $"    OR  (F_TYPE = 'PAD'   AND Number = '{Pad}')"
                        + $"    OR  (F_TYPE = 'TRIM'  AND Number = '{Trim}'))";

            //            Clipboard.SetText(str);

            SqlDataAdapter da = new(str, conn);
            dt.Clear();
            da.Fill(dt); dv = dt.DefaultView;

            // MessageBox.Show(dv.Count.ToString() + " matches.", "Before filtering");

            float t = 0.00F;

            if(Book != null && Book != "") { 
                dv.RowFilter = "F_TYPE='BOOK'";
                t = 0.00F; float.TryParse(dv[0]["FLAT_CHARGE"].ToString(), out t); BaseFlatCharge += t;
                t = 0.00F; float.TryParse(dv[0]["RUN_CHARGE"].ToString(), out t); BaseRunCharge   += t;
                t = 0.00F; float.TryParse(dv[0]["FINISH_MATL"].ToString(), out t); BaseFinishCharge += t;
                t = 0.00F; float.TryParse(dv[0]["CONV_MATL"].ToString(), out t); BaseConvCharge   += t;
                t = 0.00F; float.TryParse(dv[0]["PLATE_MATL"].ToString(), out t); BasePlateCharge += t;
                t = 0.00F; float.TryParse(dv[0]["PRESS_MATL"].ToString(), out t); BasePressCharge += t;
            }

            if(Cello != null && Cello != "") { 
                dv.RowFilter = "F_TYPE='CELLO'";
                t = 0.00F; float.TryParse(dv[0]["FLAT_CHARGE"].ToString(), out t); BaseFlatCharge += t;
                t = 0.00F; float.TryParse(dv[0]["RUN_CHARGE"].ToString(), out t); BaseRunCharge   += t;
                t = 0.00F; float.TryParse(dv[0]["FINISH_MATL"].ToString(), out t); BaseFinishCharge += t;
                t = 0.00F; float.TryParse(dv[0]["CONV_MATL"].ToString(), out t); BaseConvCharge   += t;
                t = 0.00F; float.TryParse(dv[0]["PLATE_MATL"].ToString(), out t); BasePlateCharge += t;
                t = 0.00F; float.TryParse(dv[0]["PRESS_MATL"].ToString(), out t); BasePressCharge += t;
            }

            if(DrillHoles != null && DrillHoles != "" ) { 
                dv.RowFilter = "F_TYPE='DRILL HOLES'";
                t = 0.00F; float.TryParse(dv[0]["FLAT_CHARGE"].ToString(), out t); BaseFlatCharge += t;
                t = 0.00F; float.TryParse(dv[0]["RUN_CHARGE"].ToString(), out t); BaseRunCharge   += t;
                t = 0.00F; float.TryParse(dv[0]["FINISH_MATL"].ToString(), out t); BaseFinishCharge += t;
                t = 0.00F; float.TryParse(dv[0]["CONV_MATL"].ToString(), out t); BaseConvCharge   += t;
                t = 0.00F; float.TryParse(dv[0]["PLATE_MATL"].ToString(), out t); BasePlateCharge += t;
                t = 0.00F; float.TryParse(dv[0]["PRESS_MATL"].ToString(), out t); BasePressCharge += t;
            }

            if(Pad != null && Pad != "") { 
                dv.RowFilter = "F_TYPE='PAD'";
                t = 0.00F; float.TryParse(dv[0]["FLAT_CHARGE"].ToString(), out t); BaseFlatCharge += t;
                t = 0.00F; float.TryParse(dv[0]["RUN_CHARGE"].ToString(), out t); BaseRunCharge   += t;
                t = 0.00F; float.TryParse(dv[0]["FINISH_MATL"].ToString(), out t); BaseFinishCharge += t;
                t = 0.00F; float.TryParse(dv[0]["CONV_MATL"].ToString(), out t); BaseConvCharge   += t;
                t = 0.00F; float.TryParse(dv[0]["PLATE_MATL"].ToString(), out t); BasePlateCharge += t;
                t = 0.00F; float.TryParse(dv[0]["PRESS_MATL"].ToString(), out t); BasePressCharge += t;
            }

            if (Trim != null && Trim != "") { 
                dv.RowFilter = "F_TYPE='TRIM'";
                t = 0.00F; float.TryParse(dv[0]["FLAT_CHARGE"].ToString(), out t); BaseFlatCharge += t;
                t = 0.00F; float.TryParse(dv[0]["RUN_CHARGE"].ToString(), out t); BaseRunCharge   += t;
                t = 0.00F; float.TryParse(dv[0]["FINISH_MATL"].ToString(), out t); BaseFinishCharge += t;
                t = 0.00F; float.TryParse(dv[0]["CONV_MATL"].ToString(), out t); BaseConvCharge   += t;
                t = 0.00F; float.TryParse(dv[0]["PLATE_MATL"].ToString(), out t); BasePlateCharge += t;
                t = 0.00F; float.TryParse(dv[0]["PRESS_MATL"].ToString(), out t); BasePressCharge += t;
            }

            dv.RowFilter = "";

            CalcAll();

        }

        private void SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        { GrandTotal(); }

        private void PctChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        {
            CalcAll();
        }

        private void CalcAll()
        {
            CalculatedConvCharge   = BaseConvCharge   * (1 + (ConvChargePct   / 100));
            CalculatedFinishCharge = BaseFinishCharge * (1 + (FinishChargePct / 100));
            CalculatedFlatCharge   = BaseFlatCharge   * (1 + (FlatChargePct   / 100));
            CalculatedRunCharge    = BaseRunCharge    * (1 + (RunChargePct    / 100));
            CalculatedPlateCharge  = BasePlateCharge  * (1 + (PlateChargePct  / 100));
            CalculatedPressCharge  = BasePressCharge  * (1 + (PressChargePct  / 100));

            FlatTotal = 
                  CalculatedConvCharge 
                + CalculatedFinishCharge
                + CalculatedFlatCharge
                + CalculatedPlateCharge
                + CalculatedPressCharge;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        { Close(); }

    }
}