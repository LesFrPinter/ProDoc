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
    public partial class InkColors : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) 
         { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

        // Test data added for this screen ; remove later
        //UPDATE FEATURES SET FINISH_MATL = 10, PRESS_MATL = 20, CONV_MATL = 30 where CATEGORY = 'INK' AND PRESS_SIZE = '11'
        //UPDATE FEATURES SET RUN_CHARGE = 4.00 WHERE CATEGORY = 'INK' AND PRESS_SIZE = '11' AND F_TYPE = 'SPLIT FTN'
        // Note that only SP press sizes have values for "FourColor"

        #region Properties

        private int    maxColors;   public int    MaxColors   { get { return maxColors;   } set { maxColors   = value; OnPropertyChanged(); } }
        private string pressSize;   public string PressSize   { get { return pressSize;   } set { pressSize   = value; OnPropertyChanged(); } }
        private string quoteNum;    public string QuoteNum    { get { return quoteNum;    } set { quoteNum    = value; OnPropertyChanged(); } }
        private string quoteNo;     public string QuoteNo     { get { return quoteNo;     } set { quoteNo     = value; OnPropertyChanged(); } }

        private int    std;         public int    Std         { get { return std;         } set { std         = value; OnPropertyChanged(); } }
        private int    blackStd;    public int    BlackStd    { get { return blackStd;    } set { blackStd    = value; OnPropertyChanged(); } }
        private int    pms;         public int    PMS         { get { return pms;         } set { pms         = value; OnPropertyChanged(); } }
        private int    desens;      public int    Desens      { get { return desens;      } set { desens      = value; OnPropertyChanged(); } }
        private int    split;       public int    Split       { get { return split;       } set { split       = value; OnPropertyChanged(); } }
        private int    thermo;      public int    Thermo      { get { return thermo;      } set { thermo      = value; OnPropertyChanged(); } }
        private int    fourColor;   public int    FourColor   { get { return fourColor;   } set { fourColor   = value; OnPropertyChanged(); } }
        private int    waterMark;   public int    WaterMark   { get { return waterMark;   } set { waterMark   = value; OnPropertyChanged(); } }
        private int    fluorSel;    public int    FluorSel    { get { return fluorSel;    } set { fluorSel    = value; OnPropertyChanged(); } }

        private float  stdCharge   = 0.00F; public float  StdCharge   { get { return stdCharge;   } set { stdCharge   = value; OnPropertyChanged(); } }
        private float  blackstdChg = 0.00F; public float  BlackStdChg { get { return blackstdChg; } set { blackstdChg = value; OnPropertyChanged(); } }
        private float  pmsChg      = 0.00F; public float  PMSChg      { get { return pmsChg;      } set { pmsChg      = value; OnPropertyChanged(); } }
        private float  desChg      = 0.00F; public float  DesChg      { get { return desChg;      } set { desChg      = value; OnPropertyChanged(); } }
        private float  splitChg    = 0.00F; public float  SplitChg    { get { return splitChg;    } set { splitChg    = value; OnPropertyChanged(); } }
        private float  thermoChg   = 0.00F; public float  ThermoChg   { get { return thermoChg;   } set { thermoChg   = value; OnPropertyChanged(); } }
        private float  fourChg     = 0.00F; public float  FourChg     { get { return fourChg;     } set { fourChg     = value; OnPropertyChanged(); } }
        private float  waterChg    = 0.00F; public float  WaterChg    { get { return waterChg;    } set { waterChg    = value; OnPropertyChanged(); } }
        private float  fluorChg    = 0.00F; public float  FluorChg    { get { return fluorChg;    } set { fluorChg    = value; OnPropertyChanged(); } }

        private float pt1; public float Pt1 { get { return pt1; } set { pt1 = value; OnPropertyChanged(); } }
        private float pt2; public float Pt2 { get { return pt2; } set { pt2 = value; OnPropertyChanged(); } }
        private float pt3; public float Pt3 { get { return pt3; } set { pt3 = value; OnPropertyChanged(); } }
        private float pt4; public float Pt4 { get { return pt4; } set { pt4 = value; OnPropertyChanged(); } }
        private float pt5; public float Pt5 { get { return pt5; } set { pt5 = value; OnPropertyChanged(); } }
        private float pt6; public float Pt6 { get { return pt6; } set { pt6 = value; OnPropertyChanged(); } }
        private float pt7; public float Pt7 { get { return pt7; } set { pt7 = value; OnPropertyChanged(); } }
        private float pt8; public float Pt8 { get { return pt8; } set { pt8 = value; OnPropertyChanged(); } }
        private float pt9; public float Pt9 { get { return pt9; } set { pt9 = value; OnPropertyChanged(); } }

        private float  totPT = 0.00F;   public float  TotPT       { get { return totPT;       } set { totPT       = value; OnPropertyChanged(); } }
        private float  totFlat = 0.00F; public float  TotFlat     { get { return totFlat;     } set { totFlat     = value; OnPropertyChanged(); } }
        private float  flatTotal;       public float  FlatTotal   { get { return flatTotal;   } set { flatTotal   = value; OnPropertyChanged(); } }
        private int    max;             public int    Max         { get { return max;         } set { max         = value; OnPropertyChanged(); } }
        private float  flat_charge;     public float  FLAT_CHARGE { get { return flat_charge; } set { flat_charge = value; OnPropertyChanged(); } }
        private float  total;           public float  Total       { get { return total;       } set { total       = value; OnPropertyChanged(); } }
        private string cmd;             public string Cmd         { get { return cmd;         } set { cmd         = value; OnPropertyChanged(); } }
        private string backer;          public string Backer      { get { return backer;      } set { backer      = value; OnPropertyChanged(); } }     // Read from Quote_Details

        private float backerFlatCharge; public float BackerFlatCharge { get { return backerFlatCharge; } set { backerFlatCharge = value; OnPropertyChanged(); } }            // Add to other charges in CalcTotal()
        private float backerRunCharge;  public float BackerRunCharge  { get { return backerRunCharge;  } set { backerRunCharge  = value; OnPropertyChanged(); } }            //               "
        private string backerDetails;   public string BackerDetails   { get { return backerDetails;    } set { backerDetails    = value; OnPropertyChanged(); } }            //               "

        private string fieldList;       public string FieldList   { get { return fieldList;   } set { fieldList   = value; OnPropertyChanged(); } }

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

        public string ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public SqlConnection? conn;
        public SqlDataAdapter? da;
        public DataTable? dt;
        public SqlCommand? scmd;

        #endregion

        public InkColors(string PRESSSIZE, string QUOTENUM)
        {
            InitializeComponent();
            this.DataContext = this;

            Title = "Quote #: " + QUOTENUM;
            QuoteNum  = QUOTENUM;
            PressSize = PRESSSIZE;

            FieldList = "F_TYPE, FLAT_CHARGE, RUN_CHARGE, PLATE_MATL, FINISH_MATL, CONV_MATL, PRESS_MATL";

            LoadMaxColors();
            LoadSavedDetails();

            PreviewKeyDown += (s, e) => { if (e.Key == Key.Escape) Close(); };
        }

        private void LoadSavedDetails()
        {
            string str = $"SELECT * FROM [ESTIMATING].[dbo].[Quote_Details] WHERE QUOTE_NUM = '{QuoteNum}' AND CATEGORY = 'Ink Color'";
            SqlConnection conn = new(ConnectionString);
            dt = new DataTable();
            da = new SqlDataAdapter(str, conn); da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                Std       = int.Parse(dt.Rows[0]["Value1"].ToString());
                BlackStd  = int.Parse(dt.Rows[0]["Value2"].ToString());
                PMS       = int.Parse(dt.Rows[0]["Value3"].ToString());
                Desens    = int.Parse(dt.Rows[0]["Value4"].ToString());
                Split     = int.Parse(dt.Rows[0]["Value5"].ToString());
                Thermo    = int.Parse(dt.Rows[0]["Value6"].ToString());
                FourColor = int.Parse(dt.Rows[0]["Value7"].ToString());
                WaterMark = int.Parse(dt.Rows[0]["Value8"].ToString());
                FluorSel  = int.Parse(dt.Rows[0]["Value9"].ToString());

                FlatChargePct   = int.Parse(dt.Rows[0]["FlatChargePct"].ToString());
                RunChargePct    = int.Parse(dt.Rows[0]["RunChargePct"].ToString());
                PlateChargePct  = int.Parse(dt.Rows[0]["PlateChargePct"].ToString());
                FinishChargePct = int.Parse(dt.Rows[0]["FinishChargePct"].ToString());
                PressChargePct  = int.Parse(dt.Rows[0]["PressChargePct"].ToString());
                ConvChargePct   = int.Parse(dt.Rows[0]["ConvertChargePct"].ToString());
            }
        }

        public void OnLoad(object sender, RoutedEventArgs e)
        {
            this.Height = this.Height *= 1.6;
            this.Width = this.Width *= 1.6;
        }

        private void LoadMaxColors()
        {
            PressSize = PressSize;

            SqlConnection cn = new(ConnectionString);
            string str = $"SELECT Number FROM [ESTIMATING].[dbo].[PressSizeColors] WHERE Size = '{PressSize}'";
            SqlDataAdapter da = new(str, cn); DataTable dt = new DataTable(); da.Fill(dt);
            MaxColors = Int32.Parse(dt.Rows[0]["Number"].ToString());

            // Retrieve value for Backer if one was previously entered 
            str = $"SELECT PARAM1, TotalFlatChg, PerThousandChg FROM [ESTIMATING].[dbo].[Quote_Details] WHERE QUOTE_NUM = '{QuoteNum}' AND CATEGORY = 'Backer'";
            da.SelectCommand.CommandText = str; dt.Rows.Clear(); da.Fill(dt);

            if(dt.Rows.Count > 0 ) 
            {   Backer = dt.Rows[0]["Param1"].ToString();
                float t1 = 0; float.TryParse(dt.Rows[0]["TotalFlatChg"].ToString(),   out t1); BackerFlatCharge = t1;
                float t2 = 0; float.TryParse(dt.Rows[0]["PerThousandChg"].ToString(), out t2); BackerRunCharge  = t2;
                BackerDetails = t1.ToString("c");
            }

            str = $"SELECT F_TYPE, MAX(Number) AS MaxColors" 
                + $" FROM [ESTIMATING].[dbo].[FEATURES] WHERE Category = 'INK' AND Press_Size = '{PressSize}' GROUP BY F_TYPE";
            da.SelectCommand.CommandText = str; dt.Rows.Clear(); da.Fill(dt);

            DataView dv = new DataView(dt);

            dv.RowFilter = "F_TYPE='STD'";           Std1.Maximum = int.Parse(dv[0]["MaxColors"].ToString());
            dv.RowFilter = "F_TYPE='BLK & STD'";     Blk1.Maximum = int.Parse(dv[0]["MaxColors"].ToString());
            dv.RowFilter = "F_TYPE='DESENSITIZED'";  Des1.Maximum = int.Parse(dv[0]["MaxColors"].ToString());
            dv.RowFilter = "F_TYPE='PMS'";           Pms1.Maximum = int.Parse(dv[0]["MaxColors"].ToString());
            dv.RowFilter = "F_TYPE='SPLIT FTN'";     Spl1.Maximum = int.Parse(dv[0]["MaxColors"].ToString());
            dv.RowFilter = "F_TYPE='THERMO'";        The1.Maximum = int.Parse(dv[0]["MaxColors"].ToString());
            dv.RowFilter = "F_TYPE='ART WATERMARK'"; Wat1.Maximum = int.Parse(dv[0]["MaxColors"].ToString());
            dv.RowFilter = "F_TYPE='FLUOR SEL'";     Flo1.Maximum = int.Parse(dv[0]["MaxColors"].ToString());
            dv.RowFilter = "F_TYPE='4 CLR PRO'";     if (dv.Count > 0) { Fou1.Maximum = int.Parse(dv[0]["MaxColors"].ToString()); }
        }

        private void RadNumericUpDown_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        {
            if (backer == null) return;
            string chosen   = ((System.Windows.FrameworkElement)sender).Name;
            int backercount = (Backer.ToString().Length > 0) ? 1 : 0;
            int totalcount  = Std + BlackStd + PMS + Desens + Split + Thermo + FourColor + WaterMark + FluorSel + backercount;
            if (totalcount  > MaxColors) { MessageBox.Show("Total number of colors plus backer can't exceed " + MaxColors.ToString()); }
            GetCharges(chosen);
            CalcTotal();
        }

        private void GetCharges(string ctl)
        {
            string cmd = "SELECT F_TYPE, NUMBER, FLAT_CHARGE, RUN_CHARGE, PLATE_MATL, FINISH_MATL, PRESS_MATL, CONV_MATL"
                       + $"  FROM [ESTIMATING].[dbo].[FEATURES]"
                       + $" WHERE CATEGORY = 'INK'           AND PRESS_SIZE = '{PressSize}'"
                       + $"   AND ((F_TYPE = 'ART WATERMARK' AND Number = {WaterMark} )"
                       + $"    OR  (F_TYPE = 'BLK & STD'     AND Number = {BlackStd}  )"
                       + $"    OR  (F_TYPE = 'DESENSITIZED'  AND Number = {Desens}    )"
                       + $"    OR  (F_TYPE = 'FLUOR SEL'     AND Number = {FluorSel})"
                       + $"    OR  (F_TYPE = 'PMS'           AND Number = {PMS})"
                       + $"    OR  (F_TYPE = 'SPLIT FTN'     AND Number = {Split})"
                       + $"    OR  (F_TYPE = 'STD'           AND Number = {Std})"
                       + $"    OR  (F_TYPE = '4 CLR PRO'     AND Number = {FourColor})"
                       + $"    OR  (F_TYPE = 'THERMO'        AND Number = {Thermo}) )"
                       +  "    ORDER BY F_TYPE, NUMBER";

            conn = new SqlConnection(ConnectionString);
            da = new SqlDataAdapter(cmd, conn); dt = new DataTable(); da.Fill(dt);
            DataView dv = dt.DefaultView;

            BaseFlatCharge   = BackerFlatCharge;
            BaseRunCharge    = BackerRunCharge;
            BaseFinishCharge = 0;
            BaseConvCharge   = 0;
            BasePlateCharge  = 0;
            BasePressCharge  = 0;

            FlatTotal = 0;

            for (int i=0; i<dv.Count; i++) 
            {   float t1 = 0; float.TryParse(dv[i]["FLAT_CHARGE"].ToString(), out t1); BaseFlatCharge   += t1;
                float t2 = 0; float.TryParse(dv[i]["RUN_CHARGE"].ToString(),  out t2); BaseRunCharge    += t2;
                float t3 = 0; float.TryParse(dv[i]["FINISH_MATL"].ToString(), out t3); BaseFinishCharge += t3;
                float t4 = 0; float.TryParse(dv[i]["CONV_MATL"].ToString(),   out t4); BaseConvCharge   += t4;
                float t5 = 0; float.TryParse(dv[i]["PLATE_MATL"].ToString(),  out t5); BasePlateCharge  += t5;
                float t6 = 0; float.TryParse(dv[i]["PRESS_MATL"].ToString(),  out t6); BasePressCharge  += t6;
            }
            CalcTotal();
        }

        private void CalcTotal()
        {
            CalculatedFlatCharge   = BaseFlatCharge   * (1 + FlatChargePct   / 100);
            CalculatedRunCharge    = BaseRunCharge    * (1 + RunChargePct    / 100);
            CalculatedPlateCharge  = BasePlateCharge  * (1 + PlateChargePct  / 100);
            CalculatedFinishCharge = BaseFinishCharge * (1 + FinishChargePct / 100);
            CalculatedPressCharge  = BasePressCharge  * (1 + PressChargePct  / 100);
            CalculatedConvCharge   = BaseConvCharge   * (1 + ConvChargePct   / 100);

            FlatTotal = CalculatedFlatCharge + CalculatedPlateCharge + CalculatedFinishCharge + CalculatedPressCharge + CalculatedConvCharge;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // Delete current detail line
            string cmd = "DELETE [ESTIMATING].[dbo].[Quote_Details] WHERE Quote_Num = '" + QuoteNum + "' AND Category = 'Ink Color'";
            conn = new SqlConnection(ConnectionString); SqlCommand scmd = new SqlCommand(cmd, conn); conn.Open();

            try                  { scmd.ExecuteNonQuery();      }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally              { conn.Close();                }

            cmd = "INSERT INTO [ESTIMATING].[dbo].[Quote_Details] ("
                +  "  Quote_Num,        Category,       Sequence,          Param1,          Param2,             Param3,              Param4,             Param5,            Param6,        Param7,        Param8,       Param9, "
                +  "                                                       Value1,          Value2,             Value3,              Value4,             Value5,            Value6,        Value7,        Value8,       Value9, " 
                +  "  Amount,           FlatCharge,     FlatChargePct,     RunChargePct,    PlateChargePct,     FinishChargePct,     PressChargePct,     ConvertChargePct,  TotalFlatChg,  PerThousandChg ) VALUES ( "
                + $" {QuoteNum},       'Ink Color',     2,                'Std',           'BlackStd',         'PMS',               'Desens',           'Split',           'Thermo',      'Watermark',   'FluorSel',   'FourColor', "
                + $"                                                     '{Std}',         '{BlackStd}',       '{PMS}',             '{Desens}',         '{Split}',         '{Thermo}',    '{WaterMark}', '{FluorSel}', '{FourColor}', "
                + $" '{FLAT_CHARGE}', '{FlatCharge}', '{FlatChargePct}', '{RunChargePct}','{PlateChargePct}', '{FinishChargePct}', '{PressChargePct}', '{ConvChargePct}', '{FlatTotal}', '{CalculatedRunCharge}' )";

            scmd.CommandText = cmd;
            conn.Open();
            try { scmd.ExecuteNonQuery(); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { conn.Close(); scmd = null; conn = null; }

            this.Close();
        }

        private void PctChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        { CalcTotal(); }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        { this.Close(); }

    }
}