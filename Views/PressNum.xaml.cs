using MediaFoundation;
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

        private int max;          public int    Max       { get { return max;       } set { max       = value; OnPropertyChanged(); } }
        private string pressSize; public string PressSize { get { return pressSize; } set { pressSize = value; OnPropertyChanged(); } }
        private string quoteNum;  public string QuoteNum  { get { return quoteNum;  } set { quoteNum  = value; OnPropertyChanged(); } }

        private int red; public int Red { get { return red; } set { red = value; OnPropertyChanged(); } }
        private int blk; public int Blk { get { return blk; } set { blk = value; OnPropertyChanged(); } }
        private int inv; public int Inv { get { return inv; } set { inv = value; OnPropertyChanged(); } }
        private int flo; public int Flo { get { return flo; } set { flo = value; OnPropertyChanged(); } }

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
            PreviewKeyDown += (s, e) => { if (e.Key == Key.Escape) Close(); };
        }

        public void OnLoad(object sender, RoutedEventArgs e)
        {
            this.Height = this.Height *= 1.8;
            this.Width = this.Width *= 1.8;
        }


        private void LoadMaxima()
        {
            // Retrieve value for Backer if one was previously entered 
            string str = $"SELECT F_TYPE, MAX(Number) AS Max"
                + $" FROM [ESTIMATING].[dbo].[FEATURES] WHERE Category = 'PRESS NUMBERING' AND Press_Size = '{PressSize}' GROUP BY F_TYPE";
            SqlConnection conn = new(ConnectionString);
            SqlDataAdapter da = new(str, conn); DataTable dt = new(); dt.Rows.Clear(); da.Fill(dt);
            DataView dv = new DataView(dt);
            dv.RowFilter = "F_TYPE='RED'";          M1.Maximum = int.Parse(dv[0]["Max"].ToString());
            dv.RowFilter = "F_TYPE='BLK'";          M2.Maximum = int.Parse(dv[0]["Max"].ToString());
            dv.RowFilter = "F_TYPE='INVISIBLE'";    M3.Maximum = int.Parse(dv[0]["Max"].ToString());
            dv.RowFilter = "F_TYPE='RED TO FLUOR'"; M4.Maximum = int.Parse(dv[0]["Max"].ToString());

            M3.Maximum = 1; // Temporary
//            dv.RowFilter = "F_TYPE='TRN'"; M3.Maximum = int.Parse(dv[0]["Max"].ToString());
        }

        private void LoadData()
        {
            string str = $"SELECT * FROM [ESTIMATING].[dbo].[Quote_Details] WHERE QUOTE_NUM = '{QuoteNum}' AND Category = 'PressNum'";
            SqlConnection conn = new(ConnectionString);
            SqlDataAdapter da = new(str, conn); DataTable dt = new(); dt.Rows.Clear(); da.Fill(dt);
            if (dt.Rows.Count == 0) return;
            DataView dv = new DataView(dt);
            Red = int.Parse(dv[0]["Value1"].ToString());
            Blk = int.Parse(dv[0]["Value2"].ToString());
            Inv = int.Parse(dv[0]["Value3"].ToString());
            Flo = int.Parse(dv[0]["Value4"].ToString());
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
                + " Param1, Param2, Param3, Param4, "
                + " Value1, Value2, Value3, Value4 ) VALUES ( "
                + $"'{QuoteNum}', 'PressNum', 6, "
                + " 'RED', 'BLK', 'INVISIBLE', 'RED TO FLUOR', "
                + $" '{Red}', '{Blk}', '{Inv}', '{Flo}' )";

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