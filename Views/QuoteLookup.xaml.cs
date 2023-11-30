using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ProDocEstimate.Views
{
    public partial class QuoteLookup : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

        public string ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public SqlConnection? cn = new SqlConnection();
        public SqlDataAdapter? da;

        private string? quote_Num  = "";    public string?  Quote_Num   { get { return quote_Num = ""; } set { quote_Num  = value; OnPropertyChanged(); } }
        private string? selQuote   = "";    public string?  SelQuote    { get { return selQuote;       } set { selQuote   = value; OnPropertyChanged(); } }
        private string? cust_Name  = "";    public string?  Cust_Name   { get { return cust_Name;      } set { cust_Name  = value; OnPropertyChanged(); } }
        private string? cust_Num;           public string?  Cust_Num    { get { return cust_Num;       } set { cust_Num   = value; OnPropertyChanged(); } }
        private bool    auto       = false; public bool     Auto        { get { return auto;           } set { auto       = value; OnPropertyChanged(); OnPropertyChanged("AutoViz"); } }
        private Visibility? autoViz;        public Visibility AutoViz   { get { return (Auto) ? Visibility.Hidden: Visibility.Visible; } }

        private bool    descending = false; public bool     Descending  { get { return descending;     } set { descending = value; OnPropertyChanged(); } }
        private bool    partial    = false; public bool     Partial     { get { return partial;        } set { partial    = value; OnPropertyChanged(); } }

        public QuoteLookup() { InitializeComponent(); this.DataContext = this; }

        private void btnGo_Click(object sender, RoutedEventArgs e)
        {
            string cmd = "SELECT * FROM [ESTIMATING].[dbo].[QUOTES] "
                       + "    LEFT JOIN [ESTIMATING].[dbo].[CUSTOMER]"
                       + " ON QUOTES.CUST_NUM = CUSTOMER.CUST_NUMB"
                       + " LEFT JOIN[ESTIMATING].[dbo].[CUSTOMER_CONTACT]"
                       + " ON [ESTIMATING].[dbo].[CUSTOMER].CUST_NUMB " 
                       + "  = [ESTIMATING].[dbo].[CUSTOMER_CONTACT].CUST_NUMB";

            if (txtCustName.Text.Trim().Length > 0)
            {   if (Partial == true)
                { cmd += " AND CUST_NAME LIKE '%" + txtCustName.Text.Trim() + "%'"; }
                else
                { cmd += " AND CUST_NAME LIKE '" + txtCustName.Text.Trim() + "%'"; }
            };
            
            if (txtQuoteNum.Text.Trim().Length > 0)
            { cmd += " AND QUOTES.QUOTE_NUM = " + txtQuoteNum.Text.Trim(); }
            else
            if (txtCustNo.Text.Trim().Length > 0)
            { cmd += " AND QUOTES.CUST_NUMB = " + txtCustNo.Text.Trim(); };

            cmd += " ORDER BY QUOTE_NUM";

            if (Descending == true) { cmd += " DESC"; }

            cn = new SqlConnection(ConnectionString);
            da = new SqlDataAdapter(cmd, cn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            dgQuotes.ItemsSource = dt.DefaultView;
            Cust_Num = ""; Quote_Num = ""; Cust_Name = "";
        }

        private void dgQuotes_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DataRowView drv = (DataRowView)dgQuotes.SelectedItem;
            SelQuote = (drv["QUOTE_NUM"]).ToString();
            Hide();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        { SelQuote = null; Hide(); }
    }
}