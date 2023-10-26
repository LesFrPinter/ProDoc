using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ProDocEstimate.Views
{
    public partial class Cases : Window, INotifyPropertyChanged
    {
        #region Property declarations

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

        private float  cost=0.0F;        public float  Cost     { get { return cost;     } set { cost     = value; OnPropertyChanged(); } }
        private string quoteNum;         public string QuoteNum { get { return quoteNum; } set { quoteNum = value; OnPropertyChanged(); } }
        private bool   standard = true;  public bool   Standard { get { return standard; } set { standard = value; OnPropertyChanged(); Flip(); } }
        private bool   custom   = false; public bool   Custom   { get { return custom;   } set { custom   = value; OnPropertyChanged(); Flip(); } }

        public string ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public SqlConnection? conn;
        public SqlDataAdapter? da;
        public DataTable dt;
        public SqlCommand? scmd;

        #endregion

        public Cases(string QUOTENUM)
        {
            InitializeComponent();
            DataContext = this;

            QuoteNum = QUOTENUM;

            string cmd = $"SELECT Value1, Value2 FROM [ESTIMATING].[dbo].[QUOTE_DETAILS] WHERE QUOTE_NUM = '{QuoteNum}' AND CATEGORY = 'Cases'";
            dt   = new DataTable("Cases");
            conn = new SqlConnection(ConnectionString);
            da   = new SqlDataAdapter(cmd, conn);
            dt.Clear(); da.Fill(dt);

            if (dt.Rows.Count == 0) return;

            int StdNum = int.Parse(dt.Rows[0]["Value1"].ToString());
            Standard = (StdNum==1) ? true : false; Custom = !Standard;
            Cost     = float.Parse(dt.Rows[0]["Value2"].ToString());

            PreviewKeyDown += (s, e) => { if (e.Key == System.Windows.Input.Key.Escape) Close(); };  // ESC key activated

        }

        public void OnLoad(object sender, RoutedEventArgs e)
        {
            this.Height = this.Height *= (1.0F + MainWindow.FeatureZoom);
            this.Width = this.Width *= (1.0F + MainWindow.FeatureZoom);
            Top = 50;
        }

        private void Flip()
        {
            if (Standard == true) { txtCustomCost.Visibility = Visibility.Hidden; } else { txtCustomCost.Visibility = Visibility.Visible; }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // Delete current detail line
            string cmd = "DELETE [ESTIMATING].[dbo].[Quote_Details] WHERE Quote_Num = '" + QuoteNum + "' AND Category = 'Cases'";
            conn = new SqlConnection(ConnectionString);
            SqlCommand scmd = new SqlCommand(cmd, conn); conn.Open();
            try { scmd.ExecuteNonQuery(); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { conn.Close(); }

            int std = Standard ? 1 : 0;

            if(Standard==true) { Cost = 0.0F; }

            cmd  = "INSERT INTO [ESTIMATING].[dbo].[Quote_Details] ( Quote_Num,   Category, Sequence, Param1,     Value1,  Param2,  Value2 ) "
                 + $" VALUES (                                     '{QuoteNum}', 'Cases',   13,      'Standard', {std},   'Cost',  {Cost}  ) ";

            conn = new SqlConnection(ConnectionString);
            conn.Open();
            scmd.Connection = conn;
            scmd.CommandText = cmd;
            try { scmd.ExecuteNonQuery(); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { conn.Close(); }

            this.Close();
        }

        private void btnCanc_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}