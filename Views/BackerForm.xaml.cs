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
    public partial class BackerForm : Window, INotifyPropertyChanged
    {

        // Added for testing:
        // update features set finish_matl = '20', press_matl = '30', conv_matl = '40' where category = 'Backer' and press_Size = '11'
        // Be sure to set them back to zero after testing is finished

        #region Properties

        public string ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public SqlConnection? conn;
        public SqlDataAdapter? da;
        public DataTable dt;
        public SqlCommand? scmd;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

        private string pressSize = ""; public string PressSize { get { return pressSize; } set { pressSize = value; OnPropertyChanged(); } }

        private float flat;   public float Flat   { get { return flat;    } set { flat    = value; OnPropertyChanged(); } }
        private float chng;   public float Chng   { get { return chng;    } set { chng    = value; OnPropertyChanged(); } }
        private int changes;  public int Changes  { get { return changes; } set { changes = value; OnPropertyChanged(); } }
        private float? total; public float? Total { get { return total;   } set { total   = value; OnPropertyChanged(); } }
        private bool backer;  public bool Backer  { get { return backer;  } set { backer  = value; OnPropertyChanged(); } }

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

        private float pressCharge; public float PressCharge { get { return pressCharge; } set { pressCharge = value; OnPropertyChanged(); } }
        private float basepressCharge; public float BasePressCharge { get { return basepressCharge; } set { basepressCharge = value; OnPropertyChanged(); } }
        private float pressChargePct; public float PressChargePct { get { return pressChargePct; } set { pressChargePct = value; OnPropertyChanged(); } }
        private float calculatedpressCharge; public float CalculatedPressCharge { get { return calculatedpressCharge; } set { calculatedpressCharge = value; OnPropertyChanged(); GetGrandTotal(); } }

        private float convCharge; public float ConvCharge { get { return convCharge; } set { convCharge = value; OnPropertyChanged(); } }
        private float baseconvCharge; public float BaseConvCharge { get { return baseconvCharge; } set { baseconvCharge = value; OnPropertyChanged(); } }
        private float convChargePct; public float ConvChargePct { get { return convChargePct; } set { convChargePct = value; OnPropertyChanged(); } }
        private float calculatedconvCharge; public float CalculatedConvCharge { get { return calculatedconvCharge; } set { calculatedconvCharge = value; OnPropertyChanged(); } }

        private float  flatTotal; public float  FlatTotal { get { return flatTotal; } set { flatTotal = value; OnPropertyChanged(); } }
        private string fieldList; public string FieldList { get { return fieldList; } set { fieldList = value; OnPropertyChanged(); } }

        #endregion

        public BackerForm(string PSize, string QUOTENUM)
        {
            InitializeComponent();

            DataContext = this;

            PressSize   = PSize;
            Title       = Title = "Quote #: " + QUOTENUM + "  PressSize: " + PressSize;  // passed in when the form is instantiated in Quotations.xaml.cs.
            Total       = -1.00F;
            QuoteNo     = QUOTENUM;
            FieldList   = "F_TYPE, FLAT_CHARGE, RUN_CHARGE, PLATE_MATL, FINISH_MATL, CONV_MATL, PRESS_MATL";

            LoadQuote();
            CalcTotal();

            PreviewKeyDown += (s, e) => { if (e.Key == Key.Escape) Close(); };
        }

        public void OnLoad(object sender, RoutedEventArgs e)
        {
            this.Height = this.Height *= 1.8;
            this.Width = this.Width *= 1.8;
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

                // Load percentges here....

                FlatChargePct   = int.Parse(dt.Rows[0]["FlatChargePct"].ToString());
                RunChargePct    = int.Parse(dt.Rows[0]["RunChargePct"].ToString());
                PlateChargePct  = int.Parse(dt.Rows[0]["PlateChargePct"].ToString());
                FinishChargePct = int.Parse(dt.Rows[0]["FinishChargePct"].ToString());
                ConvChargePct   = int.Parse(dt.Rows[0]["ConvertChargePct"].ToString());
                PressChargePct  = int.Parse(dt.Rows[0]["PressChargePct"].ToString());

                Changes = Int32.Parse(dt.Rows[0]["Value2"].ToString());

                Total = float.Parse(dt.Rows[0]["Amount"].ToString());

            }

            conn.Close();
        }

        private void CalcTotal()
        {
            if (Total == -1.00F) { Total = 0.00F; return; } // The first time that it's called is spurious

            if ((Button1 + Button2 + Button3) == 0) 
            {
                //    MessageBox.Show("Please select an option."); return; 
                Button1 = 1; One = true;
            }

            string cmd = $"SELECT {FieldList} FROM [ESTIMATING].[dbo].[FEATURES] WHERE CATEGORY = 'BACKER' AND PRESS_SIZE = '{PressSize}' AND F_TYPE = '";
            if (Button1 == 1) { cmd += "BACKER'"; }
            if (Button2 == 1) { cmd += "BACKER STD'"; }
            if (Button3 == 1) { cmd += "BACKER PMS'"; }

            Clipboard.SetText(cmd);

            dt   = new DataTable("Charges");
            conn = new SqlConnection(ConnectionString);
            da   = new SqlDataAdapter(cmd, conn);
            dt.Clear(); da.Fill(dt);

            if(dt.Rows.Count == 0) { MessageBox.Show("No data"); return; }

            DataView dv = dt.DefaultView;

            float fc = 0.00F; float.TryParse(dv[0]["FLAT_CHARGE"].ToString(), out fc); BaseFlatCharge = fc;
            float rc = 0.00F; float.TryParse(dv[0]["RUN_CHARGE"].ToString(), out rc); BaseRunCharge = rc;
            float pm = 0.00F; float.TryParse(dv[0]["PLATE_MATL"].ToString(), out pm); BasePlateCharge = pm;
            float fm = 0.00F; float.TryParse(dv[0]["FINISH_MATL"].ToString(), out fm); BaseFinishCharge = fm;
            float pr = 0.00F; float.TryParse(dv[0]["PRESS_MATL"].ToString(), out pr); BasePressCharge = pr;
            float cm = 0.00F; float.TryParse(dv[0]["CONV_MATL"].ToString(), out cm); BaseConvCharge = cm;

            //BaseFlatCharge  = float.Parse   (dt.Rows[0][1].ToString());
            //float ch = 0.00F; float.TryParse(dt.Rows[0][2].ToString(), out ch); BaseRunCharge = ch;
            //float bp = 0.00F; float.TryParse(dt.Rows[0][3].ToString(), out bp); BasePlateCharge = bp;
            //float bf = 0.00F; float.TryParse(dt.Rows[0][4].ToString(), out bf); BaseFinishCharge = bf;
            //float pr = 0.00F; float.TryParse(dt.Rows[0][5].ToString(), out pr); BasePressCharge = pr;
            //float co = 0.00F; float.TryParse(dt.Rows[0][6].ToString(), out co); BaseConvCharge = co;

            //  I think it was this before: BasePlateCharge = Changes * rc;     // $5.00 * Changes
            BasePlateCharge = Changes * pm;

            CalculatedFlatCharge = BaseFlatCharge   * (1 + FlatChargePct   / 100);
            CalculatedRunCharge    = BaseRunCharge    * (1 + RunChargePct    / 100);
            CalculatedPlateCharge  = BasePlateCharge  * (1 + PlateChargePct  / 100);
            CalculatedFinishCharge = BaseFinishCharge * (1 + FinishChargePct / 100);
            CalculatedPressCharge  = BasePressCharge  * (1 + PressChargePct  / 100);
            CalculatedConvCharge   = BaseConvCharge   * (1 + ConvChargePct   / 100);

            Total = CalculatedFlatCharge + CalculatedPlateCharge + CalculatedFinishCharge + CalculatedPressCharge + CalculatedConvCharge;

            // Which total does the number of changes affect?

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if((Button1+Button2+Button3)==0) { MessageBox.Show("Please select an option."); return; }
            // Delete current detail line
            string cmd = "DELETE [ESTIMATING].[dbo].[Quote_Details] WHERE Quote_Num = '" + QuoteNo + "' AND Category = 'Backer'";
            conn = new SqlConnection(ConnectionString);
            SqlCommand scmd = new SqlCommand(cmd, conn); conn.Open();
            try     { scmd.ExecuteNonQuery(); }
            catch   ( Exception ex ) { MessageBox.Show(ex.Message); }
            finally { conn.Close(); }

            // Store in Quote_Detail table:
            cmd =   "INSERT INTO [ESTIMATING].[dbo].[Quote_Details] ( Quote_Num, Category, Sequence, Param1, Param2, Value2, ";
            cmd +=  " FlatChargePct, RunChargePct, PlateChargePct, FinishChargePct, ConvertChargePct, PressChargePct, TotalFlatChg, PerThousandChg, FlatCharge, Amount ) VALUES  ";
            cmd += $"( '{QuoteNo}', 'Backer', 1, ";

            // This goes in Param1
            if (Button1 == 1) { cmd += "'BACKER',    "; }   // Which radio
            if (Button2 == 1) { cmd += "'BACKER STD',"; }   // button was
            if (Button3 == 1) { cmd += "'BACKER PMS',"; }   // selected?

            // Param2 is 'Changes'; number of changes goes in Value2
            cmd += $" 'Changes', '{Changes}', ";
            cmd += $" '{FlatChargePct}', '{RunChargePct}', '{PlateChargePct}', '{FinishChargePct}', '{ConvChargePct}', '{PressChargePct}', '{FlatTotal}', '{CalculatedRunCharge}', '{FlatTotal}', '{Total}' )";

            // Write to SQL
            conn = new SqlConnection(ConnectionString);
            conn.Open();
            scmd.Connection = conn;
            scmd.CommandText = cmd; 
            try     { scmd.ExecuteNonQuery(); }
            catch   ( Exception ex) { MessageBox.Show(ex.Message); }
            finally { conn.Close(); }
            
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

        private void RadNumericUpDown_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        { CalcTotal(); }

        private void Checked(object sender, RoutedEventArgs e)
        { CalcTotal(); }

        private void PctChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        {
            CalculatedFlatCharge    = (float)BaseFlatCharge     * (1.00F + (float)FlatChargePct   / 100.00F);
            CalculatedRunCharge     = (float)BaseRunCharge      * (1.00F + (float)RunChargePct    / 100.00F);
            CalculatedPlateCharge   = (float)BasePlateCharge    * (1.00F + (float)PlateChargePct  / 100.00F);
            CalculatedFinishCharge  = (float)BaseFinishCharge   * (1.00F + (float)FinishChargePct / 100.00F);
            CalculatedConvCharge    = (float)BaseConvCharge     * (1.00F + (float)ConvChargePct   / 100.00F);
            CalculatedPressCharge   = (float)BasePressCharge    * (1.00F + (float)PressChargePct  / 100.00F);
            GetGrandTotal();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        { Close(); }

    }
}