using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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
  public partial class QuoteLookup : Window
  {
		public string ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
		public SqlConnection? cn = new SqlConnection();
		public SqlDataAdapter? da;

		public QuoteLookup() { InitializeComponent(); }

		private void btnGo_Click(object sender, RoutedEventArgs e)
		{
      string cmd = "SELECT * FROM QUOTES, CRC_CU10"
         + " WHERE QUOTES.CUST_NUM = CRC_CU10.CUST_NUMB";
      if(txtQuoteNum.Text.Trim().Length == 0 )
           { cmd += " AND CUST_NAME LIKE '%'" + txtCustName.Text.Trim() + "'%' "; }
      else { cmd += " AND QUOTES.QUOTE_NUM = " + txtQuoteNum.Text.Trim(); }
      cn = new SqlConnection(ConnectionString);
      da = new SqlDataAdapter(cmd, cn);
      DataSet ds = new DataSet();
      da.Fill(ds);
      DataTable dt = new DataTable();
      dt = ds.Tables[0];
      dgQuotes.ItemsSource = dt.DefaultView;
      this.DataContext = this;
    }
  }
}
