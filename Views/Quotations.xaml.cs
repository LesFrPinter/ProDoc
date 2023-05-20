using Microsoft.EntityFrameworkCore.Internal;
using ProDocEstimate.Views;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Telerik.Windows.Controls.Charting;

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

        private string? papertype; public string? PAPERTYPE { get { return papertype; } set { papertype = value; OnPropertyChanged(); } }
        private string? rollwidth; public string? ROLLWIDTH { get { return rollwidth; } set { rollwidth = value; OnPropertyChanged(); } }
        private string? presssize; public string? PRESSSIZE { get { return presssize; } set { presssize = value; OnPropertyChanged(); } }
        private bool? lineholes; public bool? LINEHOLES { get { return lineholes; } set { lineholes = value; OnPropertyChanged(); } }
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

        private DataTable?   features; public DataTable? Features   { get { return features;   } set { features   = value; OnPropertyChanged(); } }
        private DataTable?   elements; public DataTable? Elements   { get { return elements;   } set { elements   = value; OnPropertyChanged(); } }
        private DataTable?  projTypes; public DataTable? ProjTypes  { get { return projTypes;  } set { projTypes  = value; OnPropertyChanged(); } }

        private DataTable? papertypes; public DataTable? PaperTypes { get { return papertypes; } set { papertypes = value; OnPropertyChanged(); } }
        private DataTable? rollwidths; public DataTable? RollWidths { get { return rollwidths; } set { rollwidths = value; OnPropertyChanged(); } }
        private DataTable?     colors; public DataTable? Colors     { get { return colors;     } set { colors     = value; OnPropertyChanged(); } }

        private float mult; public float MULT { get { return mult; } set { mult = value; OnPropertyChanged(); } }

        #endregion

        public Quotations()
        {
            InitializeComponent();
            DataContext = this;
            LoadPressSizes();
            LoadElements();
            LoadProjTypes();
            LoadPaperTypes();  // hard-coded to three values for now
            LoadColors();

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
        }

        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {
            QuoteLookup ql = new();
            ql.ShowDialog();
            QUOTE_NUM = ql.SelQuote; ql.Close();
            if (QUOTE_NUM == null || QUOTE_NUM.Length == 0) return;

            // Retrieve selected quote and populate bindings

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

            this.txtPaperType.DataContext = this;
            this.txtPaperType.ItemsSource = PaperTypes.DefaultView;
            this.txtPaperType.DisplayMemberPath = "PaperType";
            this.txtPaperType.SelectedValuePath = "PaperType";
        }

        private void LoadColors()
        {
            //Colors = new DataTable("Colors");
            //Colors.Columns.Add("Color");
            SqlConnection cn = new(ConnectionString);

            string str = "SELECT Color FROM Colors ORDER BY Color";
            SqlDataAdapter da = new(str, cn);
            DataTable Colors = new("Colors");
            da.Fill(Colors);

            // Did this break the row extraction routines on Page 2?

            txtColor.Items.Clear();
            for(int i=0; i< Colors.Rows.Count; i++)
            { txtColor.Items.Add(Colors.Rows[i][0].ToString().TrimEnd()); }

            //txtColor.DataContext = this;
            //txtColor.ItemsSource = Colors.DefaultView;
            //txtColor.DisplayMemberPath = "Color";
            //txtColor.SelectedValuePath = "Color";
        }

        private void LoadProjTypes()
        {
            cmbProjectType.Items.Add("SHEET");              // SHT
            cmbProjectType.Items.Add("CONTINUOUS");     // ROLL
        }

        private void LoadPressSizes()
        {
            // load distinct Cylinder values from the PressSizes table
            string str = "SELECT DISTINCT Cylinder FROM PressSizes ORDER BY Cylinder";
            SqlConnection cn = new(ConnectionString);
            cn.Open();
            if (cn.State != ConnectionState.Open)
            { MessageBox.Show("Couldn't open SQL Connection"); return; }
            SqlDataAdapter da = new(str, cn);
            DataSet ds = new("PressSizes"); da.Fill(ds); DataTable dt = ds.Tables[0];
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
            string formType = ProjectType.Substring(0, 1);
            string paperType = PAPERTYPE.Substring(0, 1);

            string cmd =
                 "SELECT SetNum, FormType, PaperType, Paper, Color, Weight,PType, " +
                 "CONVERT(VarChar(20),NULL) AS Description, " +
                 "000000 AS Pounds, " + 
                 "00.00 AS LastPOCost, " + 
                 "00.00 AS AverageCost, " +
                 "00.00 AS MastInvCost, " +
                 "00.00 AS SelectedCost, " + 
                 "000000.00 AS PaperCost " +
                 "FROM [ESTIMATING].[dbo].ESTPAPER " +
                $"WHERE PaperType='{paperType}' " +
                $"  AND FormType LIKE '{formType}%' " +
                $"  AND SetNum={setNum}";

            SqlConnection cn = new(ConnectionString); cn.Open();
            SqlDataAdapter da = new(cmd, cn);
            DataSet ds = new("Papers");
            da.Fill(ds);
            DataTable dt = ds.Tables[0];

            for (int r = 0; r < dt.DefaultView.Count; r++)
            {
                string clr  = dt.Rows[r]["Color"].ToString();
                string wt   = dt.Rows[r]["Weight"].ToString();
                string it   = dt.Rows[r]["PType"].ToString().Substring(0, 1) + "%";
                string size = cmbRollWidth.SelectedValue.ToString();

                string projType = cmbProjectType.SelectedValue.ToString();
                projType = (projType == "SHEET") ? "SHT" : "ROLL";   // Sheet or Continuous

                string cmd2 = "SELECT Description, CostPerFactor AS MastInvCost " +
                    " FROM [ProVisionDev].[dbo].MasterInventory " +
                    " WHERE Color = '" + clr + "'" +
                    "  AND ProdType = '" + projType + "'" +
                    "  AND SubWT = '" + wt + "'" +
                    "  AND LEFT(ItemType,1) = '" + cmbPaperType.SelectedValue.ToString().Substring(0, 1) + "'" +
                    "  AND Size  = '" + size + "'";

                SqlDataAdapter da2 = new(cmd2, cn);
                DataTable dt2 = new("Desc");

                da2.Fill(dt2);

                string desc = "";

                if (dt2.Rows.Count == 0)
                { desc = "Not found"; }
                else
                { 
                  desc = dt2.Rows[0][0].ToString().TrimEnd();
                }


                dt.Rows[r][4] = clr;
                dt.Rows[r][5] = wt;
                dt.Rows[r][6] = it.Replace("%", "");
                dt.Rows[r][7] = desc;
                dt.Rows[r]["Pounds"] = (r+1) * 1000;    // Temporary until PaperCalc is implemented
                dt.Rows[r][11] = dt2.Rows[0][1];        // MasterInventory.CostPerFactor

                // Get the average and most recent costs for this paper type
                SqlDataAdapter da4 = new SqlDataAdapter("MostRecentCost", ConnectionString);
                da4.SelectCommand.CommandType = CommandType.StoredProcedure;
                da4.SelectCommand.Parameters.Add("@Description", SqlDbType.VarChar, 50).Value = desc; 
                DataTable dtRec = new("Rec");
                da4.Fill( dtRec);
                if (dtRec.Rows.Count > 0)
                { 
                    dt.Rows[r][9] = dtRec.Rows[0][0].ToString();
                }

                SqlDataAdapter da5 = new SqlDataAdapter("AverageCost", ConnectionString);
                da5.SelectCommand.CommandType = CommandType.StoredProcedure;
                da5.SelectCommand.Parameters.Add("@Description", SqlDbType.VarChar, 50).Value = desc;
                DataTable dtAvg = new("Avg");
                da5.Fill(dtAvg);
                if(dtAvg.Rows.Count>0) 
                { 
                    dt.Rows[r][10] = dtAvg.Rows[0][0].ToString();   // I rewrote the query to return just one column (average cost)
                    dt.Rows[r][12] = dtAvg.Rows[0][0].ToString();   // SelectedCost defaults to average cost
                }

            }

            dgSheetsOfPaper.ItemsSource = dt.DefaultView;

            Page2.IsEnabled = true;
            Page3.IsEnabled = true;
            Page2.Focus();

        }

        private void cmbPaperType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Reload the RollWidth combobox items list
            if (cmbPaperType.SelectedValue is null) return;
            string? val = cmbPaperType.SelectedValue.ToString();
            string cmd2 = "SELECT RollWidth FROM [ProVisionDev].[dbo].RollWidth " +
                "WHERE Abbreviation = '" + val + "'";
            SqlConnection cn = new(ConnectionString);
            SqlDataAdapter da2 = new(cmd2, cn);
            DataTable dt3 = new("RT");
            da2.Fill(dt3);
            cmbRollWidth.Items.Clear();
            for (int r = 0; r < dt3.Rows.Count; r++)
            { cmbRollWidth.Items.Add(dt3.Rows[r][0].ToString()); }
              cmbRollWidth.SelectedIndex = 0;
        }

        private void dgSheetsOfPaper_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            int rownum = dgSheetsOfPaper.SelectedIndex;
            if (rownum < 0) return; 

            DataRowView dataRow = (DataRowView)dgSheetsOfPaper.SelectedItem;

            // Pull out four key values and display them in editable comboboxes, above.

            string tmpPaperType = dataRow[2].ToString();
            switch (tmpPaperType) { case "B": tmpPaperType = "BOND"; break; case "C": tmpPaperType = "CRBNLS"; break; case "E": tmpPaperType = "ELEC"; break; }
            txtPaperType.Text = tmpPaperType;
            txtBasis.Text = dataRow[5].ToString();
            txtColor.Text = dataRow[4].ToString();
            txtRollWidth.Text = cmbRollWidth.Text.ToString();  // dataRow[4].ToString();

            // Insert the value in the selected column into the "SelectedCost" column (#12)
            DataGridColumn chosen = dgSheetsOfPaper.CurrentColumn;

            var x = (chosen == null) ? "" : chosen.ToString();
            if (chosen != null)
            { 
                int idx = chosen.DisplayIndex + 7;

                if (idx < 9 || idx > 11) return;    // Which three columns are the ones to use
                dataRow[12] = dataRow[idx];
//                dgSheetsOfPaper[idx]
            }

            double a = double.Parse(dataRow[8].ToString()) * double.Parse(dataRow[12].ToString());
            dataRow[13] = a;

            dgSheetsOfPaper.SelectedIndex = -1;

        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            // Create a new Description field, then verify that a matching "Item" field exists in the MasterInventory table.
            // MessageBox.Show("No matching record in inventory")
            // TODO: What do I do if there IS a matching record?

            string WhereClause =
                  " WHERE ReportType = '" + txtPaperType.Text.TrimEnd() + "'"
                + "  AND SubWT      = '" + txtBasis.Text.TrimEnd() + "'"
                + "  AND Color      = '" + txtColor.Text.TrimEnd() + "'"
                + "  AND Size       = '" + txtRollWidth.Text + "'";

            string cmd = "SELECT COUNT(*) FROM MasterInventory " + WhereClause;

            //MessageBox.Show(cmd);
            //System.Windows.Clipboard.SetText(cmd);
            //return;

            SqlConnection cn = new(ConnectionString);
            SqlDataAdapter da2 = new(cmd, cn); DataTable dt3 = new("CT"); da2.Fill(dt3);
            if (dt3.Rows.Count == 0) { MessageBox.Show("Record not found in MasterInventory"); return; }

            MessageBox.Show("What now?");

            //cmd = "UPDATE MasterInventory SET "
            //    + " ReportType = '" + txtPaperType.Text.TrimEnd() + "'"
            //    + " SubWT      = '" + txtBasis.Text.TrimEnd() + "'"
            //    + " Color      = '" + txtColor.Text.TrimEnd() + "'"
            //    + " Size       = '" + txtRollWidth.Text + "'"
            //    + WhereClause;
            //try { 
            //    cn.Open();
            //    SqlCommand sc = new(cmd, cn); sc.ExecuteNonQuery();
            //    if (dt3.Rows.Count == 0) { MessageBox.Show("Record not found in inventory"); return; }
            //}
            //catch ( SqlException ex )
            //{
            //    MessageBox.Show(ex.Message, "Error trying to update row in MasterInventory", MessageBoxButton.OK, MessageBoxImage.Stop);
            //}

        }

        private void txtPaperType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Reload the BasisWt (subWT) and RollWidth combobox items lists
            if (cmbPaperType.SelectedValue is null) return;
            string? val = cmbPaperType.SelectedValue.ToString();

            string cmd2 = "SELECT RollWidth FROM [ProVisionDev].[dbo].RollWidth WHERE Abbreviation = '" + val + "'";
            SqlConnection cn = new(ConnectionString);
            SqlDataAdapter da2 = new(cmd2, cn); DataTable dt3 = new("RT"); da2.Fill(dt3);
            txtRollWidth.Items.Clear();
            for (int r = 0; r < dt3.Rows.Count; r++)
            { txtRollWidth.Items.Add(dt3.Rows[r][0].ToString()); }
            txtRollWidth.SelectedIndex = 0;

            string cmd4 = "SELECT DISTINCT SUBWT FROM [ProVisionDev].[dbo].MasterInventory WHERE ReportType = '" + val + "' ORDER BY SUBWT";
            SqlConnection  cn4 = new(ConnectionString);
            SqlDataAdapter da4 = new(cmd4, cn); DataTable dt4 = new("WT"); da4.Fill(dt4);
            txtBasis.Items.Clear();
            for (int r = 0; r < dt4.Rows.Count; r++) { txtBasis.Items.Add(dt4.Rows[r][0].ToString().TrimEnd()); }
            txtRollWidth.SelectedIndex = 0;

        }

        private void dgSheetsOfPaper_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            // Recalculate cost
            string? rowNum = e.Row.DataContext.ToString();
            string prop1 = (e.Row.Item as DataRowView).Row[1].ToString();

        }

        private void dgSheetsOfPaper_CurrentCellChanged(object sender, System.EventArgs e)
        {
//          dgSheetsOfPaper.Columns[e.ColumnIndex].DefaultCellStyle.BackColor = Color.Aqua;
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

            lblUp.Content = float.Parse(PRESSSIZE) / totval;
            lblUpMessage.Content = "up";

        }
    }
}