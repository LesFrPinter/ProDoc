using ProDocEstimate.Views;
using SharpDX;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ProDocEstimate
{
    public partial class Quotations : Window, INotifyPropertyChanged
    {
        HistoricalPrices hp = new();

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

        public string ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public SqlConnection? cn = new();
        public SqlDataAdapter? da;

        #region Property Declarations

        private string? quote_num = ""; public string? QUOTE_NUM { get { return quote_num; } set { quote_num = value; OnPropertyChanged(); } }
        private string? cust_num; public string? CUST_NUM { get { return cust_num; } set { cust_num = value; OnPropertyChanged(); } }

        private string? projectType = ""; public string? ProjectType { get { return projectType; } set { projectType = value; OnPropertyChanged(); } }

        private string? fsint1; public string? FSINT1 { get { return fsint1; } set { fsint1 = value; OnPropertyChanged(); } }
        private string? fsfrac1; public string? FSFRAC1 { get { return fsfrac1; } set { fsfrac1 = value; OnPropertyChanged(); } }
        private string? fsint2; public string? FSINT2 { get { return fsint2; } set { fsint2 = value; OnPropertyChanged(); } }
        private string? fsfrac2; public string? FSFRAC2 { get { return fsfrac2; } set { fsfrac2 = value; OnPropertyChanged(); } }
        private int? parts; public int? PARTS { get { return parts; } set { parts = value; OnPropertyChanged(); } }

        private string? papertype; public string? PAPERTYPE { get { return papertype; } set { papertype = value; OnPropertyChanged(); LoadRollWidths(); LoadItemTypes(PAPERTYPE); } }
        private string? rollwidth; public string? ROLLWIDTH { get { return rollwidth; } set { rollwidth = value; OnPropertyChanged(); } }
        private string? presssize; public string? PRESSSIZE { get { return presssize; } set { presssize = value; OnPropertyChanged(); LoadCollator(); } }
        private bool?   lineholes; public bool?   LINEHOLES  { get { return lineholes; } set { lineholes = value; OnPropertyChanged(); } }
        private string? collatorcut; public string? COLLATORCUT { get { return collatorcut; } set { collatorcut = value; OnPropertyChanged(); } }

        private string? customerName; public string? CustomerName { get { return customerName; } set { customerName = value; OnPropertyChanged(); } }
        private string? address; public string? Address { get { return address; } set { address = value; OnPropertyChanged(); } }
        private string? city; public string? City { get { return city; } set { city = value; OnPropertyChanged(); } }
        private string? state; public string? State { get { return state; } set { state = value; OnPropertyChanged(); } }
        private string? zip; public string? ZIP { get { return zip; } set { zip = value; OnPropertyChanged(); } }
        private string? location; public string? Location { get { return location; } set { location = value; OnPropertyChanged(); } }
        private string? phone; public string? Phone { get { return phone; } set { phone = value; OnPropertyChanged(); } }
        private string? csz; public string? CSZ { get { return csz; } set { csz = value; OnPropertyChanged(); } }

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

        private int? qty1 = 10000;    public int? Qty1    { get { return qty1;  } set { qty1  = value; OnPropertyChanged(); } }
        private int? qty2 = 25000;    public int? Qty2    { get { return qty2;  } set { qty2  = value; OnPropertyChanged(); } }
        private int? qty3 = 50000;    public int? Qty3    { get { return qty3;  } set { qty3  = value; OnPropertyChanged(); } }
        private int? qty4 = 100000;   public int? Qty4    { get { return qty4;  } set { qty4  = value; OnPropertyChanged(); } }

        private int? wastePct = 5; public int? WastePct {  get { return wastePct; } set {  wastePct = value; OnPropertyChanged(); } }
        private string? itemType;  public string? ItemType {  get { return itemType; } set { itemType = value; OnPropertyChanged(); LoadPage2Combos(); } }

        private int? selectedQty; public int? SelectedQty { get { return selectedQty; } set { selectedQty = value; OnPropertyChanged(); PaperCalc(); } }
        private string? calcMsg; public string? CalcMsg {  get { return calcMsg;  } set { calcMsg = value; OnPropertyChanged(); } }

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

        #endregion

        public Quotations()
        {
            InitializeComponent();
            DataContext = this;

            LoadPressSizes();
//            LoadElements();   // What was this used for?
            LoadFormTypes();
            LoadPaperTypes();
            //LoadSubWT();
            //LoadColors();
            //LoadRollWidths();

            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);

            // This also works:
            //	PreviewKeyDown += (s, e) => { if (e.Key == Key.Escape) Close(); };
        }

        private void btnLookup_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox
                .Show(" Customer Lookup screen here ");
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox
                .Show(" Do this in the Customer Lookup screen");
        }

        private void btnBaseCharge_Click(object sender, RoutedEventArgs e)
        {
            Page2.IsEnabled = true;
            Page3.IsEnabled = true;
            Page2.Focus();
            // call dgSheetsOfPaper_SelectedCellsChanged
            dgSheetsOfPaper.RaiseEvent(new RoutedEventArgs(DataGrid.SelectedEvent));

        }

        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {
            QuoteLookup ql = new();
            ql.ShowDialog();
            QUOTE_NUM = ql.SelQuote; ql.Close();
            if (QUOTE_NUM == null || QUOTE_NUM.Length == 0) return;

            GetQuote();
        }

        private void GetQuote()
        {
            SqlConnection cn = new(ConnectionString);
            if (QUOTE_NUM == null || QUOTE_NUM == "") QUOTE_NUM = txtQuoteNum.Text.ToString();
            string str = "SELECT * FROM QUOTES WHERE QUOTE_NUM = " + QUOTE_NUM;
            SqlDataAdapter da = new(str, cn);
            DataSet ds = new();
            da.Fill(ds);

            if (ds.Tables[0].Rows.Count == 0)
            { MessageBox.Show("Not found..."); return; }
            else
            {
                DataTable dt = ds.Tables[0]; DataRow dr = dt.Rows[0];
                DataRow dr3 = dt.Rows[0];
                CUST_NUM = dt.Rows[0]["CUST_NUM"].ToString();
                ProjectType = dt.Rows[0]["PROJECTTYPE"].ToString();
                FSINT1 = dt.Rows[0]["FSINT1"].ToString();  // dr3[6];
                FSFRAC1 = dt.Rows[0]["FSFRAC1"].ToString();  // dr3[7];
                FSINT2 = dt.Rows[0]["FSINT2"].ToString();  // dr3[8];
                FSFRAC2 = dt.Rows[0]["FSFRAC2"].ToString();  // dr3[9];
                PARTS = int.Parse(dt.Rows[0]["PARTS"].ToString());

                PAPERTYPE = dt.Rows[0]["PAPERTYPE"].ToString();  // dr3[9];
                ROLLWIDTH = dt.Rows[0]["ROLLWIDTH"].ToString();  // dr3[9];
                PRESSSIZE = dt.Rows[0]["PRESSSIZE"].ToString();  // dr3[9];     "PressSize.Cylinder"
                LINEHOLES = bool.Parse(dt.Rows[0]["LINEHOLES"].ToString());
                COLLATORCUT = dt.Rows[0]["COLLATORCUT"].ToString();  // dr3[9]; "PressSize.FormSize"	

                txtCustomerNum_LostFocus(this, null);
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

        private void LoadElements()
        {
            this.Elements = new DataTable("Elements");
            this.Elements.Columns.Add("SEQ");
            this.Elements.Columns.Add("ITEM");
            this.Elements.Columns.Add("PRICE");
            this.Elements.Columns.Add("MULT");
            this.Elements.Columns.Add("EXT");

            this.Elements.Rows.Add("10", "Base Charge", 1.23F, 1.00F, 2.20F);
            this.Elements.Rows.Add("11", "Order Entry", 0.00D, 0.00D, 0.00D);
            this.Elements.Rows.Add("12", "Pre Press", 0.00D, 0.00D, 0.00D);
            this.Elements.Rows.Add("13", @"Wht 11 5/8"" CB", 0.00D, 1.00D, 0.00D);
            this.Elements.Rows.Add("14", @"Can 11 5/8"" CFB", 0.00D, 1.00D, 0.00D);
            this.Elements.Rows.Add("15", @"Pink 11 5/8"" CF", 0.00D, 1.00D, 0.00D);

            this.dgElements.DataContext = this;
            this.dgElements.ItemsSource = Elements.DefaultView;
        }

        private void LoadFeatures()
        {
            this.Features = new DataTable("Features");
            this.Features.Columns.Add("SHOW");
            this.Features.Columns.Add("ITEM");

            this.Features.Rows.Add(0, "Ink Color");
            this.Features.Rows.Add(0, "Backer");
            this.Features.Rows.Add(0, "Punching");
            this.Features.Rows.Add(0, "Cross Perf");
            this.Features.Rows.Add(0, "Booking");
            this.Features.Rows.Add(0, "Fan a part");
            this.Features.Rows.Add(0, "Shrink Wrap");
            this.Features.Rows.Add(0, "Padding");

            //this.dgFeatures.DataContext = this;
            //this.dgFeatures.ItemsSource = Features.DefaultView;
        }

        private void LoadPaperTypes()
        {
            this.PaperTypes = new DataTable("PaperTypes");
            this.PaperTypes.Columns.Add("PaperType");
            this.PaperTypes.Rows.Add("BOND");
            this.PaperTypes.Rows.Add("CRBNLS");
            this.PaperTypes.Rows.Add("ELEC");

            this.cmbPaperType.DataContext = this;
            this.cmbPaperType.ItemsSource = PaperTypes.DefaultView;
            this.cmbPaperType.DisplayMemberPath = "PaperType";
            this.cmbPaperType.SelectedValuePath = "PaperType";

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
            // load distinct Cylinder values from the PressSizes table
//            string str = "SELECT DISTINCT Cylinder FROM PressSizes ORDER BY Cylinder";
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

        private void btnShowHideFeaturesPicker_Click(object sender, RoutedEventArgs e)
        {
            //pckFeatures.Visibility = (pckFeatures.Visibility == Visibility.Hidden) ? Visibility.Visible : Visibility.Hidden;
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
        {
            int CustKey = int.Parse(txtCustomerNum.Text);
            SqlConnection cn = new(ConnectionString);
            string str = "SELECT *, RTRIM(CITY) + ', ' + STATE + ' ' + ZIP AS CSZ FROM CUSTOMERS WHERE CUST_NUMB = " + CustKey.ToString();
            SqlDataAdapter da = new(str, cn);
            DataSet ds = new();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                DataRow dr = dt.Rows[0];
                CustomerName = dr[2].ToString();
                Address = dr[3].ToString();
                Phone = dr[9].ToString();
                CSZ = dr[10].ToString();
            }
            else { MessageBox.Show("No match"); }
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

        private void dgElements_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            int RowNumber = dgElements.SelectedIndex;
            int ColumnNumber = dgElements.CurrentCell.Column.DisplayIndex;
            string? rn = Elements?.Rows[RowNumber][0].ToString();
            if (rn == "10" || rn == "11" || rn == "12") return;
            if (ColumnNumber != 2) return;

            hp = new HistoricalPrices();
            hp.ShowDialog();
            float? savedPrice = hp.Price;
            hp.Close();
            if (savedPrice != null) { Elements.Rows[RowNumber][ColumnNumber] = savedPrice; }
        }

        private void HandleEsc(object sender, KeyEventArgs e) { if (e.Key == Key.Escape) Close(); }

        private void txtQuoteNum_LostFocus(object sender, RoutedEventArgs e)
        {
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
            DataTable CollCuts = new DataTable("CollCuts");
            CollCuts.Columns.Add("CutSize");

            //string cmd =
            //        "SELECT FormSize FROM PressSizes WHERE cylinder = '" + PRESSSIZE
            //    + "'  ORDER BY CASE WHEN CHARINDEX(' ', FormSize) > 0 "
            //    + "                 THEN CONVERT(int, SUBSTRING(FormSize,0, charindex(' ', FormSize))) "
            //    + "                 ELSE FormSize END";

            string cmd = "SELECT CutSize FROM CylCutSizes WHERE Press = '" + PRESSSIZE + "'";

            SqlConnection cn = new(ConnectionString); cn.Open();
            SqlDataAdapter da = new(cmd, cn);

            da.Fill(CollCuts);
            cmbCollatorCut.Items.Clear();

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
            // Load into the ComboBoxItems of cmbCollatorCut
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
            int setNum = int.Parse(PartsSpinner.Value.ToString());
            string formType  = ProjectType.Substring(0, 1);
            string paperType = PAPERTYPE.Substring(0, 1);
            string rollWidth = ROLLWIDTH.ToString();

            //string cmd =
            //     "SELECT SetNum, FormType, PaperType, Paper, Color, Weight,PType, " +
            //     "CONVERT(VarChar(20),NULL) AS Description, " +
            //     "000000 AS Pounds, " + 
            //     "00.00 AS LastPOCost, " + 
            //     "00.00 AS AverageCost, " +
            //     "00.00 AS MastInvCost, " +
            //     "00.00 AS SelectedCost, " + 
            //     "000000.00 AS PaperCost " +
            //     "FROM [ESTIMATING].[dbo].ESTPAPER " +
            //    $"WHERE PaperType='{paperType}' " +
            //    $"  AND FormType LIKE '{formType}%' " +
            //    $"  AND SetNum={setNum}";

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
                " 0.00 AS Pounds," +
                " 0.00 AS LastPOCost," +
                " 0.00 AS AverageCost," +
                "M.COSTPERFACTOR AS MastInvCost," +
                " 0.00 AS SelectedCost," +
                " 0.00 AS PaperCost" +
                            " FROM PROVISIONDEV.DBO.MasterInventory M" +
                " RIGHT OUTER JOIN PROVISIONDEV.DBO.ESTPAPER E" +
                " ON M.ITEMTYPE     = E.PTYPE"       +
                " AND M.COLOR       = E.COLOR"       +
                " AND M.SubWT       = E.WEIGHT"      +
                " WHERE E.FORMTYPE  = '"  + formType  + "'" +
                "   AND E.PAPERTYPE = '"  + paperType + "'" +
                "   AND M.SIZE      = '"  + rollWidth + "'" +
                "   AND E.SETNUM    = "   + setNum    +
                " AND Inventoryable = 1"              +
                " ORDER BY E.SETNUM, E.SEQ";

            Clipboard.SetText(cmd);
//            MessageBox.Show(cmd);
//            Debugger.Break();

            SqlConnection cn = new(ConnectionString); cn.Open();
            SqlDataAdapter da = new(cmd, cn);
            DataSet ds = new("Papers");
            da.Fill(ds);
            DataTable? dt = ds.Tables[0];

            for (int r = 0; r < dt.DefaultView.Count; r++)
            {
// These are already in the table; all we need is average and most recent cost for each paper 
                string? clr  = dt.Rows[r]["Color"].ToString();
                string? wt   = dt.Rows[r]["SubWT"].ToString();
                string? it   = dt.Rows[r]["PType"].ToString();
                it = (it == null) ? "" : it.Substring(0, 1) + "%";
                string? size = cmbRollWidth.SelectedValue.ToString();
                size = (size is null) ? "" : size.TrimEnd();

                string? projType = cmbProjectType.SelectedValue.ToString();
                projType = (projType == "SHEET") ? "SHT" : "ROLL";   // Sheet or Continuous

                //string cmd2 = "SELECT Description, CostPerFactor AS MastInvCost " +
                //    " FROM [ProVisionDev].[dbo].MasterInventory " +
                //    " WHERE Color   = '" + clr      + "'" +
                //    "  AND ProdType = '" + projType + "'" +
                //    "  AND SubWT    = '" + wt.TrimEnd()   + "'" +
                //    "  AND ItemType LIKE '" + it    + "'" +
                //    "  AND Size     = '" + size     + "'";     // TODO Should this be the next size up?

                //SqlDataAdapter da2 = new(cmd2, cn);
                //DataTable dt2 = new("Desc");
                //da2.Fill(dt2);

                //if (dt2 is null) return;

                //string? desc = "";
                //if  (dt2.Rows.Count == 0) { MessageBox.Show(cmd2, "Description not found" ); return;  } 
                //  else 
                //    { desc = dt2.Rows[0][0].ToString(); }
                //desc = (desc is null) ? null : desc.TrimEnd();

                // Examine table dt here:

                //              dt.Rows[r][4] = clr;
                //              dt.Rows[r][5] = wt.TrimEnd();
                //              dt.Rows[r][6] = it.Replace("%", "");
                //              dt.Rows[r][7] = desc;
                //              dt.Rows[r][11] = dt2.Rows[0][1];        // MasterInventory.CostPerFactor

                dt.Rows[r]["Pounds"] = (r + 1) * 1000;    // Temporary until PaperCalc is implemented

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
                    {  dt.Rows[r]["AverageCost"] = dtAvg.Rows[0][0].ToString();   // I rewrote the query to return just one column (average cost)
                       dt.Rows[r]["SelectedCost"] = dtAvg.Rows[0][0].ToString();   // SelectedCost defaults to average cost
                    }
            }

            dgSheetsOfPaper.ItemsSource = dt.DefaultView;

            // TODO: Select the first description and populate the comboboxes above the grid

            Page2.IsEnabled = true;
            Page3.IsEnabled = true;
            Page2.Focus();

            dgSheetsOfPaper.SelectedIndex = -1;     // Force loading of labels 
            dgSheetsOfPaper.SelectedIndex = 0;      //  using the first row description

        }

        private void cmbPaperType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Reload the RollWidth combobox items list
            //if (cmbPaperType.SelectedValue is null) return;
            //string? val = cmbPaperType.SelectedValue.ToString();
            //string cmd2 = "SELECT RollWidth FROM [ProVisionDev].[dbo].RollWidth " +
            //    "WHERE Abbreviation = '" + val + "'";
            //SqlConnection cn = new(ConnectionString);
            //SqlDataAdapter da2 = new(cmd2, cn);
            //DataTable dt3 = new("RT");
            //da2.Fill(dt3);
            //cmbRollWidth.Items.Clear();
            //for (int r = 0; r < dt3.Rows.Count; r++)
            //{ cmbRollWidth.Items.Add(dt3.Rows[r][0].ToString()); }
            //cmbRollWidth.SelectedIndex = 0;
//            LoadRollWidths();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgSheetsOfPaper_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            int rownum = dgSheetsOfPaper.SelectedIndex;
            SelRowIdx = rownum;             // Needed for the Update button code
            if (rownum < 0) return;
            if(Tabs.SelectedIndex  == 0) { return; }

            DataRowView dataRow = (DataRowView)dgSheetsOfPaper.SelectedItem;
            DRV = dataRow;
            if (DRV is null) return;

            // Pull out four key values and display them in editable comboboxes, above.

            string  tmpPaperType = dataRow[7].ToString().Substring(0,1); // Should this be column 10 instead of 7?
            switch (tmpPaperType) { case "B": tmpPaperType = "BOND"; break; case "C": tmpPaperType = "CRBNLS"; break; case "E": tmpPaperType = "ELEC"; break; }

            txtItemType.Text = tmpPaperType;
            txtColor.SelectedValue = dataRow[8].ToString();     // Should this set SelectedItem instead?
            txtBasis.SelectedValue = dataRow[2].ToString().TrimEnd();

            lblItemType.Content = tmpPaperType;
            lblColor.Content = dataRow[8].ToString();
            lblBasis.Content = dataRow[2].ToString().TrimEnd();
            lblPaperType.Content = dataRow[7].ToString().TrimEnd();  //  tmpPaperType;

            // RollWidth (size) is just to the right of "COLOR" in Description (dataRow[7])
            string col  = lblColor.Content.ToString();
            string Desc = dataRow[0].ToString();
            int ColorAt = Desc.IndexOf(col) + 4;
            string RW   = Desc.Substring(ColorAt);  // RollWidth

            lblRollWidth.Content = RW.ToString().TrimEnd();

            // Insert the value in the selected column into the "SelectedCost" column (#12)
            DataGridColumn chosen = dgSheetsOfPaper.CurrentColumn;

            var x = (chosen == null) ? "" : chosen.ToString();
            if (chosen != null)
              { int idx = chosen.DisplayIndex + 10;
                if (idx < 12 || idx > 14) return;    // Only three of the columns should be clickable
                dataRow[15] = dataRow[idx];         // Use the value they clicked as the "cost to use"
              }

            double a = double.Parse(dataRow[11].ToString()) * double.Parse(dataRow[15].ToString());
            dataRow[16] = a;
            //MessageBox.Show(a.ToString("C"));

            dgSheetsOfPaper.SelectedIndex = -1;

        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            // Do they want to change anything on the current row?

            if (   txtRollWidth.Text.Length == 0
                && txtPaperType.Text.Length == 0
                && txtColor.Text.Length     == 0
                && txtBasis.Text.Length     == 0)
                { MessageBox.Show("No changes to make."); return; }

            string                              WhereClause  = " WHERE ReportType = '" + PAPERTYPE + "'";
            if (txtRollWidth.Text.Length > 0) { WhereClause +=   " AND Size = '"       + lblRollWidth.Content.ToString().TrimEnd() + "'"; }
            if (txtColor.Text.Length     > 0) { WhereClause +=   " AND Color = '"      + lblColor.Content.ToString().TrimEnd() + "'"; }
            if (txtBasis.Text.Length     > 0) { WhereClause +=   " AND SubWT = '"      + lblBasis.Content.ToString().TrimEnd() + "'"; }

            //    + "   AND SubWT      = '" + lblBasis.Content.ToString().TrimEnd() + "'"
            //    + "   AND Color      = '" + lblColor.Content.ToString().TrimEnd() + "'"
            //    + "   AND Size       = '" + lblRollWidth.Content.ToString().TrimEnd() + "'";

            string cmd = "SELECT COUNT(*) FROM MasterInventory " + WhereClause;
            System.Windows.Clipboard.SetText(cmd);

            SqlConnection cn = new(ConnectionString);
            SqlDataAdapter da2 = new(cmd, cn); DataTable dt3 = new("CT"); da2.Fill(dt3);

            if (dt3.Rows.Count == 0) { MessageBox.Show("Paper not found in MasterInventory"); return; }

            // if(txtItemType.Text.TrimEnd() == "BND") { txtItemType.Text = "BOND";  }

            if (lblItemType.Content.ToString().Length > 0 && txtItemType.Text.ToString().TrimEnd().Length > 0 && lblItemType.Content != txtItemType.Text ) 
            { 
                // We have to change "BND" to "BOND", among other problems...

                //DRV[7] = DRV[7].ToString().Replace(lblPaper.Content.ToString().TrimEnd(), txtPaperType.Text.ToString().TrimEnd());
                //string p = txtPaperType.Text.ToString().TrimEnd();
                //if (p == "BOND") { p = "B"; }
                //if (p == "CRBNLS") { p = "C"; }
                //if (p == "ELEC") { p = "E"; }
                //DRV[6] = p;
                //DRV[2] = p;
            }

            if (lblPaperType.Content.ToString().Length > 0 && txtPaperType.Text.ToString().TrimEnd().Length > 0)
            {
                DRV[0] = DRV[0].ToString().Replace(lblPaperType.Content.ToString().TrimEnd(), txtPaperType.Text.ToString().TrimEnd());
                DRV[7] = txtPaperType.Text.ToString().TrimEnd();
            }

            if (lblBasis.Content.ToString().Length > 0 && txtBasis.Text.ToString().TrimEnd().Length > 0) 
            { 
                DRV[0] = DRV[0].ToString().Replace(lblBasis.Content.ToString().TrimEnd(), txtBasis.Text.ToString().TrimEnd());
                DRV[4] = DRV[4].ToString().Replace(lblBasis.Content.ToString().TrimEnd(), txtBasis.Text.ToString().TrimEnd());
                DRV[2] = txtBasis.Text.ToString().TrimEnd();
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

//        private void txtPaperType_SelectionChanged(object sender, SelectionChangedEventArgs e) { }

        private void dgSheetsOfPaper_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {   // Recalculate cost
            string? rowNum = e.Row.DataContext.ToString();
            string prop1 = (e.Row.Item as DataRowView).Row[1].ToString();
        }

        private async void dgSheetsOfPaper_CurrentCellChanged(object sender, System.EventArgs e)
        {
            txtItemType.Focus();        // TODO: How do I suggest that they pick an ItemType so that the other comboboxes will be populated?
//            txtItemType.IsDropDownOpen = true;

            Msg2.Visibility = Visibility.Visible;
            //Task delay = Task.Delay(1000);
            //await delay;
            //Msg2.Visibility = Visibility.Hidden;
        }

        private void cmbCollatorCut_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbCollatorCut.SelectedValue == null) return;
 
            lblUpMessage.Content = "Calculating...";

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
            
            lblUp.Content = float.Parse(intPart) / totval;    // remove text characters from PRESSSIZE before calculating...
            lblUpMessage.Content = "up";

        }

        private void PaperCalc()
        {
            CalcMsg = "Recalculating paper weight...";
        }

        private void ToggleMsg()
        {
            Msg1.Visibility = Visibility.Visible;
            Task.Delay(2000);
            Msg1.Visibility = Visibility.Hidden;
        }

        private void txtPaperType_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
        //    LoadSubWT();
        }

        private void txtQty1_PreviewMouseDown(object sender, MouseButtonEventArgs e) { SelectedQty = int.Parse(txtQty1.Text); Msg1.Visibility = Visibility.Visible; }
        private void txtQty2_PreviewMouseDown(object sender, MouseButtonEventArgs e) { SelectedQty = int.Parse(txtQty2.Text); Msg1.Visibility = Visibility.Visible; }
        private void txtQty3_PreviewMouseDown(object sender, MouseButtonEventArgs e) { SelectedQty = int.Parse(txtQty3.Text); Msg1.Visibility = Visibility.Visible; }
        private void txtQty4_PreviewMouseDown(object sender, MouseButtonEventArgs e) { SelectedQty = int.Parse(txtQty4.Text); Msg1.Visibility = Visibility.Visible; }

        private void txtQty1_GotFocus(object sender, RoutedEventArgs e) { txtQty1.SelectAll(); }
        private void txtQty2_GotFocus(object sender, RoutedEventArgs e) { txtQty2.SelectAll(); }
        private void txtQty3_GotFocus(object sender, RoutedEventArgs e) { txtQty3.SelectAll(); }
        private void txtQty4_GotFocus(object sender, RoutedEventArgs e) { txtQty4.SelectAll(); }

        private void cmbProjectType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadRollWidths();   // What else needs to be done when Project Type changes?
        }

        private void txtItemType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Reload the two dropdowns to the right of ItemType on Page 2
        }

        private void LoadPage2Combos()
        {
            LoadSubWT();
            LoadColors();
            LoadRollWidths();
        }

        private void txtItemType_MouseDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void txtItemType_DropDownOpened(object sender, System.EventArgs e)
        {
            Msg2.Visibility = Visibility.Hidden;
        }

        private void dgSheetsOfPaper_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
//            MessageBox.Show(e.Source.ToString());
        }

        private void cmbPaperType_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            LoadRollWidths(); 
        }

    }
}