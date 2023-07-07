using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace ProDocEstimate.Views
{
    public partial class MICR : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

        #region Properties

        public string ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public SqlConnection? conn;
        public SqlDataAdapter? da;
        public DataTable? dt;
        public SqlCommand? scmd;
        public DataView? dv;

        private int     max;                public int    Max        { get { return max;        } set { max        = value; OnPropertyChanged(); } }
        private string  quoteNum;           public string QuoteNum   { get { return quoteNum;   } set { quoteNum   = value; OnPropertyChanged(); } }
        private string  pressSize;          public string PressSize  { get { return pressSize;  } set { pressSize  = value; OnPropertyChanged(); } }
        private string  category;           public string Category   { get { return category;   } set { category   = value; OnPropertyChanged(); } }
        private string  ftype;              public string FType      { get { return ftype;      } set { ftype      = value; OnPropertyChanged(); } }

        private int     digital;            public int    Digital    { get { return digital;    } set { digital    = value; OnPropertyChanged(); } }
        private int     pack2pack;          public int    Pack2Pack  { get { return pack2pack;  } set { pack2pack  = value; OnPropertyChanged(); } }
        private int     press;              public int    Press      { get { return press;      } set { press      = value; OnPropertyChanged(); } }

        private float   flatCharge;         public float  FlatCharge { get { return flatCharge; } set { flatCharge = value; OnPropertyChanged(); } }
        private float   baseflatCharge;     public float  BaseFlatCharge { get { return baseflatCharge; } set { baseflatCharge = value; OnPropertyChanged(); } }
        private float   flatChargePct;      public float  FlatChargePct  { get { return flatChargePct;  } set { flatChargePct  = value; OnPropertyChanged(); } }
        private float   calculatedflatCharge; public float CalculatedFlatCharge { get { return calculatedflatCharge; } set { calculatedflatCharge = value; OnPropertyChanged(); } }

        private float   runCharge;            public float RunCharge     { get { return runCharge;      } set { runCharge       = value; OnPropertyChanged(); } }
        private float   baserunCharge;        public float BaseRunCharge { get { return baserunCharge;  } set { baserunCharge   = value; OnPropertyChanged(); } }
        private float   runChargePct;         public float RunChargePct  { get { return runChargePct;   } set { runChargePct    = value; OnPropertyChanged(); } }
        private float   calculatedrunCharge;  public float CalculatedRunCharge { get { return calculatedrunCharge; } set { calculatedrunCharge = value; OnPropertyChanged(); } }

        #endregion

        public MICR(string _PRESSSIZE, string _QUOTENUM)
        {
            // The next three are used by the DetailFeatures UserControl
            QuoteNum = _QUOTENUM;
            PressSize = _PRESSSIZE;
            Category = "MICR";

            InitializeComponent();
            DataContext = this;
            Title = "Quote #: " + _QUOTENUM;
            FType = "DIGITAL";

            LoadMaxima();
            LoadData();

            PreviewKeyDown += (s, e) => { if (e.Key == Key.Escape) Close(); };
        }

        private void LoadDataView()
        {
            string cmd =  "SELECT F_TYPE, FLAT_CHARGE, RUN_CHARGE, PRESS_SETUP_TIME, PRESS_SLOWDOWN FROM [ESTIMATING].[dbo].FEATURES" 
                       + $" WHERE CATEGORY='MICR' AND PRESS_SIZE = '{PressSize}' ORDER BY F_TYPE";
            conn = new SqlConnection(ConnectionString);
            da = new SqlDataAdapter(cmd, conn); dt = new DataTable(); da.Fill(dt);
            DataView dv = dt.AsDataView();
        }

        private void LoadMaxima()
        {
            // Retrieve value for MICR if one was previously entered.
            string str =  "SELECT F_TYPE, MAX(Number) AS Max"
                       + $" FROM [ESTIMATING].[dbo].[FEATURES] WHERE Category = 'MICR' AND Press_Size = '{PressSize}' GROUP BY F_TYPE";
            SqlConnection conn = new(ConnectionString);
            SqlDataAdapter da = new(str, conn); DataTable dt = new(); dt.Rows.Clear(); da.Fill(dt);
            DataView dv = new DataView(dt);
            dv.RowFilter = "F_TYPE='DIGITAL'";      M1.Maximum = int.Parse(dv[0]["Max"].ToString());
            dv.RowFilter = "F_TYPE='PACK2PACK'";    M2.Maximum = int.Parse(dv[0]["Max"].ToString());
            dv.RowFilter = "F_TYPE='PRESS'";        M3.Maximum = int.Parse(dv[0]["Max"].ToString());
        }

        private void LoadData()
        {
            string str = $"SELECT * FROM [ESTIMATING].[dbo].[Quote_Details] WHERE QUOTE_NUM = '{QuoteNum}' AND Category = 'MICR'";
            SqlConnection conn = new(ConnectionString);
            SqlDataAdapter da = new(str, conn); DataTable dt = new(); dt.Rows.Clear(); da.Fill(dt);
            if (dt.Rows.Count == 0) return;
            DataView dv2 = new DataView(dt);
            Digital    = int.Parse(dv2[0]["Value1"].ToString());
            Pack2Pack  = int.Parse(dv2[0]["Value2"].ToString());
            Press      = int.Parse(dv2[0]["Value3"].ToString());

            GetFlatCharge();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string cmd = "DELETE [ESTIMATING].[dbo].[Quote_Details] WHERE Quote_Num = '" + QuoteNum + "' AND Category = 'MICR'";
            conn = new SqlConnection(ConnectionString); SqlCommand scmd = new SqlCommand(cmd, conn); conn.Open();

            try { scmd.ExecuteNonQuery(); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { conn.Close(); }

            // Add flat charges for up to three F_TYPE values;

            cmd = "INSERT INTO [ESTIMATING].[dbo].[Quote_Details] ("
                + " Quote_Num, Category, Sequence,"
                + " Param1, Param2, Param3, "
                + " Value1, Value2, Value3, FlatCharge ) VALUES ( "
                + $"'{QuoteNum}', 'MICR', 4, "
                + " 'Digital', 'Pack2Pack', 'Press', "
                + $" '{Digital}', '{Pack2Pack}', '{Press}', {FlatCharge} )";

            scmd.CommandText = cmd;
            conn.Open();
            try { scmd.ExecuteNonQuery(); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { conn.Close(); scmd = null; conn = null; }

            this.Close();
        }

        //TODO: Each of these NumericUpDown controls calls the same SQL routine before calling GetFlatCharge; refactor?

        private void M1_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        {   // Calculate Flat charge based on the selected NumericUpDowns and pass it to the DetailFeatures UserControl
            // NOTE: Data could be returned and the dataview loaded just once when the MICR component is first opened...
            string cmd = $"SELECT F_TYPE, FLAT_CHARGE, RUN_CHARGE, PRESS_SETUP_TIME, PRESS_SLOWDOWN FROM [ESTIMATING].[dbo].FEATURES WHERE CATEGORY='MICR' AND PRESS_SIZE = '{PressSize}' ORDER BY F_TYPE";
            conn = new SqlConnection(ConnectionString);
            da = new SqlDataAdapter(cmd, conn); dt = new DataTable("Ftypes"); da.Fill(dt);
            dv = dt.DefaultView;
            GetFlatCharge();
        }

        private void M2_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        {
            string cmd = $"SELECT F_TYPE, FLAT_CHARGE, RUN_CHARGE, PRESS_SETUP_TIME, PRESS_SLOWDOWN FROM [ESTIMATING].[dbo].FEATURES WHERE CATEGORY='MICR' AND PRESS_SIZE = '{PressSize}' ORDER BY F_TYPE";
            conn = new SqlConnection(ConnectionString);
            da = new SqlDataAdapter(cmd, conn); dt = new DataTable(); da.Fill(dt);
            dv = dt.DefaultView;
            GetFlatCharge();
        }

        private void M3_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        {
            string cmd = $"SELECT F_TYPE, FLAT_CHARGE, RUN_CHARGE, PRESS_SETUP_TIME, PRESS_SLOWDOWN FROM [ESTIMATING].[dbo].FEATURES WHERE CATEGORY='MICR' AND PRESS_SIZE = '{PressSize}' ORDER BY F_TYPE";
            conn = new SqlConnection(ConnectionString);
            da = new SqlDataAdapter(cmd, conn); dt = new DataTable(); da.Fill(dt);
            dv = dt.DefaultView;
            GetFlatCharge();
        }

        private void GetFlatCharge()
        {
            string cmd = "SELECT F_TYPE, FLAT_CHARGE, RUN_CHARGE, PRESS_SETUP_TIME, PRESS_SLOWDOWN FROM [ESTIMATING].[dbo].FEATURES"
                       + $" WHERE CATEGORY='MICR' AND PRESS_SIZE = '{PressSize}' ORDER BY F_TYPE";
            conn = new SqlConnection(ConnectionString);
            da = new SqlDataAdapter(cmd, conn); dt = new DataTable(); da.Fill(dt);
            DataView dv = dt.AsDataView();

            if (dv == null) return;

            // Total Flat Charge
            dv.RowFilter = "F_TYPE='DIGITAL'";   float T1 = float.Parse(dv[0]["FLAT_CHARGE"].ToString());
            dv.RowFilter = "F_TYPE='PACK2PACK'"; float T2 = float.Parse(dv[0]["FLAT_CHARGE"].ToString());
            dv.RowFilter = "F_TYPE='PRESS'";     float T3 = float.Parse(dv[0]["FLAT_CHARGE"].ToString());
            BaseFlatCharge       = (Digital * T1) + (Pack2Pack * T2) + (Press * T3);
            CalculatedFlatCharge = (float)BaseFlatCharge * (1.00F + (float)FlatChargePct / 100.00F);

            // Total Run Charge
            dv.RowFilter = "F_TYPE='DIGITAL'";   float R1 = float.Parse(dv[0]["RUN_CHARGE"].ToString());
            dv.RowFilter = "F_TYPE='PACK2PACK'"; float R2 = float.Parse(dv[0]["RUN_CHARGE"].ToString());
            dv.RowFilter = "F_TYPE='PRESS'";     float R3 = float.Parse(dv[0]["RUN_CHARGE"].ToString());
            BaseRunCharge = (Digital * R1) + (Pack2Pack * R2) + (Press * R3);
            CalculatedRunCharge = (float)BaseRunCharge * (1.00F + (float)RunChargePct / 100.00F);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void FCPct_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        {
            GetFlatCharge() ;
            CalculatedFlatCharge = (float)BaseFlatCharge * (1.00F + (float)FlatChargePct / 100.00F);
        }

        private void RCPct_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        {
            GetFlatCharge();
            CalculatedRunCharge = (float)BaseRunCharge * (1.00F + (float)RunChargePct / 100.00F);
        }
    }
}