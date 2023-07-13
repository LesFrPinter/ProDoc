using System;
using System.Data;
using System.Windows;
using System.Configuration;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ProDocEstimate.Views
{
    public partial class BackerForm : Window, INotifyPropertyChanged
    {

        #region Properties

        public string ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public SqlConnection? conn;
        public SqlDataAdapter? da;
        public DataTable dt;
        public SqlCommand? scmd;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

        private string pressSize = ""; public string PressSize { get { return pressSize; } set { pressSize = value; OnPropertyChanged(); } }

        private float flat;  public float Flat  { get { return flat;    } set { flat    = value; OnPropertyChanged(); } }
        private float chng;  public float Chng  { get { return chng;    } set { chng    = value; OnPropertyChanged(); } }
        private int changes; public int Changes { get { return changes; } set { changes = value; OnPropertyChanged(); } }
        private float? total; public float? Total { get { return total;   } set { total   = value; OnPropertyChanged(); } }

        private bool backer; public bool Backer { get { return backer;  } set { backer  = value; OnPropertyChanged(); } }

        private bool one;    public bool One    { get { return one;     } set { one     = value; OnPropertyChanged(); Button1 = (One) ? 1 : 0; } }
        private bool two;    public bool Two    { get { return two;     } set { two     = value; OnPropertyChanged(); Button2 = (Two) ? 1 : 0; } }
        private bool three;  public bool Three  { get { return three;   } set { three   = value; OnPropertyChanged(); Button3 = (Three) ? 1 : 0; } }

        private int button1; public int Button1 { get { return button1; } set { button1 = value; OnPropertyChanged(); } }
        private int button2; public int Button2 { get { return button2; } set { button2 = value; OnPropertyChanged(); } }
        private int button3; public int Button3 { get { return button3; } set { button3 = value; OnPropertyChanged(); } }

        private string quoteNo; public string QuoteNo { get { return quoteNo; } set { quoteNo = value; OnPropertyChanged(); } }

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

        private float convCharge; public float ConvCharge { get { return convCharge; } set { convCharge = value; OnPropertyChanged(); } }
        private float baseconvCharge; public float BaseConvCharge { get { return baseconvCharge; } set { baseconvCharge = value; OnPropertyChanged(); } }
        private float convChargePct; public float ConvChargePct { get { return convChargePct; } set { convChargePct = value; OnPropertyChanged(); } }
        private float calculatedconvCharge; public float CalculatedConvCharge { get { return calculatedconvCharge; } set { calculatedconvCharge = value; OnPropertyChanged(); } }

        private float pressCharge; public float PressCharge { get { return pressCharge; } set { pressCharge = value; OnPropertyChanged(); } }
        private float basepressCharge; public float BasePressCharge { get { return basepressCharge; } set { basepressCharge = value; OnPropertyChanged(); } }
        private float pressChargePct; public float PressChargePct { get { return pressChargePct; } set { pressChargePct = value; OnPropertyChanged(); } }
        private float calculatedpressCharge; public float CalculatedPressCharge { get { return calculatedpressCharge; } set { calculatedpressCharge = value; OnPropertyChanged(); GetGrandTotal(); } }

        private float  flatTotal; public float  FlatTotal { get { return flatTotal; } set { flatTotal = value; OnPropertyChanged(); } }
        private string fieldList; public string FieldList { get { return fieldList; } set { fieldList = value; OnPropertyChanged(); } }

        #endregion

        public BackerForm(string PSize, string QUOTENUM)
        {
            InitializeComponent();
            DataContext = this;
            PressSize = PSize;
            Title = Title = "Quote #: " + QUOTENUM + "  PressSize: " + PressSize;  // passed in when the form is instantiated in Quotations.xaml.cs.
            Total = -1.00F;
            QuoteNo = QUOTENUM;
            FieldList = "F_TYPE, FLAT_CHARGE, RUN_CHARGE, PLATE_MATL, FINISH_MATL, CONV_MATL, PRESS_MATL";

            LoadQuote();
            PreviewKeyDown += (s, e) => { if (e.Key == Key.Escape) Close(); };
        }

        private void LoadQuote()
        {
            string cmd = "SELECT * FROM [ESTIMATING].[dbo].[QUOTE_DETAILS] WHERE QUOTE_NUM = '" + QuoteNo + "' AND CATEGORY = 'Backer'";
            dt   = new DataTable("Details");
            conn = new SqlConnection(ConnectionString);
            da   = new SqlDataAdapter(cmd, conn); da.Fill(dt);
            if(dt.Rows.Count > 0 )
            {
                string? F_TYPE = dt.Rows[0]["Param1"].ToString();
                switch (F_TYPE)
                {
                    case "BACKER": One = true; break;
                    case "BACKER STD": Two = true; break;
                    case "BACKER PMS": Three = true; break;
                }
                Changes = Int32.Parse(dt.Rows[0]["Value2"].ToString());
                Total = float.Parse(dt.Rows[0]["Amount"].ToString());
            }
            conn.Close();
        }

        private void CalcTotal()
        {
            if (Total == -1.00F) { Total = 0.00F; return; } // First time it's called is spurious

            if ((Button1 + Button2 + Button3) == 0) { MessageBox.Show("Please select an option."); return; }

            string cmd = $"SELECT {FieldList} FROM [ESTIMATING].[dbo].[FEATURES] WHERE CATEGORY = 'BACKER' AND PRESS_SIZE = '{PressSize}' AND F_TYPE = '";
            if (Button1 == 1) { cmd += "BACKER'"; }
            if (Button2 == 1) { cmd += "BACKER STD'"; }
            if (Button3 == 1) { cmd += "BACKER PMS'"; }

            Clipboard.SetText(cmd);

            dt   = new DataTable("Charges");
            conn = new SqlConnection(ConnectionString);
            da   = new SqlDataAdapter(cmd, conn); da.Fill(dt);

            if(dt.Rows.Count == 0) { MessageBox.Show("No data"); return; }

            Flat                  = float.Parse(dt.Rows[0][1].ToString());
            float ch = 0.00F; float.TryParse(dt.Rows[0][2].ToString(), out ch); 
            Chng = ch;
            float bp = 0.00F; float.TryParse(dt.Rows[0][3].ToString(), out bp); 
            BasePlateCharge = bp;
            BaseFlatCharge        = Flat;
            BaseRunCharge         = Chng;
            CalculatedFlatCharge  = Flat;            FlatChargePct  = 0.00F;
            CalculatedRunCharge   = Chng * Changes;  RunChargePct   = 0.00F;
            CalculatedPlateCharge = BasePlateCharge; PlateChargePct = 0.00F;

            // Add in any other flat charges; none show now because most of the columns in FieldList are empty...

            Total = CalculatedFlatCharge + CalculatedPlateCharge;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if((Button1+Button2+Button3)==0) { MessageBox.Show("Please select an option."); return; }
            // Delete current detail line
            string cmd = "DELETE [ESTIMATING].[dbo].[Quote_Details] WHERE Quote_Num = '" + QuoteNo + "' AND Category = 'Backer'";
            conn = new SqlConnection(ConnectionString);
            SqlCommand scmd = new SqlCommand(cmd, conn); conn.Open();
            try { scmd.ExecuteNonQuery(); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { conn.Close(); }

            // Store in Quote_Detail table:
            cmd = "INSERT INTO [ESTIMATING].[dbo].[Quote_Details] ( Quote_Num, Category, Sequence, Param1, Param2, Value2, Amount ) VALUES ( ";
            cmd += "'" + QuoteNo.ToString() + "', 'Backer', 1, ";
            // Which radiobutton was selected?
            if (Button1 == 1) { cmd += "'BACKER',    "; }
            if (Button2 == 1) { cmd += "'BACKER STD',"; }
            if (Button3 == 1) { cmd += "'BACKER PMS',"; }
            // How many changes?
            cmd += " 'Changes', " + Changes.ToString().TrimEnd() + ", ";
            // Finally, the total charge for Backer:
            cmd += Total.ToString() + " )";

            // Write to SQL

            // conn = new SqlConnection(ConnectionString);
            scmd.CommandText = cmd; 
            conn.Open();
            try { scmd.ExecuteNonQuery(); }
            catch ( Exception ex) { MessageBox.Show(ex.Message); }
            finally { conn.Close(); scmd = null; conn = null; }
            
            this.Close();
        }

        private void GetGrandTotal()
        {
            FlatTotal =
               CalculatedFlatCharge
             + CalculatedPlateCharge
             + CalculatedFinishCharge
             + CalculatedConvCharge
             + CalculatedPressCharge;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        { this.Close(); }

        private void RadNumericUpDown_LostFocus(object sender, RoutedEventArgs e)
        { CalcTotal(); }

        private void RadNumericUpDown_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        { CalcTotal(); }

        private void B1_Checked(object sender, RoutedEventArgs e)
        { CalcTotal(); }

        private void B2_Checked(object sender, RoutedEventArgs e)
        { CalcTotal(); }

        private void B3_Checked(object sender, RoutedEventArgs e)
        { CalcTotal(); }

        private void FCPct_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        { CalculatedFlatCharge = (float)BaseFlatCharge * (1.00F + (float)FlatChargePct / 100.00F); GetGrandTotal(); }

        private void RCPct_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        { CalculatedRunCharge = (float)BaseRunCharge * (1.00F + (float)RunChargePct / 100.00F); GetGrandTotal(); }

        private void PCPct_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        { CalculatedPlateCharge = (float)BasePlateCharge * (1.00F + (float)PlateChargePct / 100.00F); GetGrandTotal(); }

        private void CFPct_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        { CalculatedFinishCharge = (float)BaseFinishCharge * (1.00F + (float)FinishChargePct / 100.00F); GetGrandTotal(); }

        private void CCPct_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        { CalculatedPressCharge = (float)BasePressCharge * (1.00F + (float)PressChargePct / 100.00F); GetGrandTotal(); }

        private void SCPct_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        { CalculatedPressCharge = (float)BasePressCharge * (1.00F + (float)PressChargePct / 100.00F); GetGrandTotal(); }

    }
}