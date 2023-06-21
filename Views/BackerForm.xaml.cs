using System;
using System.Data;
using System.Windows;
using System.Configuration;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Runtime.CompilerServices;

// The FEATURES table has the FLAT_CHARGE and PREP_DEPT_MATL (price per change) for each press size and F_TYPE. It's in an Excel spreadsheet now, but Tim is going to load it.

namespace ProDocEstimate.Views
{
    public partial class BackerForm : Window, INotifyPropertyChanged
    {
        public string ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public SqlConnection? conn;
        public SqlDataAdapter? da;
        public DataTable dt;
        public SqlCommand? scmd;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

        private string pressSize = ""; public string PressSize { get { return pressSize; } set { pressSize = value; OnPropertyChanged(); } }

        private float flat;  public float Flat  { get { return flat;    } set { flat    = value; OnPropertyChanged(); } }
        private float chng;  public float Chng  { get { return chng;    } set { chng    = value; OnPropertyChanged(); } }
        private int changes; public int Changes { get { return changes; } set { changes = value; OnPropertyChanged(); } }
        private float total; public float Total { get { return total;   } set { total   = value; OnPropertyChanged(); } }

        private bool backer; public bool Backer { get { return backer;  } set { backer  = value; OnPropertyChanged(); } }

        private bool one;    public bool One    { get { return one;     } set { one     = value; OnPropertyChanged(); Button1 = (One) ? 1 : 0; } }
        private bool two;    public bool Two    { get { return two;     } set { two     = value; OnPropertyChanged(); Button2 = (Two) ? 1 : 0; } }
        private bool three;  public bool Three  { get { return three;   } set { three   = value; OnPropertyChanged(); Button3 = (Three) ? 1 : 0; } }

        private int button1; public int Button1 { get { return button1; } set { button1 = value; OnPropertyChanged(); } }
        private int button2; public int Button2 { get { return button2; } set { button2 = value; OnPropertyChanged(); } }
        private int button3; public int Button3 { get { return button3; } set { button3 = value; OnPropertyChanged(); } }

        private string quoteNo; public string QuoteNo { get { return quoteNo; } set { quoteNo = value; OnPropertyChanged(); } }

        public BackerForm(string PSize, string QUOTENUM)
        {
            InitializeComponent();
            DataContext = this;
            PressSize = PSize;
            Title = Title = "Quote #: " + QUOTENUM + "  PressSize: " + PressSize;  // passed in when the form is instantiated in Quotations.xaml.cs.
            Total = -1.00F;
            QuoteNo = QUOTENUM;
            LoadQuote();
        }

        private void LoadQuote()
        {
            string cmd = "SELECT * FROM [ESTIMATING].[dbo].[QUOTE_DETAILS] WHERE QUOTE_NUM = '" + QuoteNo + "' AND CATEGORY = 'Backer'";
            dt = new DataTable("Details");
            conn = new SqlConnection(ConnectionString);
            da = new SqlDataAdapter(cmd, conn); da.Fill(dt);
            if(dt.Rows.Count > 0 )
            {
                string? F_TYPE = dt.Rows[0]["Param1"].ToString();
                switch (F_TYPE)
                {
                    case "BACKER": One = true; break;
                    case "BACKER STD": Two = true; break;
                    case "BACKER PMS": Three = true; break;
                }
                Changes = Int32.Parse(dt.Rows[0]["Value2"].ToString());
                Total = float.Parse(dt.Rows[0]["Amount"].ToString());
            }
            conn.Close();
        }

        private void CalcTotal()
        {
            if (Total == -1.00F) { Total = 0.00F; return; } // First time it's called is spurious

            if ((Button1 + Button2 + Button3) == 0) { MessageBox.Show("Please select an option."); return; }

            string cmd = $"SELECT FLAT_CHARGE, PREP_DEPT_MATL FROM [ESTIMATING].[dbo].[FEATURES] WHERE CATEGORY = 'BACKER' AND PRESS_SIZE = '{PressSize}' AND F_TYPE = '";
            if (Button1 == 1) { cmd += "BACKER'"; }
            if (Button2 == 1) { cmd += "BACKER STD'"; }
            if (Button3 == 1) { cmd += "BACKER PMS'"; }

            dt = new DataTable("Charges");
            conn = new SqlConnection(ConnectionString);
            da = new SqlDataAdapter(cmd, conn); da.Fill(dt);

            if(dt.Rows.Count == 0) { MessageBox.Show("No data"); return; }

            Flat = float.Parse(dt.Rows[0][0].ToString());
            Chng = float.Parse(dt.Rows[0][1].ToString());
            Total = Flat + Chng * Changes;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if((Button1+Button2+Button3)==0) { MessageBox.Show("Please select an option."); return; }
            // Delete current detail line
            string cmd = "DELETE [ESTIMATING].[dbo].[Quote_Details] WHERE Quote_Num = '" + QuoteNo + "' AND Category = 'Backer'";
            conn = new SqlConnection(ConnectionString);
            SqlCommand scmd = new SqlCommand(cmd, conn); conn.Open();
            try { scmd.ExecuteNonQuery(); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { conn.Close(); }

            // Store in Quote_Detail table:
            cmd = "INSERT INTO [ESTIMATING].[dbo].[Quote_Details] ( Quote_Num, Category, Sequence, Param1, Param2, Value2, Amount ) VALUES ( ";
            cmd += "'" + QuoteNo.ToString() + "', 'Backer', 1, ";
            // Which radiobutton was selected?
            if (Button1 == 1) { cmd += "'BACKER',    "; }
            if (Button2 == 1) { cmd += "'BACKER STD',"; }
            if (Button3 == 1) { cmd += "'BACKER PMS',"; }
            // How many changes?
            cmd += " 'Changes', " + Changes.ToString().TrimEnd() + ", ";
            // Finally, the total charge for Backer:
            cmd += Total.ToString("N2") + " )";

            // Write to SQL
//            conn = new SqlConnection(ConnectionString);
            scmd.CommandText = cmd; 
            conn.Open();
            try { scmd.ExecuteNonQuery(); }
            catch ( Exception ex) { MessageBox.Show(ex.Message); }
            finally { conn.Close(); scmd = null; conn = null; }
            
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        { this.Close(); }

        private void RadNumericUpDown_LostFocus(object sender, RoutedEventArgs e)
        {
            CalcTotal();
        }

        private void RadNumericUpDown_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        {
            CalcTotal();
        }
    }
}
