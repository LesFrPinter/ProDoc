using MediaFoundation;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Printing;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Telerik.Pivot.Core.Totals;
using Telerik.Windows.Documents.Spreadsheet.Expressions.Functions;

namespace ProDocEstimate.Views
{
    public partial class MICR : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

        #region Properties

        // T-Sql to reset values in Features table;

        // UPDATE [ESTIMATING].[dbo].[FEATURES] set FINISH_MATL = 0.00 where GUID = '370044B0-DFD4-4EB4-B6B2-ECFF5F2B03BA'
        // UPDATE [ESTIMATING].[dbo].[FEATURES] set CONV_MATL   = 0.00 where GUID = '370044B0-DFD4-4EB4-B6B2-ECFF5F2B03BA'
        // UPDATE [ESTIMATING].[dbo].[FEATURES] set PRESS_MATL  = 0.00 where GUID = '370044B0-DFD4-4EB4-B6B2-ECFF5F2B03BA'

        //Columns added to Quote_Detail:

        //ALTER TABLE Quote_Details ADD FlatChargePct    VarChar(10);
        //ALTER TABLE Quote_Details ADD RunChargePct     VarChar(10);
        //ALTER TABLE Quote_Details ADD PlateChargePct   VarChar(10);
        //ALTER TABLE Quote_Details ADD FinishChargePct  VarChar(10);
        //ALTER TABLE Quote_Details ADD PressChargePct   VarChar(10);
        //ALTER TABLE Quote_Details ADD ConvertChargePct VarChar(10);
        //ALTER TABLE Quote_Details ADD TotalFlatChg     VarChar(10);
        //ALTER TABLE Quote_Details ADD PerThousandChg   VarChar(10);

        //UPDATE FEATURES
        //   SET PRESS_SETUP_TIME = 45,
        //       PRESS_SLOWDOWN = 50,
        //       FINISH_MATL = 25,
        //       PRESS_MATL = 65,
        //       CONV_MATL = 45,
        //       ADDTL_BIND_TIME = 15,
        //       COLLATOR_SETUP = 30,
        //       COLLATOR_SLOWDOWN = 15,
        //       BINDERY_SETUP = 25,
        //       BINDERY_SLOWDOWN = 35
        // WHERE CATEGORY = 'MICR'
        //   AND PRESS_SIZE = '11'

        public string ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public SqlConnection? conn;
        public SqlDataAdapter? da;
        public DataTable? dt;
        public SqlCommand? scmd;
        public DataView? dv;            // Loaded in LoadDataView(); stays in memory.

        private int    max;                   public int    Max                    { get { return max;                  } set { max                 = value; OnPropertyChanged(); } }
        private string quoteNum;              public string QuoteNum               { get { return quoteNum;             } set { quoteNum            = value; OnPropertyChanged(); } }
        private string pressSize;             public string PressSize              { get { return pressSize;            } set { pressSize           = value; OnPropertyChanged(); } }
        private string category;              public string Category               { get { return category;             } set { category            = value; OnPropertyChanged(); } }
        private string ftype;                 public string FType                  { get { return ftype;                } set { ftype               = value; OnPropertyChanged(); } }

        private int    digital;               public int    Digital                { get { return digital;              } set { digital             = value; OnPropertyChanged(); } }
        private int    pack2pack;             public int    Pack2Pack              { get { return pack2pack;            } set { pack2pack           = value; OnPropertyChanged(); } }
        private int    press;                 public int    Press                  { get { return press;                } set { press               = value; OnPropertyChanged(); } }

        private float  flatCharge;            public float  FlatCharge             { get { return flatCharge;           } set { flatCharge          = value; OnPropertyChanged(); } }
        private float  baseflatCharge;        public float  BaseFlatCharge         { get { return baseflatCharge;       } set { baseflatCharge      = value; OnPropertyChanged(); } }
        private float  flatChargePct;         public float  FlatChargePct          { get { return flatChargePct;        } set { flatChargePct       = value; OnPropertyChanged(); } }
        private float  calculatedflatCharge;  public float  CalculatedFlatCharge   { get { return calculatedflatCharge; } set { calculatedflatCharge= value; OnPropertyChanged(); } }

        private float  runCharge;             public float  RunCharge              { get { return runCharge;            } set { runCharge           = value; OnPropertyChanged(); } }
        private float  baserunCharge;         public float  BaseRunCharge          { get { return baserunCharge;        } set { baserunCharge       = value; OnPropertyChanged(); } }
        private float  runChargePct;          public float  RunChargePct           { get { return runChargePct;         } set { runChargePct        = value; OnPropertyChanged(); } }
        private float  calculatedrunCharge;   public float  CalculatedRunCharge    { get { return calculatedrunCharge;  } set { calculatedrunCharge = value; OnPropertyChanged(); } }

        private float  plateCharge;           public float  PlateCharge            { get { return plateCharge;          } set { plateCharge         = value; OnPropertyChanged(); } }
        private float  baseplateCharge;       public float  BasePlateCharge        { get { return baseplateCharge;      } set { baseplateCharge     = value; OnPropertyChanged(); } }
        private float  plateChargePct;        public float  PlateChargePct         { get { return plateChargePct;       } set { plateChargePct      = value; OnPropertyChanged(); } }
        private float  calculatedplateCharge; public float  CalculatedPlateCharge  { get { return calculatedplateCharge;} set { calculatedplateCharge=value; OnPropertyChanged(); } }

        private float  finishCharge;          public float FinishCharge            { get { return finishCharge;         } set { finishCharge        = value; OnPropertyChanged(); } }
        private float  basefinishCharge;      public float BaseFinishCharge        { get { return basefinishCharge;     } set { basefinishCharge    = value; OnPropertyChanged(); } }
        private float  finishChargePct;       public float FinishChargePct         { get { return finishChargePct;      } set { finishChargePct     = value; OnPropertyChanged(); } }
        private float  calculatedfinishCharge;public float CalculatedFinishCharge  { get { return calculatedfinishCharge;} set {calculatedfinishCharge=value;OnPropertyChanged(); } }

        private float  convCharge;            public float ConvCharge              { get { return convCharge;           } set { convCharge          = value; OnPropertyChanged(); } }
        private float  baseconvCharge;        public float BaseConvCharge          { get { return baseconvCharge;       } set { baseconvCharge      = value; OnPropertyChanged(); } }
        private float  convChargePct;         public float ConvChargePct           { get { return convChargePct;        } set { convChargePct       = value; OnPropertyChanged(); } }
        private float  calculatedconvCharge;  public float CalculatedConvCharge    { get { return calculatedconvCharge; } set { calculatedconvCharge= value; OnPropertyChanged(); } }

        private float  pressCharge;           public float PressCharge             { get { return pressCharge;          } set { pressCharge         = value; OnPropertyChanged(); } }
        private float  basepressCharge;       public float BasePressCharge         { get { return basepressCharge;      } set { basepressCharge     = value; OnPropertyChanged(); } }
        private float  pressChargePct;        public float PressChargePct          { get { return pressChargePct;       } set { pressChargePct      = value; OnPropertyChanged(); } }
        private float  calculatedpressCharge; public float CalculatedPressCharge   { get { return calculatedpressCharge;} set { calculatedpressCharge=value; OnPropertyChanged(); } }   // previously also called GetGrandTotal...

        private float flatTotal;              public float FlatTotal               { get { return flatTotal;            } set { flatTotal           = value; OnPropertyChanged(); } }

        private string fieldList;             public string FieldList              { get { return fieldList;            } set { fieldList           = value; OnPropertyChanged(); } }

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

        public MICR(string _PRESSSIZE, string _QUOTENUM)
        {
            // The next three were used by the DetailFeatures UserControl; may no longer be needed.
            QuoteNum = _QUOTENUM;
            PressSize = _PRESSSIZE;
            Category = "MICR";

            InitializeComponent();

            DataContext = this;
            Title = "Quote #: " + _QUOTENUM;
            FType = "DIGITAL";

            LoadMaxima();       // Set the upper limits on the value of NUMBER for each F_TYPE
            LoadQuote();        // get user's values for the three parameters used to load from FEATURES
            LoadBaseValues();   // Load from the FEATURES table

            PreviewKeyDown += (s, e) => { if (e.Key == Key.Escape) Close(); };
        }

        public void OnLoad(object sender, RoutedEventArgs e)
        {
            this.Height = this.Height *= 1.8;
            this.Width = this.Width *= 1.8;
            Top = 50;
        }

        private void LoadBaseValues()
        { string cmd = 
                "SELECT * FROM [ESTIMATING].[dbo].[FEATURES]"
            +  " WHERE CATEGORY = 'MICR'"
            +  "   AND PRESS_SIZE = '11'"
            + $"   AND ((F_TYPE = 'PRESS'     AND Number = {Press}    )"
            + $"    OR  (F_TYPE = 'PACK2PACK' AND Number = {Pack2Pack})"
            + $"    OR  (F_TYPE = 'DIGITAL'   AND Number = {Digital}  ))"
            + " ORDER BY F_TYPE";

            conn = new SqlConnection(ConnectionString);
            da = new SqlDataAdapter(cmd, conn); dt = new DataTable(); da.Fill(dt);
            dv = dt.AsDataView();

            BaseFlatCharge = 0;
            BaseRunCharge = 0;
            BasePlateCharge = 0;
            BaseConvCharge = 0;
            BaseFinishCharge = 0;
            BasePressCharge = 0;

            BasePressSetup = 0;
            BasePressSlowdown = 0;
            BaseCollatorSetup = 0;
            BaseBinderySetup = 0;
            BaseCollatorSlowdown = 0;
            BaseBinderySlowdown = 0;

            for (int i = 0; i < dv.Count; i++)
            {
                float t1 = 0; float.TryParse(dv[i]["FLAT_CHARGE"]     .ToString(), out t1); BaseFlatCharge   += t1;
                float t2 = 0; float.TryParse(dv[i]["RUN_CHARGE"]      .ToString(), out t2); BaseRunCharge    += t2;
                float t3 = 0; float.TryParse(dv[i]["FINISH_MATL"]     .ToString(), out t3); BaseFinishCharge += t3;
                float t4 = 0; float.TryParse(dv[i]["CONV_MATL"]       .ToString(), out t4); BaseConvCharge   += t4;
                float t5 = 0; float.TryParse(dv[i]["PLATE_MATL"]      .ToString(), out t5); BasePlateCharge  += t5;
                float t6 = 0; float.TryParse(dv[i]["PRESS_MATL"]      .ToString(), out t6); BasePressCharge  += t6;

                int l1 = 0;     int.TryParse(dv[i]["PRESS_SETUP_TIME"].ToString(), out l1); BasePressSetup += l1;
                int l2 = 0;     int.TryParse(dv[i]["PRESS_SLOWDOWN"]  .ToString(), out l2); BasePressSlowdown += l2;
                int l3 = 0;     int.TryParse(dv[i]["COLLATOR_SETUP"]  .ToString(), out l3); BaseCollatorSetup += l3;
                int l4 = 0;     int.TryParse(dv[i]["COLLATOR_SETUP"]  .ToString(), out l4); BaseCollatorSlowdown += l4;
                int l5 = 0;     int.TryParse(dv[i]["BINDERY_SETUP"]   .ToString(), out l5); BaseBinderySetup += l5;
                int l6 = 0;     int.TryParse(dv[i]["BINDERY_SLOWDOWN"].ToString(), out l6); BaseBinderySlowdown += l6;
            }

            PctChanged();
            CalculateLabor();

        }

        private void LoadMaxima()
        {
            string str = $"SELECT F_TYPE, MAX(Number) AS Max FROM [ESTIMATING].[dbo].[FEATURES] WHERE Category = 'MICR' AND Press_Size = '{PressSize}' GROUP BY F_TYPE";
            SqlConnection conn = new(ConnectionString);
            SqlDataAdapter da = new(str, conn); DataTable dt = new(); dt.Rows.Clear(); da.Fill(dt);
            DataView dv2 = new DataView(dt);
            dv2.RowFilter = "F_TYPE='DIGITAL'";      M1.Maximum = int.Parse(dv2[0]["Max"].ToString());
            dv2.RowFilter = "F_TYPE='PACK2PACK'";    M2.Maximum = int.Parse(dv2[0]["Max"].ToString());
            dv2.RowFilter = "F_TYPE='PRESS'";        M3.Maximum = int.Parse(dv2[0]["Max"].ToString());
        }

        private void LoadQuote()
        {
            string str = $"SELECT * FROM [ESTIMATING].[dbo].[Quote_Details] WHERE QUOTE_NUM = '{QuoteNum}' AND Category = 'MICR'";
            SqlConnection conn = new(ConnectionString);
            SqlDataAdapter da = new(str, conn); DataTable dt = new(); dt.Rows.Clear(); da.Fill(dt);
            if (dt.Rows.Count == 0) return;
            DataView dv3 = new DataView(dt);
            Digital    = int.Parse(dv3[0]["Value1"].ToString());
            Pack2Pack  = int.Parse(dv3[0]["Value2"].ToString());
            Press      = int.Parse(dv3[0]["Value3"].ToString());

            // Load percentages and recalculate all displayed property values
            FlatChargePct       = float.Parse(dv3[0]["FlatChargePct"].ToString());
            RunChargePct        = float.Parse(dv3[0]["RunChargePct"].ToString());
            PlateChargePct      = float.Parse(dv3[0]["PlateChargePct"].ToString());
            FinishChargePct     = float.Parse(dv3[0]["FinishChargePct"].ToString());
            PressChargePct      = float.Parse(dv3[0]["PressChargePct"].ToString());
            ConvChargePct       = float.Parse(dv3[0]["ConvertChargePct"].ToString());

            FlatTotal           = float.Parse(dv3[0]["TotalFlatChg"].ToString());
            CalculatedRunCharge = float.Parse(dv3[0]["PerThousandChg"].ToString());

            LabPS  = int.Parse(dv3[0]["PRESS_ADDL_MIN"].ToString());
            LabPSL = int.Parse(dv3[0]["PRESS_SLOW_PCT"].ToString());
            LabCS  = int.Parse(dv3[0]["COLL_ADDL_MIN"].ToString());
            LabCSL = int.Parse(dv3[0]["COLL_SLOW_PCT"].ToString());
            LabBS  = int.Parse(dv3[0]["BIND_ADDL_MIN"].ToString());
            LabBSL = int.Parse(dv3[0]["BIND_SLOW_PCT"].ToString());

//            GetCharges();
            LoadBaseValues();
        }

        // OnChange handlers for the three possible F_TYPE values that are in the MICR category:
        private void M1_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e) { LoadBaseValues(); }
        private void M2_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e) { LoadBaseValues(); }
        private void M3_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e) { LoadBaseValues(); }

        private void GetCharges()
        {   
            //if (dv == null || dv.Count == 0) return;
// "dv" is loaded in LoadData() and remains loaded for the life of this object.
            //dv.RowFilter = "F_TYPE='DIGITAL'";   float T1 = float.Parse(dv[0]["FLAT_CHARGE"].ToString());
            //dv.RowFilter = "F_TYPE='PACK2PACK'"; float T2 = float.Parse(dv[0]["FLAT_CHARGE"].ToString());
            //dv.RowFilter = "F_TYPE='PRESS'";     float T3 = float.Parse(dv[0]["FLAT_CHARGE"].ToString());
            //BaseFlatCharge       = (Digital * T1) + (Pack2Pack * T2) + (Press * T3);
            CalculatedFlatCharge = (float)BaseFlatCharge * (1.00F + (float)FlatChargePct / 100.00F);

            //dv.RowFilter = "F_TYPE='DIGITAL'";   float R1 = float.Parse(dv[0]["RUN_CHARGE"].ToString());
            //dv.RowFilter = "F_TYPE='PACK2PACK'"; float R2 = float.Parse(dv[0]["RUN_CHARGE"].ToString());
            //dv.RowFilter = "F_TYPE='PRESS'";     float R3 = float.Parse(dv[0]["RUN_CHARGE"].ToString());
            //BaseRunCharge = (Digital * R1) + (Pack2Pack * R2) + (Press * R3);
            CalculatedRunCharge = (float)BaseRunCharge * (1.00F + (float)RunChargePct / 100.00F);

            // The rest of the charges might be null, so tryparse has to be used...

            // ------------------------------- Plate Charges -------------------------------
            float P1; dv.RowFilter = "F_TYPE='DIGITAL'";
            if (float.TryParse(dv[0]["PLATE_MATL"].ToString(), out P1)) {}

            float P2;  dv.RowFilter = "F_TYPE='PACK2PACK'";
            if (float.TryParse(dv[0]["PLATE_MATL"].ToString(), out P2)) {}

            float P3;  dv.RowFilter = "F_TYPE='PRESS'";
            if (float.TryParse(dv[0]["PLATE_MATL"].ToString(), out P3)) {};

            BasePlateCharge = (Digital * P1) + (Pack2Pack * P2) + (Press * P3);
            CalculatedPlateCharge = (float)BasePlateCharge * (1.00F + (float)PlateChargePct / 100.00F);
            // ----------------------------- End Plate Charges -----------------------------

            // ------------------------------- Finish Charges ------------------------------
            float F1; dv.RowFilter = "F_TYPE='DIGITAL'";
            if (float.TryParse(dv[0]["FINISH_MATL"].ToString(), out F1)) { }

            float F2; dv.RowFilter = "F_TYPE='PACK2PACK'";
            if (float.TryParse(dv[0]["FINISH_MATL"].ToString(), out F2)) { }

            float F3; dv.RowFilter = "F_TYPE='PRESS'";
            if (float.TryParse(dv[0]["FINISH_MATL"].ToString(), out F3)) { };

            BaseFinishCharge = (Digital * F1) + (Pack2Pack * F2) + (Press * F3);
            CalculatedFinishCharge = (float)BaseFinishCharge * (1.00F + (float)FinishChargePct / 100.00F);
            // ----------------------------- End Finish Charges ----------------------------

            // ------------------------------- Convert Charges -----------------------------
            float C1; dv.RowFilter = "F_TYPE='DIGITAL'";
            if (float.TryParse(dv[0]["CONV_MATL"].ToString(), out C1)) { }

            float C2; dv.RowFilter = "F_TYPE='PACK2PACK'";
            if (float.TryParse(dv[0]["CONV_MATL"].ToString(), out C2)) { }

            float C3; dv.RowFilter = "F_TYPE='PRESS'";
            if (float.TryParse(dv[0]["CONV_MATL"].ToString(), out C3)) { };

            BaseConvCharge = (Digital * C1) + (Pack2Pack * C2) + (Press * C3);
            CalculatedConvCharge = (float)BaseConvCharge * (1.00F + (float)ConvChargePct / 100.00F);
            // ----------------------------- End Convert Charges ----------------------------

            // ------------------------------- Press Charges --------------------------------
            float S1; dv.RowFilter = "F_TYPE='DIGITAL'";
            if (float.TryParse(dv[0]["PRESS_MATL"].ToString(), out S1)) { }

            float S2; dv.RowFilter = "F_TYPE='PACK2PACK'";
            if (float.TryParse(dv[0]["PRESS_MATL"].ToString(), out S2)) { }

            float S3; dv.RowFilter = "F_TYPE='PRESS'";
            if (float.TryParse(dv[0]["PRESS_MATL"].ToString(), out S3)) { };

            BasePressCharge = (Digital * S1) + (Pack2Pack * S2) + (Press * S3);
            CalculatedPressCharge = (float)BasePressCharge * (1.00F + (float)PressChargePct / 100.00F);
            // ----------------------------- End Press Charges -------------------------------

        }

        private void Pct_Changed(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        {
            PctChanged();
        }

        private void PctChanged()
        {
            CalculatedFlatCharge = (float)BaseFlatCharge * (1.00F + (float)FlatChargePct / 100.00F);
            CalculatedRunCharge = (float)BaseRunCharge * (1.00F + (float)RunChargePct / 100.00F);
            CalculatedPlateCharge = (float)BasePlateCharge * (1.00F + (float)PlateChargePct / 100.00F);
            CalculatedFinishCharge = (float)BaseFinishCharge * (1.00F + (float)FinishChargePct / 100.00F);
            CalculatedConvCharge = (float)BaseConvCharge * (1.00F + (float)ConvChargePct / 100.00F);
            CalculatedPressCharge = (float)BasePressCharge * (1.00F + (float)PressChargePct / 100.00F);

            FlatTotal
             = CalculatedFlatCharge
             + CalculatedPlateCharge
             + CalculatedFinishCharge
             + CalculatedConvCharge
             + CalculatedPressCharge;
        }

        private void CalcLabor(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        {
            CalculateLabor();
        }

        private void CalculateLabor()
        {
//            if (Starting) return;
            PressSetup       = BasePressSetup       + LabPS;
            CollatorSetup    = BaseCollatorSetup    + LabCS;
            BinderySetup     = BaseBinderySetup     + LabBS;
            SetupTotal       = (int)PressSetup      + CollatorSetup + BinderySetup;

            PressSlowdown    = BasePressSlowdown    + LabPSL;
            CollatorSlowdown = BaseCollatorSlowdown + LabCSL;
            BinderySlowdown  = BaseBinderySlowdown  + LabBSL;
            SlowdownTotal    = PressSlowdown        + CollatorSlowdown + BinderySlowdown;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string cmd = $"DELETE [ESTIMATING].[dbo].[Quote_Details] WHERE Quote_Num = '{QuoteNum}' AND Category = 'MICR'";
            conn = new SqlConnection(ConnectionString); SqlCommand scmd = new SqlCommand(cmd, conn); conn.Open();
            try { scmd.ExecuteNonQuery(); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { conn.Close(); }

            // Add flat charges for up to three F_TYPE values;
            cmd = "INSERT INTO [ESTIMATING].[dbo].[Quote_Details] ("
                + "   Quote_Num,           Category,          Sequence,"
                + "   Param1,              Param2,            Param3,             Value1,              Value2,             Value3, "
                + "   FlatChargePct,       RunChargePct,      PlateChargePct,     FinishChargePct,     PressChargePct,     ConvertChargePct,  TotalFlatChg,  PerThousandChg, "
                + "   PRESS_ADDL_MIN,      COLL_ADDL_MIN,     BIND_ADDL_MIN,      PRESS_SLOW_PCT,      COLL_SLOW_PCT,      BIND_SLOW_PCT,     SETUP_MINUTES, SLOWDOWN_PERCENT ) "
                + " VALUES ( "
                + $" '{QuoteNum}',        'MICR',             4,"
                + $"  'Digital',          'Pack2Pack',       'Press',           '{Digital}',         '{Pack2Pack}',      '{Press}', "
                + $" '{FlatChargePct}',  '{RunChargePct}',  '{PlateChargePct}', '{FinishChargePct}', '{PressChargePct}', '{ConvChargePct}', '{FlatTotal}', '{CalculatedRunCharge}', "
                + $"  {LabPS},            {LabCS},           {LabBS},            {LabPSL} ,           {LabCSL},           {LabBSL},          {SetupTotal},  {SlowdownTotal} )";

            scmd.CommandText = cmd;
            conn.Open();
            try { scmd.ExecuteNonQuery(); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { conn.Close(); scmd = null; conn = null; }

            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        { Close(); }

    }
}