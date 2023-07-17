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
    public partial class Combo : Window, INotifyPropertyChanged
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

        private int combo1; public int Combo1 { get { return combo1; } set { combo1 = value; OnPropertyChanged(); } }

        private float flatCharge; public float FlatCharge { get { return flatCharge; } set { flatCharge = value; OnPropertyChanged(); } }

        public bool Removed = false;

        #endregion

        public Combo(string PRESSSIZE, string QUOTENUM)
        {
            InitializeComponent();
            this.DataContext = this;

            SqlConnection conn = new(ConnectionString);
            Title = "Quote #: " + QUOTENUM;
            QuoteNum = QUOTENUM;
            PressSize = PRESSSIZE;
            FlatCharge = 4.95F;
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
            // Retrieve value if one was previously entered 
            string str = $"SELECT F_TYPE, MAX(Number) AS Max"
                + $" FROM [ESTIMATING].[dbo].[FEATURES] WHERE Category = 'COMBO' AND Press_Size = '{PressSize}' GROUP BY F_TYPE";
            SqlConnection conn = new(ConnectionString);
            SqlDataAdapter da = new(str, conn); DataTable dt = new(); dt.Rows.Clear(); da.Fill(dt);
            DataView dv = new DataView(dt);
            dv.RowFilter = "F_TYPE='COMBO'"; M1.Maximum = int.Parse(dv[0]["Max"].ToString());
        }

        private void LoadData()
        {
            string str = $"SELECT * FROM [ESTIMATING].[dbo].[Quote_Details] WHERE QUOTE_NUM = '{QuoteNum}' AND Category = 'COMBO'";
            SqlConnection conn = new(ConnectionString);
            SqlDataAdapter da = new(str, conn); DataTable dt = new(); dt.Rows.Clear();
            try { da.Fill(dt); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            if (dt.Rows.Count == 0) return;
            DataView dv = new DataView(dt);
            Combo1 = int.Parse(dv[0]["Value1"].ToString());
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Delete();

            // Store in Quote_Detail table:
            string cmd = "INSERT INTO [ESTIMATING].[dbo].[Quote_Details] ("
                + " Quote_Num, Category, Sequence,"
                + " Param1, "
                + " Value1 ) VALUES ( "
                + $"'{QuoteNum}', 'Combo', 8, "
                + "  'Combo1', "
                + $" '{Combo1}' )";
            SqlConnection conn = new(ConnectionString); conn.Open();
            SqlCommand scmd = new(cmd, conn);

            try { scmd.ExecuteNonQuery();      }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally              { conn.Close();                }

            this.Hide();
        }

        private void Delete()
        {
            // Delete current detail line
            string cmd = "DELETE [ESTIMATING].[dbo].[Quote_Details] WHERE Quote_Num = '" + QuoteNum + "' AND Category = 'Combo'";
            conn = new SqlConnection(ConnectionString); SqlCommand scmd = new SqlCommand(cmd, conn);
            conn.Open();

            try { scmd.ExecuteNonQuery(); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { conn.Close(); }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Delete();
            Removed = true; // Call RemoveSelectedFeature() in Quotations()
            this.Hide();
        }
    }
}