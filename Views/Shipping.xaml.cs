using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ProDocEstimate.Views
{
    public partial class Shipping : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

        #region Properties

        public string ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public SqlConnection? conn;
        public SqlDataAdapter? da;
        public DataTable dt;
        public SqlCommand? scmd;

        private string quoteNum; public string QuoteNum { get { return quoteNum; } set { quoteNum = value; } }

        private float baseCharge; public float BaseCharge { get { return baseCharge; } set { baseCharge = value; OnPropertyChanged(); } }
        private float addlCharge; public float AddlCharge { get { return addlCharge; } set { addlCharge = value; OnPropertyChanged(); } }
        private int   numDrops;   public int   NumDrops   { get { return numDrops;   } set { numDrops   = value; OnPropertyChanged(); } }

        #endregion

        // TODO: Load the default charges from the SHIPPING table

        public Shipping(string QUOTENUM)
        {
            InitializeComponent();
            DataContext = this;
            QuoteNum = QUOTENUM;
            LoadData();

            PreviewKeyDown += (s, e) => { if (e.Key == System.Windows.Input.Key.Escape) Close(); };  // ESC key activated

        }

        public void OnLoad(object sender, RoutedEventArgs e)
        {
            //this.Height = this.Height *= 1.6;
            //this.Width = this.Width *= 1.6;
            Top = 50;
        }

        public void LoadData()
        {
            string cmd = $"SELECT Value1, Value2, Value3 FROM [ESTIMATING].[dbo].[QUOTE_DETAILS] WHERE QUOTE_NUM = '{QuoteNum}' AND Category = 'Shipping'";
            conn = new SqlConnection(ConnectionString); conn.Open();
            da = new SqlDataAdapter(cmd, conn); 
            dt = new DataTable();
            da.Fill(dt); DataView dv = dt.DefaultView;

            BaseCharge = 0.0F;
            AddlCharge = 0.0F;
            NumDrops = 0;

            if (dv.Count > 0 ) { 
                BaseCharge = float.Parse(dv[0]["Value1"].ToString());
                AddlCharge = float.Parse(dv[0]["Value2"].ToString());
                NumDrops   = int  .Parse(dv[0]["Value3"].ToString());
            }
            conn.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string cmd = $"DELETE [ESTIMATING].[dbo].[QUOTE_DETAILS] WHERE QUOTE_NUM = '{QuoteNum}' AND Category = 'Shipping'";
            conn = new SqlConnection(ConnectionString); conn.Open();
            scmd = new SqlCommand(cmd, conn); scmd.ExecuteNonQuery(); conn.Close();

            cmd =  "INSERT INTO [ESTIMATING].[dbo].[QUOTE_DETAILS] ( "
                +  "  QUOTE_NUM,    Category,  Sequence, Param1,       Param2,       Param3,     Value1,       Value2,       Value3    )"
                +  " VALUES ( "
                + $" '{QuoteNum}', 'Shipping', 12,      'BaseCharge', 'AddlCharge', 'NumDrops', {BaseCharge}, {AddlCharge}, {NumDrops} )";

            conn.Open();
            scmd.Connection = conn;
            scmd.CommandText = cmd;
            scmd.ExecuteNonQuery(); conn.Close();

            Close();
        }

        private void btnCanc_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void nDrops_LostFocus(object sender, RoutedEventArgs e)
        {
            AddlCharge = NumDrops * 5.00F;
        }
    }
}