using System;
using System.Data;
using System.Windows;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace ProDocEstimate.Views
{
    public partial class NewContomer : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

        public string ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public SqlConnection? conn;
        public SqlDataAdapter? da;
        public SqlCommand scmd;

        private string custName2Search4; public string CustName2Search4     { get { return custName2Search4;    } set { custName2Search4 = value; OnPropertyChanged(); } }
        private string msg;              public string Msg                  { get { return msg;                 } set { msg              = value; OnPropertyChanged(); } }
        private DataTable dt;            public DataTable Dt                { get { return dt;                  } set { dt               = value; OnPropertyChanged(); } }
        private string cust_numb;        public string CUST_NUMB            { get { return cust_numb;           } set { cust_numb        = value; OnPropertyChanged(); } }
        private string cust_name;        public string CUST_NAME            { get { return cust_name;           } set { cust_name        = value; OnPropertyChanged(); } }
        private string custid;           public string CUST_ID              { get { return custid;              } set { custid           = value; OnPropertyChanged(); } }
        private bool   trick;            public bool   TRICK                { get { return trick;               } set { trick            = value; OnPropertyChanged(); } }
        private bool   notEditingCust;   public bool   NotEditingCust       { get { return notEditingCust;      } set { notEditingCust   = value; OnPropertyChanged(); } }
        private bool   editingCust;      public bool   EditingCust          { get { return editingCust;         } set { editingCust      = value; OnPropertyChanged(); NotEditingCust = !EditingCust; } }
        private bool   addingCust;       public bool   AddingCust           { get { return addingCust;          } set { addingCust       = value; OnPropertyChanged(); } }
        private bool   notEditingCont;   public bool   NotEditingCont       { get { return notEditingCont;      } set { notEditingCont   = value; OnPropertyChanged(); } }
        private bool   editingCont;      public bool   EditingCont          { get { return editingCont;         } set { editingCont      = value; OnPropertyChanged(); NotEditingCont = !EditingCont; } }
        private bool   addingCont;       public bool   AddingCont           { get { return addingCont;          } set { addingCont       = value; OnPropertyChanged(); } }

        public NewCustomer()
        {
            InitializeComponent();
            DataContext = this;
            EditingCust = false;
            AddingCust = false;
            CustName2Search4 = "";
        }

        private void btnShowCusts_Click(object sender, RoutedEventArgs e)
        {   TRICK = true;
            string cmd = "SELECT * FROM [ESTIMATING].[dbo].[CUSTOMER]"; 
            conn = new SqlConnection(ConnectionString);
            if (CustName2Search4.ToString().TrimEnd() != "") { cmd += " WHERE CUST_NAME LIKE '%" + CustName2Search4 + "%'"; }
            cmd += " ORDER BY CUST_NAME";
            da = new SqlDataAdapter(cmd, conn); 
            dt = new DataTable(); da.Fill(dt);
            grdCustomers.ItemsSource = dt.DefaultView;
            Msg = dt.Rows.Count.ToString() + " matches.";

            if (dt.Rows.Count == 1)
            {
                CUST_ID = dt.Rows[0][0].ToString();
                CUST_NUMB = dt.Rows[0][1].ToString();
                CUST_NAME = dt.Rows[0][2].ToString();
                CustDetlPage.IsSelected = true;
                return;
            }
        }

        private void btnClearCusts_Click(object sender, RoutedEventArgs e)
        {
            grdCustomers.ItemsSource = null;
            Msg = "";
            CustName2Search4 = "";
        }

        private async void grdCustomers_SelectedCellsChanged(object sender, System.Windows.Controls.SelectedCellsChangedEventArgs e)
        {
            // If only one row matched, just return it.
            //if (grdCustomers.SelectedItems.Count == 1)
            //{
            //    CUST_ID = dt.Rows[0][0].ToString();
            //    CUST_NUMB = dt.Rows[0][1].ToString();
            //    CUST_NAME = dt.Rows[0][2].ToString();
            //    CustDetlPage.IsSelected = true;
            //    return;
            //}

            if (TRICK == true) { TRICK = false; return; }       // So that it doesn't automatically return the first row
            if (grdCustomers.SelectedIndex == -1) return;       // In case they just clicked the CLEAR button

            DataRowView dataRow = (DataRowView)grdCustomers.SelectedItem;

            CUST_ID   = dataRow[0].ToString();
            CUST_NUMB = dataRow[1].ToString();
            CUST_NAME = dataRow[2].ToString();

            await Task.Delay(500);                              // Wait half a second...
            CustDetlPage.IsSelected = true;
        }

        private void btnAddCust_Click(object sender, RoutedEventArgs e)
        {
            EditingCust = true;
            AddingCust = true;
        }

        private void btnEditCust_Click(object sender, RoutedEventArgs e)
        {
            EditingCust = true;
        }

        private void btnSaveCust_Click(object sender, RoutedEventArgs e)
        {
            string cmd = "";
            if (AddingCust)
            {
              cmd = "INSERT INTO [ESTIMATING].[dbo].[CUSTOMER] (GUID, CUST_NUMB, CUST_NAME) VALUES ( "
                  + "'" + Guid.NewGuid() + "', '" + CUST_NUMB + "', '" + CUST_NAME + "' )";
            }
            else
            {
              cmd = "UPDATE [ESTIMATING].[dbo].[CUSTOMER] "
                  + "SET CUST_NAME = '" + CUST_NAME + "' WHERE GUID = '" + CUST_ID + "'";
            }

            conn = new SqlConnection(ConnectionString); conn.Open();
            scmd = new SqlCommand(cmd, conn);
            scmd.ExecuteNonQuery(); conn.Close();

            EditingCust = false;
            AddingCust = false;
        }

        private void btnCancCust_Click(object sender, RoutedEventArgs e)
        {
            EditingCust = false;
            AddingCust = false;
        }

        private void btnDeleteCust_Click(object sender, RoutedEventArgs e)
        {
            var confirmResult =
                MessageBox.Show("Delete customer?", "Confirm delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirmResult == MessageBoxResult.Yes) 
            {
                string cmd = "DELETE [ESTIMATING].[dbo].[CUSTOMER] WHERE GUID = " + "'" + CUST_ID + "'";
                Clipboard.SetText(cmd);
                SqlConnection conn = new(ConnectionString); conn.Open();
                scmd = new SqlCommand(cmd, conn);
                int Result = scmd.ExecuteNonQuery();
                conn.Close();
                grdCustomers.ItemsSource = null;
                Msg = "";
                CustName2Search4 = "";
            }
            else 
            {
                MessageBox.Show("Delete failed.");
            }
        }
    }
}