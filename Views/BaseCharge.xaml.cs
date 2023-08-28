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
            this.Height = this.Height *= 1.8;
            this.Width = this.Width *= 1.8;
            this.Top = 150;
        }

        private void LoadData()
        {
            string cmd = $"SELECT TotalFlatChg, PerThousandChg FROM [ESTIMATING].[dbo].[QUOTE_DETAILS] WHERE QUOTE_NUM = '{Quote_Num}' AND Category = 'Base Charges'";
            conn = new SqlConnection(ConnectionString);
            da = new SqlDataAdapter(cmd, conn);
            dt = new DataTable(); da.Fill(dt);
            FlatChg = float.Parse(dt.Rows[0]["TotalFlatChg"].ToString());
            RunChg = float.Parse(dt.Rows[0]["PerThousandChg"].ToString());
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        { (sender as TextBox).SelectAll(); }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string cmd = $"DELETE [ESTIMATING].[dbo].[QUOTE_DETAILS] WHERE QUOTE_NUM = '{Quote_Num}' AND Category = 'Base Charges'";
            conn = new SqlConnection(ConnectionString); conn.Open();
            scmd = new SqlCommand(cmd, conn); scmd.ExecuteNonQuery(); conn.Close();
            cmd =  "INSERT INTO [ESTIMATING].[dbo].[QUOTE_DETAILS] ( QUOTE_NUM, Category, TotalFlatChg, PerThousandChg )" +
                  $" VALUES ( '{Quote_Num}', 'Base Charges', '{FlatChg}', '{RunChg}' )";
            conn.Open(); scmd.CommandText = cmd; scmd.ExecuteNonQuery(); conn.Close();

            this.Close(); 
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        { this.Close(); }


    }
}