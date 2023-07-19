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
        private string quoteNum;    public string QuoteNum  { get { return quoteNum;  } set { quoteNum  = value; OnPropertyChanged(); } }  // Which of these two
        private string quoteNo;     public string QuoteNo   { get { return quoteNo;   } set { quoteNo   = value; OnPropertyChanged(); } }  //  is no longer used?

        private int crash; public int Crash { get { return crash; } set { crash = value; OnPropertyChanged(); } }
        private int black; public int Black { get { return black; } set { black = value; OnPropertyChanged(); } }
        private int snap2; public int Snap2 { get { return snap2; } set { snap2 = value; OnPropertyChanged(); } }
        private int snap5; public int Snap5 { get { return snap5; } set { snap5 = value; OnPropertyChanged(); } }
        private int cont2; public int Cont2 { get { return cont2; } set { cont2 = value; OnPropertyChanged(); } }
        private int cont5; public int Cont5 { get { return cont5; } set { cont5 = value; OnPropertyChanged(); } }
        private int fold;  public int Fold  { get { return fold;  } set { fold  = value; OnPropertyChanged(); } }
        private int tape;  public int Tape  { get { return tape;  } set { tape  = value; OnPropertyChanged(); } }

        private float fc11; public float Fc11 { get { return fc11; } set { fc11 = value; OnPropertyChanged(); } }       // Flat charge for first f_type
        private float fc12; public float Fc12 { get { return fc12; } set { fc12 = value; OnPropertyChanged(); } }       // Flat charge for second f_type...
        private float fc13; public float Fc13 { get { return fc13; } set { fc13 = value; OnPropertyChanged(); } }
        private float fc14; public float Fc14 { get { return fc14; } set { fc14 = value; OnPropertyChanged(); } }
        private float fc15; public float Fc15 { get { return fc15; } set { fc15 = value; OnPropertyChanged(); } }
        private float fc16; public float Fc16 { get { return fc16; } set { fc16 = value; OnPropertyChanged(); } }

        private float fc21; public float Fc21 { get { return fc21; } set { fc21 = value; OnPropertyChanged(); } }       // Run charge for first f_type
        private float fc22; public float Fc22 { get { return fc22; } set { fc22 = value; OnPropertyChanged(); } }       // Run charge for second f_type...
        private float fc23; public float Fc23 { get { return fc23; } set { fc23 = value; OnPropertyChanged(); } }
        private float fc24; public float Fc24 { get { return fc24; } set { fc24 = value; OnPropertyChanged(); } }
        private float fc25; public float Fc25 { get { return fc25; } set { fc25 = value; OnPropertyChanged(); } }
        private float fc26; public float Fc26 { get { return fc26; } set { fc26 = value; OnPropertyChanged(); } }

        private float fc31; public float Fc31 { get { return fc31; } set { fc31 = value; OnPropertyChanged(); } }       // Run charge for first f_type
        private float fc32; public float Fc32 { get { return fc32; } set { fc32 = value; OnPropertyChanged(); } }       // Run charge for second f_type...
        private float fc33; public float Fc33 { get { return fc33; } set { fc33 = value; OnPropertyChanged(); } }
        private float fc34; public float Fc34 { get { return fc34; } set { fc34 = value; OnPropertyChanged(); } }
        private float fc35; public float Fc35 { get { return fc35; } set { fc35 = value; OnPropertyChanged(); } }
        private float fc36; public float Fc36 { get { return fc36; } set { fc36 = value; OnPropertyChanged(); } }

        private float fc41; public float Fc41 { get { return fc41; } set { fc41 = value; OnPropertyChanged(); } }       // Run charge for first f_type
        private float fc42; public float Fc42 { get { return fc42; } set { fc42 = value; OnPropertyChanged(); } }       // Run charge for second f_type...
        private float fc43; public float Fc43 { get { return fc43; } set { fc43 = value; OnPropertyChanged(); } }
        private float fc44; public float Fc44 { get { return fc44; } set { fc44 = value; OnPropertyChanged(); } }
        private float fc45; public float Fc45 { get { return fc45; } set { fc45 = value; OnPropertyChanged(); } }
        private float fc46; public float Fc46 { get { return fc46; } set { fc46 = value; OnPropertyChanged(); } }

        private float fc51; public float Fc51 { get { return fc51; } set { fc51 = value; OnPropertyChanged(); } }       // Run charge for first f_type
        private float fc52; public float Fc52 { get { return fc52; } set { fc52 = value; OnPropertyChanged(); } }       // Run charge for second f_type...
        private float fc53; public float Fc53 { get { return fc53; } set { fc53 = value; OnPropertyChanged(); } }
        private float fc54; public float Fc54 { get { return fc54; } set { fc54 = value; OnPropertyChanged(); } }
        private float fc55; public float Fc55 { get { return fc55; } set { fc55 = value; OnPropertyChanged(); } }
        private float fc56; public float Fc56 { get { return fc56; } set { fc56 = value; OnPropertyChanged(); } }

        private float fc61; public float Fc61 { get { return fc61; } set { fc61 = value; OnPropertyChanged(); } }       // Run charge for first f_type
        private float fc62; public float Fc62 { get { return fc62; } set { fc62 = value; OnPropertyChanged(); } }       // Run charge for second f_type...
        private float fc63; public float Fc63 { get { return fc63; } set { fc63 = value; OnPropertyChanged(); } }
        private float fc64; public float Fc64 { get { return fc64; } set { fc64 = value; OnPropertyChanged(); } }
        private float fc65; public float Fc65 { get { return fc65; } set { fc65 = value; OnPropertyChanged(); } }
        private float fc66; public float Fc66 { get { return fc66; } set { fc66 = value; OnPropertyChanged(); } }

        private float fc71; public float Fc71 { get { return fc71; } set { fc71 = value; OnPropertyChanged(); } }       // Run charge for first f_type
        private float fc72; public float Fc72 { get { return fc72; } set { fc72 = value; OnPropertyChanged(); } }       // Run charge for second f_type...
        private float fc73; public float Fc73 { get { return fc73; } set { fc73 = value; OnPropertyChanged(); } }
        private float fc74; public float Fc74 { get { return fc74; } set { fc74 = value; OnPropertyChanged(); } }
        private float fc75; public float Fc75 { get { return fc75; } set { fc75 = value; OnPropertyChanged(); } }
        private float fc76; public float Fc76 { get { return fc76; } set { fc76 = value; OnPropertyChanged(); } }

        private float fc81; public float Fc81 { get { return fc81; } set { fc81 = value; OnPropertyChanged(); } }       // Run charge for first f_type
        private float fc82; public float Fc82 { get { return fc82; } set { fc82 = value; OnPropertyChanged(); } }       // Run charge for second f_type...
        private float fc83; public float Fc83 { get { return fc83; } set { fc83 = value; OnPropertyChanged(); } }
        private float fc84; public float Fc84 { get { return fc84; } set { fc84 = value; OnPropertyChanged(); } }
        private float fc85; public float Fc85 { get { return fc85; } set { fc85 = value; OnPropertyChanged(); } }
        private float fc86; public float Fc86 { get { return fc86; } set { fc76 = value; OnPropertyChanged(); } }

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

            DataView dv  = new DataView(dt);
            dv.RowFilter = "F_TYPE='2 CRASH NUMB'";   M1.Maximum = int.Parse(dv[0]["Max"].ToString());
            dv.RowFilter = "F_TYPE='BLACK NUMB'";     M2.Maximum = int.Parse(dv[0]["Max"].ToString());
            dv.RowFilter = "F_TYPE='SNAP GLUE 2-4'";  M3.Maximum = int.Parse(dv[0]["Max"].ToString());
            dv.RowFilter = "F_TYPE='SNAP GLUE 5-12'"; M4.Maximum = int.Parse(dv[0]["Max"].ToString());
            dv.RowFilter = "F_TYPE='FOLDING'";        M5.Maximum = int.Parse(dv[0]["Max"].ToString());
            dv.RowFilter = "F_TYPE='CONT GLUE 2-4'";  M6.Maximum = int.Parse(dv[0]["Max"].ToString());
            dv.RowFilter = "F_TYPE='CONT GLUE 5-12'"; M7.Maximum = int.Parse(dv[0]["Max"].ToString());
            dv.RowFilter = "F_TYPE='TRANSFER TAPE'";  M8.Maximum = int.Parse(dv[0]["Max"].ToString());
            dt.Clear();

            str = $"SELECT * FROM [ESTIMATING].[dbo].[FEATURES] WHERE Category = 'Converting' AND Press_Size = '{PressSize}' ORDER BY F_TYPE";
            da.SelectCommand.CommandText = str;
            DataTable dt2 = new(); da.Fill(dt2);
            if (dt2.Rows.Count == 0) return;

            dv = dt2.DefaultView;

            float t = 0.00F;
            dv.RowFilter = "F_TYPE='2 CRASH NUMB'";
            t = 0.00F; float.TryParse(dv[0]["FLAT_CHARGE"].ToString(), out t); Fc11 = t;
            t = 0.00F; float.TryParse(dv[0]["RUN_CHARGE"].ToString(),  out t); Fc12 = t;
            t = 0.00F; float.TryParse(dv[0]["PLATE_MATL"].ToString(),  out t); Fc13 = t;
            t = 0.00F; float.TryParse(dv[0]["FINISH_MATL"].ToString(), out t); Fc14 = t;
            t = 0.00F; float.TryParse(dv[0]["PRESS_MATL"].ToString(),  out t); Fc15 = t;
            t = 0.00F; float.TryParse(dv[0]["CONV_MATL"].ToString(),   out t); Fc16 = t;

            dv.RowFilter = "F_TYPE='BLACK NUMB'";
            t = 0.00F; float.TryParse(dv[0]["FLAT_CHARGE"].ToString(), out t); Fc21 = t;
            t = 0.00F; float.TryParse(dv[0]["RUN_CHARGE"].ToString(),  out t); Fc22 = t;
            t = 0.00F; float.TryParse(dv[0]["PLATE_MATL"].ToString(),  out t); Fc23 = t;
            t = 0.00F; float.TryParse(dv[0]["FINISH_MATL"].ToString(), out t); Fc24 = t;
            t = 0.00F; float.TryParse(dv[0]["PRESS_MATL"].ToString(),  out t); Fc25 = t;
            t = 0.00F; float.TryParse(dv[0]["CONV_MATL"].ToString(),   out t); Fc26 = t;

            dv.RowFilter = "F_TYPE='SNAP GLUE 2-4'";
            t = 0.00F; float.TryParse(dv[0]["FLAT_CHARGE"].ToString(), out t); Fc31 = t;
            t = 0.00F; float.TryParse(dv[0]["RUN_CHARGE"].ToString(),  out t); Fc32 = t;
            t = 0.00F; float.TryParse(dv[0]["PLATE_MATL"].ToString(),  out t); Fc33 = t;
            t = 0.00F; float.TryParse(dv[0]["FINISH_MATL"].ToString(), out t); Fc34 = t;
            t = 0.00F; float.TryParse(dv[0]["PRESS_MATL"].ToString(),  out t); Fc35 = t;
            t = 0.00F; float.TryParse(dv[0]["CONV_MATL"].ToString(),   out t); Fc36 = t;

            dv.RowFilter = "F_TYPE='SNAP GLUE 5-12'";
            t = 0.00F; float.TryParse(dv[0]["FLAT_CHARGE"].ToString(), out t); Fc41 = t;
            t = 0.00F; float.TryParse(dv[0]["RUN_CHARGE"].ToString(),  out t); Fc42 = t;
            t = 0.00F; float.TryParse(dv[0]["PLATE_MATL"].ToString(),  out t); Fc43 = t;
            t = 0.00F; float.TryParse(dv[0]["FINISH_MATL"].ToString(), out t); Fc44 = t;
            t = 0.00F; float.TryParse(dv[0]["PRESS_MATL"].ToString(),  out t); Fc45 = t;
            t = 0.00F; float.TryParse(dv[0]["CONV_MATL"].ToString(),   out t); Fc46 = t;

            dv.RowFilter = "F_TYPE='FOLDING'";
            t = 0.00F; float.TryParse(dv[0]["FLAT_CHARGE"].ToString(), out t); Fc51 = t;
            t = 0.00F; float.TryParse(dv[0]["RUN_CHARGE"].ToString(),  out t); Fc52 = t;
            t = 0.00F; float.TryParse(dv[0]["PLATE_MATL"].ToString(),  out t); Fc53 = t;
            t = 0.00F; float.TryParse(dv[0]["FINISH_MATL"].ToString(), out t); Fc54 = t;
            t = 0.00F; float.TryParse(dv[0]["PRESS_MATL"].ToString(),  out t); Fc55 = t;
            t = 0.00F; float.TryParse(dv[0]["CONV_MATL"].ToString(),   out t); Fc56 = t;

            dv.RowFilter = "F_TYPE='CONT GLUE 2-4'";
            t = 0.00F; float.TryParse(dv[0]["FLAT_CHARGE"].ToString(), out t); Fc61 = t;
            t = 0.00F; float.TryParse(dv[0]["RUN_CHARGE"].ToString(),  out t); Fc62 = t;
            t = 0.00F; float.TryParse(dv[0]["PLATE_MATL"].ToString(),  out t); Fc63 = t;
            t = 0.00F; float.TryParse(dv[0]["FINISH_MATL"].ToString(), out t); Fc64 = t;
            t = 0.00F; float.TryParse(dv[0]["PRESS_MATL"].ToString(),  out t); Fc65 = t;
            t = 0.00F; float.TryParse(dv[0]["CONV_MATL"].ToString(),   out t); Fc66 = t;

            dv.RowFilter = "F_TYPE='CONT GLUE 2-4'";
            t = 0.00F; float.TryParse(dv[0]["FLAT_CHARGE"].ToString(), out t); Fc71 = t;
            t = 0.00F; float.TryParse(dv[0]["RUN_CHARGE"].ToString(),  out t); Fc72 = t;
            t = 0.00F; float.TryParse(dv[0]["PLATE_MATL"].ToString(),  out t); Fc73 = t;
            t = 0.00F; float.TryParse(dv[0]["FINISH_MATL"].ToString(), out t); Fc74 = t;
            t = 0.00F; float.TryParse(dv[0]["PRESS_MATL"].ToString(),  out t); Fc75 = t;
            t = 0.00F; float.TryParse(dv[0]["CONV_MATL"].ToString(),   out t); Fc76 = t;

            dv.RowFilter = "F_TYPE='TRANSFER TAPE'";
            t = 0.00F; float.TryParse(dv[0]["FLAT_CHARGE"].ToString(), out t); Fc81 = t;
            t = 0.00F; float.TryParse(dv[0]["RUN_CHARGE"].ToString(),  out t); Fc82 = t;
            t = 0.00F; float.TryParse(dv[0]["PLATE_MATL"].ToString(),  out t); Fc83 = t;
            t = 0.00F; float.TryParse(dv[0]["FINISH_MATL"].ToString(), out t); Fc84 = t;
            t = 0.00F; float.TryParse(dv[0]["PRESS_MATL"].ToString(),  out t); Fc85 = t;
            t = 0.00F; float.TryParse(dv[0]["CONV_MATL"].ToString(),   out t); Fc86 = t;
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

            CalculatedFlatCharge = 0.00F;
            CalculatedRunCharge = 0.00F;
            CalculatedPlateCharge = 0.00F;
            CalculatedFinishCharge = 0.00F;
            CalculatedPressCharge = 0.00F;
            CalculatedConvCharge = 0.00F;
        }

        private void CalcTotal()
        {
            CalculatedFlatCharge = Crash * (Fc11 * (1 + (FlatChargePct / 100)))
                                 + Black * (Fc21 * (1 + (FlatChargePct / 100)))
                                 + Snap2 * (Fc31 * (1 + (FlatChargePct / 100)))
                                 + Snap5 * (Fc41 * (1 + (FlatChargePct / 100)))
                                 + Cont2 * (Fc51 * (1 + (FlatChargePct / 100)))
                                 + Cont5 * (Fc61 * (1 + (FlatChargePct / 100)))
                                 + Fold  * (Fc71 * (1 + (FlatChargePct / 100)))
                                 + Tape  * (Fc81 * (1 + (FlatChargePct / 100)));

            CalculatedRunCharge = Crash * (Fc12 * (1 + (RunChargePct / 100)))
                                + Black * (Fc22 * (1 + (RunChargePct / 100)))
                                + Snap2 * (Fc32 * (1 + (RunChargePct / 100)))
                                + Snap5 * (Fc42 * (2 + (RunChargePct / 100)))
                                + Cont2 * (Fc52 * (2 + (RunChargePct / 100)))
                                + Cont5 * (Fc62 * (1 + (RunChargePct / 100)))
                                + Fold  * (Fc72 * (1 + (RunChargePct / 100)))
                                + Tape  * (Fc82 * (1 + (RunChargePct / 100)));

            CalculatedPlateCharge = 
                                  Crash * (Fc13 * (1 + (PlateChargePct / 100)))
                                + Black * (Fc23 * (1 + (PlateChargePct / 100)))
                                + Snap2 * (Fc33 * (1 + (PlateChargePct / 100)))
                                + Snap5 * (Fc43 * (2 + (PlateChargePct / 100)))
                                + Cont2 * (Fc53 * (2 + (PlateChargePct / 100)))
                                + Cont5 * (Fc63 * (1 + (PlateChargePct / 100)))
                                + Fold  * (Fc73 * (1 + (PlateChargePct / 100)))
                                + Tape  * (Fc83 * (1 + (PlateChargePct / 100)));

            CalculatedFinishCharge =
                                  Crash * (Fc14 * (1 + (FinishChargePct / 100)))
                                + Black * (Fc24 * (1 + (FinishChargePct / 100)))
                                + Snap2 * (Fc34 * (1 + (FinishChargePct / 100)))
                                + Snap5 * (Fc44 * (2 + (FinishChargePct / 100)))
                                + Cont2 * (Fc54 * (2 + (FinishChargePct / 100)))
                                + Cont5 * (Fc64 * (1 + (FinishChargePct / 100)))
                                + Fold  * (Fc74 * (1 + (FinishChargePct / 100)))
                                + Tape  * (Fc84 * (1 + (FinishChargePct / 100)));

            CalculatedPressCharge =
                                  Crash * (Fc15 * (1 + (PressChargePct / 100)))
                                + Black * (Fc25 * (1 + (PressChargePct / 100)))
                                + Snap2 * (Fc35 * (1 + (PressChargePct / 100)))
                                + Snap5 * (Fc45 * (2 + (PressChargePct / 100)))
                                + Cont2 * (Fc55 * (2 + (PressChargePct / 100)))
                                + Cont5 * (Fc65 * (1 + (PressChargePct / 100)))
                                + Fold  * (Fc75 * (1 + (PressChargePct / 100)))
                                + Tape  * (Fc85 * (1 + (PressChargePct / 100)));

            CalculatedConvCharge =
                                  Crash * (Fc16 * (1 + (ConvChargePct / 100)))
                                + Black * (Fc26 * (1 + (ConvChargePct / 100)))
                                + Snap2 * (Fc36 * (1 + (ConvChargePct / 100)))
                                + Snap5 * (Fc46 * (2 + (ConvChargePct / 100)))
                                + Cont2 * (Fc56 * (2 + (ConvChargePct / 100)))
                                + Cont5 * (Fc66 * (1 + (ConvChargePct / 100)))
                                + Fold  * (Fc76 * (1 + (ConvChargePct / 100)))
                                + Tape  * (Fc86 * (1 + (ConvChargePct / 100)));

            FlatTotal = CalculatedFlatCharge + CalculatedPlateCharge + CalculatedFinishCharge + CalculatedPressCharge + CalculatedConvCharge;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // Validate inputs
            // e.g., if ((Button1 + Button2 + Button3) == 0) { MessageBox.Show("Please select an option."); return; }

            // Delete current detail line
            string cmd = "DELETE [ESTIMATING].[dbo].[Quote_Details] WHERE Quote_Num = '" + QuoteNum + "' AND Category = 'Converting'";
            conn = new SqlConnection(ConnectionString); SqlCommand scmd = new SqlCommand(cmd, conn); conn.Open();

            try { scmd.ExecuteNonQuery(); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { conn.Close(); }

            // Store in Quote_Detail table:
            cmd = "INSERT INTO [ESTIMATING].[dbo].[Quote_Details] ("
                + " Quote_Num, Category, Sequence,"
                + " Param1, Param2, Param3, Param4, Param5, Param6, Param7, Param8, "
                + " Value1, Value2, Value3, Value4, Value5, Value6, Value7, Value8 ) VALUES ( "
                + $"'{QuoteNum}', 'Converting', 9, "
                + " '2 CRASH NUMB', 'BLACK NUMB', 'SNAP GLUE 2-4', 'SNAP GLUE 5-12', "
                + " 'CONT GLUE 2-4', 'CONT GLUE 5-12', 'FOLDING', 'TRANSFER TAPE', "
                + $" '{Crash}', '{Black}', '{Snap2}', '{Snap5}', "
                + $" '{Cont2}', '{Cont5}', '{Fold}',  '{Tape}' )";

            // Write to SQL
            //  conn = new SqlConnection(ConnectionString);
            scmd.CommandText = cmd;
            conn.Open();
            try { scmd.ExecuteNonQuery(); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { conn.Close(); scmd = null; conn = null; }

            this.Close();
        }

        private void Pct_Changed(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        { CalcTotal(); }

        private void ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        { CalcTotal(); }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        { Close(); }

    }
}