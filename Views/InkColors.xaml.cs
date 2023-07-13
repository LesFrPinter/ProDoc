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

        #region Properties

        private int     maxColors;   public int    MaxColors   { get { return maxColors;   } set { maxColors   = value; OnPropertyChanged(); } }
        private string  pressSize;   public string PressSize   { get { return pressSize;   } set { pressSize   = value; OnPropertyChanged(); } }
        private string  quoteNum;    public string QuoteNum    { get { return quoteNum;    } set { quoteNum    = value; OnPropertyChanged(); } }

        private int     std;         public int    Std         { get { return std;         } set { std         = value; OnPropertyChanged(); } }
        private int     blackStd;    public int    BlackStd    { get { return blackStd;    } set { blackStd    = value; OnPropertyChanged(); } }
        private int     pms;         public int    PMS         { get { return pms;         } set { pms         = value; OnPropertyChanged(); } }
        private int     desens;      public int    Desens      { get { return desens;      } set { desens      = value; OnPropertyChanged(); } }
        private int     split;       public int    Split       { get { return split;       } set { split       = value; OnPropertyChanged(); } }
        private int     thermo;      public int    Thermo      { get { return thermo;      } set { thermo      = value; OnPropertyChanged(); } }
        private int     fourColor;   public int    FourColor   { get { return fourColor;   } set { fourColor   = value; OnPropertyChanged(); } }
        private int     waterMark;   public int    WaterMark   { get { return waterMark;   } set { waterMark   = value; OnPropertyChanged(); } }
        private int     fluorSel;    public int    FluorSel    { get { return fluorSel;    } set { fluorSel    = value; OnPropertyChanged(); } }

        private float   stdCharge   = 0.00F; public float  StdCharge   { get { return stdCharge;   } set { stdCharge   = value; OnPropertyChanged(); } }
        private float   blackstdChg = 0.00F; public float  BlackStdChg { get { return blackstdChg; } set { blackstdChg = value; OnPropertyChanged(); } }
        private float   pmsChg      = 0.00F; public float  PMSChg      { get { return pmsChg;      } set { pmsChg      = value; OnPropertyChanged(); } }
        private float   desChg      = 0.00F; public float  DesChg      { get { return desChg;      } set { desChg      = value; OnPropertyChanged(); } }
        private float   splitChg    = 0.00F; public float  SplitChg    { get { return splitChg;    } set { splitChg    = value; OnPropertyChanged(); } }
        private float   thermoChg   = 0.00F; public float  ThermoChg   { get { return thermoChg;   } set { thermoChg   = value; OnPropertyChanged(); } }
        private float   fourChg     = 0.00F; public float  FourChg     { get { return fourChg;     } set { fourChg     = value; OnPropertyChanged(); } }
        private float   waterChg    = 0.00F; public float  WaterChg    { get { return waterChg;    } set { waterChg    = value; OnPropertyChanged(); } }
        private float   fluorChg    = 0.00F; public float  FluorChg    { get { return fluorChg;    } set { fluorChg    = value; OnPropertyChanged(); } }

        private float   totFlat = 0.00F;     public float  TotFlat     { get { return totFlat;     } set { totFlat     = value; OnPropertyChanged(); } }

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

        private int     max;         public int    Max         { get { return max;         } set { max         = value; OnPropertyChanged(); } }
        private float   flat_charge; public float  FLAT_CHARGE { get { return flat_charge; } set { flat_charge = value; OnPropertyChanged(); } }
        private float   total;       public float  Total       { get { return total;       } set { total       = value; OnPropertyChanged(); } }
        private string  cmd;         public string Cmd         { get { return cmd;         } set { cmd         = value; OnPropertyChanged(); } }
        private string  backer;      public string Backer      { get { return backer;      } set { backer      = value; OnPropertyChanged(); } }  // read from Quote_Details
        private string  fieldList;   public string FieldList   { get { return fieldList;   } set { fieldList   = value; OnPropertyChanged(); } }

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

            FLAT_CHARGE = 123.45F;

            Total     = 0.00F;     // Calculcate this based on user input
            Backer    = "";

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
            }
        }

        private void LoadMaxColors()
        {
            PressSize = PressSize;

            SqlConnection cn = new(ConnectionString);
            string str = $"SELECT Number FROM [ESTIMATING].[dbo].[PressSizeColors] WHERE Size = '{PressSize}'";
            SqlDataAdapter da = new(str, cn); DataTable dt = new DataTable(); da.Fill(dt);
            MaxColors = Int32.Parse(dt.Rows[0]["Number"].ToString());

            // Retrieve value for Backer if one was previously entered 
            str = $"SELECT PARAM1 FROM [ESTIMATING].[dbo].[Quote_Details] WHERE QUOTE_NUM = '{QuoteNum}' AND CATEGORY = 'Backer'";
            da.SelectCommand.CommandText = str; dt.Rows.Clear(); da.Fill(dt);
            if(dt.Rows.Count > 0 ) { Backer = dt.Rows[0]["Param1"].ToString(); }

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
        }

        private void GetCharges(string ctl)
        {
            switch (ctl)
             {
                case "Std1":
                   if (Std > 0)
                    { string cmd = $"SELECT {FieldList} FROM [ESTIMATING].[dbo].[FEATURES] WHERE CATEGORY = 'INK' AND PRESS_SIZE = '{PressSize}' AND F_TYPE = 'STD' AND NUMBER = '{Std}'";
                        dt = new DataTable("Charges"); conn = new SqlConnection(ConnectionString); da = new SqlDataAdapter(cmd, conn); 
                        da.Fill(dt); if (dt.Rows.Count == 0) { return; }
                        float.TryParse(dt.Rows[0][1].ToString(), out float result); StdCharge = result;
                        float.TryParse(dt.Rows[0][2].ToString(), out float result2); Pt1      = result2;
                    }   else { StdCharge = 0.00F; Pt1 = 0.00F; }
                break;

                case "Blk1":
                    if (BlackStd > 0)
                    {   cmd = $"SELECT {FieldList} FROM [ESTIMATING].[dbo].[FEATURES] WHERE CATEGORY = 'INK' AND PRESS_SIZE = '{PressSize}' AND F_TYPE = 'BLK & STD' AND NUMBER = '{BlackStd}'";
                        da.SelectCommand.CommandText = cmd; dt.Clear(); da.Fill(dt); if (dt.Rows.Count == 0) { return; }
                        float.TryParse(dt.Rows[0][1].ToString(), out float result);  BlackStdChg = result;
                        float.TryParse(dt.Rows[0][2].ToString(), out float result2); Pt2 = result2;
                    }
                    else { BlackStdChg = 0.00F; Pt2 = 0.00F; }
                break;

                case "Pms1":
                    if (PMS > 0)
                    {   cmd = $"SELECT {FieldList} FROM [ESTIMATING].[dbo].[FEATURES] WHERE CATEGORY = 'INK' AND PRESS_SIZE = '{PressSize}' AND F_TYPE = 'PMS' AND NUMBER = '{PMS}'";
                        da.SelectCommand.CommandText = cmd; dt.Clear(); da.Fill(dt); if (dt.Rows.Count == 0) { return; }
                        float.TryParse(dt.Rows[0][1].ToString(), out float result);  PMSChg = result;
                        float.TryParse(dt.Rows[0][2].ToString(), out float result2); Pt3 = result2;
                    }
                    else { PMSChg = 0.00F; Pt3 = 0.00F; }
                break;

                case "Des1":
                    if (Desens > 0)
                    {   cmd = $"SELECT {FieldList} FROM [ESTIMATING].[dbo].[FEATURES] WHERE CATEGORY = 'INK' AND PRESS_SIZE = '{PressSize}' AND F_TYPE = 'DESENSITIZED' AND NUMBER = '{Desens}'";
                        da.SelectCommand.CommandText = cmd; dt.Clear(); da.Fill(dt); if (dt.Rows.Count == 0) { return; }
                        float.TryParse(dt.Rows[0][1].ToString(), out float result);  DesChg = result;
                        float.TryParse(dt.Rows[0][2].ToString(), out float result2); Pt4    = result2;
                    }
                    else { DesChg = 0.00F; Pt4 = 0.00F; }
                break;

                case "Spl1":
                    if (Split > 0)
                    {   cmd = $"SELECT {FieldList} FROM [ESTIMATING].[dbo].[FEATURES] WHERE CATEGORY = 'INK' AND PRESS_SIZE = '{PressSize}' AND F_TYPE = 'SPLIT FTN' AND NUMBER = '{Split}'";
                        da.SelectCommand.CommandText = cmd; dt.Clear(); da.Fill(dt); if (dt.Rows.Count == 0) { return; }
                        float.TryParse(dt.Rows[0][1].ToString(), out float result);  SplitChg = result;
                        float.TryParse(dt.Rows[0][2].ToString(), out float result2); Pt5 = result2;
                    }
                    else { SplitChg = 0.00F; Pt5 = 0.00F; }
                break;

                case "Fou1":
                    if (FourColor > 0)
                    {   cmd = $"SELECT {FieldList} FROM [ESTIMATING].[dbo].[FEATURES] WHERE CATEGORY = 'INK' AND PRESS_SIZE = '{PressSize}' AND F_TYPE = '4 CLR PRO' AND NUMBER = '{FourColor}'";
                        da.SelectCommand.CommandText = cmd; dt.Clear(); da.Fill(dt); if (dt.Rows.Count == 0) { return; }
                        float.TryParse(dt.Rows[0][1].ToString(), out float result);  FourChg = result;
                        float.TryParse(dt.Rows[0][2].ToString(), out float result2); Pt6 = result2;
                    } else { FourChg = 0.00F; Pt6 = 0.00F; }
                break;

                case "The1":
                    if (Thermo > 0)
                    {   cmd = $"SELECT {FieldList} FROM [ESTIMATING].[dbo].[FEATURES] WHERE CATEGORY = 'INK' AND PRESS_SIZE = '{PressSize}' AND F_TYPE = 'THERMO' AND NUMBER = '{Thermo}'";
                        da.SelectCommand.CommandText = cmd; dt.Clear(); da.Fill(dt); if (dt.Rows.Count == 0) { return; }
                        float.TryParse(dt.Rows[0][1].ToString(), out float result);  ThermoChg = result;
                        float.TryParse(dt.Rows[0][2].ToString(), out float result2); Pt7 = result2;
                    }
                    else { ThermoChg = 0.00F; Pt7 = 0.00F; }
                    break;

                case "Wat1":
                    if (WaterMark > 0)
                    {   cmd = $"SELECT {FieldList} FROM [ESTIMATING].[dbo].[FEATURES] WHERE CATEGORY = 'INK' AND PRESS_SIZE = '{PressSize}' AND F_TYPE = 'ART WATERMARK' AND NUMBER = '{WaterMark}'";
                        da.SelectCommand.CommandText = cmd; dt.Clear(); da.Fill(dt); if (dt.Rows.Count == 0) { return; }
                        float.TryParse(dt.Rows[0][1].ToString(), out float result); WaterChg = result;
                        float.TryParse(dt.Rows[0][2].ToString(), out float result2); Pt8 = result2;
                    }
                    else { WaterChg = 0.00F; Pt8 = 0.00F; }
                    break;

                case "Flo1":
                    if (FluorSel > 0)
                    {   cmd = $"SELECT {FieldList} FROM [ESTIMATING].[dbo].[FEATURES] WHERE CATEGORY = 'INK' AND PRESS_SIZE = '{PressSize}' AND F_TYPE = 'FLUOR SEL' AND NUMBER = '{FluorSel}'";
                        da.SelectCommand.CommandText = cmd; dt.Clear(); da.Fill(dt); if (dt.Rows.Count == 0) { return; }
                        float.TryParse(dt.Rows[0][1].ToString(), out float result);  FluorChg = result;
                        float.TryParse(dt.Rows[0][2].ToString(), out float result2); Pt9 = result2;
                    }
                    else { FluorChg = 0.00F; Pt9 = 0.00F; }
                    break;

                default: break;
             }

            TotFlat = StdCharge + BlackStdChg + PMSChg + DesChg + SplitChg + ThermoChg + FourChg + WaterChg + FluorChg;
            TotPT   = Pt1 + Pt2 + Pt3 + Pt4 + Pt5 + Pt6 + Pt7 + Pt8 + Pt9;
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
                + " Quote_Num, Category, Sequence,"
                + " Param1, Param2, Param3, Param4, Param5, Param6, Param7, Param8, Param9,"
                + " Value1, Value2, Value3, Value4, Value5, Value6, Value7, Value8, Value9, TotalFlatChg, PerThousandChg) VALUES ( "
                + $" {QuoteNum}, 'Ink Color', 2, "
                + "   'Std',   'BlackStd',   'PMS',   'Desens',   'Split',   'Thermo',   'Watermark',   'FluorSel',   'FourColor', "
                + $" '{Std}', '{BlackStd}', '{PMS}', '{Desens}', '{Split}', '{Thermo}', '{WaterMark}', '{FluorSel}', '{FourColor}', "
                + $" '{TotFlat}', '{TotPT}' )";

            scmd.CommandText = cmd;
            conn.Open();
            try { scmd.ExecuteNonQuery(); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { conn.Close(); scmd = null; conn = null; }

            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        { this.Close(); }

    }
}