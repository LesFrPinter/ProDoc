using System;
using System.Data;
using System.Windows;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Configuration;
using System.Windows.Controls;
using Telerik.Windows.Documents.Spreadsheet.Expressions.Functions;

namespace ProDocEstimate.Views
{
    public partial class InkColors : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) 
         { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

        #region Properties

        private int    maxColors; public int    MaxColors { get { return maxColors; } set { maxColors = value; OnPropertyChanged(); } }
        private string pressSize; public string PressSize { get { return pressSize; } set { pressSize = value; OnPropertyChanged(); } }
        private string quoteNum;  public string QuoteNum  { get { return quoteNum;  } set { quoteNum  = value; OnPropertyChanged(); } }

        private int desens;     public int Desens   { get { return desens;   } set { desens   = value; OnPropertyChanged(); } }
        private int split;      public int Split    { get { return split;    } set { split    = value; OnPropertyChanged(); } }
        private int thermo;     public int Thermo   { get { return thermo;   } set { thermo   = value; OnPropertyChanged(); } }
        private int backer;     public int Backer   { get { return backer;   } set { backer   = value; OnPropertyChanged(); } }
        private int std;        public int Std      { get { return std;      } set { std      = value; OnPropertyChanged(); } }
        private int blackStd;   public int BlackStd { get { return blackStd; } set { blackStd = value; OnPropertyChanged(); } }
        private int pms;        public int PMS      { get { return pms;      } set { pms      = value; OnPropertyChanged(); } }

        private float total;    public float Total  { get { return total;    } set { total    = value; OnPropertyChanged(); } }

        public string ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public SqlConnection? conn;
        public SqlDataAdapter? da;
        public DataTable? dt;
        public SqlCommand? scmd;

        #endregion

        public InkColors(string PRESSSIZE, string QUOTENUM)
        {
            InitializeComponent();
            this.DataContext = this;
            Title = "Quote #: " + QUOTENUM;
            QuoteNum = QUOTENUM;
            PressSize = PRESSSIZE;

            Total = 49.95F;     // Calculcate this based on user input

            LoadMaxColors();
        }

        private void LoadMaxColors()
        {
            SqlConnection cn = new(ConnectionString);
            string str = "SELECT Number FROM [Estimating].[dbo].[PressSizeColors] WHERE Size = '" + PressSize + "'";
            SqlDataAdapter da = new(str, cn); DataTable dt = new DataTable(); da.Fill(dt);

            if (dt.Rows.Count == 0) { MessageBox.Show("Not found..."); return; }
            else                    { MaxColors = Int32.Parse(dt.Rows[0]["Number"].ToString()); }
        }

        private void RadNumericUpDown_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        {
            string chosen = ((System.Windows.FrameworkElement)sender).Name;
            switch (chosen)
            {   case "Std1":      { BlackStd = 0; PMS      = 0; break; }
                case "BlackStd1": { Std      = 0; PMS      = 0; break; }
                case "PMS1":      { Std      = 0; BlackStd = 0; break; }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // Validate inputs
            // e.g., if ((Button1 + Button2 + Button3) == 0) { MessageBox.Show("Please select an option."); return; }

            // Delete current detail line
            string cmd = "DELETE [ESTIMATING].[dbo].[Quote_Details] WHERE Quote_Num = '" + QuoteNum + "' AND Category = 'Ink Color'";
            conn = new SqlConnection(ConnectionString); SqlCommand scmd = new SqlCommand(cmd, conn); conn.Open();

            try                  { scmd.ExecuteNonQuery();      }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally              { conn.Close();                }

            // Store in Quote_Detail table:
            cmd = "INSERT INTO [ESTIMATING].[dbo].[Quote_Details] ( Quote_Num, Category, Sequence, Param1, Param2, Value2, Amount ) VALUES ( ";
            cmd += "'" + QuoteNum.ToString() + "', 'Ink Color', 2, ";

            // Which radiobutton was selected?
            //if (Button1 == 1) { cmd += "'BACKER',    "; }
            //if (Button2 == 1) { cmd += "'BACKER STD',"; }
            //if (Button3 == 1) { cmd += "'BACKER PMS',"; }
            // How many changes?
            //cmd += " 'Changes', " + Changes.ToString().TrimEnd() + ", ";

            cmd += " 'a', 'b', 'c', ";

            // Finally, the total charge for Backer:
            cmd += Total.ToString("N2") + " )";

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
        { this.Close(); }

    }
}