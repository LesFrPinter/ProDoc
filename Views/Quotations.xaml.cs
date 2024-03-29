﻿using ProDocEstimate.Views;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ProDocEstimate
{
    public partial class Quotations : Window, INotifyPropertyChanged {
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

		private string? fsint1;  public string? FSINT1  { get { return fsint1;  } set { fsint1  = value; OnPropertyChanged(); } }
		private string? fsfrac1; public string? FSFRAC1 { get { return fsfrac1; } set { fsfrac1 = value; OnPropertyChanged(); } }
		private string? fsint2;  public string? FSINT2  { get { return fsint2;  } set { fsint2  = value; OnPropertyChanged(); } }
		private string? fsfrac2; public string? FSFRAC2 { get { return fsfrac2; } set { fsfrac2 = value; OnPropertyChanged(); } }
		private int?    parts;   public int? PARTS      { get { return parts;   } set { parts   = value; OnPropertyChanged(); } }

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

		private DataTable? features;   public DataTable? Features { get { return features; } set { features = value; OnPropertyChanged(); } }
		private DataTable? elements;   public DataTable? Elements { get { return elements; } set { elements = value; OnPropertyChanged(); } }
		private DataTable? projTypes;  public DataTable? ProjTypes { get { return projTypes; } set { projTypes = value; OnPropertyChanged(); } }

		private DataTable? papertypes; public DataTable? PaperTypes { get { return papertypes; } set { papertypes = value; OnPropertyChanged(); } }
		private DataTable? rollwidths; public DataTable? RollWidths { get { return rollwidths; } set { rollwidths = value; OnPropertyChanged(); } }
		private float mult; public float MULT { get { return mult; } set { mult = value; OnPropertyChanged(); } }

		#endregion

		public Quotations() {
			InitializeComponent();
			DataContext = this;
			LoadPressSizes();
			LoadElements();
			LoadProjTypes();
			LoadPaperTypes();
			LoadRollWidths();

			this.PreviewKeyDown += new KeyEventHandler(HandleEsc);

			// This also works:
			//	PreviewKeyDown += (s, e) => { if (e.Key == Key.Escape) Close(); };
		}

		private void btnLookup_Click(object sender, RoutedEventArgs e) {
			System.Windows.MessageBox
				.Show(" Customer Lookup screen here ");
		}

		private void btnNew_Click(object sender, RoutedEventArgs e) {
			System.Windows.MessageBox
				.Show(" Do this in the Customer Lookup screen");
		}

		private void btnBaseCharge_Click(object sender, RoutedEventArgs e) {
			Page2.IsEnabled = true;
			Page3.IsEnabled = true;
			Page2.Focus();
		}

		private void btnCopy_Click(object sender, RoutedEventArgs e) {
			QuoteLookup ql = new();
			ql.ShowDialog();
			QUOTE_NUM = ql.SelQuote; ql.Close();
			if (QUOTE_NUM == null || QUOTE_NUM.Length == 0) return;

			// Retrieve selected quote and populate bindings

			GetQuote();
		}

		private void GetQuote() {

			SqlConnection cn = new(ConnectionString);
			if (QUOTE_NUM == null || QUOTE_NUM == "") QUOTE_NUM = txtQuoteNum.Text.ToString();
			string str = "SELECT * FROM QUOTES WHERE QUOTE_NUM = " + QUOTE_NUM;
			SqlDataAdapter da = new(str, cn);
			DataSet ds = new();
			da.Fill(ds);

			if (ds.Tables[0].Rows.Count == 0)
				{ MessageBox.Show("Not found..."); return; }
			else {
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
			{ switch (Tabs.SelectedIndex) 
				{ case 0: ActivePage = "Base";    lblPageName.Content = "Base"; OnPropertyChanged("ActivePage"); break;
				  case 1: ActivePage = "Details"; lblPageName.Content = "Details"; OnPropertyChanged("ActivePage"); break;
				  case 2: ActivePage = "Pricing"; lblPageName.Content = "Pricing"; OnPropertyChanged("ActivePage"); break;
				}
			}
	
		private void txtCustomerNum_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e) 
			{ System.Windows.MessageBox.Show("For new customers, a provisional Customer Number starting with 'P' will be created, to be replaced later", "Provisional CustNum", MessageBoxButton.OK, MessageBoxImage.Information); 
			}

		private static float CalcFraction(string frac) {
			string[] parts = frac.Split('/');
			if (parts.Length == 1) { return 0.00F; }
			else { return float.Parse(parts[0]) / float.Parse(parts[1]); }
		}

		private void LoadElements() {
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

		private void LoadFeatures() {
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

		private void LoadProjTypes() {
			this.ProjTypes = new DataTable("ProjTypes");
			this.ProjTypes.Columns.Add("TEXT");
			this.ProjTypes.Rows.Add("");
			this.ProjTypes.Rows.Add("Continuous");
			this.ProjTypes.Rows.Add("Snap");
			this.ProjTypes.Rows.Add("Cut Sheet");
			this.ProjTypes.Rows.Add("RX");
			this.ProjTypes.Rows.Add("Ballot");
			this.ProjTypes.Rows.Add("Imprint");
			this.ProjTypes.Rows.Add("Other");
			this.cmbProjectType.DataContext = this;
			this.cmbProjectType.ItemsSource = ProjTypes.DefaultView;
			this.cmbProjectType.DisplayMemberPath = "TEXT";
			this.cmbProjectType.SelectedValuePath = "TEXT";
		}

		private void LoadPressSizes()
		{
			// actually, load distinct Cylinder values from the PressSizes table
			string str = "SELECT DISTINCT Cylinder FROM PressSizes ORDER BY Cylinder";
            SqlConnection cn = new(ConnectionString); cn.Open();
            if (cn.State != ConnectionState.Open)
            { MessageBox.Show("Couldn't open SQL Connection"); return; }
            SqlDataAdapter da = new(str, cn);
            DataSet ds = new("PressSizes"); da.Fill(ds); DataTable dt = ds.Tables[0];
			for (int r=0; r < dt.Rows.Count; r++)
			{ cmbPressSize.Items.Add(dt.Rows[r][0].ToString()); }
        }

        private void LoadPaperTypes() {
			SqlConnection cn = new(ConnectionString);
			cn.Open();
			if(cn.State != ConnectionState.Open)
            { MessageBox.Show("Couldn't open SQL Connection"); return; }
			string str = "SELECT PaperType FROM PaperTypes"; SqlDataAdapter da = new(str, cn);
			DataSet ds = new("PaperTypes"); da.Fill(ds); DataTable dt = ds.Tables[0];
			cmbPaperType.ItemsSource = dt.DefaultView;
		}

		private void LoadRollWidths() {
			SqlConnection cn2 = new(ConnectionString);
			string str2 = "SELECT RollWidth FROM RollWidths"; SqlDataAdapter da2 = new(str2, cn2);
			DataSet ds2 = new("dsRollWidths"); da2.Fill(ds2); DataTable dt = ds2.Tables[0];
			cmbRollWidth.ItemsSource = dt.DefaultView;
		}

		private void btnShowHideFeaturesPicker_Click(object sender, RoutedEventArgs e) {
			pckFeatures.Visibility = (pckFeatures.Visibility == Visibility.Hidden) ? Visibility.Visible : Visibility.Hidden;

		}

		private void cmbFinalSizeFrac1_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) 
		{
			string? controlname = (sender as ComboBox)?.Name;

			int Int1 = 0; int Int2 = 0;
			float Frac1 = 0.00F; float Frac2 = 0.00F;

			if ((cmbFinalSizeInches1.SelectedItem as ComboBoxItem) != null) { 
				string? Str1 = (cmbFinalSizeInches1.SelectedItem as ComboBoxItem)?.Content.ToString(); 
				Int1 = int.Parse(Str1); 
			}

			if ((cmbFinalSizeFrac1.SelectedItem as ComboBoxItem) != null) { 
				string? x = (cmbFinalSizeFrac1.SelectedItem as ComboBoxItem)?.Content.ToString(); 
				Frac1 = CalcFraction(x); 
			}

			if ((cmbFinalSizeInches2.SelectedItem as ComboBoxItem) != null) { 
				string? Str2 = (cmbFinalSizeInches2.SelectedItem as ComboBoxItem)?.Content.ToString(); 
				Int2 = int.Parse(Str2); 
			}

			if ((cmbFinalSizeFrac2.SelectedItem as ComboBoxItem) != null) { 
				string? x = (cmbFinalSizeFrac2.SelectedItem as ComboBoxItem)?.Content.ToString(); 
				Frac2 = CalcFraction(x); 
			}

			if (controlname?.Substring(controlname.Length - 1) == "1") 
						{ Decimal1.Content = Frac1 + Int1; }
			 else { Decimal2.Content = Frac2 + Int2; }
		}

		private void txtCustomerNum_LostFocus(object sender, RoutedEventArgs e) {
			int CustKey = int.Parse(txtCustomerNum.Text);
			SqlConnection cn = new(ConnectionString);
			string str = "SELECT *, RTRIM(CITY) + ', ' + STATE + ' ' + ZIP AS CSZ FROM CUSTOMERS WHERE CUST_NUMB = " + CustKey.ToString();
			SqlDataAdapter da = new(str, cn);
			DataSet ds = new();
			da.Fill(ds);
			if (ds.Tables[0].Rows.Count > 0) {
				DataTable dt = ds.Tables[0];
				DataRow dr = dt.Rows[0];
				CustomerName = dr[2].ToString();
				Address = dr[3].ToString();
				Phone = dr[9].ToString();
				CSZ = dr[10].ToString();
			}
			else { MessageBox.Show("No match"); }
		}

		private void dgElements_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e) { if (sender == null) return;
			var dg = sender as DataGrid;
			var index = dg.SelectedIndex;
			if (index < 0) return;
			string? Seq = Elements?.Rows[index][0].ToString();
			if (Seq == "10" || Seq == "11" || Seq == "12") {
				txtPaper.Text = "";
				txtColor.Text = "";
				txtBasis.Text = "";
				MULT = 1.00F;
				return;
			}
			string? cell = Elements?.Rows[index][1].ToString();
			if (cell != null) { string[] cells = cell.Split(" ");
				txtPaper.Text = cells[1].ToString() + " " + cells[2].ToString();
				txtColor.Text = cells[0].ToString();
				txtBasis.Text = cells[3].ToString();
				MULT = float.Parse(Elements.Rows[index][3].ToString());
			}
		}

		private void dgElements_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e) {
			int RowNumber = dgElements.SelectedIndex;
			int ColumnNumber = dgElements.CurrentCell.Column.DisplayIndex;
			string? rn = Elements?.Rows[RowNumber][0].ToString();
			if ( rn == "10" || rn == "11" || rn == "12" ) return;
			if (ColumnNumber != 2) return;

			hp = new HistoricalPrices();
			hp.ShowDialog();
			float? savedPrice = hp.Price;
			hp.Close();
			if (savedPrice != null) { Elements.Rows[RowNumber][ColumnNumber] = savedPrice; }
		}

		private void HandleEsc(object sender, KeyEventArgs e) { if (e.Key == Key.Escape) Close(); }

		private void txtMult_LostFocus(object sender, RoutedEventArgs e) {
			var index = dgElements.SelectedIndex;
			if (index >= 0) { MULT = float.Parse(txtMult.Text);
				Elements.Rows[index][3] = MULT.ToString();
				float Price = float.Parse(Elements.Rows[index][2].ToString());
				float Extended = MULT * Price;
				Elements.Rows[index][4] = Extended.ToString();
			}
		}

		private void txtQuoteNum_LostFocus(object sender, RoutedEventArgs e) 
			{
				GetQuote();
				//txtCustomerNum_LostFocus(this, null);
			}

		private void Qty1b_LostFocus(object sender, RoutedEventArgs e) {
				if( (QTY1a!=null && QTY1a!=0) && (CPM1a != null && CPM1a != 0) & (MAR1a != null && MAR1a != 0) )  
				{ float? result = QTY1a * CPM1a * MAR1a; EXT1a = EXT1a = result / 1000.00F; }
		}

		private void Qty2b_LostFocus(object sender, RoutedEventArgs e) {
			if ((QTY2a != null && QTY2a != 0) && (CPM2a != null && CPM2a != 0) & (MAR2a != null && MAR2a != 0)) { float? result = QTY2a * CPM2a * MAR2a; EXT2a = EXT2a = result / 1000.00F; }
		}

		private void Qty3b_LostFocus(object sender, RoutedEventArgs e) {
			if ((QTY3a != null && QTY3a != 0) && (CPM3a != null && CPM3a != 0) & (MAR3a != null && MAR3a != 0)) { float? result = QTY3a * CPM3a * MAR3a; EXT3a = EXT3a = result / 1000.00F; }
		}

		private void Qty4b_LostFocus(object sender, RoutedEventArgs e) {
			if ((QTY4a != null && QTY4a != 0) && (CPM4a != null && CPM4a != 0) & (MAR4a != null && MAR4a != 0)) { float? result = QTY4a * CPM4a * MAR4a; EXT4a = EXT4a = result / 1000.00F; }
		}

		private void Markup1_GotFocus(object sender, RoutedEventArgs e) {
			Markup1.SelectAll();
		}

		private async void SelectAll_OnTextBoxGotFocus(object sender, RoutedEventArgs e) {
			await Application.Current.Dispatcher.InvokeAsync((sender as TextBox).SelectAll);
		}

        private void cmbPressSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string cmd = 
			  "select FormSize from PressSizes where cylinder = '" + cmbPressSize.SelectedValue.ToString()
			+ "' order by case when charindex(' ', FormSize) > 0 "
			+ "then convert(int, substring(FormSize,0, charindex(' ', FormSize))) else FormSize end";
            // Load into the ComboBoxItems of cmbCollatorCut
            SqlConnection cn = new(ConnectionString); cn.Open();
            if (cn.State != ConnectionState.Open)
            { MessageBox.Show("Couldn't open SQL Connection"); return; }
            SqlDataAdapter da = new(cmd, cn);
            DataSet ds = new("CollatorCuts"); da.Fill(ds); DataTable dt = ds.Tables[0];
			cmbCollatorCut.Items.Clear();

            for (int r = 0; r < dt.DefaultView.Count; r++)
			  { cmbCollatorCut.Items.Add(dt.DefaultView[r][0].ToString()); }
			cmbCollatorCut.SelectedIndex = 0;
        }

        private void btnPressCalc_Click(object sender, RoutedEventArgs e)
        {
			PressCalc pc = new();
			pc.ShowDialog();
        }
    }
}