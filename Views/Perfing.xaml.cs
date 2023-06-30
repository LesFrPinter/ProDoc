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
    public partial class Perfing : Window, INotifyPropertyChanged
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

        private int    maxPerfs;  public int    MaxPerfs  { get { return maxPerfs;  } set { maxPerfs  = value; OnPropertyChanged(); } }
        private string pressSize; public string PressSize { get { return pressSize; } set { pressSize = value; OnPropertyChanged(); } }
        private string quoteNum;  public string QuoteNum  { get { return quoteNum;  } set { quoteNum  = value; OnPropertyChanged(); } }

        private int partCross;  public int PartCross { get { return partCross;  } set { partCross   = value; } }
        private int cross;      public int Cross     { get { return cross;      } set { cross       = value; } }
        private int partRun;    public int PartRun   { get { return partRun;    } set { partRun     = value; } }
        private int running;    public int Running   { get { return running;    } set { running     = value; } }

        #endregion

        public Perfing(string PRESSSIZE, string QUOTENUM)
        {
            InitializeComponent();
            this.DataContext = this;
            PressSize = PRESSSIZE;
            QuoteNum = QUOTENUM;
            GetMaxValues();
            GetSavedValues();
            PreviewKeyDown += (s, e) => { if (e.Key == Key.Escape) Close(); };
        }

        private void GetMaxValues()
        {
            string cmd = $"SELECT F_TYPE, MAX(Number) AS MaxPerfs FROM [ESTIMATING].[dbo].[FEATURES] WHERE CATEGORY = 'Perfing' AND PRESS_SIZE = '{PressSize}' GROUP BY F_TYPE";
            conn = new SqlConnection(ConnectionString); da = new SqlDataAdapter(cmd, conn); dt = new DataTable(); da.Fill(dt);

            //Use E-F:
            DataView dv = new DataView(dt);
            dv.RowFilter = "F_TYPE='PART CROSS'";   PC1.Maximum = int.Parse(dv[0]["MaxPerfs"].ToString());
            dv.RowFilter = "F_TYPE='CROSS'";        C1.Maximum  = int.Parse(dv[0]["MaxPerfs"].ToString());
            dv.RowFilter = "F_TYPE='PART RUN'";     PR1.Maximum = int.Parse(dv[0]["MaxPerfs"].ToString());
            dv.RowFilter = "F_TYPE='RUNNING'";      R1.Maximum  = int.Parse(dv[0]["MaxPerfs"].ToString());
        }

        private void GetSavedValues()
        {
            // Retrieve value for Backer if one was previously entered 
            string str = $"SELECT * FROM [ESTIMATING].[dbo].[Quote_Details] WHERE QUOTE_NUM = '{QuoteNum}' AND CATEGORY = 'Perfing'";
            da.SelectCommand.CommandText = str; dt.Rows.Clear(); da.Fill(dt);
            if (dt.Rows.Count > 0) 
             {
                PartCross = int.Parse(dt.Rows[0]["Value1"].ToString());
                Cross     = int.Parse(dt.Rows[0]["Value2"].ToString());
                PartRun   = int.Parse(dt.Rows[0]["Value3"].ToString());
                Running   = int.Parse(dt.Rows[0]["Value4"].ToString());
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // Validate inputs
            // e.g., if ((Button1 + Button2 + Button3) == 0) { MessageBox.Show("Please select an option."); return; }

            // Delete current detail line
            string cmd = "DELETE [ESTIMATING].[dbo].[Quote_Details] WHERE Quote_Num = '" + QuoteNum + "' AND Category = 'Perfing'";
            conn = new SqlConnection(ConnectionString); SqlCommand scmd = new SqlCommand(cmd, conn); conn.Open();

            try { scmd.ExecuteNonQuery(); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { conn.Close(); }

            // Store in Quote_Detail table:
            cmd =  "INSERT INTO [ESTIMATING].[dbo].[Quote_Details] ("
                +  " Quote_Num, Category, Sequence,"
                +  " Param1, Param2, Param3, Param4, "
                +  " Value1, Value2, Value3, Value4 ) VALUES ( "
                + $"'{QuoteNum}', 'Perfing', 3, "
                +  " 'Part Cross', 'Cross', 'Part Run', 'Running', "
                + $" '{PartCross}', '{Cross}', '{PartRun}', '{Running}' )";

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
        { this.Close(); }
    }
}