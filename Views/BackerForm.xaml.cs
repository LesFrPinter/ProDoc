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
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

        #region Properties

        public string ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public SqlConnection? conn;
        public SqlDataAdapter? da;
        public DataTable dt;
        public SqlCommand? scmd;

        private string pressSize = ""; public string PressSize { get { return pressSize; } set { pressSize = value; OnPropertyChanged(); } }

        private bool firstTime; public bool FirstTime { get { return firstTime; } set { firstTime = value; OnPropertyChanged(); } }

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

        private int pressSetup;         public int PressSetup        { get { return pressSetup;      } set { pressSetup = value; OnPropertyChanged(); } }
        private int collatorSetup;      public int CollatorSetup     { get { return collatorSetup;   } set { collatorSetup = value; OnPropertyChanged(); } }
        private int binderySetup;       public int BinderySetup      { get { return binderySetup;    } set { binderySetup = value; OnPropertyChanged(); } }

        private int basePressSetup;     public int BasePressSetup    { get { return basePressSetup;  } set { basePressSetup = value; OnPropertyChanged(); } }
        private int baseCollatorSetup;  public int BaseCollatorSetup { get { return baseCollatorSetup;} set { baseCollatorSetup = value; OnPropertyChanged(); } }
        private int baseBinderySetup;   public int BaseBinderySetup  { get { return baseBinderySetup;} set { baseBinderySetup = value; OnPropertyChanged(); } }

        private int labPS1;             public int LabPS1           { get { return labPS1;           } set { labPS1 = value; OnPropertyChanged(); } }
        private int labCS2;             public int LabCS2           { get { return labCS2;           } set { labCS2 = value; OnPropertyChanged(); } }
        private int labBS2;             public int LabBS2           { get { return labBS2;           } set { labBS2 = value; OnPropertyChanged(); } }

        private int pressSlowdown;      public int PressSlowdown    { get { return pressSlowdown;    } set { pressSlowdown = value; OnPropertyChanged(); } }
        private int collatorSlowdown;   public int CollatorSlowdown { get { return collatorSlowdown; } set { collatorSlowdown = value; OnPropertyChanged(); } }
        private int binderySlowdown;    public int BinderySlowdown  { get { return binderySlowdown;  } set { binderySlowdown = value; OnPropertyChanged(); } }

        private int basepressSlowdown;    public int BasePressSlowdown    { get { return basepressSlowdown;    } set { basepressSlowdown    = value; OnPropertyChanged(); } }
        private int basecollatorSlowdown; public int BaseCollatorSlowdown { get { return basecollatorSlowdown; } set { basecollatorSlowdown = value; OnPropertyChanged(); } }
        private int basebinderySlowdown;  public int BaseBinderySlowdown  { get { return basebinderySlowdown;  } set { basebinderySlowdown  = value; OnPropertyChanged(); } }

        private int labPSS1Pct;         public int LabPSS1Pct       { get { return labPSS1Pct;       } set { labPSS1Pct = value; OnPropertyChanged(); } }
        private int labCSS2Pct;         public int LabCSS2Pct       { get { return labCSS2Pct;       } set { labCSS2Pct = value; OnPropertyChanged(); } }
        private int labBSS2Pct;         public int LabBSS2Pct       { get { return labBSS2Pct;       } set { labBSS2Pct = value; OnPropertyChanged(); } }

        private int calculatedPS;       public int CalculatedPS     { get { return calculatedPS;     } set { calculatedPS = value; OnPropertyChanged(); } }
        private int calculatedCS;       public int CalculatedCS     { get { return calculatedCS;     } set { calculatedCS = value; OnPropertyChanged(); } }
        private int calculatedBS;       public int CalculatedBS     { get { return calculatedBS;     } set { calculatedBS = value; OnPropertyChanged(); } }

        private int parts;              public int Parts            { get { return parts;            } set { parts = value; OnPropertyChanged(); } }

        private int slowdownTotal;      public int SlowdownTotal    { get { return slowdownTotal;    } set { slowdownTotal = value; OnPropertyChanged(); } }
        private int setupTotal;         public int SetupTotal       { get { return setupTotal;       } set { setupTotal = value; OnPropertyChanged(); } }

        #endregion

        #region Notes
        // Data that I changed for testing:
        //    UPDATE FEATURES
        //       SET FINISH_MATL = '20',
        //           PRESS_MATL  = '30',
        //           CONV_MATL   = '40'
        //     WHERE CATEGORY    = 'BACKER'
        //       AND PRESS_SIZE  = '11'

        // SQL command to create testdata for the LABOR calculations:
        //    UPDATE FEATURES
        //    SET
        //    PRESS_SETUP_TIME  = .25,
        //    COLLATOR_SETUP    = .30,
        //    BINDERY_SETUP     = .40,
        //    PRESS_SLOWDOWN    =  25, -- NEXT 3 ARE PERCENTAGES
        //    COLLATOR_SLOWDOWN =  33,
        //	  SLOWDOWN_PER_PART =  50    -- BINDERY_SLOWDOWN
        //    WHERE CATEGORY    = 'BACKER'
        //	  AND PRESS_SIZE    = '11'

        // Columns added to QUOTE_DETAILS to support added labor calculations:

        //ALTER TABLE QUOTE_DETAILS ADD PRESS_MIN_PCT    INTEGER NOT NULL DEFAULT 0;
        //ALTER TABLE QUOTE_DETAILS ADD COLL_MIN_PCT     INTEGER NOT NULL DEFAULT 0;
        //ALTER TABLE QUOTE_DETAILS ADD BIND_MIN_PCT     INTEGER NOT NULL DEFAULT 0;
        //ALTER TABLE QUOTE_DETAILS ADD PRESS_SLOW_PCT   INTEGER NOT NULL DEFAULT 0;
        //ALTER TABLE QUOTE_DETAILS ADD COLL_SLOW_PCT    INTEGER NOT NULL DEFAULT 0;
        //ALTER TABLE QUOTE_DETAILS ADD BIND_SLOW_PCT    INTEGER NOT NULL DEFAULT 0;
        //ALTER TABLE QUOTE_DETAILS ADD SETUP_MINUTES    INTEGER NOT NULL DEFAULT 0;
        //ALTER TABLE QUOTE_DETAILS ADD SLOWDOWN_PERCENT INTEGER NOT NULL DEFAULT 0;
        #endregion

        public BackerForm(string PSize, string QUOTENUM)
        {
            InitializeComponent();

            FirstTime = true;

            DataContext = this;

            PressSize   = PSize;
            Title       = Title = "Quote #: " + QUOTENUM + "  PressSize: " + PressSize;  // passed in when the form is instantiated in Quotations.xaml.cs.
            Total       = -1.00F;   // Used to prevent premature calculation
            QuoteNo     = QUOTENUM; // Parameters passed in from Quotations form
            Parts = 1;   // Don't multiply by # parts

            FieldList = "F_TYPE, FLAT_CHARGE, RUN_CHARGE, "
                        + " PLATE_MATL, FINISH_MATL, CONV_MATL,  PRESS_MATL, "
                        + " PRESS_SETUP_TIME, COLLATOR_SETUP,    BINDERY_SETUP, "
                        + " PRESS_SLOWDOWN,   COLLATOR_SLOWDOWN, SLOWDOWN_PER_PART AS BINDERY_SLOWDOWN";  // from FEATURES

            LoadQuote();
            CalcTotal();

            FirstTime = false;

            PreviewKeyDown += (s, e) => { if (e.Key == Key.Escape) Close(); };
        }

        public void OnLoad(object sender, RoutedEventArgs e)
        {
            this.Height = this.Height *= 1.5;
            this.Width = this.Width *= 1.5;
            this.Top = 25;
//            FirstTime = false;
        }

        private void LoadQuote()
        {
            string cmd = "SELECT * FROM [ESTIMATING].[dbo].[QUOTE_DETAILS] WHERE QUOTE_NUM = '" + QuoteNo + "' AND CATEGORY = 'Backer'";
            dt   = new DataTable("Details");
            conn = new SqlConnection(ConnectionString);
            da   = new SqlDataAdapter(cmd, conn); da.Fill(dt);

            if(dt.Rows.Count > 0 )
            {
                // Retrieve the backer type, which is stored in Param1 (should really be in Value1...)
                string? F_TYPE = dt.Rows[0]["Param1"].ToString();

                switch (F_TYPE)
                {
                    case "BACKER": One = true; break;
                    case "BACKER STD": Two = true; break;
                    case "BACKER PMS": Three = true; break;
                }

                // Retrieve the number of changes and the total amount 
                Changes = Int32.Parse(dt.Rows[0]["Value2"].ToString());
                Total = float.Parse(dt.Rows[0]["Amount"].ToString());

// MATERIAL modifiers (are these six initializations needed?)
                FlatChargePct = 0;
                RunChargePct = 0;
                PlateChargePct = 0;
                FinishChargePct = 0;
                ConvChargePct = 0;
                PressChargePct = 0;

                int t;
                t = 0; int.TryParse(dt.Rows[0]["FlatChargePct"].ToString(), out t); FlatChargePct = t;
                t = 0; int.TryParse(dt.Rows[0]["RunChargePct"].ToString(), out t); RunChargePct = t;
                t = 0; int.TryParse(dt.Rows[0]["PlateChargePct"].ToString(), out t); PlateChargePct = t;
                t = 0; int.TryParse(dt.Rows[0]["FinishChargePct"].ToString(), out t); FinishChargePct = t;
                t = 0; int.TryParse(dt.Rows[0]["ConvertChargePct"].ToString(), out t); ConvChargePct = t;
                t = 0; int.TryParse(dt.Rows[0]["PressChargePct"].ToString(), out t); PressChargePct = t;

// LABOR modifiers:
                t = 0; int.TryParse(dt.Rows[0]["PRESS_ADDL_MIN"].ToString(), out t); LabPS1 = t;
                t = 0; int.TryParse(dt.Rows[0]["COLL_ADDL_MIN"].ToString(), out t); LabCS2 = t;
                t = 0; int.TryParse(dt.Rows[0]["BIND_ADDL_MIN"].ToString(), out t); LabBS2 = t;
                t = 0; int.TryParse(dt.Rows[0]["PRESS_SLOW_PCT"].ToString(), out t); LabPSS1Pct = t;
                t = 0; int.TryParse(dt.Rows[0]["COLL_SLOW_PCT"].ToString(), out t); LabCSS2Pct = t;
                t = 0; int.TryParse(dt.Rows[0]["BIND_SLOW_PCT"].ToString(), out t); LabBSS2Pct = t;
                t = 0; int.TryParse(dt.Rows[0]["SETUP_MINUTES"].ToString(), out t); SetupTotal = t;
                t = 0; int.TryParse(dt.Rows[0]["SLOWDOWN_PERCENT"].ToString(), out t); SlowdownTotal = t;
            }

            conn.Close();
        }

        private void CalcTotal()
        {
            if ((Button1 + Button2 + Button3) == 0) { Button1 = 1; One = true; }

            // The default values for this feature depend on the currently selected value for F_TYPE (type of backer)
            // That's why it has to be called every time the type of backer (a RadioButton control) changes.

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

            //  I think it was this before: BasePlateCharge = Changes * rc;     // $5.00 * Changes
            BasePlateCharge = Changes * pm;

            CalculatedFlatCharge   = BaseFlatCharge   * (1 + FlatChargePct   / 100);
            CalculatedRunCharge    = BaseRunCharge    * (1 + RunChargePct    / 100);
            CalculatedPlateCharge  = BasePlateCharge  * (1 + PlateChargePct  / 100);
            CalculatedFinishCharge = BaseFinishCharge * (1 + FinishChargePct / 100);
            CalculatedPressCharge  = BasePressCharge  * (1 + PressChargePct  / 100);
            CalculatedConvCharge   = BaseConvCharge   * (1 + ConvChargePct   / 100);

            Total = CalculatedFlatCharge + CalculatedPlateCharge + CalculatedFinishCharge + CalculatedPressCharge + CalculatedConvCharge;

            // The next three calculations assume that the values from the FEATURES table are decimal fractions of an your (e.g. .25 for 1/4)
            float fPressSetup    = float.Parse(dv[0]["PRESS_SETUP_TIME"].ToString()) * 60.0F; BasePressSetup       = int.Parse(fPressSetup.ToString());
            float fCollatorSetup = float.Parse(dv[0]["COLLATOR_SETUP"].ToString()) * 60.0F; BaseCollatorSetup    = int.Parse(fCollatorSetup.ToString());
            float fBinderySetup  = float.Parse(dv[0]["BINDERY_SETUP"].ToString()) * 60.0F; BaseBinderySetup     = int.Parse(fBinderySetup.ToString());

            BasePressSlowdown    = int.Parse(dv[0]["PRESS_SLOWDOWN"].ToString());
            BaseCollatorSlowdown = int.Parse(dv[0]["COLLATOR_SLOWDOWN"].ToString());
            BaseBinderySlowdown  = int.Parse(dv[0]["BINDERY_SLOWDOWN"].ToString());

            PressSlowdown = BasePressSlowdown + LabPSS1Pct;
            CollatorSlowdown = BaseCollatorSlowdown + LabCSS2Pct;
            BinderySlowdown = BaseBinderySlowdown + LabBSS2Pct;
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
            cmd = "INSERT INTO [ESTIMATING].[dbo].[Quote_Details] ( " 
                + " Quote_Num, Category, Sequence, Param1, Param2, Value2, "
                + " FlatChargePct, RunChargePct, PlateChargePct, FinishChargePct, ConvertChargePct, PressChargePct, " 
                + " TotalFlatChg, PerThousandChg, FlatCharge, Amount, "
                + " PRESS_ADDL_MIN, COLL_ADDL_MIN, BIND_ADDL_MIN, PRESS_SLOW_PCT, COLL_SLOW_PCT, BIND_SLOW_PCT, " 
                + " SETUP_MINUTES, SLOWDOWN_PERCENT )"
                + " VALUES ";

            cmd += $"( '{QuoteNo}', 'Backer', 1, ";

            // This goes in Param1 (should go in VALUE1...)
            if (Button1 == 1) { cmd += "'BACKER',    "; }   // Which radio
            if (Button2 == 1) { cmd += "'BACKER STD',"; }   // button was
            if (Button3 == 1) { cmd += "'BACKER PMS',"; }   // selected?

            // Param2 is 'Changes'; number of changes goes in Value2
            cmd += $" 'Changes', '{Changes}', ";

            cmd += $" '{FlatChargePct}', '{RunChargePct}', '{PlateChargePct}', '{FinishChargePct}', '{ConvChargePct}', "
                +  $" '{PressChargePct}', '{FlatTotal}', '{CalculatedRunCharge}', '{FlatTotal}', '{Total}', "
                +  $" '{LabPS1}', '{LabCS2}', '{LabBS2}', '{LabPSS1Pct}', '{LabCSS2Pct}', '{LabBSS2Pct}', "
                +  $" '{SetupTotal}', '{SlowdownTotal}' )";

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
            if (FirstTime) return;

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

        private void CalcLabor(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        {
            // So which is it - times # parts or not?
            PressSetup       = Parts * (BasePressSetup + LabPS1);
            CollatorSetup    = Parts * (BaseCollatorSetup + LabCS2);
            BinderySetup     = Parts * (BaseBinderySetup + LabBS2);

            SetupTotal       = PressSetup + CollatorSetup + BinderySetup;

            PressSlowdown    = BasePressSlowdown + LabPSS1Pct;
            CollatorSlowdown = BaseCollatorSlowdown + LabCSS2Pct;
            BinderySlowdown  = BaseBinderySlowdown + LabBSS2Pct;

            SlowdownTotal    = PressSlowdown + CollatorSlowdown + BinderySlowdown;
        }

    }
}