using ProDocEstimate.Views;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ProDocEstimate
{
    public partial class Quotations : Window, INotifyPropertyChanged
    {
//        HistoricalPrices hp = new();

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

        public string ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public SqlConnection? conn;
        public SqlDataAdapter? da;
        public DataTable? dt;
        public SqlCommand? scmd;

        #region Property Declarations

        private bool isCalc; public bool IsCalc { get { return isCalc; } set { isCalc = value; OnPropertyChanged(); } }

        private bool adding; public bool Adding { get { return adding; } set { adding = value; OnPropertyChanged(); } }

        private string? quote_num = "";   public string? QUOTE_NUM   { get { return quote_num;   } set { quote_num   = value; OnPropertyChanged(); } }
        private string? cust_num;         public string? CUST_NUMB   { get { return cust_num;    } set { cust_num    = value; OnPropertyChanged(); } }
        private string? projectType = ""; public string? ProjectType { get { return projectType; } set { projectType = value; OnPropertyChanged(); } }

        private string? fsint1;  public string? FSINT1  { get { return fsint1;  } set { fsint1  = value; OnPropertyChanged(); } }
        private string? fsfrac1; public string? FSFRAC1 { get { return fsfrac1; } set { fsfrac1 = value; OnPropertyChanged(); } }
        private string? fsint2;  public string? FSINT2  { get { return fsint2;  } set { fsint2  = value; OnPropertyChanged(); } }
        private string? fsfrac2; public string? FSFRAC2 { get { return fsfrac2; } set { fsfrac2 = value; OnPropertyChanged(); } }
        private int     parts;   public int     PARTS   { get { return parts;   } set { parts   = value; OnPropertyChanged(); } }

        private string? papertype; public string? PAPERTYPE { get { return papertype; } set { papertype = value; OnPropertyChanged(); LoadRollWidths(); LoadItemTypes(PAPERTYPE); } }
        private string? rollwidth; public string? ROLLWIDTH { get { return rollwidth; } set { rollwidth = value; OnPropertyChanged(); } }
        private string? presssize; public string? PRESSSIZE { get { return presssize; } set { presssize = value; OnPropertyChanged(); LoadCollator(); } }
        private bool?   lineholes; public bool?   LINEHOLES  { get { return lineholes; } set { lineholes = value; OnPropertyChanged(); } }
        private string? collatorcut; public string? COLLATORCUT { get { return collatorcut; } set { collatorcut = value; OnPropertyChanged(); } }

        private string? customerName; public string? CustomerName { get { return customerName; } set { customerName = value; OnPropertyChanged(); } }
        private string? contactName;  public string? ContactName { get { return contactName; } set { contactName = value; OnPropertyChanged(); } }
        private string? address;      public string? Address { get { return address; } set { address = value; OnPropertyChanged(); } }
        private string? city;         public string? City { get { return city; } set { city = value; OnPropertyChanged(); } }
        private string? state;        public string? State { get { return state; } set { state = value; OnPropertyChanged(); } }
        private string? zip;          public string? ZIP { get { return zip; } set { zip = value; OnPropertyChanged(); } }
        private string? location;     public string? Location { get { return location; } set { location = value; OnPropertyChanged(); } }
        private string? phone;        public string? Phone { get { return phone; } set { phone = value; OnPropertyChanged(); } }
        private string? csz;          public string? CSZ { get { return csz; } set { csz = value; OnPropertyChanged(); } }
        private string? email;        public string? Email { get { return email; } set { email = value; OnPropertyChanged(); } }

        private string? activePage; public string? ActivePage { get { return activePage; } set { activePage = value; OnPropertyChanged(); } }

        private float? qty1a; public float? QTY1a { get { return qty1a; } set { qty1a = value; OnPropertyChanged(); } }
        private float? qty2a; public float? QTY2a { get { return qty2a; } set { qty2a = value; OnPropertyChanged(); } }
        private float? qty3a; public float? QTY3a { get { return qty3a; } set { qty3a = value; OnPropertyChanged(); } }
        private float? qty4a; public float? QTY4a { get { return qty4a; } set { qty4a = value; OnPropertyChanged(); } }

        private float? cpm1a; public float? CPM1a { get { return cpm1a; } set { cpm1a = value; OnPropertyChanged(); } }
        private float? cpm2a; public float? CPM2a { get { return cpm2a; } set { cpm2a = value; OnPropertyChanged(); } }
        private float? cpm3a; public float? CPM3a { get { return cpm3a; } set { cpm3a = value; OnPropertyChanged(); } }
        private float? cpm4a; public float? CPM4a { get { return cpm4a; } set { cpm4a = value; OnPropertyChanged(); } }

        private float? mar1a; public float? MAR1a { get { return mar1a; } set { mar1a = value; OnPropertyChanged(); } }
        private float? mar2a; public float? MAR2a { get { return mar2a; } set { mar2a = value; OnPropertyChanged(); } }
        private float? mar3a; public float? MAR3a { get { return mar3a; } set { mar3a = value; OnPropertyChanged(); } }
        private float? mar4a; public float? MAR4a { get { return mar4a; } set { mar4a = value; OnPropertyChanged(); } }

        private float? ext1a; public float? EXT1a { get { return ext1a; } set { ext1a = value; OnPropertyChanged(); } }
        private float? ext2a; public float? EXT2a { get { return ext2a; } set { ext2a = value; OnPropertyChanged(); } }
        private float? ext3a; public float? EXT3a { get { return ext3a; } set { ext3a = value; OnPropertyChanged(); } }
        private float? ext4a; public float? EXT4a { get { return ext4a; } set { ext4a = value; OnPropertyChanged(); } }

        private float? pSize; public float? PSize { get { return pSize; } set { pSize = value; OnPropertyChanged(); } }
        private float? cSize; public float? CSize { get { return cSize; } set { cSize = value; OnPropertyChanged(); } }

        private int? qty1 = 10000;     public int? Qty1    { get { return qty1;  } set { qty1  = value; OnPropertyChanged(); } }
        private int? qty2 = 25000;     public int? Qty2    { get { return qty2;  } set { qty2  = value; OnPropertyChanged(); } }
        private int? qty3 = 50000;     public int? Qty3    { get { return qty3;  } set { qty3  = value; OnPropertyChanged(); } }
        private int? qty4 = 100000;    public int? Qty4    { get { return qty4;  } set { qty4  = value; OnPropertyChanged(); } }

        private int?     wastePct = 5; public int? WastePct         { get { return wastePct;   } set {  wastePct  = value; OnPropertyChanged(); } }
        private string?     itemType;  public string? ItemType      { get { return itemType;   } set { itemType   = value; OnPropertyChanged(); LoadPage2Combos(); } }
        private int?           numUpp; public int? NumUpp           { get { return numUpp;     } set {  numUpp    = value; OnPropertyChanged(); } }
        private int?      selectedQty; public int? SelectedQty      { get { return selectedQty;} set { selectedQty= value; OnPropertyChanged(); } }
        private string?       calcMsg; public string? CalcMsg       { get { return calcMsg;    } set { calcMsg    = value; OnPropertyChanged(); } }

        private DataTable?   features; public DataTable? Features   { get { return features;   } set { features   = value; OnPropertyChanged(); } }
        private DataTable?   elements; public DataTable? Elements   { get { return elements;   } set { elements   = value; OnPropertyChanged(); } }
        private DataTable?  formTypes; public DataTable? FormTypes  { get { return formTypes;  } set { formTypes  = value; OnPropertyChanged(); } }
        private DataTable? papertypes; public DataTable? PaperTypes { get { return papertypes; } set { papertypes = value; OnPropertyChanged(); } }
        private DataTable? rollwidths; public DataTable? RollWidths { get { return rollwidths; } set { rollwidths = value; OnPropertyChanged(); } }
        private DataTable?      sizes; public DataTable? Sizes      { get { return sizes;      } set { sizes      = value; OnPropertyChanged(); } }
        private DataTable?     colors; public DataTable? Colors     { get { return colors;     } set { colors     = value; OnPropertyChanged(); } }
        private DataTable?    basisWT; public DataTable? BasisWT    { get { return basisWT;    } set { basisWT    = value; OnPropertyChanged(); } }
        private DataTable?  itemTypes; public DataTable? ItemTypes  { get { return itemTypes;  } set { itemTypes  = value; OnPropertyChanged(); } }

        private float mult;            public float      MULT       { get { return mult;       } set { mult       = value; OnPropertyChanged(); } }
        private int? selRowIdx;        public int?       SelRowIdx  { get { return selRowIdx;  } set { selRowIdx  = value; OnPropertyChanged(); } }
        private DataRowView? drv;      public DataRowView? DRV      { get { return drv;        } set { drv        = value; OnPropertyChanged(); } }
        private double quoteTotal;     public double QuoteTotal     { get { return quoteTotal; } set { quoteTotal = value; OnPropertyChanged(); } }

        private int maxColors;         public int MaxColors         { get { return maxColors;  } set { maxColors  = value; OnPropertyChanged(); } }
        private string? category;      public string? Category      { get { return category;   } set { category   = value; OnPropertyChanged(); } }

        #endregion

        public Quotations()
        {
            InitializeComponent();

            DataContext = this;

            LoadPressSizes();
            LoadFormTypes();
            LoadPaperTypes();
            LoadAvailableCategories();

            // set some defaults for testing
            MaxColors = 5; // Get from the [Estimating].[dbo].[PressSizeColors] table

            QUOTE_NUM = "10001";
            ProjectType = "CONTINUOUS";
            PARTS = 3;
            PAPERTYPE = "BOND";
            ROLLWIDTH = "10";
            PRESSSIZE = "11";
            COLLATORCUT = "11";

            LoadDetails();  // For page 3

            PreviewKeyDown += (s, e) => { if (e.Key == Key.Escape) Close(); };
        }

        public void OnLoad(object sender, RoutedEventArgs e)
        { 
            this.Height = this.Height *= 1.6;
            this.Width = this.Width *= 1.6;
            Top = 50;
        }

        private void LoadAvailableCategories()
        {
            lstAvailable.Items.Clear();
            lstAvailable.Items.Add("Base Charges");
            lstAvailable.Items.Add("Backer");
            lstAvailable.Items.Add("Ink Color");
            lstAvailable.Items.Add("MICR");
            lstAvailable.Items.Add("Perfing");
            lstAvailable.Items.Add("Punching");
            lstAvailable.Items.Add("Press Numbering");
            lstAvailable.Items.Add("Finishing");
            lstAvailable.Items.Add("Converting");
            lstAvailable.Items.Add("PrePress");
            lstAvailable.Items.Add("Combo");
            lstAvailable.Items.Add("Strike In");
            lstAvailable.Items.Add("Security");
        }

        private void btnLookup_Click(object sender, RoutedEventArgs e)
        {
            CustomerLookup cl = new(); cl.ShowDialog();
            CUST_NUMB = cl.CustomerCode;

            cl.Close();

            string str = "SELECT C.CUST_NAME, CC.CONTACT_NAME, C.CUST_NUMB, CC.ADDRESS1, CC.CITY, CC.STATE, CC.ZIP, CC.PHONE, CC.E_MAIL "
            + " FROM [ESTIMATING].[dbo].[CUSTOMER] C, [ESTIMATING].[dbo].[CUSTOMER_CONTACT] CC "
            + $" WHERE C.CUST_NUMB = CC.CUST_NUMB AND C.CUST_NUMB = '{CUST_NUMB}'" ;

            conn = new SqlConnection(ConnectionString);
            SqlDataAdapter da = new(str, conn);
            DataTable dt = new(); da.Fill(dt);
            CustomerName = dt.Rows[0]["CUST_NAME"].ToString();
            ContactName = dt.Rows[0]["CONTACT_NAME"].ToString();
            Address = dt.Rows[0]["ADDRESS1"].ToString();
            City = dt.Rows[0]["CITY"].ToString().TrimEnd();
            State = dt.Rows[0]["STATE"].ToString().TrimEnd();
            ZIP = dt.Rows[0]["ZIP"].ToString().TrimEnd();
            CSZ = City.TrimEnd() + ", " + State + ", " + ZIP;
            Phone = dt.Rows[0]["Phone"].ToString();
            Email = dt.Rows[0]["E_MAIL"].ToString();
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show(" Do this in the Customer Lookup screen");
        }

        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {
            QuoteLookup ql = new();
            ql.ShowDialog();
            QUOTE_NUM = ql.SelQuote; ql.Close();
            if (QUOTE_NUM == null || QUOTE_NUM.Length == 0) return;

            GetQuote();

//          btnAssignNewQuoteNum.Visibility = Visibility.Visible;

            // Always add a Backer row in Quote_Detail, Since Ink Color includes a Backer as one of the ink colors. Should it also default to "Std"?
            // Always add an Ink Color row (blank) on new quotes
            // Always add a PrePress row in Quote_Details with values of 1 for both 

            //            LoadAvailableCategories();
            //            AddDefaultDetailLines();

            //NewQuote();

            //txtQuoteNum.Focus();
            //txtQuoteNum.Background = Brushes.Red;
            //txtQuoteNum.Foreground = Brushes.Yellow;
        }

        private void AddDefaultDetailLines()
        {
            // Don't add them twice
            string cmd = $"SELECT COUNT(*) AS HowMany FROM [ESTIMATING].[dbo].[QUOTE_DETAILS] WHERE QUOTE_NUM = {QUOTE_NUM}";
            conn = new SqlConnection(ConnectionString); conn.Open();
            da = new SqlDataAdapter(cmd, conn);
            dt = new DataTable(); da.Fill(dt);
            if (int.Parse(dt.Rows[0]["HowMany"].ToString()) > 0) return;

            lstSelected.Items.Clear();

            cmd = "INSERT INTO [ESTIMATING].[dbo].[QUOTE_DETAILS] ( QUOTE_NUM, SEQUENCE, CATEGORY )"
                       + $" VALUES ( '{QUOTE_NUM}', '1', 'Backer' )";
            conn = new SqlConnection(ConnectionString); conn.Open();
            scmd = new SqlCommand(cmd, conn);
            scmd.ExecuteNonQuery(); conn.Close();

            cmd = "INSERT INTO [ESTIMATING].[dbo].[QUOTE_DETAILS] ( QUOTE_NUM, SEQUENCE, CATEGORY )"
                       + $" VALUES ( '{QUOTE_NUM}', '2', 'Ink Color' )";
            conn = new SqlConnection(ConnectionString); conn.Open();
            scmd = new SqlCommand(cmd, conn);
            scmd.ExecuteNonQuery(); conn.Close();

            cmd = "INSERT INTO [ESTIMATING].[dbo].[QUOTE_DETAILS] ( QUOTE_NUM, SEQUENCE, CATEGORY, Param1, Param2, Param3, Value1, Value2, Value3 )"
                        + $" VALUES ( '{QUOTE_NUM}', '7', 'PrePress', 'OrderEntry', 'PlateChg', 'PREPress', '1', '0', '1' )";
            conn.Open();
            scmd.CommandText = cmd;
            scmd.ExecuteNonQuery(); conn.Close();

            LoadDetails();

            lstAvailable.Items.Remove("Backer");
            lstAvailable.Items.Remove("Ink Color");
            lstAvailable.Items.Remove("PrePress");

            //TODO: Should I also always insert a Base Charges row?
        }

        private void GetQuote()
        {
            SqlConnection cn = new(ConnectionString);
            if (QUOTE_NUM == null || QUOTE_NUM == "") QUOTE_NUM = txtQuoteNum.Text.ToString();
            string str = "SELECT * FROM [ESTIMATING].[dbo].[QUOTES] WHERE QUOTE_NUM = " + QUOTE_NUM;
            SqlDataAdapter da = new(str, cn);
            DataSet ds = new();
            da.Fill(ds);

            if (ds.Tables[0].Rows.Count == 0)
            { MessageBox.Show("Not found..."); return; }
            else
            {
                DataTable dt = ds.Tables[0]; DataRow dr = dt.Rows[0];
                CUST_NUMB = dt.Rows[0]["CUST_NUM"].ToString();

                ProjectType = dt.Rows[0]["PROJECTTYPE"].ToString();
                // These columns need to be added:
                //FSINT1 = dt.Rows[0]["FSINT1"].ToString();  // dr3[6];
                //FSFRAC1 = dt.Rows[0]["FSFRAC1"].ToString();  // dr3[7];
                //FSINT2 = dt.Rows[0]["FSINT2"].ToString();  // dr3[8];
                //FSFRAC2 = dt.Rows[0]["FSFRAC2"].ToString();  // dr3[9];
                PARTS = int.Parse(dt.Rows[0]["PARTS"].ToString());
                PAPERTYPE = dt.Rows[0]["PAPERTYPE"].ToString();  // dr3[9];
                ROLLWIDTH = dt.Rows[0]["ROLLWIDTH"].ToString();  // dr3[9];
                PRESSSIZE = dt.Rows[0]["PRESSSIZE"].ToString();  // dr3[9];     "PressSize.Cylinder"
                LINEHOLES = bool.Parse(dt.Rows[0]["LINEHOLES"].ToString());
                COLLATORCUT = dt.Rows[0]["COLLATORCUT"].ToString();  // dr3[9]; "PressSize.FormSize"	

                txtCustomerNum_LostFocus(this, null);

                //TODO: Add detail lines, changing QUOTE_DETAILS.QUOTE_NUM
            }
        }

        private void Tabs_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            switch (Tabs.SelectedIndex)
            {
                case 0: ActivePage = "Base"; lblPageName.Content = "Base"; OnPropertyChanged("ActivePage"); break;
                case 1: ActivePage = "Details"; lblPageName.Content = "Details"; OnPropertyChanged("ActivePage"); break;
                case 2: ActivePage = "Pricing"; lblPageName.Content = "Pricing"; OnPropertyChanged("ActivePage"); break;
            }
        }

        private void txtCustomerNum_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            System.Windows.MessageBox.Show("For new customers, a provisional Customer Number starting with 'P' will be created, to be replaced later", "Provisional CustNum", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private static float CalcFraction(string frac)
        {
            string[] parts = frac.Split('/');
            if (parts.Length == 1) { return 0.00F; }
            else { return float.Parse(parts[0]) / float.Parse(parts[1]); }
        }

        private void LoadPaperTypes()
        {
            this.PaperTypes = new DataTable("PaperTypes");
            this.PaperTypes.Columns.Add("PaperType");

            string str = "SELECT DISTINCT RTRIM(ReportType) AS PaperType FROM MasterInventory ORDER BY RTRIM(ReportType)";

            SqlConnection cn = new(ConnectionString);
            cn.Open(); if (cn.State != ConnectionState.Open) { MessageBox.Show("Couldn't open SQL Connection"); return; }
            SqlDataAdapter da = new(str, cn);
            da.Fill(PaperTypes);

            cmbPaperType.ItemsSource = PaperTypes.DefaultView;
            cmbPaperType.DisplayMemberPath = "PaperType";
            cmbPaperType.SelectedValuePath = "PaperType";

            this.cmbPaperType.DataContext = this;

            this.cmbPaperType.ItemsSource = PaperTypes.DefaultView;
            this.cmbPaperType.DisplayMemberPath = "PaperType";
            this.cmbPaperType.SelectedValuePath = "PaperType";

            this.txtPaperType.ItemsSource = PaperTypes.DefaultView;
            this.txtPaperType.DisplayMemberPath = "PaperType";
            this.txtPaperType.SelectedValuePath = "PaperType";
        }

        private void LoadColors()
        {
            this.Colors = new DataTable("Colors");
            this.Colors.Columns.Add("Color");

            string str = "SELECT DISTINCT Color FROM [ProVisionDev].[dbo].MasterInventory WHERE ReportType = '" + PAPERTYPE + "' AND ItemType = '" + ItemType + "' ORDER BY Color";
            SqlConnection cn = new(ConnectionString);
            cn.Open(); if (cn.State != ConnectionState.Open) { MessageBox.Show("Couldn't open SQL Connection"); return; }
            SqlDataAdapter da = new(str, cn);
            da.Fill(Colors);

            txtColor.ItemsSource = Colors.DefaultView;
            txtColor.DisplayMemberPath = "Color";
            txtColor.SelectedValuePath = "Color";
        }

        private void LoadItemTypes(string RptType)
        {
            this.ItemTypes = new DataTable("ItemTypes");
            this.ItemTypes.Columns.Add("ItemType");

            string str = "SELECT DISTINCT ItemType FROM [ProVisionDev].[dbo].[MasterInventory] WHERE ReportType = '" + RptType + "'";
            SqlConnection cn = new(ConnectionString);
            cn.Open(); if (cn.State != ConnectionState.Open) { MessageBox.Show("Couldn't open SQL Connection"); return; }
            SqlDataAdapter da = new(str, cn);
            da.Fill(ItemTypes);

            txtItemType.ItemsSource = ItemTypes.DefaultView;
            txtItemType.DisplayMemberPath = "ItemType";
            txtItemType.SelectedValuePath = "ItemType";
        }

        private void LoadSubWT()
        {
            if (PAPERTYPE is null) return;
            
            this.BasisWT = new DataTable("BasisWT");
            this.BasisWT.Columns.Add("SubWT");

            string str = "SELECT DISTINCT SubWT FROM [ProVisionDev].[dbo].MasterInventory WHERE ReportType = '" + PAPERTYPE + "' AND ItemType = '" + ItemType + "' ORDER BY SUBWT";
            SqlConnection cn = new(ConnectionString);
            cn.Open(); if (cn.State != ConnectionState.Open) { MessageBox.Show("Couldn't open SQL Connection"); return; }
            SqlDataAdapter da = new(str, cn);
            da.Fill(BasisWT);

            txtBasis.ItemsSource = BasisWT.DefaultView;
            txtBasis.DisplayMemberPath = "SubWT";
            txtBasis.SelectedValuePath = "SubWT";
        }

        private void LoadRollWidths()
        {
            if (PAPERTYPE is null) return;

            this.Sizes = new DataTable("Sizes");
            this.Sizes.Columns.Add("RollWidth");

            string str = "SELECT RollWidth FROM RollWidth WHERE Abbreviation = '" + PAPERTYPE + "'";
            SqlConnection cn = new(ConnectionString);
            cn.Open(); if (cn.State != ConnectionState.Open) { MessageBox.Show("Couldn't open SQL Connection"); return; }
            SqlDataAdapter da = new(str, cn);
            da.Fill(Sizes);

            cmbRollWidth.ItemsSource = Sizes.DefaultView;
            cmbRollWidth.DisplayMemberPath = "RollWidth";
            cmbRollWidth.SelectedValuePath = "RollWidth";

            txtRollWidth.ItemsSource = Sizes.DefaultView;
            txtRollWidth.DisplayMemberPath = "RollWidth";
            txtRollWidth.SelectedValuePath = "RollWidth";
        }

        private void LoadRollWidthsOnPage1()
        {
            if (PAPERTYPE is null) return;

            this.RollWidths = new DataTable("RollWidths");
            this.RollWidths.Columns.Add("RollWidth");

            string str = "SELECT RollWidth FROM [ProVisionDev].[dbo].RollWidth WHERE Abbreviation = '" + PAPERTYPE + "'";
            SqlConnection cn = new(ConnectionString);
            cn.Open(); if (cn.State != ConnectionState.Open) { MessageBox.Show("Couldn't open SQL Connection"); return; }
            SqlDataAdapter da = new(str, cn);
            da.Fill(RollWidths);

            cmbRollWidth.ItemsSource = RollWidths.DefaultView;
            cmbRollWidth.DisplayMemberPath = "RollWidth";
            cmbRollWidth.SelectedValuePath = "RollWidth";
        }

        private void LoadFormTypes()
        {
            FormTypes = new DataTable("FormTypes");
            FormTypes.Columns.Add("Form");

            string str = "SELECT Form FROM [ESTIMATING].[dbo].[ESTFormType] ORDER BY Form";

            SqlConnection cn = new(ConnectionString);
            SqlDataAdapter da = new(str, cn);
            da.Fill(FormTypes);

            cmbProjectType.ItemsSource = FormTypes.DefaultView;
            cmbProjectType.DisplayMemberPath = "Form";
            cmbProjectType.SelectedValuePath = "Form";
        }

        private void LoadPressSizes()
        {
            // This is not the table to use for Press Sizes. See Tim
            string str = "SELECT PRESS FROM CYLCUTSIZES GROUP BY PRESS, SEQ ORDER BY SEQ";
            SqlConnection cn = new(ConnectionString);
            cn.Open();
            if (cn.State != ConnectionState.Open)
            { MessageBox.Show("Couldn't open SQL Connection"); return; }
            SqlDataAdapter da = new(str, cn);
            DataSet ds = new("PressSizes"); da.Fill(ds); DataTable dt = ds.Tables[0];
            cmbPressSize.Items.Clear();
            for (int r = 0; r < dt.Rows.Count; r++)
            { cmbPressSize.Items.Add(dt.Rows[r][0].ToString()); }
        }

        private void cmbFinalSizeFrac1_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string? controlname = (sender as ComboBox)?.Name;

            int Int1 = 0; int Int2 = 0;
            float Frac1 = 0.00F; float Frac2 = 0.00F;

            if ((cmbFinalSizeInches1.SelectedItem as ComboBoxItem) != null)
            {
                string? Str1 = (cmbFinalSizeInches1.SelectedItem as ComboBoxItem)?.Content.ToString();
                Int1 = int.Parse(Str1);
            }

            if ((cmbFinalSizeFrac1.SelectedItem as ComboBoxItem) != null)
            {
                string? x = (cmbFinalSizeFrac1.SelectedItem as ComboBoxItem)?.Content.ToString();
                Frac1 = CalcFraction(x);
            }

            if ((cmbFinalSizeInches2.SelectedItem as ComboBoxItem) != null)
            {
                string? Str2 = (cmbFinalSizeInches2.SelectedItem as ComboBoxItem)?.Content.ToString();
                Int2 = int.Parse(Str2);
            }

            if ((cmbFinalSizeFrac2.SelectedItem as ComboBoxItem) != null)
            {
                string? x = (cmbFinalSizeFrac2.SelectedItem as ComboBoxItem)?.Content.ToString();
                Frac2 = CalcFraction(x);
            }

            if (controlname?.Substring(controlname.Length - 1) == "1")
            { Decimal1.Content = Frac1 + Int1; }
            else { Decimal2.Content = Frac2 + Int2; }
        }

        private void txtCustomerNum_LostFocus(object sender, RoutedEventArgs e)
        {   if(txtCustomerNum.Text.ToString().TrimEnd().Length == 0) { return; }   
            int CustKey = int.Parse(txtCustomerNum.Text);
            string FixedCust = txtCustomerNum.Text.Trim();
            FixedCust = FixedCust.PadRight(6);
            SqlConnection cn = new(ConnectionString);
            string str = "SELECT * FROM [ESTIMATING].[dbo].[CUSTOMER] C, [ESTIMATING].[dbo].[CUSTOMER_CONTACT] CC " +
                " WHERE C.CUST_NUMB = '" + FixedCust + "' AND C.CUST_NUMB = CC.CUST_NUMB";
            SqlDataAdapter da = new(str, cn);
            DataSet ds = new();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                DataRow dr = dt.Rows[0];
                DataRowView drv = dt.DefaultView[dt.Rows.IndexOf(dr)];
                CustomerName = drv["CUST_NAME"].ToString();
                ContactName  = drv["CONTACT_NAME"].ToString();
                Address      = drv["ADDRESS1"].ToString();
                Phone        = drv["PHONE"].ToString();
                Email        = drv["E_MAIL"].ToString();
                CSZ = drv["CITY"].ToString().TrimEnd() + ", " + drv["STATE"].ToString().TrimEnd() + " " + drv["ZIP"].ToString().TrimEnd();
            }
        }

        private void dgElements_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (sender == null) return;
            var dg = sender as DataGrid;
            var index = dg.SelectedIndex;
            if (index < 0) return;

            string? Seq = Elements?.Rows[index][0].ToString();
            if (Seq == "10" || Seq == "11" || Seq == "12") { MULT = 1.00F; return; }

            string? cell = Elements?.Rows[index][1].ToString();
            if (cell != null)
            { string[] cells = cell.Split(" "); MULT = float.Parse(Elements.Rows[index][3].ToString()); }
        }

        //private void dgElements_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        //{
        //    int RowNumber = dgElements.SelectedIndex;
        //    int ColumnNumber = dgElements.CurrentCell.Column.DisplayIndex;
        //    string? rn = Elements?.Rows[RowNumber][0].ToString();
        //    if (rn == "10" || rn == "11" || rn == "12") return;
        //    if (ColumnNumber != 2) return;

        //    hp = new HistoricalPrices();
        //    hp.ShowDialog();
        //    float? savedPrice = hp.Price;
        //    hp.Close();
        //    if (savedPrice != null) { Elements.Rows[RowNumber][ColumnNumber] = savedPrice; }
        //}

        private void HandleEsc(object sender, KeyEventArgs e) { if (e.Key == Key.Escape) Close(); }

        private void txtQuoteNum_LostFocus(object sender, RoutedEventArgs e)
        {
            txtQuoteNum.Foreground = Brushes.Black;
            txtQuoteNum.Background = Brushes.White;

            GetQuote();
        }

        private void Qty1b_LostFocus(object sender, RoutedEventArgs e)
        {
            if ((QTY1a != null && QTY1a != 0) && (CPM1a != null && CPM1a != 0) & (MAR1a != null && MAR1a != 0))
            { float? result = QTY1a * CPM1a * MAR1a; EXT1a = EXT1a = result / 1000.00F; }
        }

        private void Qty2b_LostFocus(object sender, RoutedEventArgs e)
        {
            if ((QTY2a != null && QTY2a != 0) && (CPM2a != null && CPM2a != 0) & (MAR2a != null && MAR2a != 0)) { float? result = QTY2a * CPM2a * MAR2a; EXT2a = EXT2a = result / 1000.00F; }
        }

        private void Qty3b_LostFocus(object sender, RoutedEventArgs e)
        {
            if ((QTY3a != null && QTY3a != 0) && (CPM3a != null && CPM3a != 0) & (MAR3a != null && MAR3a != 0)) { float? result = QTY3a * CPM3a * MAR3a; EXT3a = EXT3a = result / 1000.00F; }
        }

        private void Qty4b_LostFocus(object sender, RoutedEventArgs e)
        {
            if ((QTY4a != null && QTY4a != 0) && (CPM4a != null && CPM4a != 0) & (MAR4a != null && MAR4a != 0)) { float? result = QTY4a * CPM4a * MAR4a; EXT4a = EXT4a = result / 1000.00F; }
        }

        private void Markup1_GotFocus(object sender, RoutedEventArgs e)
        {
            Markup1.SelectAll();
        }

        private async void SelectAll_OnTextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            await Application.Current.Dispatcher.InvokeAsync((sender as TextBox).SelectAll);
        }

        private void LoadCollator()
        {
            if(PRESSSIZE.Length == 0 ) { return; }      // needed if we look up a quote and blank any previous values on PAGE 1

            DataTable CollCuts = new DataTable("CollCuts");
            CollCuts.Columns.Add("CutSize");

            string cmd = "SELECT CutSize FROM CylCutSizes WHERE Press = '" + PRESSSIZE + "'";

            SqlConnection cn = new(ConnectionString); cn.Open();
            SqlDataAdapter da = new(cmd, cn);

            da.Fill(CollCuts);
            if(cmbCollatorCut.ItemsSource == null) cmbCollatorCut.Items.Clear();

            this.cmbCollatorCut.DataContext = this;
            this.cmbCollatorCut.ItemsSource = CollCuts.DefaultView;
            this.cmbCollatorCut.DisplayMemberPath = "CutSize";
            this.cmbCollatorCut.SelectedValuePath = "CutSize";
        }

        private void cmbPressSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string cmd =
                  "SELECT FormSize FROM PressSizes WHERE cylinder = '" + cmbPressSize.SelectedValue.ToString()
                + "'  ORDER BY CASE WHEN CHARINDEX(' ', FormSize) > 0 "
                + "                 THEN CONVERT(int, SUBSTRING(FormSize,0, charindex(' ', FormSize))) " 
                + "                 ELSE FormSize END";

            SqlConnection cn = new(ConnectionString); cn.Open();
            if (cn.State != ConnectionState.Open)
            { MessageBox.Show("Couldn't open SQL Connection"); return; }
            SqlDataAdapter da = new(cmd, cn);
            DataSet ds = new("CollatorCuts"); da.Fill(ds); DataTable dt = ds.Tables[0];
            cmbCollatorCut.Items.Clear();

            for (int r = 0; r < dt.DefaultView.Count; r++)
            { cmbCollatorCut.Items.Add(dt.DefaultView[r][0].ToString()); }
              cmbCollatorCut.SelectedIndex = -1;

            lblUpMessage.Content = "Select a value to recalculate...";
        }

        private void btnPressCalc_Click(object sender, RoutedEventArgs e)
        {
            PressCalc pc = new();
            pc.ShowDialog();
        }

        private void btnLoadGrid_Click(object sender, RoutedEventArgs e)
        {
            if(ProjectType.Length==0) { MessageBox.Show("Please select a project type"); return; }

            int setNum       = int.Parse(PartsSpinner.Value.ToString());
            string formType  = ProjectType.Substring(0, 1);
            string paperType = PAPERTYPE.Substring(0, 1);
            string rollWidth = ROLLWIDTH.ToString();

            string cmd = "SELECT M.Description," +
                "M.ItemType," +
                "M.SubWT," +
                "M.COLOR AS MCOLOR," +
                "E.PAPER," +
                "E.SETNUM," +
                "E.SEQ," +
                "E.PTYPE," +
                "E.COLOR," +
                "E.FORMTYPE," +
                "E.PAPERTYPE," +
                "0.00 AS Pounds," +
                "0.00 AS LastPOCost," +
                "0.00 AS AverageCost," +
                "M.COSTPERFACTOR AS MastInvCost," +
                "0.00 AS SelectedCost," +
                "0.00 AS PaperCost" +
                " FROM             PROVISIONDEV.DBO.MasterInventory M" +
                " RIGHT OUTER JOIN PROVISIONDEV.DBO.ESTPAPER        E" +
                "  ON M.ITEMTYPE    = E.PTYPE"       +
                " AND M.COLOR       = E.COLOR"       +
                " AND M.SubWT       = E.WEIGHT"      +
               $" WHERE E.FORMTYPE  = '{formType}'"  +
               $"   AND E.PAPERTYPE = '{paperType}'" +
               $"   AND M.SIZE      = '{rollWidth}'" +
               $"   AND E.SETNUM    =  {setNum}"     +
                " AND Inventoryable = 1"             +
                " ORDER BY E.SETNUM, E.SEQ";

            SqlConnection cn = new(ConnectionString); cn.Open();
            SqlDataAdapter da = new(cmd, cn);
            DataSet ds = new("Papers");
            da.Fill(ds);
            DataTable? dt = ds.Tables[0];

            for (int r = 0; r < dt.DefaultView.Count; r++)
            {
                string? clr  = dt.Rows[r]["Color"].ToString();
                string? wt   = dt.Rows[r]["SubWT"].ToString();
                string? it   = dt.Rows[r]["PType"].ToString();
                it = (it == null) ? "" : it.Substring(0, 1) + "%";
                string? size = cmbRollWidth.SelectedValue.ToString();
                size = (size is null) ? "" : size.TrimEnd();

                string? projType = cmbProjectType.SelectedValue.ToString();
                projType = (projType == "SHEET") ? "SHT" : "ROLL";   // Sheet or Continuous

                dt.Rows[r]["Pounds"] = 0;    // Zero out all Pounds; PaperCalc will calculate them.

                // Get the most recent and the average costs for this paper type:
                SqlDataAdapter da4 = new SqlDataAdapter("MostRecentCost", ConnectionString);
                da4.SelectCommand.CommandType = CommandType.StoredProcedure;
                da4.SelectCommand.Parameters.Add("@Description", SqlDbType.VarChar, 50).Value = dt.Rows[r][0]; //desc; 
                DataTable dtRec = new("Rec");
                da4.Fill( dtRec);
                if (dtRec.Rows.Count > 0) 
                    {  dt.Rows[r]["LastPOCost"] = dtRec.Rows[0][0].ToString(); }

                SqlDataAdapter da5 = new SqlDataAdapter("AverageCost", ConnectionString);
                da5.SelectCommand.CommandType = CommandType.StoredProcedure;
                da5.SelectCommand.Parameters.Add("@Description", SqlDbType.VarChar, 50).Value = dt.Rows[r][0]; // desc;
                DataTable dtAvg = new("Avg");
                da5.Fill(dtAvg);
                if(dtAvg.Rows.Count>0) 
                    {  dt.Rows[r]["AverageCost"] = dtAvg.Rows[0][0].ToString();     // I rewrote the query to return just one column (average cost)
                       dt.Rows[r]["SelectedCost"] = dtAvg.Rows[0][0].ToString();    // SelectedCost defaults to average cost
                    }
                else { dt.Rows[r]["SelectedCost"] = dt.Rows[r]["LastPOCost"]; }     // Default is AverageCost unless it was zero
            }

            dgSheetsOfPaper.ItemsSource = dt.DefaultView;

            // TODO: Select the first description and populate the comboboxes above the grid

            Page2.IsEnabled = true;
            Page3.IsEnabled = true;
            Page2.Focus();

            dgSheetsOfPaper.SelectedIndex = -1;     // Force loading of labels 
            dgSheetsOfPaper.SelectedIndex = 0;      //  using the first row description

        }

        public void CalculateRowCost()
        {
            int rownum = dgSheetsOfPaper.SelectedIndex;
            SelRowIdx = rownum;             // Needed for the Update button code
            if (rownum < 0) return;
            if (Tabs.SelectedIndex == 0) { return; }

            DataRowView dataRow = (DataRowView)dgSheetsOfPaper.SelectedItem;
            DRV = dataRow;
            if (DRV is null) return;

            // Pull out four key values and display them in editable comboboxes, above.

            string tmpPaperType = dataRow[7].ToString().Substring(0, 1); // Should this be column 10 instead of 7?
            switch (tmpPaperType) { case "B": tmpPaperType = "BOND"; break; case "C": tmpPaperType = "CRBNLS"; break; case "E": tmpPaperType = "ELEC"; break; }

            txtItemType.Text = tmpPaperType;
            txtColor.SelectedValue = dataRow[8].ToString();     // Should this set SelectedItem instead?
            txtBasis.SelectedValue = dataRow[2].ToString().TrimEnd();

            lblItemType.Content = tmpPaperType;
            lblColor.Content = dataRow[8].ToString();
            lblBasis.Content = dataRow[2].ToString().TrimEnd();

            // If the Description field basis doesn't match the contents that came from ESTPAPER.SubWT, fix it.
            if (dataRow[0].ToString().Contains(".")) 
                {  // Get basis from Description starting right after the first chr(32) up to but not including the "#":
                    int starts  = dataRow[0].ToString().IndexOf(" ") + 1;
                    string temp = dataRow[0].ToString().Substring(starts);
                    int ends = temp.ToString().IndexOf("#");
                    lblBasis.Content = temp.Substring(0, ends);
                }

            lblPaperType.Content = dataRow[7].ToString().TrimEnd();

            // RollWidth (size) is just to the right of "COLOR" in Description (dataRow[7])
            string col  = lblColor.Content.ToString();
            string Desc = dataRow[0].ToString();
            int ColorAt = Desc.IndexOf(col) + 4;
            string RW   = Desc.Substring(ColorAt);  // RollWidth

            lblRollWidth.Content = RW.ToString().TrimEnd();

            // Insert the value in the selected column into the "SelectedCost" column (12)
            DataGridColumn chosen = dgSheetsOfPaper.CurrentColumn;

            var x = (chosen == null) ? "" : chosen.ToString();
            if (chosen != null)
            {   // select a price if they clicked columns 12, 13 or 14
                int idx = chosen.DisplayIndex + 10;

                if (idx < 12 || idx > 14) return;    // Only three of the columns should be clickable
                                                    
                txtItemType.IsDropDownOpen = false; // collapse the ItemType dropdown
                dataRow[15] = dataRow[idx];         // Use the value they clicked as the "cost to use"
            }

            double a = double.Parse(dataRow[11].ToString()) * double.Parse(dataRow[15].ToString());
            dataRow[16] = a;

            dgSheetsOfPaper.SelectedIndex = -1;

            SumRowTotals();

        }

        private void dgSheetsOfPaper_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            CalculateRowCost();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            // Do they want to change anything on the current row?
            if (   txtRollWidth.Text.Length == 0
                && txtItemType.Text.Length == 0
                && txtColor.Text.Length     == 0
                && txtBasis.Text.Length     == 0)
                { MessageBox.Show("No changes to make."); return; }

            // Verify that the new description exists in the MasterInventory table description field
 
            string? TestDesc = DRV[0].ToString().TrimEnd();
            if (lblPaperType.Content.ToString().Length > 0 && txtPaperType.Text.ToString().TrimEnd().Length > 0)
                { TestDesc = TestDesc.Replace(lblPaperType.Content.ToString().TrimEnd(), ItemType.ToString().TrimEnd()); }

            if (lblBasis.Content.ToString().Length > 0 && txtBasis.Text.ToString().TrimEnd().Length > 0)
                { TestDesc = TestDesc.Replace(lblBasis.Content.ToString().TrimEnd() + "#", txtBasis.Text.ToString().TrimEnd() + "#"); }
            
            if (lblColor.Content.ToString().Length > 0 && txtColor.Text.ToString().TrimEnd().Length > 0)
                { TestDesc = TestDesc.Replace(lblColor.Content.ToString().TrimEnd(), txtColor.Text.ToString().TrimEnd()); }

            if (lblRollWidth.Content.ToString().Length > 0 && txtRollWidth.Text.ToString().TrimEnd().Length > 0)
                { TestDesc = TestDesc.Replace(lblRollWidth.Content.ToString().TrimEnd(), txtRollWidth.Text.ToString().TrimEnd()); }

            string cmd = "SELECT COUNT(*) FROM MasterInventory WHERE Description = '" + TestDesc + "'";

            SqlConnection cn = new(ConnectionString);
            SqlDataAdapter da4 = new(cmd, cn); DataTable dt4 = new("CT"); da4.Fill(dt4);

            if (int.Parse(dt4.Rows[0][0].ToString()) == 0) 
              { MessageBox.Show("Paper not found", TestDesc); return; }

            string WhereClause  = " WHERE ReportType = '" + PAPERTYPE.ToString().TrimEnd() + "'";
            if (txtRollWidth.Text.Length > 0) { WhereClause +=   " AND Size = '"       + lblRollWidth.Content.ToString().TrimEnd() + "'"; }
            if (txtColor.Text.Length     > 0) { WhereClause +=   " AND Color = '"      + lblColor.Content.ToString().TrimEnd() + "'"; }
            if (txtBasis.Text.Length     > 0) { WhereClause +=   " AND SubWT = '"      + lblBasis.Content.ToString().TrimEnd() + "'"; }

            cmd = "SELECT COUNT(*) FROM MasterInventory " + WhereClause;

            SqlDataAdapter da5 = new(cmd, cn); DataTable dt5 = new("CT"); da5.Fill(dt5);

            if (dt5.Rows.Count == 0) { MessageBox.Show("Paper not found in MasterInventory"); return; }

            if (lblPaperType.Content.ToString().Length > 0 && txtPaperType.Text.ToString().TrimEnd().Length > 0)
            {
                DRV[0] = DRV[0].ToString().Replace(lblPaperType.Content.ToString().TrimEnd(), ItemType.ToString().TrimEnd());
                DRV[7] = ItemType;
            }

            if (lblBasis.Content.ToString().Length > 0 && txtBasis.Text.ToString().TrimEnd().Length > 0)
            {
                DRV[0] = DRV[0].ToString().Replace(lblBasis.Content.ToString().TrimEnd() + "#", txtBasis.Text.ToString().TrimEnd() + "#");
                DRV[4] = DRV[4].ToString().Replace(lblBasis.Content.ToString().TrimEnd() + "#", txtBasis.Text.ToString().TrimEnd() + "#");
                DRV[2] = txtBasis.Text.ToString().TrimEnd();    // What is this?
            }

            if (lblColor.Content.ToString().Length > 0 && txtColor.Text.ToString().TrimEnd().Length > 0) 
            { 
                DRV[0] = DRV[0].ToString().Replace(lblColor.Content.ToString().TrimEnd(), txtColor.Text.ToString().TrimEnd());
                DRV[4] = DRV[4].ToString().Replace(lblColor.Content.ToString().TrimEnd(), txtColor.Text.ToString().TrimEnd());
                DRV[3] = txtColor.Text.ToString().TrimEnd();
                DRV[8] = txtColor.Text.ToString().TrimEnd();
            }

            if (lblRollWidth.Content.ToString().Length > 0 && txtRollWidth.Text.ToString().TrimEnd().Length > 0) 
            { 
                DRV[0] = DRV[0].ToString().Replace(lblRollWidth.Content.ToString().TrimEnd(), txtRollWidth.Text.ToString().TrimEnd());
                // There is no RollWidth column in DRV, because DRV comes from Estimating.dbo.ESTPAPER; should we add one?
            }

            return;
        }

        private void dgSheetsOfPaper_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {   // What is this?
            // Recalculate cost
            //string? rowNum = e.Row.DataContext.ToString();
            //string prop1 = (e.Row.Item as DataRowView).Row[1].ToString();
        }

        private void dgSheetsOfPaper_CurrentCellChanged(object sender, System.EventArgs e)
        {
            txtItemType.Focus();  // TODO: Provide a visual cue that combobox itemlists have been refreshed and they need to pick anything they want to change
            PickMsg.Visibility = Visibility.Visible;
            txtItemType.IsDropDownOpen = true;
        }

        private void cmbCollatorCut_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbCollatorCut.SelectedValue == null) return;
 
            string? val = cmbCollatorCut.SelectedValue.ToString().TrimEnd();
            string sval1, sval2;
            sval1 = "";
            sval2 = "";
            if (val.IndexOf(" ") > 0)
                { sval1 = val.Substring(0,val.IndexOf(" ")); 
                  sval2 = val.Substring(val.IndexOf(" ") + 1); 
                }
            else
                { sval1 = val; }
            float val1 = int.Parse(sval1);
            float val2 = 0.0F;
            val2 = CalcFraction(sval2);
            float totval = val1 + val2;

            string intPart = String.Join("", PRESSSIZE.Where(char.IsDigit));    // Might end in a 3-char press name...
            float UpVal = float.Parse(intPart) / totval;    // remove text characters from PRESSSIZE before calculating...
            int intVal = (int)Math.Round(UpVal);
            lblUp.Content = UpVal.ToString();
            lblUpMessage.Content = "up";
        }

        private void txtQty1_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            SelectedQty = Qty1;
            GridCalc();
            QTY1a = SelectedQty;
            CPM1a = float.Parse(QuoteTotal.ToString()) / float.Parse(SelectedQty.ToString());
            CPM1a = CPM1a * 1000.00F;
        }
        private void txtQty2_PreviewMouseDown(object sender, MouseButtonEventArgs e) 
        {   SelectedQty = Qty2;
            GridCalc();
            QTY2a = SelectedQty;
            CPM2a = float.Parse(QuoteTotal.ToString()) / float.Parse(SelectedQty.ToString());
            CPM2a = CPM2a * 1000.00F;
        }
        private void txtQty3_PreviewMouseDown(object sender, MouseButtonEventArgs e) 
        {   SelectedQty = Qty3;
            GridCalc();
            QTY3a = SelectedQty;
            CPM3a = float.Parse(QuoteTotal.ToString()) / float.Parse(SelectedQty.ToString());
            CPM3a = CPM3a * 1000.00F;
        }
        private void txtQty4_PreviewMouseDown(object sender, MouseButtonEventArgs e) 
        {   SelectedQty = Qty4;
            QTY4a = SelectedQty;
            CPM4a = float.Parse(QuoteTotal.ToString()) / float.Parse(SelectedQty.ToString());
            CPM4a = CPM4a * 1000.00F;
        }

        private void GridCalc()
        {
            Calculating c = new Calculating(); c.Show();    // Show the "busy" message...
            PressCalc pc = new PressCalc();
            pc.WastePct = (int)WastePct;
            pc.Up = int.Parse(lblUp.Content.ToString());
            QuoteTotal = 0.00D;
            int r = 0;
            for (r = 0; r < dgSheetsOfPaper.Items.Count; r++)
            {
                pc.NumDocs = (int)SelectedQty;
                pc.Material = ((System.Data.DataRowView)dgSheetsOfPaper.Items[r]).Row.ItemArray[0].ToString();
                pc.PressSize = PRESSSIZE;
                dgSheetsOfPaper.SelectedIndex = r;
                pc.Calc();
                DataRowView dataRow = (DataRowView)dgSheetsOfPaper.Items[r];
                dataRow[11] = pc.Pounds;    // multiply UseThisPrice times Pounds and store in last column
                dataRow[16] = float.Parse(pc.Pounds.ToString()) * float.Parse(dataRow[15].ToString());
                QuoteTotal += double.Parse(dataRow[16].ToString());
            }
            c.Close();
            SumRowTotals();
        }

        private void SumRowTotals()
        {
            QuoteTotal = 0.00D; int r = 0;
            for (r = 0; r < dgSheetsOfPaper.Items.Count; r++)
            { DataRowView dataRow = (DataRowView)dgSheetsOfPaper.Items[r]; 
              QuoteTotal += double.Parse(dataRow[16].ToString()); 
            }
        }

        private void txtQty1_GotFocus(object sender, RoutedEventArgs e) { txtQty1.SelectAll(); }
        private void txtQty2_GotFocus(object sender, RoutedEventArgs e) { txtQty2.SelectAll(); }
        private void txtQty3_GotFocus(object sender, RoutedEventArgs e) { txtQty3.SelectAll(); }
        private void txtQty4_GotFocus(object sender, RoutedEventArgs e) { txtQty4.SelectAll(); }

        private void cmbProjectType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        { LoadRollWidths(); }

        private void LoadPage2Combos()
        {
            LoadSubWT();
            LoadColors();
            LoadRollWidths();
        }

        private void txtItemType_DropDownOpened(object sender, System.EventArgs e)
        {
            PickMsg.Visibility = Visibility.Hidden;
        }

        private void cmbPaperType_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        { LoadRollWidths(); }

        private void btnNewQuote_Click(object sender, RoutedEventArgs e)
        {
            NewQuote();
        }

        private void NewQuote()
        {
            Adding = true; // to suppress "DELETE ALL ROWS?" message connected to ROLL_WIDTH validation

            conn = new SqlConnection(ConnectionString); conn.Open();
            SqlCommand scmd = new SqlCommand();
            scmd.Connection = conn;
            scmd.CommandType = CommandType.Text;
            scmd.CommandText = "SELECT MAX(QUOTE_NUM) FROM [ESTIMATING].[dbo].[QUOTES]";
            string NextQuoteNum = (string)scmd.ExecuteScalar();
            int nextNum = int.Parse(NextQuoteNum.ToString().TrimEnd());
            nextNum += 1;
            QUOTE_NUM = nextNum.ToString() + " ";

            ProjectType = "";
            PAPERTYPE = "";
            ROLLWIDTH = "";
            PRESSSIZE = "";
            LINEHOLES = false;
            COLLATORCUT = "";

            LoadAvailableCategories();
            AddDefaultDetailLines();        // Is this where this call goes?
        }

        private void lstSelected_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Trace.WriteLine("Right-clicked");  // Ctrl+Alt+O
            if (lstSelected.SelectedItem == null) return;

            string? x = lstSelected.SelectedItem.ToString().TrimEnd();
            string check;
//            check = ((char)0x221A).ToString();
            check = "🗹";

            if (x.IndexOf(check) > 0) { x = x.Substring(0, x.IndexOf(check)); }
            x = x.Trim();

            switch (x)
            {

                case "Base Charges":
                    {
                        BaseCharge bc = new BaseCharge(QUOTE_NUM); bc.ShowDialog();
                        break;
                    }

                case "Backer":
                    {
                        PARTS = 3;
                        BackerForm backer = new BackerForm(PRESSSIZE, QUOTE_NUM); backer.ShowDialog();
                        backer.Close();
                        break;
                    }

                case "Ink Color":
                    {
                        Ink Ink = new Ink(PRESSSIZE, QUOTE_NUM); Ink.ShowDialog();
                        break;
                    }

                case "Perfing":
                    {
                        Perfing perf = new Perfing(PRESSSIZE, QUOTE_NUM); perf.ShowDialog();
                        break;
                    }

                case "MICR":
                    {
                        MICR micr = new MICR(PRESSSIZE, QUOTE_NUM); 
                        micr.QuoteNum = QUOTE_NUM; 
                        micr.PressSize = PRESSSIZE; 
                        micr.ShowDialog();
                        break;
                    }

                case "Punching":
                    {
                        Punching punch = new Punching(PRESSSIZE, QUOTE_NUM); punch.ShowDialog();
                        break;
                    }

                case "Press Numbering":
                    {
                        PressNum pressnum = new PressNum(PRESSSIZE, QUOTE_NUM); pressnum.ShowDialog();
                        break;
                    }

                case "Finishing":
                    {
                        //MessageBox.Show("In progress..."); return;

                        Finishing finishing = new Finishing(PRESSSIZE, QUOTE_NUM); finishing.ShowDialog();
                        break;
                    }

                case "Converting":
                    {
                        Converting converting = new Converting(PRESSSIZE, QUOTE_NUM); converting.ShowDialog();
                        break;
                    }

                case "PrePress":
                    {
                        PrePress prepress = new PrePress(PRESSSIZE, QUOTE_NUM); prepress.ShowDialog();
                        break;
                    }

                case "Combo":
                    {
                        Combo combo = new Combo(PRESSSIZE, QUOTE_NUM); combo.ShowDialog(); 
                        if (combo.Removed) { lstSelected_MouseDoubleClick(null, null); }
                        break;
                    }

                case "Strike In":
                    {
                        StrikeIn strikein = new StrikeIn(PRESSSIZE, QUOTE_NUM); strikein.ShowDialog();
                        if (strikein.Removed) { lstSelected_MouseDoubleClick(null, null); }
                        break;
                    }

                case "Security":
                    {
                        Security security = new Security(PRESSSIZE, QUOTE_NUM); security.ShowDialog();
                        break;
                    }
            }

            CheckRows();

        }

        private void CheckRows()
        {

            // If any of the ten Value columns contains anything, show a checkmark.
            // Note that Base Charges, Backer, Security are always checked

            string cmd = $"SELECT * FROM [ESTIMATING].[dbo].[QUOTE_DETAILS] WHERE QUOTE_NUM = '{QUOTE_NUM}' ORDER BY SEQUENCE";

            dt   = new DataTable("Details");
            conn = new SqlConnection(ConnectionString);
            da   = new SqlDataAdapter(cmd, conn); da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string V1 = dt.Rows[i]["Value1"].ToString();
                string V2 = dt.Rows[i]["Value2"].ToString();
                string V3 = dt.Rows[i]["Value3"].ToString();
                string V4 = dt.Rows[i]["Value4"].ToString();
                string V5 = dt.Rows[i]["Value5"].ToString();
                string V6 = dt.Rows[i]["Value6"].ToString();
                string V7 = dt.Rows[i]["Value7"].ToString();
                string V8 = dt.Rows[i]["Value8"].ToString();
                string V9 = dt.Rows[i]["Value9"].ToString();
                string V10 = dt.Rows[i]["Value10"].ToString();

                Int32 I1 = 0;
                Int32 I2 = 0;
                Int32 I3 = 0;
                Int32 I4 = 0;
                Int32 I5 = 0;
                Int32 I6 = 0;
                Int32 I7 = 0;
                Int32 I8 = 0;
                Int32 I9 = 0;
                Int32 I10 = 0;

                bool B1 = int.TryParse(dt.Rows[i]["Value1"].ToString(), out I1);
                bool B2 = int.TryParse(dt.Rows[i]["Value2"].ToString(), out I2);
                bool B3 = int.TryParse(dt.Rows[i]["Value3"].ToString(), out I3);
                bool B4 = int.TryParse(dt.Rows[i]["Value4"].ToString(), out I4);
                bool B5 = int.TryParse(dt.Rows[i]["Value5"].ToString(), out I5);
                bool B6 = int.TryParse(dt.Rows[i]["Value6"].ToString(), out I6);
                bool B7 = int.TryParse(dt.Rows[i]["Value7"].ToString(), out I7);
                bool B8 = int.TryParse(dt.Rows[i]["Value8"].ToString(), out I8);
                bool B9 = int.TryParse(dt.Rows[i]["Value9"].ToString(), out I9);
                bool B10 = int.TryParse(dt.Rows[i]["Value10"].ToString(), out I10);

                Int32 RowTotal = 0;

                string? CheckCategory = dt.Rows[i]["Category"].ToString();

                if (CheckCategory.ToUpper().Contains("BASE"))     { RowTotal = 1; }
                if (CheckCategory.ToUpper().Contains("BACKER"))   { RowTotal = 1; }
                if (CheckCategory.ToUpper().Contains("SECURITY")) { RowTotal = 1; }

                if (B1 == true) { RowTotal += I1; }
                if (B2 == true) { RowTotal += I2; }
                if (B3 == true) { RowTotal += I3; }
                if (B4 == true) { RowTotal += I4; }
                if (B5 == true) { RowTotal += I5; }
                if (B6 == true) { RowTotal += I6; }
                if (B7 == true) { RowTotal += I7; }
                if (B8 == true) { RowTotal += I8; }
                if (B9 == true) { RowTotal += I9; }
                if (B10 == true) { RowTotal += I10; }

//                string suffix = (RowTotal>0) ? " " + ((char)0x221A).ToString() : string.Empty;
                string suffix = (RowTotal > 0) ? " " + "🗹" : string.Empty;
                
                if (i < lstSelected.Items.Count)
                 { lstSelected.Items[i] = lstSelected.Items[i].ToString() + (" ").PadRight(20);
                   lstSelected.Items[i] = lstSelected.Items[i].ToString().Substring(0,17) + suffix;
                 }

            }

            conn.Close();
        }
        
        private void Page3_GotFocus(object sender, RoutedEventArgs e)
        {
            LoadDetails();
            return;
        }

        private void LoadDetails()
        {
            if(Adding) lstSelected.Items.Clear();

            // Load any selected categories and remove them from the "Available" listbox

            string cmd = $"SELECT CATEGORY, Amount FROM [ESTIMATING].[dbo].[QUOTE_DETAILS] WHERE QUOTE_NUM = '{QUOTE_NUM}' ORDER BY Sequence";
            dt = new DataTable("Details");
            conn = new SqlConnection(ConnectionString);
            da = new SqlDataAdapter(cmd, conn); da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {   string s = dt.Rows[i]["CATEGORY"].ToString();
                if (s == "PressNum") { s = "Press Numbering"; }  // Is this Kludge still needed?
                bool found = false;
                if (lstSelected.Items.Count > 0)
                 { for (int j = 0; j < lstSelected.Items.Count; j++) { if (lstSelected.Items[j].ToString().Contains(s)) { found = true; } } }
                if(!found)lstSelected.Items.Add(s);
                lstAvailable.Items.Remove(s);
            }
            conn.Close();
            Adding = false;

            CheckRows();        // Add a checkmark at the end for items that have been selected

        }

        private void lstAvailable_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string? x = lstAvailable.SelectedItem.ToString();
            lstSelected.Items.Add(x);
            lstAvailable.Items.Remove(x);
        }

        private void lstSelected_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Category = lstSelected.SelectedItem.ToString().TrimEnd();
            string check = ((char)0x221A).ToString(); // Previously I used the square root symbol; I replaced it with the character seen below.
            check = "🗹";

            if (Category.Contains(check)) { Category = Category.Substring(0, Category.IndexOf(check)).TrimEnd(); }

            if(Category=="Press Numbering")Category = "PressNum";

            string cmd = $"DELETE [ESTIMATING].[dbo].[Quote_Details] WHERE Quote_Num = '{QUOTE_NUM}' AND Category LIKE '{Category}%'";
            conn = new SqlConnection(ConnectionString); SqlCommand scmd = new SqlCommand(cmd, conn); conn.Open();
            try { scmd.ExecuteNonQuery(); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { conn.Close(); }

            RemoveSelectedFeature();
            LoadAvailableCategories();
            LoadDetails();
        }

        private void RemoveSelectedFeature()
        {
            string? x = lstSelected.SelectedItem.ToString();
            lstSelected.Items.Remove(x);
        }

        private void btnAssignNewQuoteNum_Click(object sender, RoutedEventArgs e)
        {
            QuoteLookup ql = new();
            ql.ShowDialog();
            QUOTE_NUM = ql.SelQuote; ql.Close();

            if (QUOTE_NUM == null || QUOTE_NUM.Length == 0) return;

            GetQuote();
            NewQuote();

            txtQuoteNum.Focus();
            txtQuoteNum.Background = Brushes.Red;
            txtQuoteNum.Foreground = Brushes.Yellow;
        }

        private void cmbPressSize_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (Adding) return;

            if(lstSelected.Items.Count == 0 || cmbPressSize.SelectedIndex == -1) return;
            if(cmbPressSize.Text == cmbPressSize.SelectedItem.ToString()) return;
            if (MessageBox.Show("This will delete any selected features; continue?", "Feature selection depends on Press Size", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) != MessageBoxResult.No)
            {   //TODO: Should this also delete Base Charges, Ink and Backer?
                string cmd = $"DELETE [ESTIMATING].[dbo].[QUOTE_DETAILS] WHERE QUOTE_NUM = '{QUOTE_NUM}'";
                conn = new SqlConnection(ConnectionString);
                conn.Open();
                scmd = new SqlCommand(cmd, conn); scmd.ExecuteNonQuery();
                conn.Close();
                lstSelected.Items.Clear(); 
                LoadPressSizes();
                MessageBox.Show("Features removed.", "Done", MessageBoxButton.OK, MessageBoxImage.Information );
            }
        }

        private void btnSaveQuote_Click(object sender, RoutedEventArgs e)
        {
            // Do I need to save the bit value LineHoles as well?
            string cmd
                =  "INSERT INTO QUOTES (   QUOTE_NUM,     CUST_NUM,     PARTS,    PAPERTYPE,     ROLLWIDTH,   LINEHOLES,   PRESSSIZE,     COLLATORCUT,     PROJECTTYPE,   Date,       Amount      ) "
                + $"            VALUES ( '{QUOTE_NUM}', '{CUST_NUMB}', {PARTS}, '{PAPERTYPE}', '{ROLLWIDTH}', 0,         '{PRESSSIZE}', '{COLLATORCUT}', '{ProjectType}', GetDate(), {QuoteTotal} )";
            Clipboard.SetText(cmd);
            SqlConnection conn = new SqlConnection(ConnectionString);
            conn.Open(); scmd = new SqlCommand(cmd, conn); scmd.ExecuteNonQuery(); conn.Close(); MessageBox.Show(" Quote saved ", "Done", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
        }

        private void btnCancelQuote_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("Delete this new quote?", "Erase this new quote?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) 
            {   string cmd = $"DELETE QUOTE_DETAILS WHERE QUOTE_NUM = '{QUOTE_NUM}'";
                conn = new SqlConnection(ConnectionString);
                scmd = new SqlCommand(cmd, conn); scmd.ExecuteNonQuery();
                MessageBox.Show("Done");
                Close();
            }
        }

        private void Page4_GotFocus(object sender, RoutedEventArgs e)
        {
            string cmd = $"SELECT SUM(TotalFlatChg) AS TotalCharge FROM [ESTIMATING].[dbo].[QUOTE_DETAILS] WHERE QUOTE_NUM = {QUOTE_NUM}";
            SqlConnection conn = new SqlConnection(ConnectionString);
            da = new SqlDataAdapter (cmd, conn); 
            dt = new DataTable("Tot"); da.Fill(dt);
            QuoteTotal  = float.Parse(dt.Rows[0]["TotalCharge"].ToString());
            conn.Close();
        }
    }
}