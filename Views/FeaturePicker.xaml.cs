using System.Data;
using System.Windows;
using System.Windows.Input;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Controls;

namespace ProDocEstimate.Views
{
	public partial class FeaturePicker : UserControl
	{
		public string ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
		public SqlConnection cn = new SqlConnection();
		public SqlDataAdapter? da;

		public FeaturePicker()
		{ InitializeComponent();
			this.PreviewKeyDown += (ss, ee) => { if (ee.Key == Key.Escape) { Visibility = Visibility.Hidden; } };
		}

		private void btnGo_Click(object sender, RoutedEventArgs e)
		{
			cn = new SqlConnection(ConnectionString);
			cn.Open();
			string str = txtPartDesc.Text.Trim().Length > 0 ? txtPartDesc.Text : "''";
			string cmd = "SELECT FEATURE, FEATURE_NUM, FEATURE_DESCRIPTION FROM FEATURE_STANDARDS WHERE FEATURE_DESCRIPTION LIKE '%" + str + "%' ORDER BY 1, 2, 3" ;
			da = new SqlDataAdapter(cmd, cn);
			DataSet ds = new DataSet();
			da.Fill(ds);
			dgFeatures.ItemsSource = ds.Tables[0].DefaultView; dgFeatures.SelectedIndex = 0;
		}
	}
}