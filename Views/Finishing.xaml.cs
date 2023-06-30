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
    public partial class Finishing : Window, INotifyPropertyChanged
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

        private int max;          public int Max          { get { return max;       } set { max         = value; OnPropertyChanged(); } }
        private string pressSize; public string PressSize { get { return pressSize; } set { pressSize   = value; OnPropertyChanged(); } }
        private string quoteNum;  public string QuoteNum  { get { return quoteNum;  } set { quoteNum    = value; OnPropertyChanged(); } }

        private int book;       public int Book         { get { return book;        } set { book        = value; OnPropertyChanged(); } }
        private int cello;      public int Cello        { get { return cello;       } set { cello       = value; OnPropertyChanged(); } }
        private int drillHoles; public int DrillHoles   { get { return drillHoles;  } set { drillHoles  = value; OnPropertyChanged(); } }
        private int pad;        public int Pad          { get { return pad;         } set { pad         = value; OnPropertyChanged(); } }
        private int trim;       public int Trim         { get { return trim;        } set { trim        = value; OnPropertyChanged(); } }

        #endregion

        public Finishing(string PRESSSIZE, string QUOTENUM)
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
            // Retrieve values for Finishing 
            string str = $"SELECT F_TYPE, MAX(Number) AS Max"
                + $" FROM [ESTIMATING].[dbo].[FEATURES] WHERE Category = 'FINISHING' AND Press_Size = '{PressSize}' GROUP BY F_TYPE";

            Clipboard.SetText(str);

            SqlConnection conn = new(ConnectionString);
            SqlDataAdapter da = new(str, conn); DataTable dt = new(); dt.Rows.Clear(); da.Fill(dt);
            DataView dv = new DataView(dt);

            Debugger.Break();

            dv.RowFilter = "F_TYPE='BOOK'";         M1.Maximum = int.Parse(dv[0]["Max"].ToString());
            dv.RowFilter = "F_TYPE='CELLO'";        M2.Maximum = int.Parse(dv[0]["Max"].ToString());
            dv.RowFilter = "F_TYPE='DRILL HOLES'";  M3.Maximum = int.Parse(dv[0]["Max"].ToString());
            dv.RowFilter = "F_TYPE='PAD'";          M4.Maximum = int.Parse(dv[0]["Max"].ToString());
            dv.RowFilter = "F_TYPE='TRIM'";         M5.Maximum = int.Parse(dv[0]["Max"].ToString());
        }

        private void LoadData()
        {
            string str = $"SELECT * FROM [ESTIMATING].[dbo].[Quote_Details] WHERE QUOTE_NUM = '{QuoteNum}' AND Category = 'Finishing'";
            SqlConnection conn = new(ConnectionString);
            SqlDataAdapter da = new(str, conn); DataTable dt = new(); dt.Rows.Clear(); da.Fill(dt);
            if (dt.Rows.Count == 0) return;
            DataView dv = new DataView(dt);
            Book        = int.Parse(dv[0]["Value1"].ToString());
            Cello       = int.Parse(dv[0]["Value2"].ToString());
            DrillHoles  = int.Parse(dv[0]["Value3"].ToString());
            Pad         = int.Parse(dv[0]["Value4"].ToString());
            Trim        = int.Parse(dv[0]["Value5"].ToString());
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // Validate inputs
            // e.g., if ((Button1 + Button2 + Button3) == 0) { MessageBox.Show("Please select an option."); return; }

            // Delete current detail line
            string cmd = "DELETE [ESTIMATING].[dbo].[Quote_Details] WHERE Quote_Num = '" + QuoteNum + "' AND Category = 'Finishing'";
            conn = new SqlConnection(ConnectionString); SqlCommand scmd = new SqlCommand(cmd, conn); conn.Open();

            try { scmd.ExecuteNonQuery(); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { conn.Close(); }

            // Store in Quote_Detail table:
            cmd = "INSERT INTO [ESTIMATING].[dbo].[Quote_Details] ("
                + " Quote_Num, Category, Sequence,"
                + " Param1, Param2, Param3, Param4, Param5 "
                + " Value1, Value2, Value3, Value4, Value5 ) VALUES ( "
                + $"'{QuoteNum}', 'MICR', 7, "
                + " 'Book', 'Cello', 'Drill Holes', 'Pad', 'Trim', "
                + $" '{Book}', '{Cello}', '{DrillHoles}', '{Pad}', '{Trim}' )";

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