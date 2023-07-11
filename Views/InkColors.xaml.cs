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

        private int     maxColors;  public int    MaxColors { get { return maxColors;   } set { maxColors   = value; OnPropertyChanged(); } }
        private string  pressSize;  public string PressSize { get { return pressSize;   } set { pressSize   = value; OnPropertyChanged(); } }
        private string  quoteNum;   public string QuoteNum  { get { return quoteNum;    } set { quoteNum    = value; OnPropertyChanged(); } }

        private int     std;        public int    Std       { get { return std;         } set { std         = value; OnPropertyChanged(); } }
        private int     blackStd;   public int    BlackStd  { get { return blackStd;    } set { blackStd    = value; OnPropertyChanged(); } }
        private int     pms;        public int    PMS       { get { return pms;         } set { pms         = value; OnPropertyChanged(); } }
        private int     desens;     public int    Desens    { get { return desens;      } set { desens      = value; OnPropertyChanged(); } }
        private int     split;      public int    Split     { get { return split;       } set { split       = value; OnPropertyChanged(); } }
        private int     thermo;     public int    Thermo    { get { return thermo;      } set { thermo      = value; OnPropertyChanged(); } }
        private int     fourColor;  public int    FourColor { get { return fourColor;   } set { fourColor   = value; OnPropertyChanged(); } }
        private int     waterMark;  public int    WaterMark { get { return waterMark;   } set { waterMark   = value; OnPropertyChanged(); } }
        private int     fluorSel;   public int    FluorSel  { get { return fluorSel;    } set { fluorSel    = value; OnPropertyChanged(); } }

        private int     max;        public int    Max       { get { return max;         } set { max         = value; OnPropertyChanged(); } }

        private float  flat_charge; public float  FLAT_CHARGE{get {return flat_charge;  } set { flat_charge = value; OnPropertyChanged(); } }
        private float   total;      public float  Total     { get { return total;       } set { total       = value; OnPropertyChanged(); } }
        private string  cmd;        public string Cmd       { get { return cmd;         } set { cmd         = value; OnPropertyChanged(); } }

        private string backer;      public string Backer    { get { return backer;      } set { backer      = value; OnPropertyChanged(); } }  // read from Quote_Details

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
            string chosen = ((System.Windows.FrameworkElement)sender).Name;
            int backercount = (Backer.ToString().Length > 0) ? 1 : 0;
            int totalcount = Std + BlackStd + PMS + Desens + Split + Thermo + FourColor + WaterMark + FluorSel + backercount;
            if (totalcount > MaxColors) { MessageBox.Show("Total number of colors plus backer can't exceed " + MaxColors.ToString()); }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // Validate inputs and return if not valid

            // Delete current detail line
            string cmd = "DELETE [ESTIMATING].[dbo].[Quote_Details] WHERE Quote_Num = '" + QuoteNum + "' AND Category = 'Ink Color'";
            conn = new SqlConnection(ConnectionString); SqlCommand scmd = new SqlCommand(cmd, conn); conn.Open();

            try                  { scmd.ExecuteNonQuery();      }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally              { conn.Close();                }

            cmd = "INSERT INTO [ESTIMATING].[dbo].[Quote_Details] ("
                + " Quote_Num, Category, Sequence,"
                + " Param1, Param2, Param3, Param4, Param5, Param6, Param7, Param8, Param9,"
                + " Value1, Value2, Value3, Value4, Value5, Value6, Value7, Value8, Value9) VALUES ( "
                + $" {QuoteNum}, 'Ink Color', 2, "
                + "   'Std',   'BlackStd',   'PMS',   'Desens',   'Split',   'Thermo',   'Watermark',   'FluorSel',   'FourColor', "
                + $" '{Std}', '{BlackStd}', '{PMS}', '{Desens}', '{Split}', '{Thermo}', '{WaterMark}', '{FluorSel}', '{FourColor}')";
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