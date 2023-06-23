using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProDocEstimate.Views
{
    public partial class PressNum : Window, INotifyPropertyChanged
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

        private bool red; public bool Red { get { return red; } set { red = value; OnPropertyChanged(); } }
        private bool blk; public bool Blk { get { return blk; } set { blk = value; OnPropertyChanged(); } }

        #endregion

        public PressNum(string PRESSSIZE, string QUOTENUM)
        {
            InitializeComponent();
            this.DataContext = this;
            Title = "Quote #: " + QUOTENUM;
            QuoteNum = QUOTENUM;
            PressSize = PRESSSIZE;
            LoadMaxima();
            LoadData();
        }

        private void LoadMaxima()
        {
            // Retrieve value for Backer if one was previously entered 
            string str = $"SELECT F_TYPE, MAX(Number) AS Max"
                + $" FROM [ESTIMATING].[dbo].[FEATURES] WHERE Category = 'PRESS NUMBERING' AND Press_Size = '{PressSize}' GROUP BY F_TYPE";
            SqlConnection conn = new(ConnectionString);
            SqlDataAdapter da = new(str, conn); DataTable dt = new(); dt.Rows.Clear(); da.Fill(dt);
            DataView dv = new DataView(dt);
            //dv.RowFilter = "F_TYPE='DIGITAL'"; M1.Maximum = int.Parse(dv[0]["Max"].ToString());
            //dv.RowFilter = "F_TYPE='PACK2PACK'"; M2.Maximum = int.Parse(dv[0]["Max"].ToString());
            //dv.RowFilter = "F_TYPE='PRESS'"; M3.Maximum = int.Parse(dv[0]["Max"].ToString());
        }

        private void LoadData()
        {
            string str = $"SELECT * FROM [ESTIMATING].[dbo].[Quote_Details] WHERE QUOTE_NUM = '{QuoteNum}' AND Category = 'PressNum'";
            SqlConnection conn = new(ConnectionString);
            SqlDataAdapter da = new(str, conn); DataTable dt = new(); dt.Rows.Clear(); da.Fill(dt);
            if (dt.Rows.Count == 0) return;
            DataView dv = new DataView(dt);
            //Digital = int.Parse(dv[0]["Value1"].ToString());
            //Pack2Pack = int.Parse(dv[0]["Value2"].ToString());
            //Press = int.Parse(dv[0]["Value3"].ToString());
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // Validate inputs
            // e.g., if ((Button1 + Button2 + Button3) == 0) { MessageBox.Show("Please select an option."); return; }

            // Delete current detail line
            string cmd = "DELETE [ESTIMATING].[dbo].[Quote_Details] WHERE Quote_Num = '" + QuoteNum + "' AND Category = 'PressNum'";
            conn = new SqlConnection(ConnectionString); SqlCommand scmd = new SqlCommand(cmd, conn); conn.Open();

            try { scmd.ExecuteNonQuery(); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { conn.Close(); }

            // Store in Quote_Detail table:
            cmd = "INSERT INTO [ESTIMATING].[dbo].[Quote_Details] ("
                + " Quote_Num, Category, Sequence,"
                + " Param1, Param2, "
                + " Value1, Value2 ) VALUES ( "
                + $"'{QuoteNum}', 'PressNum', 6, "
                + " 'RED', 'BLK' "
                + $" '{Red}', '{Blk}' )";

            // Write to SQL
            //            conn = new SqlConnection(ConnectionString);
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