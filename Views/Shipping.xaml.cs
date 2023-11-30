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
        public SqlCommand? scmd;

        private string quoteNum; public string QuoteNum { get { return quoteNum; } set { quoteNum = value; } }

        private float standardCharge;   public float StandardCharge { get { return standardCharge;  } set { standardCharge  = value; OnPropertyChanged(); } }
        private int   numDrops;         public int NumDrops         { get { return numDrops;        } set { numDrops        = value; OnPropertyChanged(); } }
        private float oneShipment;      public float OneShipment    { get { return oneShipment;     } set { oneShipment     = value; OnPropertyChanged(); } }
        private float addlCharge;       public float AddlCharge     { get { return addlCharge;      } set { addlCharge      = value; OnPropertyChanged(); } }

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
            this.Height = this.Height *= (1.0F + MainWindow.FeatureZoom);
            this.Width = this.Width *= (1.0F + MainWindow.FeatureZoom);
            Top = 50;
        }

        public void LoadData()
        {
            string cmd = $"SELECT StandardCharge, IncrementalCharge FROM [ESTIMATING].[dbo].[Shipping]";
            conn = new SqlConnection(ConnectionString); conn.Open();
            SqlDataAdapter da2 = new SqlDataAdapter(cmd, conn); DataTable dt2 = new DataTable(); da2.Fill(dt2); DataView dv2 = dt2.DefaultView;
            StandardCharge  = float.Parse(dv2[0]["StandardCharge"].ToString());
            OneShipment     = float.Parse(dv2[0]["IncrementalCharge"].ToString());
            string tooltip = OneShipment.ToString("C") + " per shipment";
            updownShipments.ToolTip = $"{tooltip}";
            txtAddlCharge.ToolTip = $"{tooltip}";

            cmd = $"SELECT Value1, Value2, Value3 FROM [ESTIMATING].[dbo].[QUOTE_DETAILS] WHERE QUOTE_NUM = '{QuoteNum}' AND Category = 'Shipping'";
            conn = new SqlConnection(ConnectionString); conn.Open();
            da = new SqlDataAdapter(cmd, conn); 
            DataTable dt = new DataTable("Details"); da.Fill(dt); DataView dv = dt.DefaultView;

            NumDrops = 0; AddlCharge = 0.0F;
            if (dv.Count > 0) { 
                NumDrops   = int  .Parse(dv[0]["Value3"].ToString());
                AddlCharge = StandardCharge + ( OneShipment * NumDrops );
            }
            conn.Close();
        }

        private void RadNumericUpDown_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        {
            AddlCharge = StandardCharge + (NumDrops * OneShipment);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string cmd = $"DELETE [ESTIMATING].[dbo].[QUOTE_DETAILS] WHERE QUOTE_NUM = '{QuoteNum}' AND Category = 'Shipping'";
            conn = new SqlConnection(ConnectionString); conn.Open();
            scmd = new SqlCommand(cmd, conn); scmd.ExecuteNonQuery(); conn.Close();

            cmd =  "INSERT INTO [ESTIMATING].[dbo].[QUOTE_DETAILS] ( "
                +  "  QUOTE_NUM,    Category,  Sequence, Param1,           Param2,       Param3,     Value1,           Value2,       Value3    )"
                +  " VALUES ( "
                + $" '{QuoteNum}', 'Shipping', 12,      'StandardCharge', 'AddlCharge', 'NumDrops', {StandardCharge}, {AddlCharge}, {NumDrops} )";

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

    }
}