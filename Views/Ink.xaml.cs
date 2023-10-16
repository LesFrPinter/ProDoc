using ProDocEstimate.Editors;
using SharpDX.Direct2D1;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Controls.FieldList;
using Telerik.Windows.Documents.Spreadsheet.Expressions.Functions;
using Xceed.Wpf.Toolkit;

namespace ProDocEstimate.Views
{
    public partial class Ink : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

        #region Properties

        // Test data that I added:
        //UPDATE FEATURES
        //   SET
        //    PRESS_SETUP_TIME = 5,
        //    COLLATOR_SETUP = 10,
        //    BINDERY_SETUP = 15,
        //    PRESS_SLOWDOWN = 20,
        //    COLLATOR_SLOWDOWN = 25,
        //    BINDERY_SLOWDOWN = 30
        // WHERE CATEGORY = 'INK'
        //   AND PRESS_SIZE = '11'

        private float makeReadyInkCost; public float MakeReadyInkCost { get { return makeReadyInkCost; } set { makeReadyInkCost = value; OnPropertyChanged(); } }

        private int    maxColors; public int    MaxColors   { get { return maxColors; } set { maxColors = value; OnPropertyChanged(); } }
        private string pressSize; public string PressSize   { get { return pressSize; } set { pressSize = value; OnPropertyChanged(); } }
        private string quoteNum;  public string QuoteNum    { get { return quoteNum;  } set { quoteNum  = value; OnPropertyChanged(); } }
        private string quoteNo;   public string QuoteNo     { get { return quoteNo;   } set { quoteNo   = value; OnPropertyChanged(); } }
        private int    parts;     public int    Parts       { get { return parts;     } set { parts     = value; OnPropertyChanged(); } }
        private string collcut;   public string CollatorCut { get { return collcut;   } set { collcut   = value; OnPropertyChanged(); } }

        private int std; public int Std { get { return std; } set { std = value; OnPropertyChanged(); } }
        private int blackStd; public int BlackStd { get { return blackStd; } set { blackStd = value; OnPropertyChanged(); } }
        private int pms; public int PMS { get { return pms; } set { pms = value; OnPropertyChanged(); } }
        private int desens; public int Desens { get { return desens; } set { desens = value; OnPropertyChanged(); } }
        private int split; public int Split { get { return split; } set { split = value; OnPropertyChanged(); } }
        private int thermo; public int Thermo { get { return thermo; } set { thermo = value; OnPropertyChanged(); } }
        private int fourColor; public int FourColor { get { return fourColor; } set { fourColor = value; OnPropertyChanged(); } }
        private int waterMark; public int WaterMark { get { return waterMark; } set { waterMark = value; OnPropertyChanged(); } }
        private int fluorSel; public int FluorSel { get { return fluorSel; } set { fluorSel = value; OnPropertyChanged(); } }

        private float stdCharge = 0.00F; public float StdCharge { get { return stdCharge; } set { stdCharge = value; OnPropertyChanged(); } }
        private float blackstdChg = 0.00F; public float BlackStdChg { get { return blackstdChg; } set { blackstdChg = value; OnPropertyChanged(); } }
        private float pmsChg = 0.00F; public float PMSChg { get { return pmsChg; } set { pmsChg = value; OnPropertyChanged(); } }
        private float desChg = 0.00F; public float DesChg { get { return desChg; } set { desChg = value; OnPropertyChanged(); } }
        private float splitChg = 0.00F; public float SplitChg { get { return splitChg; } set { splitChg = value; OnPropertyChanged(); } }
        private float thermoChg = 0.00F; public float ThermoChg { get { return thermoChg; } set { thermoChg = value; OnPropertyChanged(); } }
        private float fourChg = 0.00F; public float FourChg { get { return fourChg; } set { fourChg = value; OnPropertyChanged(); } }
        private float waterChg = 0.00F; public float WaterChg { get { return waterChg; } set { waterChg = value; OnPropertyChanged(); } }
        private float fluorChg = 0.00F; public float FluorChg { get { return fluorChg; } set { fluorChg = value; OnPropertyChanged(); } }

        private float pt1; public float Pt1 { get { return pt1; } set { pt1 = value; OnPropertyChanged(); } }
        private float pt2; public float Pt2 { get { return pt2; } set { pt2 = value; OnPropertyChanged(); } }
        private float pt3; public float Pt3 { get { return pt3; } set { pt3 = value; OnPropertyChanged(); } }
        private float pt4; public float Pt4 { get { return pt4; } set { pt4 = value; OnPropertyChanged(); } }
        private float pt5; public float Pt5 { get { return pt5; } set { pt5 = value; OnPropertyChanged(); } }
        private float pt6; public float Pt6 { get { return pt6; } set { pt6 = value; OnPropertyChanged(); } }
        private float pt7; public float Pt7 { get { return pt7; } set { pt7 = value; OnPropertyChanged(); } }
        private float pt8; public float Pt8 { get { return pt8; } set { pt8 = value; OnPropertyChanged(); } }
        private float pt9; public float Pt9 { get { return pt9; } set { pt9 = value; OnPropertyChanged(); } }

        private float totPT = 0.00F; public float TotPT { get { return totPT; } set { totPT = value; OnPropertyChanged(); } }
        private float totFlat = 0.00F; public float TotFlat { get { return totFlat; } set { totFlat = value; OnPropertyChanged(); } }
        private float flatTotal; public float FlatTotal { get { return flatTotal; } set { flatTotal = value; OnPropertyChanged(); } }
        private int max; public int Max { get { return max; } set { max = value; OnPropertyChanged(); } }
        private float flat_charge; public float FLAT_CHARGE { get { return flat_charge; } set { flat_charge = value; OnPropertyChanged(); } }
        private float total; public float Total { get { return total; } set { total = value; OnPropertyChanged(); } }
        private string cmd; public string Cmd { get { return cmd; } set { cmd = value; OnPropertyChanged(); } }
        private string backer; public string Backer { get { return backer; } set { backer = value; OnPropertyChanged(); } }     // Read from Quote_Details

        private int backerCount; public int BackerCount { get { return backerCount; } set { backerCount = value; OnPropertyChanged(); } }
        private int totalCount;  public int TotalCount  { get { return totalCount;  } set { totalCount  = value; OnPropertyChanged(); } }

        private float backerFlatCharge; public float BackerFlatCharge { get { return backerFlatCharge; } set { backerFlatCharge = value; OnPropertyChanged(); } }            // Add to other charges in CalcTotal()
        private float backerRunCharge; public float BackerRunCharge { get { return backerRunCharge; } set { backerRunCharge = value; OnPropertyChanged(); } }            //               "
        private string backerDetails; public string BackerDetails { get { return backerDetails; } set { backerDetails = value; OnPropertyChanged(); } }            //               "

        private string fieldList; public string FieldList { get { return fieldList; } set { fieldList = value; OnPropertyChanged(); } }

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

        // Labor percentages in NumericUpDowns
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

        //private int labPSS1Pct; public int LabPSS1Pct { get { return labPSS1Pct; } set { labPSS1Pct = value; OnPropertyChanged(); } }
        //private int labCSS2Pct; public int LabCSS2Pct { get { return labCSS2Pct; } set { labCSS2Pct = value; OnPropertyChanged(); } }
        //private int labBSS2Pct; public int LabBSS2Pct { get { return labBSS2Pct; } set { labBSS2Pct = value; OnPropertyChanged(); } }

        private int calculatedPS; public int CalculatedPS { get { return calculatedPS; } set { calculatedPS = value; OnPropertyChanged(); } }
        private int calculatedCS; public int CalculatedCS { get { return calculatedCS; } set { calculatedCS = value; OnPropertyChanged(); } }
        private int calculatedBS; public int CalculatedBS { get { return calculatedBS; } set { calculatedBS = value; OnPropertyChanged(); } }

        private int slowdownTotal; public int SlowdownTotal { get { return slowdownTotal; } set { slowdownTotal = value; OnPropertyChanged(); } }
        private int setupTotal; public int SetupTotal { get { return setupTotal; } set { setupTotal = value; OnPropertyChanged(); } }

        #endregion

        public Ink(string PRESSSIZE, string QUOTENUM, int PARTS, string COLLATORCUT)
        {
            InitializeComponent();
            this.DataContext = this;

            Title = "Quote #: " + QUOTENUM;
            QuoteNum = QUOTENUM;
            PressSize = PRESSSIZE;
            Parts = PARTS;
            CollatorCut = COLLATORCUT;

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
                int I1 = 0;
                int I2 = 0;
                int I3 = 0;
                int I4 = 0;
                int I5 = 0;
                int I6 = 0;
                int I7 = 0;
                int I8 = 0;
                int I9 = 0;

                int.TryParse(dt.Rows[0]["Value1"].ToString(), out I1);
                int.TryParse(dt.Rows[0]["Value2"].ToString(), out I2);
                int.TryParse(dt.Rows[0]["Value3"].ToString(), out I3);
                int.TryParse(dt.Rows[0]["Value4"].ToString(), out I4);
                int.TryParse(dt.Rows[0]["Value5"].ToString(), out I5);
                int.TryParse(dt.Rows[0]["Value6"].ToString(), out I6);
                int.TryParse(dt.Rows[0]["Value7"].ToString(), out I7);
                int.TryParse(dt.Rows[0]["Value8"].ToString(), out I8);
                int.TryParse(dt.Rows[0]["Value9"].ToString(), out I9);

                Std = I1;
                BlackStd = I2;
                PMS = I3;
                Desens = I4;
                Split = I5;
                Thermo = I6;
                FourColor = I7;
                WaterMark = I8;
                FluorSel = I9;

                int.TryParse(dt.Rows[0]["FlatChargePct"]   .ToString(), out I1);
                int.TryParse(dt.Rows[0]["RunChargePct"]    .ToString(), out I2);
                int.TryParse(dt.Rows[0]["PlateChargePct"]  .ToString(), out I3);
                int.TryParse(dt.Rows[0]["FinishChargePct"] .ToString(), out I4);
                int.TryParse(dt.Rows[0]["PressChargePct"]  .ToString(), out I5);
                int.TryParse(dt.Rows[0]["ConvertChargePct"].ToString(), out I6);

                FlatChargePct = I1;
                RunChargePct = I2;
                PlateChargePct = I3;
                FinishChargePct = I4;
                PressChargePct = I5;
                ConvChargePct = I6;

                int.TryParse(dt.Rows[0]["PRESS_ADDL_MIN"].ToString(), out I1);
                int.TryParse(dt.Rows[0]["COLL_ADDL_MIN"] .ToString(), out I2);
                int.TryParse(dt.Rows[0]["BIND_ADDL_MIN"] .ToString(), out I3);
                int.TryParse(dt.Rows[0]["PRESS_SLOW_PCT"].ToString(), out I4);
                int.TryParse(dt.Rows[0]["COLL_SLOW_PCT"] .ToString(), out I5);
                int.TryParse(dt.Rows[0]["BIND_SLOW_PCT"] .ToString(), out I6);

                LabPS = I1;
                LabCS = I2;
                LabBS = I3;
                LabPSL = I4;
                LabCSL = I5;
                LabBSL = I6;
            }
        }

        public void OnLoad(object sender, RoutedEventArgs e)
        {
            this.Height = this.Height *= 1.6;
            this.Width = this.Width *= 1.6;
            Top = 50;
        }

        private void LoadMaxColors()
        {
//            PressSize = PRESSSIZE;

            SqlConnection cn = new(ConnectionString);
            string str = $"SELECT Number FROM [ESTIMATING].[dbo].[PressSizeColors] WHERE Size = '{PressSize}'";
            SqlDataAdapter da = new(str, cn); DataTable dt = new DataTable(); da.Fill(dt);
            MaxColors = Int32.Parse(dt.Rows[0]["Number"].ToString());

            Backer = "";
            // Retrieve value for Backer if one was previously entered 
            str = $"SELECT PARAM1, TotalFlatChg, PerThousandChg FROM [ESTIMATING].[dbo].[Quote_Details] WHERE QUOTE_NUM = '{QuoteNum}' AND CATEGORY = 'Backer'";
            da.SelectCommand.CommandText = str; dt.Rows.Clear(); da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                Backer = dt.Rows[0]["Param1"].ToString();
                float t1 = 0; float.TryParse(dt.Rows[0]["TotalFlatChg"].ToString(), out t1); BackerFlatCharge = t1;
                float t2 = 0; float.TryParse(dt.Rows[0]["PerThousandChg"].ToString(), out t2); BackerRunCharge = t2;
                BackerDetails = t1.ToString("c");
            }

            str = $"SELECT F_TYPE, MAX(Number) AS MaxColors"
                + $" FROM [ESTIMATING].[dbo].[FEATURES] WHERE Category = 'INK' AND Press_Size = '{PressSize}' GROUP BY F_TYPE";
            da.SelectCommand.CommandText = str; dt.Rows.Clear(); da.Fill(dt);

            DataView dv = new DataView(dt);

            dv.RowFilter = "F_TYPE='STD'"; Std1.Maximum = int.Parse(dv[0]["MaxColors"].ToString());
            dv.RowFilter = "F_TYPE='BLK & STD'"; Blk1.Maximum = int.Parse(dv[0]["MaxColors"].ToString());
            dv.RowFilter = "F_TYPE='DESENSITIZED'"; Des1.Maximum = int.Parse(dv[0]["MaxColors"].ToString());
            dv.RowFilter = "F_TYPE='PMS'"; Pms1.Maximum = int.Parse(dv[0]["MaxColors"].ToString());
            dv.RowFilter = "F_TYPE='SPLIT FTN'"; Spl1.Maximum = int.Parse(dv[0]["MaxColors"].ToString());
            dv.RowFilter = "F_TYPE='THERMO'"; The1.Maximum = int.Parse(dv[0]["MaxColors"].ToString());
            dv.RowFilter = "F_TYPE='ART WATERMARK'"; Wat1.Maximum = int.Parse(dv[0]["MaxColors"].ToString());
            dv.RowFilter = "F_TYPE='FLUOR SEL'"; Flo1.Maximum = int.Parse(dv[0]["MaxColors"].ToString());
            dv.RowFilter = "F_TYPE='4 CLR PRO'"; if (dv.Count > 0) { Fou1.Maximum = int.Parse(dv[0]["MaxColors"].ToString()); }
        }

        private void RadNumericUpDown_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        {
            //TODO: This is being called ten times, once for each numeric ink count.
            string s = sender.ToString(); // Add to Watch window...

            if (e.OldValue == null && e.NewValue == 0) return;

            BackerCount = (Backer.ToString().Length > 0) ? 1 : 0;
            TotalCount  = Std + BlackStd + PMS + Desens + Split + Thermo + FourColor + WaterMark + FluorSel + BackerCount;
            if (TotalCount > MaxColors) { System.Windows.MessageBox.Show("Total number of colors plus backer can't exceed " + MaxColors.ToString()); return; }

            GetCharges();
            CalcTotal();

            // Calculate the Cost per pound using the new Ink_Cost table and store it in Press_Materials for later use in the Quotation calculation

            string cmd = ""; string root = "";
            if (PressSize.ToString().Contains("SP"))
                { root = "SELECT Ink, CostPerInch, MakeReady, CostPerLb FROM [ESTIMATING].[dbo].[Ink_Cost] WHERE (SP = 'SP')"; }
            else
                { root = "SELECT Ink, CostPerInch, MakeReady, CostPerLb FROM [ESTIMATING].[dbo].[Ink_Cost] WHERE (SP = '  ')"; }

            string addon = " AND (";
            if (Std       > 0) { addon += "(Ink = 'STD')           OR"; }
            if (BlackStd  > 0) { addon += "(Ink = 'BLK & STD')     OR"; }
            if (PMS       > 0) { addon += "(Ink = 'PMS')           OR"; }
            if (Thermo    > 0) { addon += "(Ink = 'THERMO')        OR"; }
            if (Desens    > 0) { addon += "(Ink = 'DESENSITIZED')  OR"; }
            if (Split     > 0) { addon += "(Ink = 'SPLIT FTN')     OR"; }
            if (WaterMark > 0) { addon += "(Ink = 'WATERMARK')     OR"; }
            if (FluorSel  > 0) { addon += "(Ink = 'FLUOR SEL')     OR"; }
            if (FourColor > 0) { addon += "(Ink = '4 CLR PRO')     OR"; }

            addon = addon.Substring(0, addon.Length - 3) + ")";
            if (addon.Length < 10) return;

            cmd = root + addon;
            conn = new SqlConnection(ConnectionString);
            SqlDataAdapter da = new SqlDataAdapter(cmd, conn);
            DataTable dtInkCost = new DataTable(); da.Fill(dtInkCost); DataView dvInkCost = dtInkCost.DefaultView;

            float TotalInkCost = 0.0F; 
            MakeReadyInkCost   = 0.0F;

            for(int i=0; i<dvInkCost.Count; i++)
            {
                if (dvInkCost[i]["Ink"].ToString() == "STD")           
                { TotalInkCost     += (Std       * float.Parse(dvInkCost[i]["CostPerInch"].ToString())); 
                  MakeReadyInkCost += (Std       * float.Parse(dvInkCost[i]["MakeReady"]  .ToString())) * float.Parse(dvInkCost[i]["CostPerLb"].ToString()); }

                if (dvInkCost[i]["Ink"].ToString() == "BLK & STD")     
                { TotalInkCost     += (BlackStd  * float.Parse(dvInkCost[i]["CostPerInch"].ToString()));
                  MakeReadyInkCost += (BlackStd  * float.Parse(dvInkCost[i]["MakeReady"]  .ToString())) * float.Parse(dvInkCost[i]["CostPerLb"].ToString()); }

                if (dvInkCost[i]["Ink"].ToString() == "PMS")           
                { TotalInkCost     += (PMS       * float.Parse(dvInkCost[i]["CostPerInch"].ToString()));
                  MakeReadyInkCost += (PMS       * float.Parse(dvInkCost[i]["MakeReady"]  .ToString())) * float.Parse(dvInkCost[i]["CostPerLb"].ToString()); }

                if (dvInkCost[i]["Ink"].ToString() == "THERMO")        
                { TotalInkCost     += (Thermo    * float.Parse(dvInkCost[i]["CostPerInch"].ToString()));
                  MakeReadyInkCost += (Thermo    * float.Parse(dvInkCost[i]["MakeReady"]  .ToString())) * float.Parse(dvInkCost[i]["CostPerLb"].ToString()); }

                if (dvInkCost[i]["Ink"].ToString() == "DESENSITIZED")
                { TotalInkCost     += (Desens    * float.Parse(dvInkCost[i]["CostPerInch"].ToString()));
                  MakeReadyInkCost += (Desens    * float.Parse(dvInkCost[i]["MakeReady"]  .ToString())) * float.Parse(dvInkCost[i]["CostPerLb"].ToString()); }

                if (dvInkCost[i]["Ink"].ToString() == "SPLIT FTN")
                { TotalInkCost     += (Split     * float.Parse(dvInkCost[i]["CostPerInch"].ToString()));
                  MakeReadyInkCost += (Split     * float.Parse(dvInkCost[i]["MakeReady"]  .ToString())) * float.Parse(dvInkCost[i]["CostPerLb"].ToString()); }

                if (dvInkCost[i]["Ink"].ToString() == "WATERMARK")
                { TotalInkCost     += (WaterMark * float.Parse(dvInkCost[i]["CostPerInch"].ToString()));
                  MakeReadyInkCost += (WaterMark * float.Parse(dvInkCost[i]["MakeReady"]  .ToString())) * float.Parse(dvInkCost[i]["CostPerLb"].ToString()); }

                if (dvInkCost[i]["Ink"].ToString() == "FLUOR SEL")
                { TotalInkCost     += (FluorSel * float.Parse(dvInkCost[i]["CostPerInch"].ToString()));
                  MakeReadyInkCost += (FluorSel * float.Parse(dvInkCost[i]["MakeReady"]  .ToString()))  * float.Parse(dvInkCost[i]["CostPerLb"].ToString()); }

                if (dvInkCost[i]["Ink"].ToString() == "4 CLR PRO")
                { TotalInkCost     += (FourColor * float.Parse(dvInkCost[i]["CostPerInch"].ToString()));
                  MakeReadyInkCost += (FourColor * float.Parse(dvInkCost[i]["MakeReady"]  .ToString())) * float.Parse(dvInkCost[i]["CostPerLb"].ToString()); }
            }

            BasePressCharge = TotalInkCost * Parts;
            CalcTotal(); // If this isn't here, it fills in the bogus test data that I loaded for Press Material...
        }

        private void GetCharges()
        {
            string cmd =  "SELECT * FROM [ESTIMATING].[dbo].[FEATURES]"
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
                       + "    ORDER BY F_TYPE, NUMBER";

            conn = new SqlConnection(ConnectionString);
            da = new SqlDataAdapter(cmd, conn); dt = new DataTable(); da.Fill(dt);
            DataView dv = dt.DefaultView;

            BaseFlatCharge = BackerFlatCharge;
            BaseRunCharge = BackerRunCharge;
            BaseFinishCharge = 0;
            BaseConvCharge = 0;
            BasePlateCharge = 0;
            BasePressCharge = 0;

            // Labor base minutes:
            BasePressSetup = 0;
            BaseCollatorSetup = 0;
            BaseBinderySetup = 0;
            BasePressSlowdown = 0;
            BaseCollatorSlowdown = 0;
            BaseBinderySlowdown = 0;

            FlatTotal = 0;

            for (int i = 0; i < dv.Count; i++)
            {
                // Load material costs
                float t1 = 0; float.TryParse(dv[i]["FLAT_CHARGE"]    .ToString(), out t1); BaseFlatCharge += t1;
                float t2 = 0; float.TryParse(dv[i]["RUN_CHARGE"]     .ToString(), out t2); BaseRunCharge += t2;
                float t3 = 0; float.TryParse(dv[i]["FINISH_MATL"]    .ToString(), out t3); BaseFinishCharge += t3;
                float t4 = 0; float.TryParse(dv[i]["CONV_MATL"]      .ToString(), out t4); BaseConvCharge += t4;
                float t5 = 0; float.TryParse(dv[i]["PLATE_MATL"]     .ToString(), out t5); BasePlateCharge += t5;
                float t6 = 0; float.TryParse(dv[i]["PRESS_MATL"]     .ToString(), out t6); BasePressCharge += t6;

                // Load labor costs
                int l1 = 0;   int.TryParse(dv[i]["PRESS_SETUP_TIME"] .ToString(), out l1); BasePressSetup += l1;
                int l2 = 0;   int.TryParse(dv[i]["COLLATOR_SETUP"]   .ToString(), out l2); BaseCollatorSetup += l2;
                int l3 = 0;   int.TryParse(dv[i]["BINDERY_SETUP"]    .ToString(), out l3); BaseBinderySetup += l3;
                int l4 = 0;   int.TryParse(dv[i]["PRESS_SLOWDOWN"]   .ToString(), out l4); BasePressSlowdown += l4;
                int l5 = 0;   int.TryParse(dv[i]["COLLATOR_SLOWDOWN"].ToString(), out l5); BaseCollatorSlowdown += l5;
                int l6 = 0;   int.TryParse(dv[i]["BINDERY_SLOWDOWN"] .ToString(), out l6); BaseBinderySlowdown += l6;
            }

            // Adjust the Base Plate Charge to reflect the use of a backer:
            // Load backer ink cost if there's a Backer record
            cmd = $"SELECT BACKER_INK_COST FROM [ESTIMATING].[dbo].[FEATURES] WHERE CATEGORY = 'BACKER' AND F_TYPE = '{Backer}'";
            da = new SqlDataAdapter(cmd, conn);  dt = new DataTable();  da.Fill(dt);
            float Backer_Ink_Cost = 0.0F;
            if(dt.Rows.Count>0) {  DataView dv2 = dt.DefaultView; Backer_Ink_Cost = float.Parse(dv2[0]["BACKER_INK_COST"].ToString()); }
            BasePressCharge = BasePressCharge + Backer_Ink_Cost;

            //TODO: -------------------------------------------------------
            //Calculate
            //    STD          value times Std Ink_Cost.MakeReadyCost
            //  + BLK - REFLEX value times BLK & STD Ink_Cost.MakeReadyCost
            //  + PMS          value times PMS Ink_Cost.MakeReadyCost
            //  + THERMO ?
            //
            //Multiply the number of inks times one of these:
            //  FEATURES.BACKER_INK_MAKEREADY_COST?
            //  QUOTE_DETAILS.MAKE_READY_INK?
            //
            //Put the result where? QUOTE_DETAILS.Amount ?
            //-------------------------------------------------------------
            //
            // Save this in the AMOUNT column
        }

        private void CalcTotal()
        {
            CalculatedFlatCharge   = BaseFlatCharge   * (1 + FlatChargePct   / 100.00F);
            CalculatedRunCharge    = BaseRunCharge    * (1 + RunChargePct    / 100.00F);
            CalculatedPlateCharge  = BasePlateCharge  * (1 + PlateChargePct  / 100.00F);
            CalculatedFinishCharge = BaseFinishCharge * (1 + FinishChargePct / 100.00F);
            CalculatedPressCharge  = BasePressCharge  * (1 + PressChargePct  / 100.00F);
            CalculatedConvCharge   = BaseConvCharge   * (1 + ConvChargePct   / 100.00F);

            FlatTotal = CalculatedFlatCharge + CalculatedPlateCharge + CalculatedFinishCharge + CalculatedPressCharge + CalculatedConvCharge;

            CalculateLabor();
        }

        private void CalcLabor(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        {
            CalculateLabor();
        }

        private void CalculateLabor()
        {
//            if (Starting) return;
            PressSetup = BasePressSetup + LabPS;
            CollatorSetup = BaseCollatorSetup + LabCS;
            BinderySetup = BaseBinderySetup + LabBS;
            SetupTotal = (int)PressSetup + CollatorSetup + BinderySetup;

            PressSlowdown = BasePressSlowdown + LabPSL;
            CollatorSlowdown = BaseCollatorSlowdown + LabCSL;
            BinderySlowdown = BaseBinderySlowdown + LabBSL;
            SlowdownTotal = PressSlowdown + CollatorSlowdown + BinderySlowdown;
        }

        private void PctChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        { CalcTotal(); }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
//            MessageBox.Show(MakeReadyInkCost.ToString("C"), "MakeReadyInkCost");

            // Delete current detail line
            string cmd = "DELETE [ESTIMATING].[dbo].[Quote_Details] WHERE Quote_Num = '" + QuoteNum + "' AND Category = 'Ink Color'";
            conn = new SqlConnection(ConnectionString); SqlCommand scmd = new SqlCommand(cmd, conn); conn.Open();

            try { scmd.ExecuteNonQuery(); }
            catch (Exception ex) { System.Windows.MessageBox.Show(ex.Message); }
            finally { conn.Close(); }

            cmd = "INSERT INTO [ESTIMATING].[dbo].[Quote_Details] ("
                + "  Quote_Num,        Category,       Sequence,            Param1,            Param2,             Param3,              Param4,             Param5,            Param6,        Param7,        Param8,       Param9, "
                + "                                                         Value1,            Value2,             Value3,              Value4,             Value5,            Value6,        Value7,        Value8,       Value9, "
                + "  Amount,            FlatCharge,      FlatChargePct,     RunChargePct,      PlateChargePct,     FinishChargePct,     PressChargePct,     ConvertChargePct,  TotalFlatChg,  PerThousandChg, "
                + "  PRESS_ADDL_MIN,    COLL_ADDL_MIN,   BIND_ADDL_MIN,     PRESS_SLOW_PCT,    COLL_SLOW_PCT,      BIND_SLOW_PCT,  "
                + "  PressSetupMin,     PressSlowPct,    CollSetupMin,      CollSlowPct,       BindSetupMin,       BindSlowPct,         MAKE_READY_INK) " 
                + " VALUES ( "
                + $" {QuoteNum},       'Ink Color',      2,                'Std',             'BlackStd',         'PMS',               'Desens',           'Split',           'Thermo',      'Watermark',   'FluorSel',   'FourColor', "
                + $"                                                      '{Std}',           '{BlackStd}',       '{PMS}',             '{Desens}',         '{Split}',         '{Thermo}',    '{WaterMark}', '{FluorSel}', '{FourColor}', "
                + $" '{CalculatedPressCharge}', '{FlatCharge}',  '{FlatChargePct}', '{RunChargePct}',  '{PlateChargePct}', '{FinishChargePct}', '{PressChargePct}', '{ConvChargePct}', '{FlatTotal}', '{CalculatedRunCharge}', "
                + $"  {LabPS},         {LabCS},         {LabBS},           {LabPSL},          {LabCSL},           {LabBSL}, "
                + $"  {PressSetup},    {PressSlowdown}, {CollatorSetup},   {CollatorSlowdown},{BinderySetup},     {BinderySlowdown},   {MakeReadyInkCost} )";

            scmd.CommandText = cmd;
            conn.Open();
            try { scmd.ExecuteNonQuery(); }
            catch (Exception ex) { System.Windows.MessageBox.Show(ex.Message); }
            finally { conn.Close(); scmd = null; conn = null; }

            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        { this.Close(); }

    }
}