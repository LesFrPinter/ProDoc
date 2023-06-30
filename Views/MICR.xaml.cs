using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Windows;

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

        private int     max;            public int    Max        { get { return max;        } set { max        = value; OnPropertyChanged(); } }
        private string  pressSize;      public string PressSize  { get { return pressSize;  } set { pressSize  = value; OnPropertyChanged(); } }
        private string  quoteNum;       public string QuoteNum   { get { return quoteNum;   } set { quoteNum   = value; OnPropertyChanged(); } }
        private string  category;       public string Category   { get { return category;   } set { category   = value; OnPropertyChanged(); } }
        private string  ftype;          public string FType      { get { return ftype;      } set { ftype      = value; OnPropertyChanged(); } }

        private int     digital;        public int    Digital    { get { return digital;    } set { digital    = value; OnPropertyChanged(); } }
        private int     pack2pack;      public int    Pack2Pack  { get { return pack2pack;  } set { pack2pack  = value; OnPropertyChanged(); } }
        private int     press;          public int    Press      { get { return press;      } set { press      = value; OnPropertyChanged(); } }

        private float   flatCharge;     public float  FlatCharge { get { return flatCharge; } set { flatCharge = value; OnPropertyChanged(); } }

        #endregion

        public MICR(string PRESSSIZE, string QUOTENUM)
        {
            InitializeComponent();

            this.DataContext = this;

            Title = "Quote #: " + QUOTENUM;

            QuoteNum = QUOTENUM;
            PressSize = PRESSSIZE;

            Category = "MICR";
            FType = "DIGITAL";

            LoadMaxima();
            LoadData();
        }

        private void LoadMaxima()
        {
            // Retrieve value for Backer if one was previously entered 
            string str = $"SELECT F_TYPE, MAX(Number) AS Max"
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
            DataView dv = new DataView(dt);
            Digital    = int.Parse(dv[0]["Value1"].ToString());
            Pack2Pack  = int.Parse(dv[0]["Value2"].ToString());
            Press      = int.Parse(dv[0]["Value3"].ToString());

            FlatCharge = float.Parse(dv[0]["FlatCharge"].ToString());       // Percentage markup to apply to the FlatCharge value stored in the Features table

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
                + $" '{Digital}', '{Pack2Pack}', '{Press}', {Details.FlatCharge} )";

            scmd.CommandText = cmd;
            conn.Open();
            try { scmd.ExecuteNonQuery(); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { conn.Close(); scmd = null; conn = null; }

            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}