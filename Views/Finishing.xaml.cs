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
        public DataView? dv;

        private int    max;         public int    Max        { get { return max;        } set { max         = value; OnPropertyChanged(); } }
        private string pressSize;   public string PressSize  { get { return pressSize;  } set { pressSize   = value; OnPropertyChanged(); } }
        private string quoteNum;    public string QuoteNum   { get { return quoteNum;   } set { quoteNum    = value; OnPropertyChanged(); } }

        private string book;        public string Book       { get { return book;       } set { book        = value; OnPropertyChanged(); } }
        private string cello;       public string Cello      { get { return cello;      } set { cello       = value; OnPropertyChanged(); } }
        private string drillHoles;  public string DrillHoles { get { return drillHoles; } set { drillHoles  = value; OnPropertyChanged(); } }
        private string pad;         public string Pad        { get { return pad;        } set { pad         = value; OnPropertyChanged(); } }
        private string trim;        public string Trim       { get { return trim;       } set { trim        = value; OnPropertyChanged(); } }

        private float  flat;        public float  Flat       { get { return flat;       } set { flat        = value; OnPropertyChanged(); } }
        private float  run;         public float  Run        { get { return run;        } set { run         = value; OnPropertyChanged(); } }

        private float  flatTotal;   public float  FlatTotal  { get { return flatTotal;  } set { flatTotal   = value; OnPropertyChanged(); } }
        private float  thouTotal;   public float  ThouTotal  { get { return thouTotal;  } set { thouTotal   = value; OnPropertyChanged(); } }

        #endregion

        public Finishing(string PRESSSIZE, string QUOTENUM)
        {
            InitializeComponent();
            this.DataContext = this;
            Title = "Quote #: " + QUOTENUM;
            QuoteNum = QUOTENUM;
            PressSize = PRESSSIZE;
            LoadDropDowns();
            LoadData();
            PreviewKeyDown += (s, e) => { if (e.Key == Key.Escape) Close(); };
        }

        private void LoadDropDowns()
        {
            string str = $"SELECT F_TYPE, CONVERT(VARCHAR(10),CONVERT(INT,NUMBER)) AS NUMBER FROM [ESTIMATING].[dbo].[FEATURES] WHERE CATEGORY = 'FINISHING' AND PRESS_SIZE = '{PressSize}' AND NUMBER NOT LIKE '%-%'"
                       + $" UNION ALL SELECT F_TYPE,                                  NUMBER FROM [ESTIMATING].[dbo].[FEATURES] WHERE CATEGORY = 'FINISHING' AND PRESS_SIZE = '{PressSize}' AND (NUMBER LIKE '%-%') ORDER BY F_TYPE";

//            Clipboard.SetText(str);

            SqlConnection conn = new(ConnectionString);
            SqlDataAdapter da = new(str, conn); dt = new(); da.Fill(dt);
            DataView dv = (dt.DefaultView as DataView) as DataView;

            //NOTE: If they need the ability to blank a value that was previously entered, add "" to the top of each ItemsList.

            dv.RowFilter = "F_TYPE='BOOK'";         for (int i = 0; i < dv.Count; i++) { M1.Items.Add(dv[i]["number"].ToString()); }
            dv.RowFilter = "F_TYPE='CELLO'";        for (int i = 0; i < dv.Count; i++) { M2.Items.Add(dv[i]["number"].ToString()); }
            dv.RowFilter = "F_TYPE='DRILL HOLES'";  for (int i = 0; i < dv.Count; i++) { M3.Items.Add(dv[i]["number"].ToString()); }
            dv.RowFilter = "F_TYPE='PAD'";          for (int i = 0; i < dv.Count; i++) { M4.Items.Add(dv[i]["number"].ToString()); }
            dv.RowFilter = "F_TYPE='TRIM'";         for (int i = 0; i < dv.Count; i++) { M5.Items.Add(dv[i]["number"].ToString()); }
        }

        private void LoadData()
        {
            string str = $"SELECT * FROM [ESTIMATING].[dbo].[Quote_Details] WHERE QUOTE_NUM = '{QuoteNum}' AND Category = 'Finishing'";
            SqlConnection conn = new(ConnectionString);
            SqlDataAdapter da = new(str, conn); DataTable dt = new(); dt.Rows.Clear(); da.Fill(dt);
            if (dt.Rows.Count == 0) return;
            DataView dv = new DataView(dt);
            Book        = dv[0]["Value1"].ToString();
            Cello       = dv[0]["Value2"].ToString();
            DrillHoles  = dv[0]["Value3"].ToString();
            Pad         = dv[0]["Value4"].ToString();
            Trim        = dv[0]["Value5"].ToString();
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
                + " Param1, Param2, Param3, Param4, Param5, "
                + " Value1, Value2, Value3, Value4, Value5 ) VALUES ( "
                + $"'{QuoteNum}', 'Finishing', 7, "
                + "   'Book',   'Cello',   'Drill Holes',  'Pad',   'Trim', "
                + $" '{Book}', '{Cello}', '{DrillHoles}', '{Pad}', '{Trim}' )";

            // Write to SQL
            //            conn = new SqlConnection(ConnectionString);
            scmd.CommandText = cmd;
            conn.Open();
            try { scmd.ExecuteNonQuery(); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { conn.Close(); }

            this.Close();
        }

        private void GrandTotal()
        {   FlatTotal = 0.00F; ThouTotal = 0.00F; dt = new();

            // TODO: Can you return all five values in a single SELECT?

            float Flat2 = 0.00F;
            dt = new();
            string str = $"SELECT FLAT_CHARGE, RUN_CHARGE FROM [ESTIMATING].[dbo].[FEATURES] WHERE CATEGORY = 'FINISHING' AND PRESS_SIZE = '{PressSize}' AND F_TYPE = 'BOOK' AND Number = '{Book}'";
            SqlConnection conn = new(ConnectionString); SqlDataAdapter da = new(str, conn); da.Fill(dt);
            dv = dt.DefaultView; string s = dv[0][0].ToString();
            Flat = 0.00F; if (dv[0][0] != null) { if (float.TryParse(s, out Flat2)) { Flat = Flat2; } }
            Run  = 0.00F; if (dv[0][1] != null) { Run  = float.Parse(dv[0][1].ToString()); }
            FlatTotal += Flat; ThouTotal += Run;

            dt.Clear(); 
            str = $"SELECT FLAT_CHARGE, RUN_CHARGE FROM [ESTIMATING].[dbo].[FEATURES] WHERE CATEGORY = 'FINISHING' AND PRESS_SIZE = '{PressSize}' AND F_TYPE = 'CELLO' AND Number = '{Cello}'";
            da.SelectCommand.CommandText = str; da.Fill(dt);
            dv = dt.DefaultView;
            Flat = 0.00F; if (dv[0][0] != null) { if (float.TryParse(s, out Flat2)) { Flat = Flat2; } }
            Run = 0.00F; if (dt.Rows[0][1] != null) { Run  = float.Parse(dv[0][1].ToString()); }
            FlatTotal += Flat; ThouTotal += Run;

            dt.Clear();
            str = $"SELECT FLAT_CHARGE, RUN_CHARGE FROM [ESTIMATING].[dbo].[FEATURES] WHERE CATEGORY = 'FINISHING' AND PRESS_SIZE = '{PressSize}' AND F_TYPE = 'DRILL HOLES' AND Number = '{DrillHoles}'";
            da.SelectCommand.CommandText = str; da.Fill(dt);
            dv = dt.DefaultView;
            Flat = 0.00F; if (dv[0]["FLAT_CHARGE"] != null) { if (float.TryParse(dv[0]["FLAT_CHARGE"].ToString(), out Flat2)) { Flat = Flat2; } }
            Run = 0.00F; if (dv[0][1] != null) { Run  = float.Parse(dv[0][1].ToString()); }
            FlatTotal += Flat; ThouTotal += Run;

            dt.Clear();
            str = $"SELECT FLAT_CHARGE, RUN_CHARGE FROM [ESTIMATING].[dbo].[FEATURES] WHERE CATEGORY = 'FINISHING' AND PRESS_SIZE = '{PressSize}' AND F_TYPE = 'PAD' AND Number = '{Pad}'";
            da.SelectCommand.CommandText = str; da.Fill(dt);
            dv = dt.DefaultView;
            Flat = 0.00F; if (dv[0][0] != null) { if (float.TryParse(s, out Flat2)) { Flat = Flat2; } }
            Run = 0.00F; if (dv[0][1] != null) { Run  = float.Parse(dv[0][1].ToString()); }
            FlatTotal += Flat; ThouTotal += Run;

            dt.Clear();
            str = $"SELECT FLAT_CHARGE, RUN_CHARGE FROM [ESTIMATING].[dbo].[FEATURES] WHERE CATEGORY = 'FINISHING' AND PRESS_SIZE = '{PressSize}' AND F_TYPE = 'TRIM' AND Number = '{Trim}'";
            da.SelectCommand.CommandText = str; da.Fill(dt);
            dv = dt.DefaultView;
            Flat = 0.00F; if (dv[0][0] != null) { if (float.TryParse(s, out Flat2)) { Flat = Flat2; } }
            Run = 0.00F; if (dv[0][1] != null) { Run  = float.Parse(dv[0][1].ToString()); }
            FlatTotal += Flat; ThouTotal += Run;
        }

        private void M1_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        { GrandTotal(); }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        { Close(); }

    }
}