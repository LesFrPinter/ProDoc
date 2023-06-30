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
    /// <summary>
    /// Interaction logic for Punching.xaml
    /// </summary>
    public partial class Punching : Window, INotifyPropertyChanged
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

        private int max; public int Max { get { return max; } set { max = value; OnPropertyChanged(); } }
        private string pressSize; public string PressSize { get { return pressSize; } set { pressSize = value; OnPropertyChanged(); } }
        private string quoteNum; public string QuoteNum { get { return quoteNum; } set { quoteNum = value; OnPropertyChanged(); } }

        private int two;   public int Two   { get { return two; } set { two = value; OnPropertyChanged(); } }
        private int three; public int Three { get { return three; } set { three = value; OnPropertyChanged(); } }
        private int over3; public int Over3 { get { return over3; } set { over3 = value; OnPropertyChanged(); } }

        #endregion

        public Punching(string PRESSSIZE, string QUOTENUM)
        {
            InitializeComponent();
            this.DataContext = this;
            Title = "Quote #: " + QUOTENUM;
            QuoteNum = QUOTENUM;
            PressSize = PRESSSIZE;
            LoadMaxima();
            LoadData();
            PreviewKeyDown += (s, e) => { if (e.Key == Key.Escape) Close(); };
        }

        private void LoadMaxima()
        {
            // Retrieve value for Backer if one was previously entered 
            string str = $"SELECT F_TYPE, MAX(Number) AS Max"
                + $" FROM [ESTIMATING].[dbo].[FEATURES] WHERE Category = 'Punching' AND Press_Size = '{PressSize}' GROUP BY F_TYPE";
            SqlConnection conn = new(ConnectionString);
            SqlDataAdapter da = new(str, conn); DataTable dt = new(); dt.Rows.Clear(); da.Fill(dt);
            DataView dv = new DataView(dt);
            dv.RowFilter = "F_TYPE='2 HOLES'";   M1.Maximum = int.Parse(dv[0]["Max"].ToString());
            dv.RowFilter = "F_TYPE='3 HOLES'";   M2.Maximum = int.Parse(dv[0]["Max"].ToString());
            dv.RowFilter = "F_TYPE='4-5 HOLES'"; M3.Maximum = int.Parse(dv[0]["Max"].ToString());
        }

        private void LoadData()
        {
            string str = $"SELECT * FROM [ESTIMATING].[dbo].[Quote_Details] WHERE QUOTE_NUM = '{QuoteNum}' AND Category = 'Punching'";
            SqlConnection conn = new(ConnectionString);
            SqlDataAdapter da = new(str, conn); DataTable dt = new(); dt.Rows.Clear(); da.Fill(dt);
            if (dt.Rows.Count == 0) return;
            DataView dv = new DataView(dt);
            Two   = int.Parse(dv[0]["Value1"].ToString());
            Three = int.Parse(dv[0]["Value2"].ToString());
            Over3 = int.Parse(dv[0]["Value3"].ToString());
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // Validate inputs
            // e.g., if ((Button1 + Button2 + Button3) == 0) { MessageBox.Show("Please select an option."); return; }

            // Delete current detail line
            string cmd = "DELETE [ESTIMATING].[dbo].[Quote_Details] WHERE Quote_Num = '" + QuoteNum + "' AND Category = 'Punching'";
            conn = new SqlConnection(ConnectionString); SqlCommand scmd = new SqlCommand(cmd, conn); conn.Open();

            try { scmd.ExecuteNonQuery(); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { conn.Close(); }

            // Store in Quote_Detail table:
            cmd = "INSERT INTO [ESTIMATING].[dbo].[Quote_Details] ("
                + " Quote_Num, Category, Sequence,"
                + " Param1, Param2, Param3, "
                + " Value1, Value2, Value3 ) VALUES ( "
                + $"'{QuoteNum}', 'Punching', 5, "
                + "  'Two',   'Three',   'Over3', "
                + $" '{Two}', '{Three}', '{Over3}' )";
            // Should I store the F_TYPE values?

            // Write to SQL
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