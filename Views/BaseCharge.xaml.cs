using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ProDocEstimate.Views
{
    public partial class BaseCharge : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }
    
        private float  flatChg;   public float  FlatChg   {  get { return flatChg;   } set { flatChg   = value; OnPropertyChanged(); } }
        private float  runChg;    public float  RunChg    { get  { return runChg;    } set { runChg    = value; OnPropertyChanged(); } }
        private string quote_Num; public string Quote_Num { get  { return quote_Num; } set { quote_Num = value; OnPropertyChanged(); } }

        public string ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public SqlConnection? conn;
        public SqlDataAdapter? da;
        public SqlCommand scmd;
        public DataTable dt;

        public BaseCharge(string QUOTE_NUM)
        {
            InitializeComponent();
            DataContext = this;

            Quote_Num = QUOTE_NUM;
            LoadData();

            PreviewKeyDown += (s, e) => { if (e.Key == Key.Escape) Close(); };
        }

        public void OnLoad(object sender, RoutedEventArgs e)
        {
            this.Height = this.Height *= 1.6;
            this.Width = this.Width *= 1.6;
            this.Top = 50;
        }

        private void Flat_GotFocus(object sender, RoutedEventArgs e)
        { Flat.SelectAll(); }

        private void Run_GotFocus(object sender, RoutedEventArgs e)
        { Run.SelectAll(); }

        private void LoadData()
        {
            string cmd = $"SELECT TotalFlatChg, PerThousandChg FROM [ESTIMATING].[dbo].[QUOTE_DETAILS] WHERE QUOTE_NUM = '{Quote_Num}' AND Category = 'Base Charges'";

            conn = new SqlConnection(ConnectionString);
            da = new SqlDataAdapter(cmd, conn);
            dt = new DataTable(); da.Fill(dt);
            FlatChg = 0.0F; RunChg = 0.0F;
            if (dt.Rows.Count == 0) return;

            float F1 = 0.0F; float.TryParse(dt.Rows[0]["TotalFlatChg"]  .ToString(), out F1); FlatChg = F1;
            float F2 = 0.0F; float.TryParse(dt.Rows[0]["PerThousandChg"].ToString(), out F2); RunChg  = F2;
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        { (sender as TextBox).SelectAll(); }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string cmd = $"DELETE [ESTIMATING].[dbo].[QUOTE_DETAILS] WHERE QUOTE_NUM = '{Quote_Num}' AND Category = 'Base Charges'";
            conn = new SqlConnection(ConnectionString); conn.Open();
            scmd = new SqlCommand(cmd, conn); scmd.ExecuteNonQuery(); conn.Close();
            cmd =  "INSERT INTO [ESTIMATING].[dbo].[QUOTE_DETAILS]"
                +  "          ( QUOTE_NUM,    Category,       Value1,  TotalFlatChg, PerThousandChg )" +
                  $" VALUES ( '{Quote_Num}', 'Base Charges', 'Show', '{FlatChg}',  '{RunChg}'       )";
            conn.Open(); scmd.CommandText = cmd; scmd.ExecuteNonQuery(); conn.Close();

            this.Close(); 
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        { this.Close(); }

    }
}