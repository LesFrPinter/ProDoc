using System.Data;
using System.Windows;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Windows.Controls;
using System.Runtime.CompilerServices;
using System.Configuration;

namespace ProDocEstimate
{
	public partial class Quotations : Window, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler? PropertyChanged;
		protected void OnPropertyChanged([CallerMemberName] string? name = null)
		{ PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

		private string? customerName;  public string? CustomerName { get { return customerName; } set { customerName = value; OnPropertyChanged(); } }
		private string? address;       public string? Address      { get { return address;      } set { address      = value; OnPropertyChanged(); } }
		private string? city;          public string? City         { get { return city;         } set { city         = value; OnPropertyChanged(); } }
		private string? state;         public string? State        { get { return state;        } set { state        = value; OnPropertyChanged(); } }
		private string? zip;           public string? ZIP          { get { return zip;          } set { zip          = value; OnPropertyChanged(); } }
		private string? location;      public string? Location     { get { return location;     } set { location     = value; OnPropertyChanged(); } }
		private string? phone;         public string? Phone        { get { return phone;        } set { phone        = value; OnPropertyChanged(); } }
		private string? csz;					 public string? CSZ          { get { return csz;          }	set { csz          = value; OnPropertyChanged(); } }

		private string?  activePage;   public string? ActivePage   { get { return activePage;   } set { activePage   = value; OnPropertyChanged(); } }
		private decimal? qty1;         public decimal? Qty1        { get { return qty1;         } set { qty1      = value; OnPropertyChanged(); } }
		private decimal? qty2;         public decimal? Qty2        { get { return qty2;         } set { qty2      = value; OnPropertyChanged(); } }
		private decimal? qty3;         public decimal? Qty3        { get { return qty3;         } set { qty3      = value; OnPropertyChanged(); } }
		private decimal? qty4;         public decimal? Qty4        { get { return qty4;         } set { qty4      = value; OnPropertyChanged(); } }
		private decimal? qty5;         public decimal? Qty5        { get { return qty5;         } set { qty5      = value; OnPropertyChanged(); } }
		private decimal? qty6;         public decimal? Qty6        { get { return qty6;         } set { qty6      = value; OnPropertyChanged(); } }
		private DataTable? features;   public DataTable? Features  { get { return features;     } set { features  = value; OnPropertyChanged(); } }
		private DataTable? elements;   public DataTable? Elements  { get { return elements;     } set { elements  = value; OnPropertyChanged(); } }
		private DataTable? projTypes;  public DataTable? ProjTypes { get { return projTypes;    } set { projTypes = value; OnPropertyChanged(); } }

		public Quotations()  
		{ 
			InitializeComponent();
			DataContext = this;
			LoadFeatures();
			LoadElements();
			LoadProjTypes();
		}

		public string ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
		public SqlConnection cn = new SqlConnection();
		public SqlDataAdapter? da;

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
		{ System.Windows.MessageBox.Show("Copy by Quote # or by Job #", "Copy", MessageBoxButton.OK, MessageBoxImage.Question); }

		private void Tabs_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{ switch (Tabs.SelectedIndex) 
			{ case 0: ActivePage = "Base";    lblPageName.Content = "Base";    OnPropertyChanged("ActivePage"); break;
				case 1: ActivePage = "Details"; lblPageName.Content = "Details"; OnPropertyChanged("ActivePage"); break;
				case 2: ActivePage = "Pricing"; lblPageName.Content = "Pricing"; OnPropertyChanged("ActivePage"); break;
			}
		}

		private void txtCustomerNum_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{ System.Windows.MessageBox.Show("For new customers, a provisional Customer Number starting with 'P' will be created, to be replaced later", "Provisional CustNum", MessageBoxButton.OK, MessageBoxImage.Information); }

		private float CalcFraction(string frac)
		{
			string[] parts = frac.Split('/');
			if (parts.Length == 1)
			{ return 0.00F; }
			else
			{ return float.Parse(parts[0]) / float.Parse(parts[1]); }
		}

		private void LoadElements()
		{
			this.Elements = new DataTable("Elements");
			this.Elements.Columns.Add("SEQ");
			this.Elements.Columns.Add("ITEM");
			this.Elements.Columns.Add("PRICE");
			this.Elements.Columns.Add("MULT");
			this.Elements.Columns.Add("EXT");

			this.Elements.Rows.Add("10", "Base Charge",       1.23F, 1.00F, 2.20F);
			this.Elements.Rows.Add("11", "Order Entry",       0.00D, 0.00D, 0.00D);
			this.Elements.Rows.Add("12", "Pre Press",         0.00D, 0.00D, 0.00D);
			this.Elements.Rows.Add("13", @"Wht 11 5/8"" CB",  0.00D, 0.00D, 0.00D);
			this.Elements.Rows.Add("14", @"Can 11 5/8"" CFB", 0.00D, 0.00D, 0.00D);
			this.Elements.Rows.Add("15", @"Pink 11 5/8"" CF", 0.00D, 0.00D, 0.00D);

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

			this.dgFeatures.DataContext = this;
			this.dgFeatures.ItemsSource = Features.DefaultView;
		}

		private void LoadProjTypes()
		{
			this.ProjTypes = new DataTable("ProjTypes");
			this.ProjTypes.Columns.Add("TEXT");
			this.ProjTypes.Rows.Add("Continuous");
			this.ProjTypes.Rows.Add("Snap-in");
			this.cmbProjectType.DataContext = this;
			this.cmbProjectType.ItemsSource = ProjTypes.DefaultView;
			this.cmbProjectType.DisplayMemberPath = "TEXT";
			this.cmbProjectType.SelectedValuePath = "TEXT";
		}

		private void btnShowHideFeaturesPicker_Click(object sender, RoutedEventArgs e)
		{
			dgFeatures.Visibility = (dgFeatures.Visibility == Visibility.Hidden)? Visibility.Visible: Visibility.Hidden;
		}

		private void cmbFinalSizeFrac1_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{ //string x = ((sender as ComboBox).SelectedItem as ComboBoxItem).Content as string;
			string controlname = (sender as ComboBox).Name;

			int Int1 = 0; int Int2 = 0;
			float Frac1 = 0.00F; float Frac2 = 0.00F;

			if ((cmbFinalSizeInches1.SelectedItem as ComboBoxItem) != null)
			{ string Str1 = (cmbFinalSizeInches1.SelectedItem as ComboBoxItem).Content.ToString(); Int1 = int.Parse(Str1);  }

			if ((cmbFinalSizeFrac1.SelectedItem as ComboBoxItem) != null)
			{ string x =    (cmbFinalSizeFrac1.SelectedItem as ComboBoxItem).Content.ToString();   Frac1 = CalcFraction(x); }

			if ((cmbFinalSizeInches2.SelectedItem as ComboBoxItem) != null) 
			{ string Str2 = (cmbFinalSizeInches2.SelectedItem as ComboBoxItem).Content.ToString(); Int2 = int.Parse(Str2);  }

			if ((cmbFinalSizeFrac2.SelectedItem as ComboBoxItem) != null)
			{ string x =    (cmbFinalSizeFrac2.SelectedItem as ComboBoxItem).Content.ToString();   Frac2 = CalcFraction(x); }

			if (controlname.Substring(controlname.Length - 1) == "1")
				{ Decimal1.Content = Frac1 + Int1; }
			else
				{ Decimal2.Content = Frac2 + Int2; }
		}

		private void txtCustomerNum_LostFocus(object sender, RoutedEventArgs e)
		{
			int CustKey = int.Parse(txtCustomerNum.Text);
			SqlConnection cn = new SqlConnection(ConnectionString);
			string str = "SELECT *, RTRIM(CITY) + ', ' + STATE + ' ' + ZIP AS CSZ FROM CRC_CU10 WHERE CUST_NUMB = " + CustKey.ToString();
			da = new SqlDataAdapter(str, cn);
			DataSet ds = new DataSet();
			da.Fill(ds);
			if (ds.Tables[0].Rows.Count > 0 )
			{
				DataTable dt = ds.Tables[0];
				DataRow dr = dt.Rows[0];
				CustomerName = dr[2].ToString();
				Address = dr[3].ToString();
				//City = dr[5].ToString();
				//State = dr[6].ToString();
				//ZIP = dr[7].ToString();
				Phone = dr[9].ToString();
				CSZ = dr[10].ToString();
			}
			else
			{ MessageBox.Show("No match"); }
		}

	}
}