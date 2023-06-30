using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ProDocEstimate.Views
{
    public partial class Converting : Window, INotifyPropertyChanged
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

        private int crash; public int Crash { get { return crash; } set { crash = value; OnPropertyChanged(); } }
        private int black; public int Black { get { return black; } set { black = value; OnPropertyChanged(); } }
        private int snap2; public int Snap2 { get { return snap2; } set { snap2 = value; OnPropertyChanged(); } }
        private int snap5; public int Snap5 { get { return snap5; } set { snap5 = value; OnPropertyChanged(); } }
        private int cont2; public int Cont2 { get { return cont2; } set { cont2 = value; OnPropertyChanged(); } }
        private int cont5; public int Cont5 { get { return cont5; } set { cont5 = value; OnPropertyChanged(); } }
        private int fold;  public int Fold  { get { return fold;  } set { fold  = value; OnPropertyChanged(); } }
        private int tape;  public int Tape  { get { return tape;  } set { tape  = value; OnPropertyChanged(); } }

        #endregion

        public Converting(string PRESSSIZE, string QUOTENUM)
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
            // Retrieve values if previously entered 
            string str = $"SELECT F_TYPE, MAX(Number) AS Max"
                +  " FROM [ESTIMATING].[dbo].[FEATURES] " 
                + $" WHERE Category = 'Converting' AND Press_Size = '{PressSize}' GROUP BY F_TYPE";

            SqlConnection conn = new(ConnectionString);
            SqlDataAdapter da = new(str, conn); DataTable dt = new(); dt.Rows.Clear();

            da.Fill(dt);
            DataView dv = new DataView(dt);

            dv.RowFilter = "F_TYPE='2 CRASH NUMB'";   M1.Maximum = int.Parse(dv[0]["Max"].ToString());
            dv.RowFilter = "F_TYPE='BLACK NUMB'";     M2.Maximum = int.Parse(dv[0]["Max"].ToString());
            dv.RowFilter = "F_TYPE='SNAP GLUE 2-4'";  M3.Maximum = int.Parse(dv[0]["Max"].ToString());
            dv.RowFilter = "F_TYPE='SNAP GLUE 5-12'"; M4.Maximum = int.Parse(dv[0]["Max"].ToString());
            dv.RowFilter = "F_TYPE='FOLDING'";        M5.Maximum = int.Parse(dv[0]["Max"].ToString());
            dv.RowFilter = "F_TYPE='CONT GLUE 2-4'";  M6.Maximum = int.Parse(dv[0]["Max"].ToString());
            dv.RowFilter = "F_TYPE='CONT GLUE 5-12'"; M7.Maximum = int.Parse(dv[0]["Max"].ToString());
            dv.RowFilter = "F_TYPE='TRANSFER TAPE'";  M8.Maximum = int.Parse(dv[0]["Max"].ToString());
        }

        private void LoadData()
        {
            string str = $"SELECT * FROM [ESTIMATING].[dbo].[Quote_Details] WHERE QUOTE_NUM = '{QuoteNum}' AND Category = 'Converting'";
            SqlConnection conn = new(ConnectionString);
            SqlDataAdapter da = new(str, conn); DataTable dt = new(); dt.Rows.Clear(); da.Fill(dt);
            if (dt.Rows.Count == 0) return;
            DataView dv = new DataView(dt);

            Crash = int.Parse(dv[0]["Value1"].ToString());
            Black = int.Parse(dv[0]["Value2"].ToString());
            Snap2 = int.Parse(dv[0]["Value3"].ToString());
            Snap5 = int.Parse(dv[0]["Value4"].ToString());
            Cont2 = int.Parse(dv[0]["Value5"].ToString());
            Cont5 = int.Parse(dv[0]["Value6"].ToString());
            Fold  = int.Parse(dv[0]["Value7"].ToString());
            Tape  = int.Parse(dv[0]["Value8"].ToString());
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // Validate inputs
            // e.g., if ((Button1 + Button2 + Button3) == 0) { MessageBox.Show("Please select an option."); return; }

            // Delete current detail line
            string cmd = "DELETE [ESTIMATING].[dbo].[Quote_Details] WHERE Quote_Num = '" + QuoteNum + "' AND Category = 'Converting'";
            conn = new SqlConnection(ConnectionString); SqlCommand scmd = new SqlCommand(cmd, conn); conn.Open();

            try { scmd.ExecuteNonQuery(); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { conn.Close(); }

            // Store in Quote_Detail table:
            cmd = "INSERT INTO [ESTIMATING].[dbo].[Quote_Details] ("
                + " Quote_Num, Category, Sequence,"
                + " Param1, Param2, Param3, Param4, Param5, Param6, Param7, Param8, "
                + " Value1, Value2, Value3, Value4, Value5, Value6, Value7, Value8 ) VALUES ( "
                + $"'{QuoteNum}', 'Converting', 9, "
                + " '2 CRASH NUMB', 'BLACK NUMB', 'SNAP GLUE 2-4', 'SNAP GLUE 5-12', "
                + " 'CONT GLUE 2-4', 'CONT GLUE 5-12', 'FOLDING', 'TRANSFER TAPE', "
                + $" '{Crash}', '{Black}', '{Snap2}', '{Snap5}', "
                + $" '{Cont2}', '{Cont5}', '{Fold}',  '{Tape}' )";

            // Write to SQL
            //  conn = new SqlConnection(ConnectionString);
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