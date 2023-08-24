using System;
using System.Data;
using System.Windows;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Runtime.CompilerServices;

namespace ProDocEstimate.Views
{
    public partial class CustCont : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

        #region Properties

        public string ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public SqlConnection? conn;
        public SqlDataAdapter? da;
        public SqlCommand? scmd;

        private string? custName2Search4; public string? CustName2Search4 { get { return custName2Search4; } set { custName2Search4 = value; OnPropertyChanged(); } }
        private string? contName2Search4; public string? ContName2Search4 { get { return contName2Search4; } set { contName2Search4 = value; OnPropertyChanged(); } }

        private DataTable? dt;      public DataTable? Dt         { get { return dt;              } set { dt              = value; OnPropertyChanged(); } }
        private string? msg;        public string? Msg           { get { return msg;             } set { msg             = value; OnPropertyChanged(); } }
        private string? msg2;       public string? Msg2          { get { return msg2;            } set { msg2            = value; OnPropertyChanged(); } }

        private string? cust_numb;  public string? CUST_NUMB     { get { return cust_numb;       } set { cust_numb       = value; OnPropertyChanged(); } }
        private string? cust_name;  public string? CUST_NAME     { get { return cust_name;       } set { cust_name       = value; OnPropertyChanged(); } }

        // Save current CUST values:
        private string? sguid;      public string? SGUID         { get { return sguid;           } set { sguid           = value; OnPropertyChanged(); } }
        private string? scust_numb; public string? SCUST_NUMB    { get { return scust_numb;      } set { scust_numb      = value; OnPropertyChanged(); } }
        private string? scust_name; public string? SCUST_NAME    { get { return scust_name;      } set { scust_name      = value; OnPropertyChanged(); } }

        private string? cont_name;  public string? CONTACT_NAME { get { return cont_name;       } set { cont_name       = value; OnPropertyChanged(); } }
        private string? address1;   public string? ADDRESS1     { get { return address1;        } set { address1        = value; OnPropertyChanged(); } }
        private string? address2;   public string? ADDRESS2     { get { return address2;        } set { address2        = value; OnPropertyChanged(); } }
        private string? city;       public string? CITY         { get { return city;            } set { city            = value; OnPropertyChanged(); } }
        private string? state;      public string? STATE        { get { return state;           } set { state           = value; OnPropertyChanged(); } }
        private string? zip;        public string? ZIP          { get { return zip;             } set { zip             = value; OnPropertyChanged(); } }
        private string? phone;      public string? PHONE        { get { return phone;           } set { phone           = value; OnPropertyChanged(); } }
        private string? fax;        public string? FAX          { get { return fax;             } set { fax             = value; OnPropertyChanged(); } }
        private string? e_mail;     public string? E_MAIL       { get { return e_mail;          } set { e_mail          = value; OnPropertyChanged(); } }

        // Save current CONTACT values
        private string? scguid;     public string? SCGUID       { get { return scguid;          } set { scguid          = value; OnPropertyChanged(); } }
        private string? scont_name; public string? SCONTACT_NAME { get { return scont_name;     } set { scont_name      = value; OnPropertyChanged(); } }
        private string? saddress1;  public string? SADDRESS1    { get { return saddress1;       } set { saddress1       = value; OnPropertyChanged(); } }
        private string? saddress2;  public string? SADDRESS2    { get { return saddress2;       } set { saddress2       = value; OnPropertyChanged(); } }
        private string? scity;      public string? SCITY        { get { return scity;           } set { scity           = value; OnPropertyChanged(); } }
        private string? sstate;     public string? SSTATE       { get { return sstate;          } set { sstate          = value; OnPropertyChanged(); } }
        private string? szip;       public string? SZIP         { get { return szip;            } set { szip            = value; OnPropertyChanged(); } }
        private string? sphone;     public string? SPHONE       { get { return sphone;          } set { sphone          = value; OnPropertyChanged(); } }
        private string? sfax;       public string? SFAX         { get { return sfax;            } set { sfax            = value; OnPropertyChanged(); } }
        private string? se_mail;    public string? SE_MAIL      { get { return se_mail;         } set { se_mail         = value; OnPropertyChanged(); } }

        private bool trick;          public bool TRICK          { get { return trick;           } set { trick           = value; OnPropertyChanged(); } }
        private bool notEditingCust; public bool NotEditingCust { get { return notEditingCust;  } set { notEditingCust  = value; OnPropertyChanged(); } }
        private bool editingCust;    public bool EditingCust    { get { return editingCust;     } set { editingCust     = value; OnPropertyChanged(); NotEditingCust = !EditingCust; OnPropertyChanged("AorE"); } }
        private bool addingCust;     public bool AddingCust     { get { return addingCust;      } set { addingCust      = value; OnPropertyChanged(); OnPropertyChanged("AorE"); } }

        private bool notEditingCont; public bool NotEditingCont { get { return notEditingCont;  } set { notEditingCont  = value; OnPropertyChanged(); } }
        private bool editingCont;    public bool EditingCont    { get { return editingCont;     } set { editingCont     = value; OnPropertyChanged(); NotEditingCont = !EditingCont; OnPropertyChanged("AorECont"); } }
        private bool addingCont;     public bool AddingCont     { get { return addingCont;      } set { addingCont      = value; OnPropertyChanged(); OnPropertyChanged("AorECont"); } }

        private bool? one;           public bool? One           { get { return one;             } set { one             = value; OnPropertyChanged(); } }
        private bool? all;           public bool? All           { get { return all;             } set { all             = value; OnPropertyChanged(); } }

        private string? guid;        public string? GUID        { get { return guid;            } set { guid            = value; OnPropertyChanged(); } }
        private string? cguid;       public string? CGUID       { get { return cguid;           } set { cguid           = value; OnPropertyChanged(); } }

        public bool AorE { get { return (AddingCust || EditingCust); } }        // "Adding or Editing"
        public bool AorECont { get { return (AddingCont || EditingCont); } }        // "Adding or Editing Contacts"

        public bool CustLoaded {  get {  return CUST_NAME.ToString().Length > 0; } }

        private DataTable dtStates;  public DataTable DtStates  { get { return dtStates;        } set { dtStates        = value; OnPropertyChanged(); } }

        #endregion

        public CustCont()
        {
            InitializeComponent();

            DataContext = this;
            EditingCust = false;
            AddingCust = false;
            EditingCont = false;
            AddingCont = false;
            CustName2Search4 = "";
            All = true; One = false;
            DataContext = this;
            LoadStates();
        }

        private void LoadStates()
        {
            dtStates = new DataTable("States");
            string cmd = "SELECT Abbreviation, FullName FROM [ESTIMATING].[dbo].[STATES] ORDER BY Abbreviation";
            conn = new SqlConnection(ConnectionString);
            da = new SqlDataAdapter(cmd, conn);
            da.Fill(DtStates);
            cmbStates.ItemsSource = DtStates.DefaultView;
            conn.Close();
        }

        private void btnShowCusts_Click(object sender, RoutedEventArgs e)
        {
            TRICK = true;
            string cmd = "SELECT * FROM [ESTIMATING].[dbo].[CUSTOMER]";
            if (CustName2Search4.ToString().TrimEnd() != "") { cmd += " WHERE CUST_NAME LIKE '%" + CustName2Search4 + "%'"; }
            cmd += " ORDER BY CUST_NAME";
            conn = new SqlConnection(ConnectionString);
            da = new SqlDataAdapter(cmd, conn);
            dt = new DataTable(); da.Fill(dt);
            grdCustomers.ItemsSource = dt.DefaultView;
            Msg = dt.Rows.Count.ToString() + " matches.";

            if (dt.Rows.Count == 1)
            {
                GUID      = dt.Rows[0]["GUID"].ToString();
                CUST_NUMB = dt.Rows[0]["CUST_NUMB"].ToString();
                CUST_NAME = dt.Rows[0]["CUST_NAME"].ToString();
                CustDetlPage.IsSelected = true;

                LoadContacts();

                return;
            }
        }

        private void btnClearCusts_Click(object sender, RoutedEventArgs e)
        {
            grdCustomers.ItemsSource = null;
            Msg = "";
            CustName2Search4 = "";
        }

        private void grdCustomers_SelectedCellsChanged(object sender, System.Windows.Controls.SelectedCellsChangedEventArgs e)
        {
            if (grdCustomers.SelectedIndex == -1) return;       // In case they just clicked the CLEAR button

            DataRowView dataRow = (DataRowView)grdCustomers.SelectedItem;

            GUID = dataRow["GUID"].ToString();
            CUST_NUMB = dataRow["CUST_NUMB"].ToString();
            CUST_NAME = dataRow["CUST_NAME"].ToString();

            Task.Delay(500);                              // Wait half a second...
            CustDetlPage.IsSelected = true;

            LoadContacts();
        }

        private void LoadContacts()
        {   string cmd = $"SELECT * FROM [ESTIMATING].[dbo].[CUSTOMER_CONTACT] WHERE CUST_NUMB = '{CUST_NUMB}'";
            conn = new SqlConnection(ConnectionString);
            da = new SqlDataAdapter(cmd, conn);
            dt = new DataTable(); da.Fill(dt);
            conn.Close();
            grdContactsList.ItemsSource = dt.DefaultView;
            tabContList.IsSelected = true;
        }

        private void btnAddCust_Click(object sender, RoutedEventArgs e)
        {
            SGUID = GUID;
            SCUST_NUMB = CUST_NUMB;
            SCUST_NAME = CUST_NAME;

            conn = new SqlConnection(ConnectionString); conn.Open();
            SqlCommand scmd = new SqlCommand();
            scmd.Connection = conn;
            scmd.CommandType = CommandType.Text;
            scmd.CommandText = "SELECT MAX(CUST_NUMB) FROM [ESTIMATING].[dbo].[CUSTOMER]";
            string NextCustNum = (string)scmd.ExecuteScalar();
            int nextNum = int.Parse(NextCustNum.ToString().TrimEnd());
            nextNum += 1;
            CUST_NUMB = nextNum.ToString() + " ";

            EditingCust = true;
            AddingCust = true;
            CUST_NAME = "";
            tabCustSearch.IsEnabled = false;

            txtCUSTNAME.Focus();
        }

        private void btnEditCust_Click(object sender, RoutedEventArgs e)
        {
            EditingCust = true;
            tabCustSearch.IsEnabled = false;

            SGUID = GUID;
            SCUST_NUMB = CUST_NUMB;
            SCUST_NAME = CUST_NAME;

            txtCUSTNAME.Focus();
        }

        private void btnSaveCust_Click(object sender, RoutedEventArgs e)
        {
            string cmd = "";
            if (AddingCust)
            {
                cmd = "INSERT INTO [ESTIMATING].[dbo].[CUSTOMER] (GUID, CUST_NUMB, CUST_NAME) VALUES ( "
                    + "'" + Guid.NewGuid() + $"', '{CUST_NUMB}', '{CUST_NAME}' )";
            }
            else
            {
                cmd = "UPDATE [ESTIMATING].[dbo].[CUSTOMER] "
                    + $"SET CUST_NAME = '{CUST_NAME}' WHERE GUID = '{GUID}'";
            }

            conn = new SqlConnection(ConnectionString); conn.Open();
            scmd = new SqlCommand(cmd, conn);
            scmd.ExecuteNonQuery(); conn.Close();

            EditingCust = false;
            AddingCust = false;
            SGUID = ""; SCUST_NAME = ""; SCUST_NUMB = "";
            tabCustSearch.IsEnabled = true;
        }

        private void btnCancCust_Click(object sender, RoutedEventArgs e)
        {
            EditingCust = false;
            AddingCust = false;
            
            if (SGUID != "") 
            {   GUID = SGUID; 
                CUST_NAME = SCUST_NAME; 
                CUST_NUMB = SCUST_NUMB; 
                SGUID = ""; 
                SCUST_NUMB = ""; 
                SCUST_NAME = ""; 
            }
            
            tabCustSearch.IsEnabled = true;
        }

        private void btnDeleteCust_Click(object sender, RoutedEventArgs e)
        {
            var confirmResult = MessageBox.Show("Delete customer?", "Confirm delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (confirmResult == MessageBoxResult.Yes)
            {   string cmd = $"DELETE [ESTIMATING].[dbo].[CUSTOMER] WHERE GUID = '{GUID}'";

                SqlConnection conn = new(ConnectionString); conn.Open();
                scmd = new SqlCommand(cmd, conn);
                int Result = scmd.ExecuteNonQuery();
                scmd = null;
                
                if (conn.State == System.Data.ConnectionState.Closed) conn.Open();
                cmd = $"DELETE [ESTIMATING].[dbo].[CUSTOMER_CONTACT] WHERE CUST_NUMB = '{CUST_NUMB}'";
                scmd = new SqlCommand(cmd, conn); Result = scmd.ExecuteNonQuery(); conn.Close();
                
                grdCustomers.ItemsSource = null;
                grdContacts.ItemsSource = null;
                grdContactsList.ItemsSource = null;

                Msg = ""; 
                CustName2Search4 = ""; 
                CUST_NUMB = ""; 
                CUST_NAME = "";
            }
        }

        private void btnAddCont_Click(object sender, RoutedEventArgs e)
        {

            if (CUST_NUMB == null || CUST_NUMB == "")
            {
                MessageBox.Show("Please select or add a customer first.");
                return;
            }

            EditingCont = true;
            AddingCont = true;
            tabContSearch.IsEnabled = false;

            SCGUID = CGUID;
            SCUST_NUMB = CUST_NUMB;  // Do I need to do this?
            SCONTACT_NAME = CONTACT_NAME;
            SADDRESS1 = ADDRESS1;
            SADDRESS2 = ADDRESS2;
            SCITY = CITY;
            SSTATE = STATE;
            SZIP = ZIP;
            SPHONE = PHONE;
            SFAX = FAX;
            SE_MAIL = E_MAIL;

            CONTACT_NAME = "";
            ADDRESS1 = "";
            ADDRESS2 = "";
            CITY = "";
            STATE = "";
            ZIP = "";
            PHONE = "";
            FAX = "";
            E_MAIL = "";

            txtCONTACTNAME.Focus();

        }

        private void btnEditCont_Click(object sender, RoutedEventArgs e)
        {
            txtCUSTNUMB.IsEnabled = false;
            EditingCont = true;
            AddingCont = false;
            
            tabContSearch.IsEnabled = false;

            SCGUID = CGUID;
            SCONTACT_NAME = CONTACT_NAME;
            SADDRESS1 = ADDRESS1;
            SADDRESS2 = ADDRESS2;
            SCITY = CITY;
            SSTATE = STATE;
            SZIP = ZIP;
            SPHONE = PHONE;
            SFAX = FAX;
            SE_MAIL = E_MAIL;

            txtCUSTNUMB.IsEnabled = false;
            txtCONTACT_NAME.Focus();
        }

        private void btnSaveCont_Click(object sender, RoutedEventArgs e)
        {
            string cmd = "";
            if (AddingCont)
            { cmd = "INSERT INTO [ESTIMATING].[dbo].[CUSTOMER_CONTACT] ( "
                    + "CGUID, CUST_NUMB, CONTACT_NAME, ADDRESS1, "
                    + "ADDRESS2, CITY, STATE, ZIP, PHONE, FAX, E_MAIL) "
                    + "VALUES ( "
                    + "'" + Guid.NewGuid().ToString() + "',"
                    + $"'{CUST_NUMB}',"
                    + $"'{CONTACT_NAME}',"
                    + $"'{ADDRESS1}',"
                    + $"'{ADDRESS2}',"
                    + $"'{CITY}',"
                    + $"'{STATE}',"
                    + $"'{ZIP}',"
                    + $"'{PHONE}',"
                    + $"'{FAX}',"
                    + $"'{E_MAIL}')";
            }
            else
            {
                cmd = "UPDATE [ESTIMATING].[dbo].[CUSTOMER_CONTACT] SET " 
                    + $" CUST_NUMB = '{CUST_NUMB}',"
                    + $" CONTACT_NAME = '{CONTACT_NAME}',"
                    + $" ADDRESS1 = '{ADDRESS1}',"
                    + $" ADDRESS2 = '{ADDRESS2}',"
                    + $" CITY = '{CITY}',"
                    + $" STATE = '{STATE}',"
                    + $" ZIP = '{ZIP}',"
                    + $" PHONE = '{PHONE}',"
                    + $" FAX = '{FAX}',"
                    + $" E_MAIL = '{E_MAIL}' "
                    + $" WHERE CGUID = '{CGUID}'";
            }

            conn = new SqlConnection(ConnectionString); conn.Open();
            scmd = new SqlCommand(cmd, conn);
            scmd.ExecuteNonQuery(); conn.Close();

            EditingCont = false;
            AddingCont = false;

            tabContSearch.IsEnabled = true;
            LoadContacts();
            ContDetlPage.IsEnabled = true;
        }

        private void btnCancCont_Click(object sender, RoutedEventArgs e)
        {
            EditingCont = false;
            AddingCont = false;

            CGUID = SCGUID;
            CONTACT_NAME = SCONTACT_NAME;
            ADDRESS1 = SADDRESS1;
            ADDRESS2 = SADDRESS2;
            CITY = SCITY;
            STATE = SSTATE;
            ZIP = SZIP;
            PHONE = SPHONE;
            FAX = SFAX;
            E_MAIL = SE_MAIL;

            tabContSearch.IsEnabled = true;
        }

        private void btnDeleteCont_Click(object sender, RoutedEventArgs e)
        {
            var confirmResult = MessageBox.Show("Delete contact?", "Confirm delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (confirmResult == MessageBoxResult.Yes)
            {   string cmd = $"DELETE [ESTIMATING].[dbo].[CUSTOMER_CONTACT] WHERE CGUID = '{CGUID}'";

                SqlConnection conn = new(ConnectionString); conn.Open();
                scmd = new SqlCommand(cmd, conn);
                int Result = scmd.ExecuteNonQuery();
                conn.Close();
                grdContacts.ItemsSource = null;
                Msg2 = "";
                ContName2Search4 = "";

                //TODO: Blank all of the string properties for the Contacts page...

                LoadContacts();

            }
        }

        private void grdContacts_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (TRICK == true) { TRICK = false; return; }       // So that it doesn't automatically return the first row
            if (grdContacts.SelectedIndex == -1) return;       // In case they just clicked the CLEAR button

            DataRowView dataRow = (DataRowView)grdContacts.SelectedItem;

            CGUID = dataRow["CGUID"].ToString();
            CUST_NUMB = dataRow["CUST_NUMB"].ToString();
            CONTACT_NAME = dataRow["CONTACT_NAME"].ToString();
            ADDRESS1 = dataRow["ADDRESS1"].ToString();
            ADDRESS2 = dataRow["ADDRESS2"].ToString();
            CITY = dataRow["CITY"].ToString();
            STATE = dataRow["STATE"].ToString();
            ZIP = dataRow["ZIP"].ToString();
            PHONE = dataRow["PHONE"].ToString();
            FAX = dataRow["FAX"].ToString();
            E_MAIL = dataRow["E_MAIL"].ToString();

            Task.Delay(500);                              // Wait half a second...
            ContDetlPage.IsSelected = true;

            if(CUST_NAME != null && CUST_NAME.ToString().Length==0) { GetCustName(); }
            
        }

        private void GetCustName()
        {
            if (CUST_NUMB == null || CUST_NUMB.ToString().Length == 0) return;
            string cmd = "SELECT CUST_NAME FROM [ESTIMATING].[dbo].[CUSTOMER] WHERE CUST_NUMB = '" + CUST_NUMB + "'";
            conn = new SqlConnection(ConnectionString);
            da = new SqlDataAdapter(cmd, conn);
            dt = new DataTable(); da.Fill(dt);
            CUST_NAME = dt.Rows[0][0].ToString();
            conn.Close();
        }

        private void Label_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (CUST_NAME == null || CUST_NAME=="")
            { SearchOne.Visibility = Visibility.Hidden; }
            else
            { SearchOne.Visibility = Visibility.Visible; }
        }

        private void btnShowContacts_Click(object sender, RoutedEventArgs e)
        {
            TRICK = true;
            string cmd2 = string.Empty;
            string cmd3 = string.Empty; 

            string cmd = "SELECT CGUID, CUST_NUMB, CONTACT_NAME, ADDRESS1, ADDRESS2, CITY, STATE, ZIP,"
                       + " PHONE, FAX, E_MAIL FROM [ESTIMATING].[dbo].[CUSTOMER_CONTACT] ";

            if(ContName2Search4 != null)                { cmd2 = " CONTACT_NAME LIKE '%" + ContName2Search4 + "%'"; }
            if(CUST_NUMB != null && One == true )       { cmd3 = " CUST_NUMB = '" + CUST_NUMB + "'"; }

            // note that " RTRIM(CUST_NUMB) = ' " + CUST_NUMB.ToString().TrimEnd() + "' " would be better, because CUST_NUMB is char(6)...

            if (cmd2 != "" && cmd3 == "") { cmd += " WHERE " + cmd2; }
            if (cmd2 == "" && cmd3 != "") { cmd += " WHERE " + cmd3; }
            if (cmd2 != "" && cmd3 != "") { cmd += " WHERE " + cmd2 + " AND " + cmd3; }

            cmd += " ORDER BY CONTACT_NAME";

            conn = new SqlConnection(ConnectionString);
            da = new SqlDataAdapter(cmd, conn);
            dt = new DataTable(); da.Fill(dt);
            grdContacts.ItemsSource = dt.DefaultView;
            Msg2 = dt.Rows.Count.ToString() + " matches.";

            if (dt.Rows.Count == 1) // Just return if there was only one matching contact
            {
                CGUID       = dt.Rows[0]["CGUID"].ToString();
                CUST_NUMB   = dt.Rows[0]["CUST_NUMB"].ToString();
                CONTACT_NAME= dt.Rows[0]["CONTACT_NAME"].ToString();
                ADDRESS1    = dt.Rows[0]["ADDRESS1"].ToString();
                ADDRESS2    = dt.Rows[0]["ADDRESS2"].ToString();
                CITY        = dt.Rows[0]["CITY"].ToString();
                STATE       = dt.Rows[0]["STATE"].ToString();
                ZIP         = dt.Rows[0]["ZIP"].ToString();
                PHONE       = dt.Rows[0]["PHONE"].ToString();
                FAX         = dt.Rows[0]["FAX"].ToString();
                E_MAIL      = dt.Rows[0]["E_MAIL"].ToString();

                CustDetlPage.IsSelected = true;
                GetCustName();

                return;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void grdContactsList_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (grdContactsList.SelectedIndex == -1) return;       // In case they just clicked the CLEAR button

            DataRowView dataRow = (DataRowView)grdContactsList.SelectedItem;

            CGUID = dataRow["CGUID"].ToString();
            CONTACT_NAME = dataRow["CONTACT_NAME"].ToString();
            ADDRESS1 = dataRow["ADDRESS1"].ToString();
            ADDRESS2 = dataRow["ADDRESS2"].ToString();
            CITY = dataRow["CITY"].ToString();
            STATE = dataRow["STATE"].ToString();
            ZIP = dataRow["ZIP"].ToString();
            PHONE = dataRow["PHONE"].ToString();
            FAX = dataRow["FAX"].ToString();
            E_MAIL = dataRow["E_MAIL"].ToString();

            Task.Delay(500);                              // Wait half a second...
            ContDetlPage.IsSelected = true;

        }

        private void grdContactsList_PreviewMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e) { ActivatePage1(); }
        private void grdContacts_PreviewMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)     { ActivatePage1(); }

        private void ActivatePage1() 
        { 
            ContDetlPage.IsSelected = true;
            GetCustName();
        }

        private void grdCustomers_PreviewMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ActivatePage1();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e) { ((TextBox)sender).SelectAll(); }

    }
}