using ProDocEstimate.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Telerik.Windows.Controls;

namespace ProDocEstimate
{
    public partial class Quotations : Window, INotifyPropertyChanged
    {
        #region Property Declarations

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

        public string ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public SqlConnection? conn;
        public SqlDataAdapter? da;
        public DataTable? dt;
        public SqlCommand? scmd;

        private DataView dvFeatr; public DataView DVFeat { get { return dvFeatr; } set { dvFeatr = value; OnPropertyChanged(); } }

        // new properties for the table on page 4
        //private float featureCost;        public float FeatureCost          { get { return featureCost;         } set { featureCost        = value; OnPropertyChanged(); } }
        //private float prePressCost;       public float PrePressCost         { get { return prePressCost;        } set { prePressCost       = value; OnPropertyChanged(); } }
        //private float pressCost;          public float PressCost            { get { return pressCost;           } set { pressCost          = value; OnPropertyChanged(); } }
        //private float convertingCost;     public float ConvertingCost       { get { return convertingCost;      } set { convertingCost     = value; OnPropertyChanged(); } }
        //private float finishingCost;      public float FinishingCost        { get { return finishingCost;       } set { finishingCost      = value; OnPropertyChanged(); } }

        private float totalMaterials;       public float TotalMaterials       { get { return totalMaterials;      } set { totalMaterials     = value; OnPropertyChanged(); } }
        private float totalLabor;           public float TotalLabor           { get { return totalLabor;          } set { totalLabor         = value; OnPropertyChanged(); } }
        private float totalTotal;           public float TotalTotal           { get { return totalTotal;          } set { totalTotal         = value; OnPropertyChanged(); } }

        private float pressSlowdown;        public float PressSlowdown    { get { return pressSlowdown;    } set { pressSlowdown    = value; OnPropertyChanged(); } }
        private float collatorSlowdown;     public float CollatorSlowdown { get { return collatorSlowdown; } set { collatorSlowdown = value; OnPropertyChanged(); } }
        private float binderySlowdown;      public float BinderySlowdown  { get { return binderySlowdown;  } set { binderySlowdown  = value; OnPropertyChanged(); } }

        private float adjPressRunTime;      public float AdjPressRunTime  { get { return adjPressRunTime;  } set { adjPressRunTime  = value; OnPropertyChanged(); } }
        private float adjPressRunCost;      public float AdjPressRunCost  { get { return adjPressRunCost;  } set { adjPressRunCost  = value; OnPropertyChanged(); } }
        private float adjCollRunTime;       public float AdjCollRunTime   { get { return adjCollRunTime;   } set { adjCollRunTime   = value; OnPropertyChanged(); } }
        private float adjCollRunCost;       public float AdjCollRunCost   { get { return adjCollRunCost;   } set { adjCollRunCost   = value; OnPropertyChanged(); } }
        private float adjBindRunTime;       public float AdjBindRunTime   { get { return adjBindRunTime;   } set { adjBindRunTime   = value; OnPropertyChanged(); } }
        private float adjBindRunCost;       public float AdjBindRunCost   { get { return adjBindRunCost;   } set { adjBindRunCost   = value; OnPropertyChanged(); } }

        private string noChgMsg;            public string NoChgMsg { get { return noChgMsg; } set { noChgMsg = value; OnPropertyChanged(); } }

        private string numWidePress = "1";  public string NumWidePress { get { return numWidePress; } set { numWidePress = value; OnPropertyChanged(); } }
        private string numWideConverter = "1"; public string NumWideConverter { get { return numWideConverter; } set { numWideConverter = value; OnPropertyChanged(); } }

        private float finishMaterial;       public float FinishMaterial       { get { return finishMaterial;        } set { finishMaterial       = value; OnPropertyChanged(); } }
        private float prePressPerMilChg;    public float PrePressPerMilChg    { get { return prePressPerMilChg;     } set { prePressPerMilChg    = value; OnPropertyChanged(); } }
        private float pressMaterialCost;    public float PressMaterialCost    { get { return pressMaterialCost;     } set { pressMaterialCost    = value; OnPropertyChanged(); } }
        private float collatorMaterialCost; public float CollatorMaterialCost { get { return collatorMaterialCost;  } set { collatorMaterialCost = value; OnPropertyChanged(); } }
        private float binderyMaterialCost;  public float BinderyMaterialCost  { get { return binderyMaterialCost;   } set { binderyMaterialCost  = value; OnPropertyChanged(); } }
        private float prePressMaterialCost; public float PrePressMaterialCost { get { return prePressMaterialCost;  } set { prePressMaterialCost = value; OnPropertyChanged(); } }
        private float oeMaterialCost;       public float OEMaterialCost       { get { return oeMaterialCost;        } set { oeMaterialCost       = value; OnPropertyChanged(); } }
        private float shippingMaterialCost; public float ShippingMaterialCost { get { return shippingMaterialCost;  } set { shippingMaterialCost = value; OnPropertyChanged(); } }

        private bool already; public bool Already { get { return already; } set { already = value; OnPropertyChanged(); } } // to exit infinite loop
        private string costMsg; public string CostMsg { get { return costMsg; } set { costMsg = value; OnPropertyChanged(); } }

        private float caseCost; public float CaseCost { get { return caseCost; } set { caseCost = value; OnPropertyChanged(); } }

        private float baseShipChg; public float BaseShipChg { get { return baseShipChg; } set { baseShipChg = value; OnPropertyChanged(); } }
        private float addlShipChg; public float AddlShipChg { get { return addlShipChg; } set { addlShipChg = value; OnPropertyChanged(); } }
        private int numDrops; public int NumDrops { get { return numDrops; } set { numDrops = value; OnPropertyChanged(); } }

        private string oe; public string OE { get { return oe; } set { oe = value; OnPropertyChanged(); } }
        private string pre; public string Pre { get { return pre; } set { pre = value; OnPropertyChanged(); } }

        private float shipTime = 0.15F; public float ShipTime { get { return shipTime; } set { shipTime = value; OnPropertyChanged(); } }
        private float shipCost = 25.0F; public float ShipCost { get { return shipCost; } set { shipCost = value; OnPropertyChanged(); } }

        private float preRunTime; public float PreRunTime { get { return preRunTime; } set { preRunTime = value; OnPropertyChanged(); } }
        private float preRunCost; public float PreRunCost { get { return preRunCost; } set { preRunCost = value; OnPropertyChanged(); } }
        private float oeRunTime; public float OERunTime { get { return oeRunTime; } set { oeRunTime = value; OnPropertyChanged(); } }
        private float oeRunCost; public float OERunCost { get { return oeRunCost; } set { oeRunCost = value; OnPropertyChanged(); } }

        private float pressSetupTime; public float PressSetupTime { get { return pressSetupTime; } set { pressSetupTime = value; OnPropertyChanged(); } }
        private float pressSetupCost; public float PressSetupCost { get { return pressSetupCost; } set { pressSetupCost = value; OnPropertyChanged(); } }
        private float pressRunTime; public float PressRunTime { get { return pressRunTime; } set { pressRunTime = value; OnPropertyChanged(); } }
        private float pressRunCost; public float PressRunCost { get { return pressRunCost; } set { pressRunCost = value; OnPropertyChanged(); } }

        private float collSetupTime; public float CollSetupTime { get { return collSetupTime; } set { collSetupTime = value; OnPropertyChanged(); } }
        private float collSetupCost; public float CollSetupCost { get { return collSetupCost; } set { collSetupCost = value; OnPropertyChanged(); } }
        private float collRunTime; public float CollRunTime { get { return collRunTime; } set { collRunTime = value; OnPropertyChanged(); } }
        private float collRunCost; public float CollRunCost { get { return collRunCost; } set { collRunCost = value; OnPropertyChanged(); } }

        private float bindSetupTime; public float BindSetupTime { get { return bindSetupTime; } set { bindSetupTime = value; OnPropertyChanged(); } }
        private float bindSetupCost; public float BindSetupCost { get { return bindSetupCost; } set { bindSetupCost = value; OnPropertyChanged(); } }
        private float bindRunTime; public float BindRunTime { get { return bindRunTime; } set { bindRunTime = value; OnPropertyChanged(); } }
        private float bindRunCost; public float BindRunCost { get { return bindRunCost; } set { bindRunCost = value; OnPropertyChanged(); } }

        private string customer = "Sample Customer"; public string Customer { get { return customer; } set { customer = value; OnPropertyChanged(); } }
        private string date = new DateOnly().ToString(); public string Date { get { return date; } set { date = value; OnPropertyChanged(); } }
        private string printDate = new DateOnly().ToString(); public string PrintDate { get { return printDate; } set { printDate = value; OnPropertyChanged(); } }
        private string standard = "01"; public string Standard { get { return standard; } set { standard = value; OnPropertyChanged(); } }
        private string desc; public string Desc { get { return desc; } set { desc = value; OnPropertyChanged(); } }
        private string salesRep = "Joyce Leiner"; public string SalesRep { get { return salesRep; } set { salesRep = value; OnPropertyChanged(); } }
        private string estimator; public string Estimator { get { return estimator; } set { estimator = value; OnPropertyChanged(); } }

        private int slowDownPct; public int SlowDownPct { get { return slowDownPct; } set { slowDownPct = value; OnPropertyChanged(); } }
        private float hrs; public float Hrs { get { return hrs; } set { hrs = value; OnPropertyChanged(); } }
        private float dollarsPerHr; public float DollarsPerHr { get { return dollarsPerHr; } set { dollarsPerHr = value; OnPropertyChanged(); } }

        private string comboSel1; public string ComboSel1 { get { return comboSel1; } set { comboSel1 = value; OnPropertyChanged(); } }  // Are      CheckFirst();  
        private string comboSel2; public string ComboSel2 { get { return comboSel2; } set { comboSel2 = value; OnPropertyChanged(); } }  // these    CheckSecond(); 
        private string comboSel3; public string ComboSel3 { get { return comboSel3; } set { comboSel3 = value; OnPropertyChanged(); } }  // needed   CheckThird();  
        private string comboSel4; public string ComboSel4 { get { return comboSel4; } set { comboSel4 = value; OnPropertyChanged(); } }  // now?     CheckFourth();

        private string selectedQuantity; public string SelectedQuantity { get { return selectedQuantity; } set { selectedQuantity = value; OnPropertyChanged(); CalcNow(); } }
        private string displayQuantity = "None selected"; public string DisplayQuantity { get { return displayQuantity; } set { displayQuantity = value; OnPropertyChanged(); } }

        private int mkuppct1; public int MkUpPct1 { get { return mkuppct1; } set { mkuppct1 = value; MarkupCalc(1); OnPropertyChanged(); } }
        private int mkupamt1; public int MkUpAmt1 { get { return mkupamt1; } set { mkupamt1 = value; MarkupCalc(1); OnPropertyChanged(); } }
        private int mkuppct2; public int MkUpPct2 { get { return mkuppct2; } set { mkuppct2 = value; MarkupCalc(2); OnPropertyChanged(); } }
        private int mkupamt2; public int MkUpAmt2 { get { return mkupamt2; } set { mkupamt2 = value; MarkupCalc(2); OnPropertyChanged(); } }
        private int mkuppct3; public int MkUpPct3 { get { return mkuppct3; } set { mkuppct3 = value; MarkupCalc(3); OnPropertyChanged(); } }
        private int mkupamt3; public int MkUpAmt3 { get { return mkupamt3; } set { mkupamt3 = value; MarkupCalc(3); OnPropertyChanged(); } }
        private int mkuppct4; public int MkUpPct4 { get { return mkuppct4; } set { mkuppct4 = value; MarkupCalc(4); OnPropertyChanged(); } }
        private int mkupamt4; public int MkUpAmt4 { get { return mkupamt4; } set { mkupamt4 = value; MarkupCalc(4); OnPropertyChanged(); } }

        private DataView dvPaper; public DataView DVPaper { get { return dvPaper; } set { dvPaper = value; OnPropertyChanged(); } }

        private List<Labor> laborData; private List<Labor> LaborData { get { return laborData; } set { laborData = value; OnPropertyChanged(); } }
        private List<Paper> paperData; private List<Paper> PaperData { get { return paperData; } set { paperData = value; OnPropertyChanged(); } }

        private bool isCalc; public bool IsCalc { get { return isCalc; } set { isCalc = value; OnPropertyChanged(); } }

        private bool adding; public bool Adding { get { return adding; } set { adding = value; OnPropertyChanged(); } }

        private string? quote_num = ""; public string? QUOTE_NUM { get { return quote_num; } set { quote_num = value; OnPropertyChanged(); } }
        private string? cust_num; public string? CUST_NUMB { get { return cust_num; } set { cust_num = value; OnPropertyChanged(); } }
        private string? projectType = ""; public string? ProjectType { get { return projectType; } set { projectType = value; OnPropertyChanged(); } }

        private string? fsint1; public string? FSINT1 { get { return fsint1; } set { fsint1 = value; OnPropertyChanged(); } }
        private string? fsfrac1; public string? FSFRAC1 { get { return fsfrac1; } set { fsfrac1 = value; OnPropertyChanged(); } }
        private string? fsint2; public string? FSINT2 { get { return fsint2; } set { fsint2 = value; OnPropertyChanged(); } }
        private string? fsfrac2; public string? FSFRAC2 { get { return fsfrac2; } set { fsfrac2 = value; OnPropertyChanged(); } }
        private int parts; public int PARTS { get { return parts; } set { parts = value; OnPropertyChanged(); } }

        private string? papertype; public string? PAPERTYPE { get { return papertype; } set { papertype = value; OnPropertyChanged(); } }
        private string? rollWidth; public string? ROLLWIDTH { get { return rollWidth; } set { rollWidth = value; OnPropertyChanged(); } }
        private string? presssize; public string? PRESSSIZE { get { return presssize; } set { presssize = value; OnPropertyChanged(); LoadCollator(); } }
        private bool? lineholes; public bool? LINEHOLES { get { return lineholes; } set { lineholes = value; OnPropertyChanged(); } }
        private string? collatorcut; public string? COLLATORCUT { get { return collatorcut; } set { collatorcut = value; OnPropertyChanged(); } }

        private float decimalCollatorCut; public float DecimalCollatorCut { get { return decimalCollatorCut; } set { decimalCollatorCut = value; OnPropertyChanged(); } }

        private string? customerName; public string? CustomerName { get { return customerName; } set { customerName = value; OnPropertyChanged(); } }
        private string? contactName; public string? ContactName { get { return contactName; } set { contactName = value; OnPropertyChanged(); } }
        private string? address; public string? Address { get { return address; } set { address = value; OnPropertyChanged(); } }
        private string? city; public string? City { get { return city; } set { city = value; OnPropertyChanged(); } }
        private string? state; public string? State { get { return state; } set { state = value; OnPropertyChanged(); } }
        private string? zip; public string? ZIP { get { return zip; } set { zip = value; OnPropertyChanged(); } }
        private string? location; public string? Location { get { return location; } set { location = value; OnPropertyChanged(); } }
        private string? phone; public string? Phone { get { return phone; } set { phone = value; OnPropertyChanged(); } }
        private string? csz; public string? CSZ { get { return csz; } set { csz = value; OnPropertyChanged(); } }
        private string? email; public string? Email { get { return email; } set { email = value; OnPropertyChanged(); } }

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

        private int qty1 = 1000; public int Qty1 { get { return qty1; } set { qty1 = value; OnPropertyChanged(); } }
        private int qty2 = 0; public int Qty2 { get { return qty2; } set { qty2 = value; OnPropertyChanged(); } }
        private int qty3 = 0; public int Qty3 { get { return qty3; } set { qty3 = value; OnPropertyChanged(); } }
        private int qty4 = 0; public int Qty4 { get { return qty4; } set { qty4 = value; OnPropertyChanged(); } }

        // Used in PressCalc (around line 1107)
        private int? wastePct = 5; public int? WastePct { get { return wastePct; } set { wastePct = value; OnPropertyChanged(); } }

        private string? itemType; public string? ItemType { get { return itemType; } set { itemType = value; OnPropertyChanged(); } }     // LoadPage2Combos();
        private int? numUpp; public int? NumUpp { get { return numUpp; } set { numUpp = value; OnPropertyChanged(); } }
        private int selectedQty; public int SelectedQty { get { return selectedQty; } set { selectedQty = value; OnPropertyChanged(); } }
        private string? calcMsg; public string? CalcMsg { get { return calcMsg; } set { calcMsg = value; OnPropertyChanged(); } }

        private DataTable? features; public DataTable? Features { get { return features; } set { features = value; OnPropertyChanged(); } }
        private DataTable? elements; public DataTable? Elements { get { return elements; } set { elements = value; OnPropertyChanged(); } }
        private DataTable? formTypes; public DataTable? FormTypes { get { return formTypes; } set { formTypes = value; OnPropertyChanged(); } }
        private DataTable? papertypes; public DataTable? PaperTypes { get { return papertypes; } set { papertypes = value; OnPropertyChanged(); } }
        private DataTable? rollwidths; public DataTable? RollWidths { get { return rollwidths; } set { rollwidths = value; OnPropertyChanged(); } }
        private DataTable? sizes; public DataTable? Sizes { get { return sizes; } set { sizes = value; OnPropertyChanged(); } }
        private DataTable? colors; public DataTable? Colors { get { return colors; } set { colors = value; OnPropertyChanged(); } }
        private DataTable? basisWT; public DataTable? BasisWT { get { return basisWT; } set { basisWT = value; OnPropertyChanged(); } }
        private DataTable? itemTypes; public DataTable? ItemTypes { get { return itemTypes; } set { itemTypes = value; OnPropertyChanged(); } }

        private float mult; public float MULT { get { return mult; } set { mult = value; OnPropertyChanged(); } }
        private int? selRowIdx; public int? SelRowIdx { get { return selRowIdx; } set { selRowIdx = value; OnPropertyChanged(); } }
        private DataRowView? drv; public DataRowView? DRV { get { return drv; } set { drv = value; OnPropertyChanged(); } }
        private double quoteTotal; public double QuoteTotal { get { return quoteTotal; } set { quoteTotal = value; OnPropertyChanged(); } }

        private int maxColors; public int MaxColors { get { return maxColors; } set { maxColors = value; OnPropertyChanged(); } }
        private string? category; public string? Category { get { return category; } set { category = value; OnPropertyChanged(); } }

        private int labPct1; public int LabPct1 { get { return labPct1; } set { labPct1 = value; OnPropertyChanged(); } }
        private int labPct2; public int LabPct2 { get { return labPct2; } set { labPct2 = value; OnPropertyChanged(); } }
        private int labPct3; public int LabPct3 { get { return labPct3; } set { labPct3 = value; OnPropertyChanged(); } }
        private int labPct4; public int LabPct4 { get { return labPct4; } set { labPct4 = value; OnPropertyChanged(); } }

        private int matPct1; public int MatPct1 { get { return matPct1; } set { matPct1 = value; OnPropertyChanged(); } }
        private int matPct2; public int MatPct2 { get { return matPct2; } set { matPct2 = value; OnPropertyChanged(); } }
        private int matPct3; public int MatPct3 { get { return matPct3; } set { matPct3 = value; OnPropertyChanged(); } }
        private int matPct4; public int MatPct4 { get { return matPct4; } set { matPct4 = value; OnPropertyChanged(); } }

        private float sellPrice1; public float SellPrice1 { get { return sellPrice1; } set { sellPrice1 = value; OnPropertyChanged(); } }
        private float sellPrice2; public float SellPrice2 { get { return sellPrice2; } set { sellPrice2 = value; OnPropertyChanged(); } }
        private float sellPrice3; public float SellPrice3 { get { return sellPrice3; } set { sellPrice3 = value; OnPropertyChanged(); } }
        private float sellPrice4; public float SellPrice4 { get { return sellPrice4; } set { sellPrice4 = value; OnPropertyChanged(); } }

        #endregion

        public Quotations()
        {
            InitializeComponent();

            this.DataContext = this;

            // Defaults for testing
            MaxColors = 5;
            QUOTE_NUM = "10001";
            ProjectType = "CONTINUOUS";
            PARTS = 3;
            PAPERTYPE = "BOND";
            ROLLWIDTH = "10";
            PRESSSIZE = "11";
            COLLATORCUT = "11";

            LoadPressSizes();
            LoadFormTypes();
            LoadPaperTypes();
            LoadRollWidth();
            LoadAvailableCategories();
            LoadDetails();   // For page 3

            Date = DateTime.Now.ToString("MM/dd/yyyy");
            PrintDate = Date;
            Estimator = SalesRep;

            PreviewKeyDown += (s, e) => { if (e.Key == System.Windows.Input.Key.Escape) Close(); };
        }

        public void OnLoad(object sender, RoutedEventArgs e)
        {
            this.Height = this.Height *= (1.0F + MainWindow.FeatureZoom);
            this.Width = this.Width *= (1.0F + MainWindow.FeatureZoom);
            Top = 15;
        }

        private void LoadAvailableCategories()
        {
            lstAvailable.Items.Clear();
            lstAvailable.Items.Add("Base Charges");
            //          lstAvailable.Items.Add("Backer");
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
            lstAvailable.Items.Add("Shipping");
            lstAvailable.Items.Add("Cases");
            lstAvailable.Items.Add("Carbon");
        }

        private void btnLookup_Click(object sender, RoutedEventArgs e)
        {
            CustomerLookup cl = new(); cl.ShowDialog();
            CUST_NUMB = cl.CustomerCode;

            cl.Close();

            string str = "SELECT C.CUST_NAME, CC.CONTACT_NAME, C.CUST_NUMB, CC.ADDRESS1, CC.CITY, CC.STATE, CC.ZIP, CC.PHONE, CC.E_MAIL "
            + " FROM [ESTIMATING].[dbo].[CUSTOMER] C, [ESTIMATING].[dbo].[CUSTOMER_CONTACT] CC "
            + $" WHERE C.CUST_NUMB = CC.CUST_NUMB AND C.CUST_NUMB = '{CUST_NUMB}'";

            conn = new SqlConnection(ConnectionString);
            SqlDataAdapter da = new(str, conn);
            DataTable dt = new(); da.Fill(dt);

            if (dt.Rows.Count == 0) { return; }

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
            QuoteLookup ql = new("");
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
            da = new SqlDataAdapter(cmd, conn); dt = new DataTable(); da.Fill(dt); conn.Close();
            if (int.Parse(dt.Rows[0]["HowMany"].ToString()) > 0) return;

            lstSelected.Items.Clear();

            cmd = $"INSERT INTO [ESTIMATING].[dbo].[QUOTE_DETAILS] "
                + "        (   QUOTE_NUM,    SEQUENCE, CATEGORY,   Param1,       Param2,     Param3,     Value1, Value2, Value3, PerThousandChg )"
                + $" VALUES ( '{QUOTE_NUM}', '8',      'PrePress', 'OrderEntry', 'PlateChg', 'PrePress', 'New',   '0',   'New',   '0.0'    )";
            // See if some other default value for 'PerThousandChg' should be used

            conn.Open();
            scmd = new SqlCommand(cmd, conn);
            scmd.ExecuteNonQuery();
            conn.Close();

            // Should I add a Shipping record?
            //cmd = $"INSERT INTO [ESTIMATING].[dbo].[QUOTE_DETAILS] "
            //    + "        (   QUOTE_NUM,    SEQUENCE, CATEGORY,   Param1,       Param2,     Param3,     Value1, Value2, Value3 )"
            //    + $" VALUES ( '{QUOTE_NUM}', '8',      'Shipping', 'OrderEntry', 'PlateChg', 'PrePress', 'New',   '0',   'New'   )";

            //conn.Open();
            //scmd = new SqlCommand(cmd, conn);
            //scmd.ExecuteNonQuery();
            //conn.Close();

            LoadDetails();

            // Remove the default features from the "Available" list
            lstAvailable.Items.Remove("Base Charges");
            lstAvailable.Items.Remove("Shipping");
            lstAvailable.Items.Remove("PrePress");
        }

        private void GetQuote()
        {
            if (QUOTE_NUM == null || QUOTE_NUM == "") QUOTE_NUM = txtQuoteNum.Text.ToString();

            SqlConnection cn = new(ConnectionString);
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
                PARTS = int.Parse(dt.Rows[0]["PARTS"].ToString());
                PAPERTYPE = dt.Rows[0]["PAPERTYPE"].ToString();  // dr3[9];
                ROLLWIDTH = dt.Rows[0]["ROLLWIDTH"].ToString().TrimEnd();  // dr3[9];
                PRESSSIZE = dt.Rows[0]["PRESSSIZE"].ToString();  // dr3[9];     "PressSize.Cylinder"
                LINEHOLES = bool.Parse(dt.Rows[0]["LINEHOLES"].ToString());
                COLLATORCUT = dt.Rows[0]["COLLATORCUT"].ToString();  // dr3[9]; "PressSize.FormSize"	

                txtCustomerNum_LostFocus(this, null);

                //TODO: Add detail lines, changing QUOTE_DETAILS.QUOTE_NUM
            }
        }

        private void Tabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (Tabs.SelectedIndex)
            {
                case 0: ActivePage = "Base"; lblPageName.Content = "Base"; OnPropertyChanged("ActivePage"); break;
                case 1: ActivePage = "Details"; lblPageName.Content = "Details"; OnPropertyChanged("ActivePage"); break;
                case 2: ActivePage = "Features"; lblPageName.Content = "Features"; OnPropertyChanged("ActivePage"); break;
                case 3: ActivePage = "Pricing"; lblPageName.Content = "Pricing"; OnPropertyChanged("ActivePage"); break;
                case 4: ActivePage = "Details"; lblPageName.Content = "Summary"; OnPropertyChanged("ActivePage"); break;
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
            string str = "SELECT ReportItem FROM [ProVisionDev].[dbo].[ReportItems] WHERE Active = 1 ORDER BY ReportItem";  // I added the boolean flag 'Active'
            SqlConnection cn = new(ConnectionString); cn.Open(); if (cn.State != ConnectionState.Open) { MessageBox.Show("Couldn't open SQL Connection"); return; }
            SqlDataAdapter da = new(str, cn); DataTable dt = new DataTable(); da.Fill(dt); DataView dv = dt.DefaultView;
            cmbPaperType.Items.Clear();
            for (int i = 0; i < dv.Count; i++) { cmbPaperType.Items.Add(dv[i]["ReportItem"].ToString().TrimEnd()); }
        }

        private void LoadColors()
        {
            string str = $"SELECT DISTINCT Color FROM [ProVisionDev].[dbo].MasterInventory WHERE ReportType = '{PAPERTYPE.TrimEnd()}'";
            if (ItemType != null) { str += $" AND ItemType = '{ItemType.TrimEnd()}'"; }
            str += " ORDER BY Color";

            SqlConnection cn = new(ConnectionString);
            cn.Open(); if (cn.State != ConnectionState.Open) { MessageBox.Show("Couldn't open SQL Connection"); return; }
            SqlDataAdapter da = new(str, cn); DataTable dt = new DataTable(); da.Fill(dt); DataView dv = dt.DefaultView;
            txtColor.Items.Clear(); for (int i = 0; i < dv.Count; i++) { txtColor.Items.Add(dv[i][0].ToString()); }
        }

        private void LoadItemTypes()
        {
            if (PAPERTYPE.Length == 0) return;

            string str = $"SELECT DISTINCT ItemType FROM [ProVisionDev].[dbo].[MasterInventory] WHERE ReportType = '{PAPERTYPE}' ORDER BY ItemType";
            SqlConnection cn = new(ConnectionString); cn.Open(); if (cn.State != ConnectionState.Open) { MessageBox.Show("Couldn't open SQL Connection"); return; }
            SqlDataAdapter da = new(str, cn); DataTable dt = new DataTable(); da.Fill(dt); DataView dv = dt.DefaultView;
            txtItemType.Items.Clear(); for (int i = 0; i < dv.Count; i++) { txtItemType.Items.Add(dv[i]["ItemType"].ToString().TrimEnd()); }
        }

        private void LoadSubWT()
        {
            if (PAPERTYPE is null) return;
            if (ItemType is null) return;
            string str =
                  "SELECT DISTINCT CONVERT(float, SubWT) AS SubWT FROM[ProVisionDev].[dbo].MasterInventory"
                + $" WHERE ReportType = '{PAPERTYPE}'"
                + " AND SubWT IS NOT NULL"
                + " AND SubWT<> ''"
                + " AND SubWT<> 'UNIT'"
                + $" AND ItemType = '{ItemType.TrimEnd()}' "
                + " ORDER BY SUBWT";
            SqlConnection cn = new(ConnectionString);
            cn.Open(); if (cn.State != ConnectionState.Open) { MessageBox.Show("Couldn't open SQL Connection"); return; }
            DataTable dt = new DataTable("WT");
            SqlDataAdapter da = new(str, cn); da.Fill(dt); DataView dv = dt.DefaultView;
            txtBasis.Items.Clear(); for (int i = 0; i < dv.Count; i++) { txtBasis.Items.Add(dv[i]["SubWT"]); }
        }

        private void LoadRollWidth()
        {
            if (PAPERTYPE is null) return;

            string str = $"SELECT RollWidth FROM [ProVisionDev].[dbo].[RollWidth] WHERE Abbreviation = '{PAPERTYPE.TrimEnd()}'";
            SqlConnection cn = new(ConnectionString);
            cn.Open(); if (cn.State != ConnectionState.Open) { MessageBox.Show("Couldn't open SQL Connection"); return; }

            SqlDataAdapter da = new(str, cn); DataTable dt = new DataTable(); da.Fill(dt); DataView dv = dt.DefaultView;
            cmbRollWidth.Items.Clear(); txtRollWidth.Items.Clear();
            if (dv.Count == 0) { MessageBox.Show("No matches", "No roll widths"); return; }

            for (int i = 0; i < dv.Count; i++) { cmbRollWidth.Items.Add(dv[i][0]); txtRollWidth.Items.Add(dv[i][0]); }
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
            string str = "SELECT PRESS FROM [ProVisionDev].[dbo].[CYLCUTSIZES] GROUP BY PRESS, SEQ ORDER BY SEQ";
            SqlConnection cn = new(ConnectionString);

            cn.Open(); if (cn.State != ConnectionState.Open) { MessageBox.Show("Couldn't open SQL Connection"); return; }

            SqlDataAdapter da = new(str, cn); DataSet ds = new("PressSizes"); da.Fill(ds); DataTable dt = ds.Tables[0];
            cmbPressSize.Items.Clear(); for (int r = 0; r < dt.Rows.Count; r++) { cmbPressSize.Items.Add(dt.Rows[r][0].ToString()); }
        }

        //private void cmbFinalSizeFrac1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    string? controlname = (sender as ComboBox)?.Name;

        //    int Int1 = 0; int Int2 = 0;
        //    float Frac1 = 0.00F; float Frac2 = 0.00F;

        //    if ((cmbFinalSizeInches1.SelectedItem as ComboBoxItem) != null)
        //    {
        //        string? Str1 = (cmbFinalSizeInches1.SelectedItem as ComboBoxItem)?.Content.ToString();
        //        Int1 = int.Parse(Str1);
        //    }

        //    if ((cmbFinalSizeFrac1.SelectedItem as ComboBoxItem) != null)
        //    {
        //        string? x = (cmbFinalSizeFrac1.SelectedItem as ComboBoxItem)?.Content.ToString();
        //        Frac1 = CalcFraction(x);
        //    }

        //    if ((cmbFinalSizeInches2.SelectedItem as ComboBoxItem) != null)
        //    {
        //        string? Str2 = (cmbFinalSizeInches2.SelectedItem as ComboBoxItem)?.Content.ToString();
        //        Int2 = int.Parse(Str2);
        //    }

        //    if ((cmbFinalSizeFrac2.SelectedItem as ComboBoxItem) != null)
        //    {
        //        string? x = (cmbFinalSizeFrac2.SelectedItem as ComboBoxItem)?.Content.ToString();
        //        Frac2 = CalcFraction(x);
        //    }

        //    if (controlname?.Substring(controlname.Length - 1) == "1")
        //    { Decimal1.Content = Frac1 + Int1; }
        //    else { Decimal2.Content = Frac2 + Int2; }
        //}

        private void txtCustomerNum_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtCustomerNum.Text.ToString().TrimEnd().Length == 0) { return; }

            int CustKey = int.Parse(txtCustomerNum.Text); // I don't think this helps; it's a 6-character fixed length string...

            string FixedCust = txtCustomerNum.Text.Trim();
            FixedCust = CUST_NUMB.TrimEnd();
            if (FixedCust.Length == 4) FixedCust = "0" + FixedCust;
            FixedCust = FixedCust.PadRight(6);
            txtCustomerNum.Text = FixedCust;

            SqlConnection cn = new(ConnectionString);
            string str = "SELECT * FROM [ESTIMATING].[dbo].[CUSTOMER] C, [ESTIMATING].[dbo].[CUSTOMER_CONTACT] CC " +
                        $" WHERE C.CUST_NUMB = '{FixedCust}' AND C.CUST_NUMB = CC.CUST_NUMB";
            SqlDataAdapter da = new(str, cn);
            DataSet ds = new();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                DataRow dr = dt.Rows[0];
                DataRowView drv = dt.DefaultView[dt.Rows.IndexOf(dr)];
                CustomerName = drv["CUST_NAME"].ToString().TrimEnd();
                Customer = CustomerName;
                ContactName = drv["CONTACT_NAME"].ToString();
                Address = drv["ADDRESS1"].ToString();
                Phone = drv["PHONE"].ToString();
                Email = drv["E_MAIL"].ToString();
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

        private void HandleEsc(object sender, KeyEventArgs e) { if (e.Key == System.Windows.Input.Key.Escape) Close(); }

        private void txtQuoteNum_LostFocus(object sender, RoutedEventArgs e)
        {
            txtQuoteNum.Foreground = Brushes.Black;
            txtQuoteNum.Background = Brushes.White;

            GetQuote();
        }

        private void Qty1b_LostFocus(object sender, RoutedEventArgs e)
        {
            if ((QTY1a != null && QTY1a != 0) && (CPM1a != null && CPM1a != 0) & (MAR1a != null && MAR1a != 0))
            { float? result = QTY1a * CPM1a * MAR1a; EXT1a = EXT1a + result / 1000.00F; }
        }

        private void Qty2b_LostFocus(object sender, RoutedEventArgs e)
        {
            if ((QTY2a != null && QTY2a != 0) && (CPM2a != null && CPM2a != 0) & (MAR2a != null && MAR2a != 0))
            { float? result = QTY2a * CPM2a * MAR2a; EXT2a = EXT2a + result / 1000.00F; }
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
            //            Markup1.SelectAll();
        }

        private async void SelectAll_OnTextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            await Application.Current.Dispatcher.InvokeAsync((sender as TextBox).SelectAll);
        }

        private void LoadCollator()
        {
            if (PRESSSIZE.Length == 0) { return; }      // needed if we look up a quote and blank any previous values on PAGE 1

            DataTable CollCuts = new DataTable("CollCuts");
            CollCuts.Columns.Add("CutSize");

            string cmd = "SELECT CutSize FROM CylCutSizes WHERE Press = '" + PRESSSIZE + "'";

            SqlConnection cn = new(ConnectionString); cn.Open();
            SqlDataAdapter da = new(cmd, cn);

            da.Fill(CollCuts);
            if (cmbCollatorCut.ItemsSource == null) cmbCollatorCut.Items.Clear();

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
            LoadGrid();
        }

        public void LoadGrid()
        {
            string ErrorMessages = "";
            if (ProjectType.ToString().TrimEnd().Length == 0) { ErrorMessages += "Project type is required;\n"; }
            if (PARTS.ToString().TrimEnd().Length == 0)       { ErrorMessages += "# Parts is required;\n"; }
            if (PAPERTYPE.ToString().TrimEnd().Length == 0)   { ErrorMessages += "Paper type is required;\n"; }
            if (ROLLWIDTH.ToString().TrimEnd().Length == 0)   { ErrorMessages += "Roll width is required;\n"; }
            if (PRESSSIZE.ToString().TrimEnd().Length == 0)   { ErrorMessages += "Press size is required;\n"; }
            if (COLLATORCUT.ToString().TrimEnd().Length == 0) { ErrorMessages += "Collator cut size is required;\n"; }

            if (ErrorMessages.Length > 0) { MessageBox.Show(ErrorMessages); return; }

            AddDefaultDetailLines();

            int setNum = int.Parse(PartsSpinner.Value.ToString());  // Isn't this the same as PARTS?
            string formType = ProjectType.Substring(0, 1);
            string paperType = PAPERTYPE.Substring(0, 1);
            string rollwidth = ROLLWIDTH.ToString();

            string cmd = "SELECT " +
                "M.Description," +
                "M.ItemType," +
                "M.SubWT," +
                "M.COLOR            AS MCOLOR," +
                "E.PAPER," +
                "E.SETNUM," +
                "E.SEQ," +
                "E.PTYPE," +
                "E.COLOR," +
                "E.FORMTYPE," +
                "E.PAPERTYPE," +
                "0.00               AS Pounds," +
                "0.00               AS LastPOCost," +
                "0.00               AS AverageCost," +
                "M.COSTPERFACTOR    AS MastInvCost," +
                "0.00               AS SelectedCost," +
                "0.00               AS PaperCost" +
                " FROM             PROVISIONDEV.DBO.MasterInventory M" +
                " RIGHT OUTER JOIN PROVISIONDEV.DBO.ESTPAPER        E" +
                "  ON M.ITEMTYPE    = E.PTYPE" +
                " AND M.COLOR       = E.COLOR" +
                " AND M.SubWT       = E.WEIGHT" +
               $" WHERE E.FORMTYPE  = '{formType}'" +
               $"   AND E.PAPERTYPE = '{paperType}'" +
               $"   AND M.SIZE      = '{rollwidth}'" +
               $"   AND E.SETNUM    =  {setNum}" +
                " AND Inventoryable = 1" +
                " ORDER BY E.SETNUM, E.SEQ";

            Clipboard.SetText(cmd);     // see what's going on in SSMC

            SqlConnection cn = new(ConnectionString); cn.Open();
            SqlDataAdapter da = new(cmd, cn);
            DataSet ds = new("Papers");
            da.Fill(ds);
            DataTable? dt = ds.Tables[0];

            if (dt.DefaultView.Count == 0) { MessageBox.Show("No data was returned; the SELECT command was stored in your ClipBoard."); return; }

            bool NoCostAssigned = false;
            for (int r = 0; r < dt.DefaultView.Count; r++)
            {
                string? clr = dt.Rows[r]["Color"].ToString();
                string? wt = dt.Rows[r]["SubWT"].ToString();
                string? it = dt.Rows[r]["PType"].ToString();
                it = (it == null) ? "" : it.Substring(0, 1) + "%";
                //                string? size = cmbRollWidth.SelectedValue.ToString();
                string? size = ROLLWIDTH;
                size = (size is null) ? "" : size.TrimEnd();

                string? projType = cmbProjectType.SelectedValue.ToString();
                projType = (projType == "SHEET") ? "SHT" : "ROLL";   // Sheet or Continuous

                dt.Rows[r]["Pounds"] = 0;    // Zero out all Pounds; PaperCalc will calculate them.

                // Get the most recent and the average costs for this paper type:
                SqlDataAdapter da4 = new SqlDataAdapter("MostRecentCost", ConnectionString);
                da4.SelectCommand.CommandType = CommandType.StoredProcedure;
                da4.SelectCommand.Parameters.Add("@Description", SqlDbType.VarChar, 50).Value = dt.Rows[r][0]; //desc; 
                DataTable dtRec = new("Rec");
                da4.Fill(dtRec);
                if (dtRec.Rows.Count > 0)
                { dt.Rows[r]["LastPOCost"] = dtRec.Rows[0][0].ToString(); }

                SqlDataAdapter da5 = new SqlDataAdapter("AverageCost", ConnectionString);
                da5.SelectCommand.CommandType = CommandType.StoredProcedure;
                da5.SelectCommand.Parameters.Add("@Description", SqlDbType.VarChar, 50).Value = dt.Rows[r][0]; // desc;
                DataTable dtAvg = new("Avg");
                da5.Fill(dtAvg);
                if (dtAvg.Rows.Count > 0)
                {
                    dt.Rows[r]["AverageCost"] = dtAvg.Rows[0][0].ToString();    // I rewrote the query to return just one column (average cost)
                    dt.Rows[r]["SelectedCost"] = dtAvg.Rows[0][0].ToString();    // SelectedCost defaults to average cost
                }
                else { dt.Rows[r]["SelectedCost"] = dt.Rows[r]["LastPOCost"]; }     // Default is AverageCost unless it was zero

                if (float.Parse(dt.Rows[r]["SelectedCost"].ToString()) == 0.0F) { NoCostAssigned = true; }
            }

            if (NoCostAssigned == true)
            { CostMessage.Visibility = Visibility.Visible; }
            else
            { CostMessage.Visibility = Visibility.Hidden; }

            // SelectedCost is what goes in the "Cost used" column on page 4

            dgSheetsOfPaper.ItemsSource = dt.DefaultView;
            DVPaper = dt.DefaultView;       // used on page 4

            Page2.IsEnabled = true;
            Page3.IsEnabled = true;
            Page5.IsEnabled = true;

            if (txtItemType.Items.Count == 0)
            { LoadItemTypes(); }

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

            string tmpPaperType = dataRow[7].ToString().TrimEnd();
            if (tmpPaperType == "BND") tmpPaperType = "BOND";
            txtItemType.Text = tmpPaperType;

            string[] w = DRV[0].ToString().Split();
            txtColor.SelectedValue = w[2];
            DRV[8] = w[2];
            //            txtColor.SelectedValue = DVPaper[0]["MColor"].ToString();
            txtBasis.SelectedValue = dataRow[2].ToString().TrimEnd();

            lblItemType.Content = tmpPaperType;
            lblColor.Content = dataRow[8].ToString();

            string[] tmpBasis = DRV[0].ToString().Split();

            lblBasis.Content = tmpBasis[1].ToString().Replace("#", ""); //  dataRow[2].ToString().TrimEnd();

            if (dataRow[0].ToString().Contains("."))
            {  // Get basis from Description starting right after the first chr(32) up to but not including the "#":
                int starts = dataRow[0].ToString().IndexOf(" ") + 1;
                string temp = dataRow[0].ToString().Substring(starts);
                int ends = temp.ToString().IndexOf("#");
                lblBasis.Content = temp.Substring(0, ends);
            }

            //            lblPaperType.Content = dataRow[7].ToString().TrimEnd();
            string col = lblColor.Content.ToString();

            // RollWidth (size) is just to the right of the third blank in the descrption field
            string Desc = dataRow[0].ToString();
            int in1 = Desc.IndexOf(' ');          // get the index of first blank
            int in2 = Desc.IndexOf(' ', in1 + 1); // get the index of second blank space
            int in3 = Desc.IndexOf(' ', in2 + 1); // get the index of third blank space
            string s2 = Desc.Substring(in3 + 1);
            ROLLWIDTH = s2.TrimEnd();               // 12/06/2023 8:30 Before I added ".TrimEnd()", assigned value didn't match the trimmed value in the Items list...
            lblRollWidth.Content = ROLLWIDTH.ToString().TrimEnd();

            // Insert the value in the selected column into the "SelectedCost" column (12)
            DataGridColumn chosen = dgSheetsOfPaper.CurrentColumn;

            var x = (chosen == null) ? "" : chosen.ToString();
            if (chosen != null)
            {
                // ***************************************************
                // Select a price if they clicked columns 12, 13 or 14
                // ***************************************************
                int idx = chosen.DisplayIndex + 10;
                if (idx < 12 || idx > 14) return;    // Only three of the columns should be clickable
                txtItemType.IsDropDownOpen = false;  // collapse the ItemType dropdown
                dataRow[15] = dataRow[idx];          // Use the value they clicked as the "cost to use"
                                                     // This assumes that only one cost was not assigned;
                CostMessage.Visibility = Visibility.Hidden;

            }

            // -------------------------------------------------------------
            //                              Pounds needed           *              Price to use
            double PaperCost = double.Parse(dataRow[11].ToString()) * double.Parse(dataRow[15].ToString());
            dataRow[16] = PaperCost;
            // Pounds of paper isn't calculated until a Quantity is Selected
            // -------------------------------------------------------------

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
            if (txtItemType.Text.Length == 0
                && txtBasis.Text.Length == 0
                && txtColor.Text.Length == 0
                && txtRollWidth.Text.Length == 0)
            { NoChgMsg = "No changes to make."; return; }

            // Verify that the new description exists in the MasterInventory table description field

            string? TestDesc = DRV[0].ToString().TrimEnd();
            if (lblItemType.Content.ToString().Length > 0 && txtItemType.Text.ToString().TrimEnd().Length > 0)
            { TestDesc = TestDesc.Replace(lblItemType.Content.ToString().TrimEnd(), txtItemType.Text.ToString().TrimEnd()); }

            if (lblBasis.Content.ToString().Length > 0 && txtBasis.Text.ToString().TrimEnd().Length > 0)
            { TestDesc = TestDesc.Replace(lblBasis.Content.ToString().TrimEnd() + "#", txtBasis.Text.ToString().TrimEnd() + "#"); }

            if (lblColor.Content.ToString().Length > 0 && txtColor.Text.ToString().TrimEnd().Length > 0)
            { TestDesc = TestDesc.Replace(lblColor.Content.ToString().TrimEnd(), txtColor.Text.ToString().TrimEnd()); }

            if (lblRollWidth.Content.ToString().Length > 0 && txtRollWidth.Text.ToString().TrimEnd().Length > 0)
            {

                int in1 = TestDesc.IndexOf(' ');          // get the index of first blank
                int in2 = TestDesc.IndexOf(' ', in1 + 1); // get the index of second blank space
                int in3 = TestDesc.IndexOf(' ', in2 + 1); // get the index of third blank space

                string s1 = TestDesc.Substring(0, in3);
                TestDesc = s1 + " " + txtRollWidth.Text.ToString().TrimEnd();
                ROLLWIDTH = txtRollWidth.Text.ToString().TrimEnd();
            }

            string cmd = $"SELECT COUNT(*) FROM MasterInventory WHERE Description = '{TestDesc}'";

            Clipboard.SetText(cmd);

            SqlConnection cn = new(ConnectionString);
            SqlDataAdapter da4 = new(cmd, cn); DataTable dt4 = new("CT"); da4.Fill(dt4);

            if (int.Parse(dt4.Rows[0][0].ToString()) == 0) { MessageBox.Show("Paper not found", TestDesc); return; }

            DRV[0] = TestDesc;      // Update the current row entry
            DRV[7] = ItemType;

            ItemType = txtItemType.Text.TrimEnd(); // This should fix the itemtype problem after updating a row in the Paper table on Page 2

            CalculateCosts(TestDesc);

            if (txtColor.Text.Length == 0) { txtColor.Text = lblColor.Content.ToString().TrimEnd(); }
            if (txtBasis.Text.Length == 0) { txtBasis.Text = lblBasis.Content.ToString().TrimEnd(); }
            if (txtRollWidth.Text.Length == 0) { txtRollWidth.Text = lblRollWidth.Content.ToString().TrimEnd(); }

            lblItemType.Content = txtItemType.Text;
            lblBasis.Content = txtBasis.Text;
            lblColor.Content = txtColor.Text.TrimEnd();
            lblRollWidth.Content = txtRollWidth.Text;

            txtRollWidth.Text = "";
            txtBasis.Text = "";
            txtColor.Text = "";
            txtItemType.Text = "";

            return;
        }

        private void CalculateCosts(string Desc)
        {
            string clr = txtColor.Text.TrimEnd();
            string wt = txtBasis.Text.TrimEnd();     // dt.Rows[r]["SubWT"].ToString();
            string it = txtItemType.Text.TrimEnd();  // dt.Rows[r]["PType"].ToString();
            string? size = txtRollWidth.Text.TrimEnd(); // cmbRollWidth.SelectedValue.ToString();
                                                        // it = (it == null) ? "" : it.Substring(0, 1) + "%";
                                                        // size = (size is null) ? "" : size.TrimEnd();

            string? projType = cmbProjectType.SelectedValue.ToString();
            projType = (projType == "SHEET") ? "SHT" : "ROLL";   // Sheet or Continuous

            DRV["Pounds"] = 0;    // Zero out all Pounds; PaperCalc will calculate them.
            DRV[3] = clr; DRV[8] = clr; DRV[1] = it;

            // Get the most recent and the average costs for this paper type:
            SqlDataAdapter da4 = new SqlDataAdapter("MostRecentCost", ConnectionString);
            da4.SelectCommand.CommandType = CommandType.StoredProcedure;
            da4.SelectCommand.Parameters.Add("@Description", SqlDbType.VarChar, 50).Value = Desc;
            DataTable dtRec = new("Rec"); da4.Fill(dtRec);
            if (dtRec.Rows.Count > 0)
            { float LastPOCost = 0; float.TryParse(dtRec.Rows[0][0].ToString(), out LastPOCost); DRV["LastPOCost"] = LastPOCost; }

            SqlDataAdapter da5 = new SqlDataAdapter("AverageCost", ConnectionString);
            da5.SelectCommand.CommandType = CommandType.StoredProcedure;
            da5.SelectCommand.Parameters.Add("@Description", SqlDbType.VarChar, 50).Value = Desc;
            DataTable dtAvg = new("Avg"); da5.Fill(dtAvg);

            if (dtAvg.Rows.Count > 0)
            {
                DRV["AverageCost"] = dtAvg.Rows[0][0].ToString();    // I rewrote the query to return just one column (average cost)
                DRV["SelectedCost"] = dtAvg.Rows[0][0].ToString();    // SelectedCost defaults to average cost
            }
            else { DRV["SelectedCost"] = DRV["LastPOCost"]; }     // Default is AverageCost unless it was zero

            if (float.Parse(DRV["SelectedCost"].ToString()) == 0.0F) { /*NoCostAssigned = true;*/ }
        }

        private void dgSheetsOfPaper_CurrentCellChanged(object sender, System.EventArgs e)
        {
            txtItemType.Focus();
            // Provide a visual cue that combobox itemlists have been refreshed and they need to pick anything they want to change
            PickMsg.Visibility = Visibility.Visible;
            txtItemType.IsDropDownOpen = true;
        }

        private void cmbCollatorCut_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbCollatorCut.SelectedValue == null)
            {
                DecimalCollatorCut = 0.0F;
                return;
            }

            string? val = cmbCollatorCut.SelectedValue.ToString().TrimEnd();
            string sval1, sval2;
            sval1 = "";
            sval2 = "";
            if (val.IndexOf(" ") > 0)
            {
                sval1 = val.Substring(0, val.IndexOf(" "));
                sval2 = val.Substring(val.IndexOf(" ") + 1);
            }
            else
            { sval1 = val; }
            float val1 = int.Parse(sval1);
            float val2 = 0.0F;
            val2 = CalcFraction(sval2);
            float totval = val1 + val2;

            DecimalCollatorCut = totval;

            string intPart = String.Join("", PRESSSIZE.Where(char.IsDigit));    // Might end in a 3-char press name...
            float UpVal = float.Parse(intPart) / totval;    // remove text characters from PRESSSIZE before calculating...
            int intVal = (int)Math.Round(UpVal);
            lblUp.Content = UpVal.ToString();
            lblUpMessage.Content = "up";
        }

        private void txtQty1_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            return; // LostFocus event now does this
        }
        private void txtQty2_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            return; // LostFocus event now does this
        }

        private void txtQty3_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            return; // LostFocus event now does this
        }

        private void txtQty4_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            return; // LostFocus event now does this
        }

        private void GridCalc()
        {

            if (DVPaper == null || DVPaper.Count == 0) return;

            Calculating c = new Calculating();
            c.Owner = this;
            c.Show();    // Show the "busy" message...

            PressCalc pc = new PressCalc();
            pc.WastePct = (int)WastePct;
            pc.Up = int.Parse(lblUp.Content.ToString());
            QuoteTotal = 0.00D;
            int r = 0;

            // Populate the Paper ListView on Page 4
            List<Paper> Papers = new List<Paper>();

            for (r = 0; r < DVPaper.Count; r++)
            {
                pc.NumDocs = (int)SelectedQty;
                DataRowView drv = (DataRowView)dgSheetsOfPaper.Items[r];

                pc.Material = drv[0].ToString();
                pc.PressSize = PRESSSIZE;
                dgSheetsOfPaper.SelectedIndex = r;
                pc.Calc();
                drv[11] = pc.Pounds;    // multiply UseThisPrice times Pounds and store in last column
                drv[16] = float.Parse(pc.Pounds.ToString()) * float.Parse(drv[15].ToString());
                QuoteTotal += double.Parse(drv[16].ToString());

                Papers.Add(new Paper()
                {
                    Description = drv[0].ToString(),
                    RunCharge = float.Parse(drv[16].ToString()),
                    LbsPerThousand = int.Parse(drv[11].ToString()),
                    Extended = float.Parse(drv[16].ToString())
                });
            }

            c.Close();

            PaperGrid.ItemsSource = null;
            PaperGrid.Items.Clear();
            PaperGrid.ItemsSource = Papers;

            SumRowTotals();
        }

        private void SumRowTotals()
        { QuoteTotal = 0.00D; for (int r = 0; r < DVPaper.Count; r++) { QuoteTotal += double.Parse(DVPaper[r][16].ToString()); } }

        private void txtQty1_GotFocus(object sender, RoutedEventArgs e) { txtQty1.SelectAll(); }
        private void txtQty2_GotFocus(object sender, RoutedEventArgs e) { txtQty2.SelectAll(); }
        private void txtQty3_GotFocus(object sender, RoutedEventArgs e) { txtQty3.SelectAll(); }
        private void txtQty4_GotFocus(object sender, RoutedEventArgs e) { txtQty4.SelectAll(); }

        private void cmbProjectType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        { /*LoadRollWidth();*/ }

        private void LoadPage2Combos()
        {
            LoadSubWT();
            LoadColors();
            LoadRollWidth();
        }

        private void txtItemType_DropDownOpened(object sender, System.EventArgs e)
        {
            PickMsg.Visibility = Visibility.Hidden;
        }

        //private void cmbPaperType_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        //{ }

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
        }

        private void lstSelected_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
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
                        Ink Ink = new Ink(PRESSSIZE, QUOTE_NUM, PARTS, COLLATORCUT); Ink.ShowDialog();
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
                        Finishing finishing = new Finishing(QUOTE_NUM, PRESSSIZE, COLLATORCUT, ROLLWIDTH); finishing.ShowDialog();
                        break;
                    }

                case "Converting":
                    {
                        Converting converting = new Converting(PRESSSIZE, QUOTE_NUM); converting.ShowDialog();
                        break;
                    }

                case "PrePress":
                    {
                        Views.PrePress prepress = new Views.PrePress(PRESSSIZE, QUOTE_NUM); prepress.ShowDialog();
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

                case "Shipping":
                    {
                        Shipping sh = new Shipping(QUOTE_NUM); sh.ShowDialog();
                        break;
                    }

                case "Cases":
                    {
                        Views.Cases cases = new Views.Cases(QUOTE_NUM); cases.ShowDialog();
                        break;
                    }

                case "Carbon":
                    {
                        Carbon carbon = new Carbon(QUOTE_NUM); carbon.ShowDialog();
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

            dt = new DataTable("Details");
            conn = new SqlConnection(ConnectionString);
            da = new SqlDataAdapter(cmd, conn); da.Fill(dt);
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
                string V10= dt.Rows[i]["Value10"].ToString();

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

                // Fix for PrePress, which has nothing but two non-numeric parameters
                if ((dt.Rows[i]["Category"].ToString() == "PrePress") && (dt.Rows[i]["Value1"].ToString().Length > 0))
                {
                    B1 = true; I1 = 1;
                    PrePressPerMilChg = float.Parse(dt.Rows[i]["PerThousandChg"].ToString());
                }

                Int32 RowTotal = 0;

                string? CheckCategory = dt.Rows[i]["Category"].ToString();

                if (CheckCategory.ToUpper().Contains("BASE"))     { RowTotal = 1; }
                if (CheckCategory.ToUpper().Contains("BACKER"))   { RowTotal = 1; }
                if (CheckCategory.ToUpper().Contains("SECURITY")) { RowTotal = 1; }
                if (CheckCategory.ToUpper().Contains("CARBON"))   { RowTotal = 1; }

                if (B1 == true) { RowTotal += I1; }
                if (B2 == true) { RowTotal += I2; }
                if (B3 == true) { RowTotal += I3; }
                if (B4 == true) { RowTotal += I4; }
                if (B5 == true) { RowTotal += I5; }
                if (B6 == true) { RowTotal += I6; }
                if (B7 == true) { RowTotal += I7; }
                if (B8 == true) { RowTotal += I8; }
                if (B9 == true) { RowTotal += I9; }
                if (B10 == true){ RowTotal += I10; }

              //string suffix = (RowTotal > 0) ? " " + ((char)0x221A).ToString() : string.Empty;
                string suffix = (RowTotal > 0) ? " " + "🗹" : string.Empty;

                if (i <= lstSelected.Items.Count)
                {
                    lstSelected.Items[i] = lstSelected.Items[i].ToString() + (" ").PadRight(20);
                    lstSelected.Items[i] = lstSelected.Items[i].ToString().Substring(0, 17) + suffix;
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
            if (Adding) lstSelected.Items.Clear();

            // Load any selected categories and remove them from the "Available" listbox

            string cmd = $"SELECT CATEGORY, Amount FROM [ESTIMATING].[dbo].[QUOTE_DETAILS] WHERE QUOTE_NUM = '{QUOTE_NUM}' ORDER BY Sequence";
            dt = new DataTable("Details");
            conn = new SqlConnection(ConnectionString);
            da = new SqlDataAdapter(cmd, conn); da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string s = dt.Rows[i]["CATEGORY"].ToString();
                if (s == "PressNum") { s = "Press Numbering"; }  // Is this kludge still needed?
                bool found = false;
                if (lstSelected.Items.Count > 0)
                { for (int j = 0; j < lstSelected.Items.Count; j++) { if (lstSelected.Items[j].ToString().Contains(s)) { found = true; } } }
                if (!found) lstSelected.Items.Add(s);
                lstAvailable.Items.Remove(s);
            }
            conn.Close();
            Adding = false;

            CheckRows();  // Add a checkmark at the end for items with parameters that have been selected

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

            if (Category == "Press Numbering") Category = "PressNum";

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
            QuoteLookup ql = new("MakeCopy");
            ql.ShowDialog();
            QUOTE_NUM = ql.SelQuote; ql.Close();

            if (QUOTE_NUM == null || QUOTE_NUM.Length == 0) return;

            GetQuote();
            //            NewQuote();

            txtQuoteNum.Focus();
            txtQuoteNum.Background = Brushes.Red;
            txtQuoteNum.Foreground = Brushes.Yellow;
        }

        private void cmbPressSize_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (Adding) return;

            if (lstSelected.Items.Count == 0 || cmbPressSize.SelectedIndex == -1) return;
            if (cmbPressSize.Text == cmbPressSize.SelectedItem.ToString()) return;
            if (MessageBox
            .Show("This will delete any selected features; continue?",
            "Feature selection depends on Press Size",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question,
            MessageBoxResult.No) != MessageBoxResult.No)
            {
                string cmd = $"DELETE [ESTIMATING].[dbo].[QUOTE_DETAILS] WHERE QUOTE_NUM = '{QUOTE_NUM}'";
                conn = new SqlConnection(ConnectionString);
                conn.Open();
                scmd = new SqlCommand(cmd, conn); scmd.ExecuteNonQuery();
                conn.Close();
                lstSelected.Items.Clear();
                LoadAvailableCategories();
            }
        }

        private void btnSaveQuote_Click(object sender, RoutedEventArgs e) { SaveQuote(); }

        private void SaveQuote()
        {
            string cmd = $"DELETE [ESTIMATING].[dbo].[QUOTES] WHERE QUOTE_NUM = '{QUOTE_NUM}'";
            SqlConnection conn = new SqlConnection(ConnectionString); conn.Open();
            scmd = new SqlCommand(cmd, conn); scmd.ExecuteNonQuery(); conn.Close();
            string Today = DateTime.Now.ToString("MM/dd/yyyy");
            cmd = "INSERT INTO [ESTIMATING].[dbo].[QUOTES] (   QUOTE_NUM,     CUST_NUM,     PARTS,    PAPERTYPE,     ROLLWIDTH,   LINEHOLES,   PRESSSIZE,     COLLATORCUT,     PROJECTTYPE,     Date,     Amount      )"
                + $"                                 VALUES ( '{QUOTE_NUM}', '{CUST_NUMB}', {PARTS}, '{PAPERTYPE}', '{ROLLWIDTH}', 0,         '{PRESSSIZE}', '{COLLATORCUT}', '{ProjectType}', '{Today}', {QuoteTotal} )";
            //Clipboard.SetText(cmd);
            conn = new SqlConnection(ConnectionString); conn.Open();
            scmd.CommandText = cmd; scmd.Connection = conn; scmd.ExecuteNonQuery(); conn.Close();

            Close(); // Should I just save and continue?
        }

        private void btnCancelQuote_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Delete this new quote?", "Erase this new quote?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                string cmd = $"DELETE QUOTE_DETAILS WHERE QUOTE_NUM = '{QUOTE_NUM}'";
                conn = new SqlConnection(ConnectionString);
                scmd = new SqlCommand(cmd, conn); scmd.ExecuteNonQuery();
                MessageBox.Show("Done");
                Close();
            }
        }

        private void Page4_GotFocus(object sender, RoutedEventArgs e) { DoPage4(); }

        private void DoPage4()
        {
            // IF NOT DEBUGGING, DON'T SHOW TOOLTIPS ON TEXTBOXES:

            // if(DEBUG) { foreach (Control x in this.Page4.ChildrenOfType<TextBox>) { if (x is TextBox) { ((TextBox)x).ToolTip = String.Empty; } } }

            SqlConnection conn = new SqlConnection(ConnectionString);

            //----------------------------------------------------
            // Create the DVFeat table displayed on Page 4
            // The bottom of the page is populated in Labor_Calc()
            //----------------------------------------------------

            string cmd = "";
            DataTable dt = new DataTable("Feat");
            SqlDataAdapter da = new SqlDataAdapter();

            // --------------------------------------------------------------------
            // Recently (12/12/2023) added:
            //  Retrieve Freight from QUOTE_DETAILS and store in DVFeat[0]["Cost"]
            //  Retrieve Press   from QUOTE_DETAILS and store in DVFeat[0]["Press"]
            // --------------------------------------------------------------------

            float Freight = 0;
            SqlDataAdapter daf = new SqlDataAdapter("SELECT Value4 from [ESTIMATING].[dbo].[QUOTE_DETAILS] WHERE Category = 'Shipping'", conn);
            DataTable dt71 = new DataTable(); daf.Fill(dt71);
            if(dt71.Rows.Count > 0) { Freight = float.Parse(dt71.Rows[0]["Value4"].ToString()); }

            float PunchCost = 0;
            SqlDataAdapter dag = new SqlDataAdapter("SELECT Value1 from [ESTIMATING].[dbo].[QUOTE_DETAILS] WHERE Category = 'Punching'", conn);
            DataTable dt72 = new DataTable(); dag.Fill(dt72);
            if (dt72.Rows.Count > 0) {PunchCost = float.Parse(dt72.Rows[0]["Value1"].ToString()); }

            cmd = "SELECT "
                + " Category, "
                + " CONVERT(Numeric(10, 2), 0.00) AS Cost, "
                + " PrePress, "
                + " CONVERT(Numeric(10, 2), 0.00) AS Press, "
                + " Converting, "
                + " CONVERT(Numeric(10, 2), 0.00) AS Finishing, "
                + " PressSlowdown, "
                + " ConvertingSlowdown, "
                + " FinishingSlowdown "
                + " FROM [ESTIMATING].[dbo].[QUOTE_DETAILS] "
                + $" WHERE QUOTE_NUM = '{QUOTE_NUM}'"
                + " ORDER BY SEQUENCE";

            SqlCommand sc = new SqlCommand(cmd, conn);
            da.SelectCommand = sc;
            da.Fill(dt);

            DVFeat = dt.DefaultView;
            dgFeatures.ItemsSource = null;
            dgFeatures.Items.Clear();
            dgFeatures.ItemsSource = DVFeat;

            DVFeat.RowFilter = "Category = 'Converting'";
//            if (DVFeat.Count > 0) { float conv = float.Parse(DVFeat[0]["Converting"].ToString()); DVFeat[0]["Converting"] = SelectedQty * conv; }
            if (DVFeat.Count > 0) { DVFeat[0]["Converting"] = CollatorMaterialCost; }

            DVFeat.RowFilter = "Category = 'Shipping'";
            if ((DVFeat.Count > 0) && (Freight > 0)) { DVFeat[0]["Cost"] = Freight; }

            DVFeat.RowFilter = "Category = 'Punching'";
            if ((DVFeat.Count > 0) && (PunchCost > 0)) { DVFeat[0]["Press"] = PunchCost; }

            DVFeat.RowFilter = "";

            // ------------------------------------------------------------------------
            // Next, use feature parameters entered so far to populate the DVFeat table
            // Retrieve some values from the Ink Color feature
            // ------------------------------------------------------------------------

            float Amount = 0.0F; float Make_Ready_Ink = 0.0F;

            cmd = $"SELECT Amount, Make_Ready_Ink FROM [ESTIMATING].[dbo].[QUOTE_DETAILS] WHERE QUOTE_NUM = '{QUOTE_NUM}' AND CATEGORY = 'Ink Color'";
            SqlDataAdapter da51 = new SqlDataAdapter(cmd, conn);
            DataTable dt51 = new DataTable("Features");
            da51.Fill(dt51);
            if (dt51.Rows.Count > 0)
            {
                Amount = float.Parse(dt51.Rows[0]["Amount"].ToString());
                Make_Ready_Ink = float.Parse(dt51.Rows[0]["Make_Ready_Ink"].ToString());
            }

            // -----------------------------------------------
            // Retrieve some values from the Perfing feature
            // -----------------------------------------------

            float PressSetupLaborPerfing = 0.0F; float PressSetupMin = 0.0F;

            cmd = $"SELECT TotalFlatChg, PressSetupMin FROM [ESTIMATING].[dbo].[QUOTE_DETAILS] WHERE QUOTE_NUM = '{QUOTE_NUM}' AND CATEGORY = 'Perfing'";
            SqlDataAdapter da52 = new SqlDataAdapter(cmd, conn);
            DataTable dt52 = new DataTable("Perf");
            da52.Fill(dt52);
            if (dt52.Rows.Count > 0)
            {
                PressSetupLaborPerfing = float.Parse(dt52.Rows[0]["TotalFlatChg"].ToString());
                PressSetupMin = float.Parse(dt52.Rows[0]["PressSetupMin"].ToString());    // convert to fraction of an hour before adding
            }

            // Sum the SLOWDOWN columns' values into the cells at the bottom of page 4
            PressSlowdown = 0;
            CollatorSlowdown = 0;
            BinderySlowdown = 0;

            for (int i = 0; i < DVFeat.Count; i++)      // Sum all 3 slowdown percentages
            {
                PressSlowdown += float.Parse(DVFeat[i]["PressSlowdown"].ToString());
                CollatorSlowdown += float.Parse(DVFeat[i]["ConvertingSlowdown"].ToString());
                BinderySlowdown += float.Parse(DVFeat[i]["FinishingSlowdown"].ToString());

                // If these columns are initially loaded as zeros, this isn't needed:
                // if (DVFeat[i]["Category"].ToString() == "Converting") { DVFeat[i]["Converting"] = 0.0F; }

                // Probably better not to insert values into the other columns when creating the DVFeat DataView
                if (DVFeat[i]["Category"].ToString() == "Ink Color") { DVFeat[i]["Press"] = (SelectedQty * Amount) + Make_Ready_Ink; }
                if (DVFeat[i]["Category"].ToString() == "Perfing") { DVFeat[i]["Press"] = PressSetupLaborPerfing; }  // from the TotalFlatCharge column in the "Perfing" row of Quote_Details
            }

            // TODO: Apply sums of adjustments to calculated totals
            AdjPressRunTime = PressRunTime * (1.0F + (PressSlowdown / 100.0F));
            AdjPressRunCost = PressRunCost * (1.0F + (PressSlowdown / 100.0F));

            AdjCollRunTime = CollRunTime * (1.0F + (CollatorSlowdown / 100.0F));
            AdjCollRunCost = CollRunCost * (1.0F + (CollatorSlowdown / 100.0F));

            AdjBindRunTime = BindRunTime * (1.0F + (BinderySlowdown / 100.0F));
            AdjBindRunCost = BindRunCost * (1.0F + (BinderySlowdown / 100.0F));

            //-----------------------------------------------------
            // Finishing
            //-----------------------------------------------------
            cmd = "SELECT CONVERT(integer,Value1) AS Books,  " +
                         " CONVERT(integer,Value2) AS Cellos, " +
                         " Value6 AS LinearInchCostCello,     " +
                         " Value7 AS LinearInchCostBooks      " +
                   "  FROM [ESTIMATING].[dbo].[QUOTE_DETAILS] " +
                  $" WHERE QUOTE_NUM = '{QUOTE_NUM}' AND Category = 'Finishing'";

            SqlDataAdapter da8 = new SqlDataAdapter(cmd, conn);
            DataTable dt11 = new DataTable("BookCello"); da8.Fill(dt11); DataView dvx = dt11.DefaultView;

            int BookSet = 0; int CelloSet = 0;
            float LinearInchCostCello = 0.0F; float LinearInchCostBooks = 0.0F; float TotalBookCost = 0.0F; float TotalCelloCost = 0.0F;

            if (dvx.Count > 0)  // Only if there WAS a "Finishing" feature row:
            {
                BookSet = int.Parse(dvx[0]["Books"].ToString());
                CelloSet = int.Parse(dvx[0]["Cellos"].ToString());
                LinearInchCostCello = float.Parse(dvx[0]["LinearInchCostCello"].ToString());
                LinearInchCostBooks = float.Parse(dvx[0]["LinearInchCostBooks"].ToString());

                if (BookSet > 0)
                {
                    int NumBooks = SelectedQty / BookSet;
                    StringToNumber sTOn2 = new StringToNumber();
                    float collCuts2 = sTOn2.Convert(COLLATORCUT);
                    float linearInches = (float)NumBooks * collCuts2;
                    TotalBookCost = linearInches * LinearInchCostBooks;
                }

                if (CelloSet > 0)
                {
                    int NumCellos = SelectedQty / CelloSet;
                    StringToNumber sTOn3 = new StringToNumber();
                    float collCuts3 = sTOn3.Convert(COLLATORCUT);
                    float linearInches = (float)NumCellos * collCuts3;
                    TotalCelloCost = linearInches * LinearInchCostCello;
                }

                BinderyMaterialCost = TotalBookCost + TotalCelloCost;           // Which I'm told will "almost never" happen...

                //TODO: Copy this to the Finishing column of the Finishing row of DVFeat, then set it to zero
                DVFeat.RowFilter = "Category = 'Finishing'";
                DVFeat[0]["Finishing"] = BinderyMaterialCost;
                BinderyMaterialCost = 0.00F;
                DVFeat.RowFilter = "";  // Turn the filter off
            }

            //-----------------------------------------------------
            // Collator 
            //-----------------------------------------------------

            // Can I just move the next 15 lines of code down below the calculation of the MATERIALS column and add to CollatorMaterialCost?

            // Use the parameters passed from the QUOTE_DETAIL record to calculate CollatorMaterialCost:
            CollatorMaterialCost = 0;
            cmd = $"SELECT * FROM [ESTIMATING].[dbo].[QUOTE_DETAILS] WHERE CATEGORY = 'CONVERTING' AND Value8 > 0";        // add "and transfer tape was selected"
            SqlDataAdapter da21 = new SqlDataAdapter(cmd, conn);
            DataTable dt21 = new DataTable("conv"); da21.Fill(dt21);
            if (dt21.Rows.Count > 0)
              { cmd = $"SELECT CONV_MATL FROM [ESTIMATING].[dbo].[FEATURES] WHERE CATEGORY = 'CONVERTING' AND F_TYPE = 'TRANSFER TAPE'";
                SqlDataAdapter da12 = new SqlDataAdapter(cmd, conn);
                DataTable dt12 = new DataTable("conv"); da12.Fill(dt12);
                DataView dvy = dt12.DefaultView;
                float convmatl = float.Parse(dvy[0]["CONV_MATL"].ToString());
                StringToNumber sTOn = new StringToNumber();
                float collCuts = sTOn.Convert(COLLATORCUT);
                float LinearInches = ((float)SelectedQty * collCuts) / 12.0F;
                CollatorMaterialCost = convmatl * LinearInches;                                     // <======

                InkCalc();
              }

            // Sum the column values for DVFeat and display under MATERIALS at the bottom of Page 4

            PressMaterialCost    = 0.0F;
//          CollatorMaterialCost = 0.0F;                                                          // <====== Should I not zero this out, since it might have been calculated immediately above?
            BinderyMaterialCost  = 0.0F;
            PrePressMaterialCost = 0.0F;
//          OEMaterialCost       = 0.0F;
            ShippingMaterialCost = 0.0F;

            for (int i = 0; i < DVFeat.Count; i++) 
            {
                PressMaterialCost += float.Parse(DVFeat[i]["Press"].ToString());
//              CollatorMaterialCost += float.Parse(DVFeat[i]["Converting"].ToString());             // <======  This shouldn't be 24...
                BinderyMaterialCost += float.Parse(DVFeat[i]["Finishing"].ToString());
                PrePressMaterialCost += float.Parse(DVFeat[i]["PrePress"].ToString());
//              OEMaterialCost += float.Parse(DVFeat[i]["????"].ToString());
                ShippingMaterialCost += float.Parse(DVFeat[i]["Cost"].ToString());
            }

            TotalLabor
            = PressSetupCost
            + CollSetupCost
            + BindSetupCost
            + PreRunCost
            + OERunCost
            + ShipCost
            + AdjPressRunCost
            + AdjCollRunCost
            + AdjBindRunCost;

            TotalMaterials
            = PressMaterialCost
            + CollatorMaterialCost
            + BinderyMaterialCost
            + PrePressMaterialCost
            + OEMaterialCost
            + ShippingMaterialCost;

            // Add paper costs:
            for (int i = 0; i < dvPaper.Count; i++) { TotalMaterials += float.Parse(dvPaper[i]["PaperCost"].ToString()); }

            TotalTotal = TotalLabor + TotalMaterials;

            if (SelectedQty == Qty1) { CPM1a = TotalTotal / (Qty1 / 1000); }
            if (SelectedQty == Qty2) { CPM2a = TotalTotal / (Qty2 / 1000); }
            if (SelectedQty == Qty3) { CPM3a = TotalTotal / (Qty3 / 1000); }
            if (SelectedQty == Qty4) { CPM4a = TotalTotal / (Qty4 / 1000); }

        }
        // Currently 250 lines of code.

        private void MarkupCalc(int selnum)
        {
            switch(selnum)
            {
                case 1:
                    SellPrice1 = TotalTotal;
                    if (MkUpPct1 > 0) { SellPrice1 = TotalTotal * ( 1 + MkUpPct1 / 100.0F ); }
                    if (MkUpAmt1 > 0) { SellPrice1 = TotalTotal + MkUpAmt1; }
                    break;

                case 2:
                    SellPrice2 = TotalTotal;
                    if (MkUpPct2 > 0) { SellPrice2 = TotalTotal * (1 + MkUpPct2 / 100.0F); }
                    if (MkUpAmt1 > 0) { SellPrice2 = TotalTotal + MkUpAmt2; }
                    break;

                case 3:
                    SellPrice3 = TotalTotal;
                    if (MkUpPct3 > 0) { SellPrice3 = TotalTotal * (1 + MkUpPct3 / 100.0F); }
                    if (MkUpAmt1 > 0) { SellPrice3 = TotalTotal + MkUpAmt2; }
                    break;

                case 4:
                    SellPrice4 = TotalTotal;
                    if (MkUpPct4 > 0) { SellPrice4 = TotalTotal * (1 + MkUpPct4 / 100.0F); }
                    if (MkUpAmt1 > 0) { SellPrice4 = TotalTotal + MkUpAmt4; }
                    break;
            }
        }

        private void InkCalc()
        {
            if (DVFeat == null) return;

            string cmdstr = $"SELECT Amount, MAKE_READY_INK FROM [ESTIMATING].[dbo].[QUOTE_DETAILS] WHERE QUOTE_NUM = '{QUOTE_NUM}' AND Category = 'Ink Color'";
            SqlDataAdapter da9 = new SqlDataAdapter(cmdstr, conn);
            DataTable dt14 = new DataTable("CostPerInch"); da9.Fill(dt14);
            DataView dvz = dt14.DefaultView;

            if (dvz.Count == 0) return;     // There is no INK entry in QUOTE_DETAILS

            float f = float.Parse(dvz[0]["Amount"].ToString());
            decimal a = (decimal)f;
            decimal b = decimal.Parse(PARTS.ToString());
            decimal c = decimal.Parse(DecimalCollatorCut.ToString());
            decimal d = decimal.Parse(SelectedQty.ToString());
            float MakeReady = float.Parse(dvz[0]["MAKE_READY_INK"].ToString());
            decimal InkRunCost = (a * b * c * d) + decimal.Parse(MakeReady.ToString());
            DVFeat[0][1] = InkRunCost;
        }

        private void txtQty1_LostFocus(object sender, RoutedEventArgs e)
        {
            Q1();
        }

        private void Q1()
        {
            if (Qty1 == 0) return;
            SelectedQty = Qty1;
            GridCalc();
            QTY1a = SelectedQty;
            CPM1a = float.Parse(QuoteTotal.ToString()) / float.Parse(SelectedQty.ToString());
            CPM1a = CPM1a * 1000.00F;
            Labor_Calc();
            InkCalc();
        }

        private void txtQty2_LostFocus(object sender, RoutedEventArgs e)
        {
            Q2();
        }

        private void Q2()
        {
            if (Qty2 == 0) { QTY2a = 0; CPM2a = 0.0F; return; }
            SelectedQty = Qty2;
            GridCalc();
            QTY2a = SelectedQty;
            CPM2a = float.Parse(QuoteTotal.ToString()) / float.Parse(SelectedQty.ToString());
            CPM2a = CPM2a * 1000.00F;
            Labor_Calc();
            InkCalc();
        }

        private void txtQty3_LostFocus(object sender, RoutedEventArgs e)
        {
            Q3();
        }

        private void Q3()
        {
            if (Qty3 == 0) { QTY3a = 0; CPM3a = 0.0F; return; }
            SelectedQty = Qty3;
            GridCalc();
            QTY3a = SelectedQty;
            CPM3a = float.Parse(QuoteTotal.ToString()) / float.Parse(SelectedQty.ToString());
            CPM3a = CPM3a * 1000.00F;
            Labor_Calc();
            InkCalc();
        }

        private void txtQty4_LostFocus(object sender, RoutedEventArgs e)
        {
            Q4();
        }

        private void Q4()
        {
            if (Qty4 == 0) { QTY4a = 0; CPM4a = 0.0F; return; }
            SelectedQty = Qty4;
            GridCalc();
            QTY4a = SelectedQty;
            CPM4a = float.Parse(QuoteTotal.ToString()) / float.Parse(SelectedQty.ToString());
            CPM4a = CPM4a * 1000.00F;
            Labor_Calc();
            InkCalc();
        }

        private void CheckFirst()
        {
            if (ComboSel1.Length == 0) { Ext1.IsEnabled = false; Ext1c.IsEnabled = false; return; }
            MkUpAmt1 = 0; MkUpPct1 = 0;
            if (ComboSel1.Substring(ComboSel1.Length - 1, 1) == "%")
            { Ext1.IsEnabled = true; Ext1c.IsEnabled = false; Ext1.Focus(); }
            else
            { Ext1.IsEnabled = false; Ext1c.IsEnabled = true; Ext1c.Focus(); }
        }

        private void CheckSecond()
        {
            if (ComboSel2.Length == 0) { Ext2.IsEnabled = false; Ext2c.IsEnabled = false; return; }
            MkUpAmt2 = 0; MkUpPct2 = 0;
            if (ComboSel2.Substring(ComboSel2.Length - 1, 1) == "%")
            { Ext2.IsEnabled = true; Ext2c.IsEnabled = false; Ext2.Focus(); }
            else
            { Ext2.IsEnabled = false; Ext2c.IsEnabled = true; Ext2c.Focus(); }
        }

        private void CheckThird()
        {
            if (ComboSel3.Length == 0) { Ext3.IsEnabled = false; Ext3c.IsEnabled = false; return; }
            MkUpAmt3 = 0; MkUpPct3 = 0;
            if (ComboSel3.Substring(ComboSel3.Length - 1, 1) == "%")
            { Ext3.IsEnabled = true; Ext3c.IsEnabled = false; Ext3.Focus(); }
            else
            { Ext3.IsEnabled = false; Ext3c.IsEnabled = true; Ext3c.Focus(); }
        }

        private void CheckFourth()
        {
            if (ComboSel4.Length == 0) { Ext4.IsEnabled = false; Ext4c.IsEnabled = false; return; }
            MkUpAmt4 = 0; MkUpPct4 = 0;
            if (ComboSel4.Substring(ComboSel4.Length - 1, 1) == "%")
            { Ext4.IsEnabled = true; Ext4c.IsEnabled = false; Ext4.Focus(); }
            else
            { Ext4.IsEnabled = false; Ext4c.IsEnabled = true; Ext4c.Focus(); }
        }

        private void MarkupBasis1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Already = true;
            CheckFirst();
        }

        private void MarkupBasis2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Already = true;
            CheckSecond();
        }

        private void MarkupBasis3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckThird();
        }

        private void MarkupBasis4_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckFourth();
        }

        // To add weight and selected cost in the PAPER grid on page 4, add those two columns here and load the values around line 1567
        public class Paper
        {
            public string Description { get; set; }
            public float RunCharge { get; set; }
            public float LbsPerThousand { get; set; }
            public float Extended { get; set; }
        }

        //private List<Paper> LoadPaperCosts()
        //{   
        //    List<Paper> PaperList = new List<Paper>();
        //    PaperList.Add(new Paper() { Description = "White PV - CB Coated back NCR",  RunCharge = 19.92F, LbsPerThousand = 9.97F, Extended = 12.43F });
        //    PaperList.Add(new Paper() { Description = "Canary PV - CFB Coated frt/bac", RunCharge = 23.40F, LbsPerThousand = 8.73F, Extended = 14.01F });
        //    PaperList.Add(new Paper() { Description = "Pink PV - CF Coated front NCR",  RunCharge = 14.17F, LbsPerThousand = 9.60F, Extended = 9.07F });

        //    return PaperList;
        //}

        public class Labor
        {
            public string Department { get; set; }
            public float SetupTime { get; set; }
            public float SetupCost { get; set; }
            public float RunTime { get; set; }
            public float RunCost { get; set; }
        }

        private List<Labor> LoadLaborCosts()
        {
            List<Labor> laborItems = new List<Labor>();
            laborItems.Add(new Labor() { Department = "Press", SetupTime = 0.35F, SetupCost = 22.75F, RunTime = 0.30F, RunCost = 19.50F });
            laborItems.Add(new Labor() { Department = "Collator", SetupTime = 0.35F, SetupCost = 14.00F, RunTime = 0.09F, RunCost = 3.60F });
            laborItems.Add(new Labor() { Department = "Bindery", SetupTime = 0.00F, SetupCost = 0.00F, RunTime = 0.00F, RunCost = 0.00F });
            laborItems.Add(new Labor() { Department = "Prep", SetupTime = 1.61F, SetupCost = 56.35F, RunTime = 0.00F, RunCost = 0.00F });
            laborItems.Add(new Labor() { Department = "Packaging", SetupTime = 0.00F, SetupCost = 0.00F, RunTime = 0.00F, RunCost = 0.00F });

            return laborItems;
        }

        private void Select1_Click(object sender, RoutedEventArgs e)
        {
            QBO1.Focus();
            if (Qty1 > 0)
            {
                DisplayQuantity = "(using " + Qty1.ToString() + ")";
                Already = true;
                Q1();
                //SellPrice1 = ((float)Qty1 / 1000.0F) * float.Parse(CPM1a.ToString());
                //if (MkUpPct1 > 0.0F) { SellPrice1 *= (100.0F + MkUpPct1) / 100.0F; }
                //if (MkUpAmt1 > 0.0F) { SellPrice1 += MkUpAmt1; }
                DoPage4();
                MarkupCalc(1);
            }
        }

        private void Select2_Click(object sender, RoutedEventArgs e)
        {
            QBO2.Focus();
            if (Qty2 > 0)
            {
                DisplayQuantity = "(using " + Qty2.ToString() + ")";
                Already = true;
                Q2();
                //SellPrice2 = ((float)Qty2 / 1000.0F) * float.Parse(CPM2a.ToString());
                //if (MkUpPct2 > 0.0F) { SellPrice2 *= (100.0F + MkUpPct2) / 100.0F; }
                //if (MkUpAmt2 > 0.0F) { SellPrice2 += MkUpAmt2; }
                DoPage4();
                MarkupCalc(2);
            }
        }

        private void Select3_Click(object sender, RoutedEventArgs e)
        {
            QBO3.Focus();
            if (Qty3 > 0)
            {
                DisplayQuantity = "(using " + Qty3.ToString() + ")";
                Already = true;
                Q3();
                //SellPrice3 = ((float)Qty3 / 1000.0F) * float.Parse(CPM3a.ToString());
                //if (MkUpPct3 > 0.0F) { SellPrice3 *= (100.0F + MkUpPct3) / 100.0F; }
                //if (MkUpAmt3 > 0.0F) { SellPrice3 += MkUpAmt3; }
                DoPage4();
                MarkupCalc(3);
            }
        }

        private void Select4_Click(object sender, RoutedEventArgs e)
        {
            QBO4.Focus();
            if (Qty4 > 0)
            {
                DisplayQuantity = "(using " + Qty4.ToString() + ")";
                Already = true;
                Q4();
                //SellPrice4 = ((float)Qty4 / 1000.0F) * float.Parse(CPM4a.ToString());
                //if (MkUpPct4 > 0.0F) { SellPrice4 *= (100.0F + MkUpPct4) / 100.0F; }
                //if (MkUpAmt4 > 0.0F) { SellPrice4 += MkUpAmt4; }
                DoPage4();
                MarkupCalc(4);
            }
        }

        private void Page2_GotFocus(object sender, RoutedEventArgs e)
        {
            string PaperSize = "8 1/2 X 11";    // Where does this come from?
            Desc = ProjectType + "  ";
            Desc += PARTS.ToString() + " part  ";
            Desc += PaperSize + "  " + PAPERTYPE;
        }

        private void btnLaborCalc_Click(object sender, RoutedEventArgs e)
        {
            Labor_Calc();
        }

        private void Labor_Calc()
        {

            // See if DVFeat has already been created and is accessible

            if (DVFeat is null) return;

            string rw = ROLLWIDTH;

            float FeetPerPart = (float.Parse(SelectedQty.ToString()) * DecimalCollatorCut) / 12.0F;

            string cmd = "SELECT * FROM [ESTIMATING].[dbo].[Press_Speeds] ";
            string WhereClause = $" WHERE Cylinder = '{PRESSSIZE}' AND {FeetPerPart} BETWEEN MinQty AND MaxQty"; cmd += WhereClause;

            da = new SqlDataAdapter(cmd, ConnectionString); dt = new DataTable("Speeds"); da.Fill(dt); DataView dv1 = dt.DefaultView;
            if (dv1.Count == 0) { MessageBox.Show("No matching data in Press_Speeds table", WhereClause); }

            int FPM = int.Parse(dv1[0]["FeetPerMinute"].ToString());
            int SetupMins = int.Parse(dv1[0]["SetupMinutes"].ToString());

            // Adjust for the percent slowdown for all selected features
            da.SelectCommand.CommandText = $"SELECT SUM(PressSetupMin) AS SETUPMINUTES, SUM(SLOWDOWN_PERCENT) AS SLOWDOWN_PERCENT FROM [ESTIMATING].[dbo].[QUOTE_DETAILS] WHERE QUOTE_NUM = '{QUOTE_NUM}'";
            DataTable dt2 = new DataTable(); da.Fill(dt2); DataView dv2 = dt2.DefaultView;

            if (dv2.Count == 0) return; // If there are not yet any rows in the QUOTE_DETAILS table     

            int SumSetupMin = 0;
            int SumSlowDown = 0;

            if (dv2.Count > 0)
            {
                if ((dv2[0]["SETUPMINUTES"].ToString() != null)
                 && (dv2[0]["SETUPMINUTES"].ToString().Length > 0))
                {
                    SumSetupMin = int.Parse(dv2[0]["SETUPMINUTES"].ToString());
                    SumSlowDown = int.Parse(dv2[0]["SLOWDOWN_PERCENT"].ToString());
                }
            }

            float correction = (100.0F - (float)SlowDownPct) / 100.0F;
            if (correction < 0) { correction = 1.0F; }
            FPM = (int)(FPM * correction);
            float MinutesPerPart = FeetPerPart / (float)FPM;
            int TotalMinutes = (int)(MinutesPerPart * (float)PARTS);

            float hrs = TotalMinutes / 60.0F;
            hrs += .01F;    // In case hrs = .50 (.5 rounds down, .51 rounds up), force it to round up 
            Hrs = (float)Math.Round(hrs, 1);

            int NumWide = int.Parse(NumWidePress.ToString());

            Hrs = Hrs / NumWide;

            DollarsPerHr = Hrs * float.Parse(dv1[0]["DollarsPerHr"].ToString());
            PressRunTime = Hrs;
            PressRunCost = DollarsPerHr;

            float SetupHrs = ((float)SumSetupMin + (float)SetupMins) / 60.0F;
            PressSetupTime = SetupHrs;

            //------------------------------------------------------------------------------------------------------------------------------
            // 12/11 9:30am time was being addded twice:
            // PressSetupTime for Perfing was already counted when we added up all PressSetupMin values for all QUOTE_DETAIL records
            // Perfing adjustment:
            //float PressSetupMin = 0.0F;
            //cmd = $"SELECT PressSetupMin FROM [ESTIMATING].[dbo].[QUOTE_DETAILS] WHERE QUOTE_NUM = '{QUOTE_NUM}' AND CATEGORY = 'Perfing'";
            //SqlDataAdapter da53 = new SqlDataAdapter(cmd, conn);
            //DataTable dt53 = new DataTable("Perf");
            //da53.Fill(dt53);
            //if (dt53.Rows.Count > 0) { PressSetupMin = float.Parse(dt53.Rows[0]["PressSetupMin"].ToString()); }
            //PressSetupTime += (PressSetupMin / 60.0F);    // convert to fraction of an hour first...
            //------------------------------------------------------------------------------------------------------------------------------

            float SetupDollars = float.Parse(dv1[0][7].ToString()) * SetupHrs;
            PressSetupCost = SetupDollars;

            cmd = "SELECT * FROM [ESTIMATING].[dbo].[Collator_Speeds] WHERE ";
            WhereClause = $" CutSize = '{COLLATORCUT}' AND {SelectedQty} BETWEEN MinCutQty AND MaxCutQty";
            cmd += WhereClause;
            da = new SqlDataAdapter(cmd, ConnectionString);
            dt = new DataTable("Speeds"); da.Fill(dt); DataView dv3 = dt.DefaultView;

            CollRunTime = 0; CollRunCost = 0; int SetupMin = 0; CollSetupCost = 0;
            if (dv3.Count > 0)
            {
                CollRunTime = SelectedQty / float.Parse(dv3[0]["StdCutsPerHr"].ToString());
                CollRunCost = CollRunTime * float.Parse(dv3[0]["DollarsPerHr"].ToString());
                SetupMin = int.Parse(dv3[0]["SetupMin"].ToString());
            }

            // **************************************************
            // ** Calculate SetupRunTime and SetupRunCost here **
            // **************************************************
            // What was this for?
            // int NumberOfRollChanges = float.Parse(dv3[0]["MaxRollChgQty"].ToString()) / (float)SelectedQty;

            cmd = $"SELECT SUM(CollSetupMin) AS SETUPMINUTES FROM [ESTIMATING].[dbo].[QUOTE_DETAILS] WHERE QUOTE_NUM = '{QUOTE_NUM}'";

            DataAdapter da4 = new SqlDataAdapter(cmd, ConnectionString);
            DataSet ds = new DataSet(); DataTable dt4 = new DataTable();
            da4.Fill(ds); dt4 = ds.Tables[0]; DataView dv4 = dt4.DefaultView;

            if (dv4.Count > 0)
            { if ((dv4[0]["SETUPMINUTES"].ToString() != null) && (dv4[0]["SETUPMINUTES"].ToString().Length > 0))
               { CollSetupTime = float.Parse(dv4[0]["SETUPMINUTES"].ToString()) + (float)SetupMin; CollSetupTime /= 60.0F; }
            }

            if (dv3.Count > 0) { CollSetupCost = CollSetupTime * float.Parse(dv3[0]["DollarsPerHr"].ToString()); }  // from the Collator_Speeds table query, supra.

            // **************************************************
            // ** BINDERY                                      **
            // **************************************************

            cmd = $"SELECT * FROM [ESTIMATING].[dbo].[QUOTE_DETAILS] WHERE QUOTE_NUM = '{QUOTE_NUM}' AND CATEGORY = 'Finishing'";

            // We need BindSetupMin, Value1 (books), Value2 (Cello), Value3 (Drill Holes), Value4 (Pad) and Value5 (Trim)

            DataAdapter da5 = new SqlDataAdapter(cmd, conn);
            ds = new DataSet();
            DataTable dt5 = new DataTable();
            da5.Fill(ds);
            dt5 = ds.Tables[0];
            DataView dv5 = dt5.DefaultView;

            // *******************************************************************
            // If they didn't use the "Finishing" feature, don't do the following:
            // *******************************************************************

            if (dv5.Count > 0)
            {
                int BindSetupMinutes = int.Parse(dv5[0]["BindSetupMin"].ToString());
                int p1 = 0; int.TryParse(dv5[0]["Value1"].ToString(), out p1); int Books = p1;
                int p2 = 0; int.TryParse(dv5[0]["Value2"].ToString(), out p2); int Cello = p2;
                int p3 = 0; int.TryParse(dv5[0]["Value3"].ToString(), out p3); int DrillHoles = p3;
                int p4 = 0; int.TryParse(dv5[0]["Value4"].ToString(), out p4); int Pad = p4;
                int p5 = 0; int.TryParse(dv5[0]["Value5"].ToString(), out p5); int Trim = p5;

                if (dv5[0]["Value3"].ToString() != null && dv5[0]["Value3"].ToString() == "4 -5") { DrillHoles = 4; }

                // Bindery RunTime calculation:

                BindRunTime = 0.0F; BindRunCost = 0.0F;

                cmd = $"SELECT Production_Goal, Dollars_Per_Hr FROM [ESTIMATING].[dbo].[Finishing_Speeds] WHERE ProjectType = 'BOOK'        AND Quantity = {Books};"
                    + $"SELECT Production_Goal, Dollars_Per_Hr FROM [ESTIMATING].[dbo].[Finishing_Speeds] WHERE ProjectType = 'PAD'         AND Quantity = {Pad};"
                    + $"SELECT Production_Goal, Dollars_Per_Hr FROM [ESTIMATING].[dbo].[Finishing_Speeds] WHERE ProjectType = 'CELLO'       AND Quantity = {Cello};"
                    + $"SELECT Production_Goal, Dollars_Per_Hr FROM [ESTIMATING].[dbo].[Finishing_Speeds] WHERE ProjectType = 'DRILL HOLES' AND Quantity = {DrillHoles};"
                    + $"SELECT Production_Goal, Dollars_Per_Hr FROM [ESTIMATING].[dbo].[Finishing_Speeds] WHERE ProjectType = 'TRIM'        AND Quantity = {Trim};";

                SqlDataAdapter da01 = new SqlDataAdapter(cmd, conn);

                DataSet ds01 = new DataSet(); da01.Fill(ds01);

                DataView dv01 = ds01.Tables[0].DefaultView;
                DataView dv02 = ds01.Tables[1].DefaultView;
                DataView dv03 = ds01.Tables[2].DefaultView;
                DataView dv04 = ds01.Tables[3].DefaultView;
                DataView dv05 = ds01.Tables[4].DefaultView;

                if (Books > 0)
                {
                    float NumBooks = float.Parse(SelectedQty.ToString()) / float.Parse(Books.ToString());
                    float Production_Goal = float.Parse(dv01[0]["Production_Goal"].ToString());
                    float DollarsPerHour = float.Parse(dv01[0]["Dollars_Per_Hr"].ToString());
                    BindRunTime += (NumBooks / Production_Goal); BindRunCost += (DollarsPerHour * BindRunTime);
                }

                if (Pad > 0)
                {
                    float NumBooks = float.Parse(SelectedQty.ToString()) / float.Parse(Pad.ToString());
                    float Production_Goal = float.Parse(dv02[0]["Production_Goal"].ToString());
                    float DollarsPerHour = float.Parse(dv02[0]["Dollars_Per_Hr"].ToString());
                    BindRunTime += (NumBooks / Production_Goal); BindRunCost += (DollarsPerHour * BindRunTime);
                }

                if (Cello > 0)
                {
                    float NumBooks = float.Parse(SelectedQty.ToString()) / float.Parse(Cello.ToString());
                    float Production_Goal = float.Parse(dv03[0]["Production_Goal"].ToString());
                    float DollarsPerHour = float.Parse(dv03[0]["Dollars_Per_Hr"].ToString());
                    BindRunTime += (NumBooks / Production_Goal); BindRunCost += (DollarsPerHour * BindRunTime);
                }

                if (DrillHoles > 0)
                {
                    float NumBooks = int.Parse(SelectedQty.ToString()) / 100;
                    float Production_Goal = float.Parse(dv04[0]["Production_Goal"].ToString());
                    float DollarsPerHour = float.Parse(dv04[0]["Dollars_Per_Hr"].ToString());
                    BindRunTime += (NumBooks / Production_Goal); BindRunCost += (DollarsPerHour * BindRunTime);
                }

                if (Trim > 0)
                {
                    float NumBooks = int.Parse(SelectedQty.ToString()) / 100;
                    float Production_Goal = float.Parse(dv05[0]["Production_Goal"].ToString());
                    float DollarsPerHour = float.Parse(dv05[0]["Dollars_Per_Hr"].ToString());
                    BindRunTime += (NumBooks / Production_Goal); BindRunCost += (DollarsPerHour * BindRunTime);
                }
            }

            cmd = $"SELECT Dollars_Per_Hr FROM [ESTIMATING].[dbo].[Finishing_Speeds] WHERE ProjectType = 'BOOK'";
            SqlDataAdapter da10 = new SqlDataAdapter(cmd, conn);
            DataSet ds10 = new DataSet(); da10.Fill(ds10);
            DataView dv10 = ds10.Tables[0].DefaultView;

            float bindmins = 0;
            if (dv5.Count > 0) bindmins = float.Parse(dv5[0]["BindSetupMin"].ToString()) / 60.0F;   // This uses the value (currently 12) of Quote_Details .. Category = "Finishing"
            BindSetupTime = bindmins;
            BindSetupCost = float.Parse(dv10[0]["Dollars_per_hr"].ToString()) * bindmins;

            // -----------------------------------------------------------------------
            // TODO: Do the same thing for "CELLO"  (adding AND "Quantity = {Cello}" ?
            // NOTE: I did something with "BOOK" and "CELLO" somewhere...
            // -----------------------------------------------------------------------

            // Fill in rows 4 and 5
            cmd = $"SELECT Value1, Value3 FROM [ESTIMATING].[dbo].[QUOTE_DETAILS] WHERE QUOTE_NUM = '{QUOTE_NUM}' AND CATEGORY = 'PrePress'";
            DataAdapter da6 = new SqlDataAdapter(cmd, conn);
            DataSet ds6 = new DataSet(); da6.Fill(ds6); DataView dv6 = ds6.Tables[0].DefaultView;

            Pre = ""; OE = "";
            if (dv6.Count > 0) { Pre = dv6[0]["Value1"].ToString(); OE = dv6[0]["Value3"].ToString(); }

            if (DVFeat != null && DVFeat.Count > 0)
            { for (int i = 0; i < DVFeat.Count; i++) { if (DVFeat[i][0].ToString() == "PrePress") { DVFeat[i][1] = PrePressPerMilChg * (SelectedQty / 1000); } } }

            cmd = "SELECT * FROM [ESTIMATING].[dbo].[PrePressOESpeeds]";
            DataAdapter da7 = new SqlDataAdapter(cmd, conn);
            DataSet ds7 = new DataSet(); da7.Fill(ds7); DataView dv7 = ds7.Tables[0].DefaultView;

            PreRunTime = 0; PreRunCost = 0; OERunTime = 0; OERunCost = 0;

            if (dv7.Count > 0 && Pre.Length > 0 && OE.Length > 0)
            {
                dv7.RowFilter = $"F_TYPE = 'PREPRESS' AND NEC = '{Pre}'";
                PreRunTime = float.Parse(dv7[0]["Hours"].ToString());
                PreRunCost = float.Parse(dv7[0]["DollarsPerHr"].ToString());
                PreRunCost *= PreRunTime;

                dv7.RowFilter = $"F_TYPE = 'OE'       AND NEC = '{OE}'";
                OERunTime = float.Parse(dv7[0]["Hours"].ToString());
                OERunCost = float.Parse(dv7[0]["DollarsPerHr"].ToString());
                OERunCost *= OERunTime;
            }

            //------------------------------------
            // SHIPPING cost calculation goes here
            //------------------------------------

            //TODO: Read the shipping costs from the SHIPPING table

            cmd = $"SELECT Value2, Value3 FROM [ESTIMATING].[dbo].[QUOTE_DETAILS] WHERE QUOTE_NUM = '{QUOTE_NUM}' AND CATEGORY = 'Shipping'";
            DataAdapter da8 = new SqlDataAdapter(cmd, conn);
            DataSet ds8 = new DataSet(); da8.Fill(ds8); DataView dv8 = ds8.Tables[0].DefaultView;

            ShipCost = 0; if (dv8.Count > 0)
            {
                ShipCost = float.Parse(dv8[0]["Value3"].ToString());
                NumDrops =   int.Parse(dv8[0]["Value2"].ToString());
            }

            ShipTime = 15 + (NumDrops * 5); // minutes
            ShipTime /= 60.0F;  // Fraction of an hour. TODO: WHERE DOES THIS GO?

            if (DVFeat != null && DVFeat.Count > 0)
            { for (int i = 0; i < DVFeat.Count; i++) { if (DVFeat[i][0].ToString() == "Shipping") { DVFeat[i][1] = ShipCost; } } }

            //---------------------------------
            // CASES cost calculation goes here
            //---------------------------------

            cmd = $"SELECT Value1, Value2, Value3 FROM [ESTIMATING].[dbo].[QUOTE_DETAILS] WHERE QUOTE_NUM = '{QUOTE_NUM}' AND CATEGORY = 'Cases'";
            DataAdapter da14 = new SqlDataAdapter(cmd, conn);
            DataSet ds14 = new DataSet(); da14.Fill(ds14);
            DataView dv14 = ds14.Tables[0].DefaultView;

            // If standard case cost, read from a table; otherwise, use the custom cost that was entered
            int stdUsed = 0; float custCost = 0.0F;
            if (dv14.Count > 0)
            {
                stdUsed = int.Parse(dv14[0]["Value1"].ToString()); // if stdUsed == 1, get standard case number from a table
                custCost = float.Parse(dv14[0]["Value2"].ToString());
            }

            // Standard cost comes from the CASES table, and is based on RollWidth
            cmd = $"SELECT SmallWidthCost, LargeWidthCost, DocsPerCase FROM [ESTIMATING].[dbo].[CASES] WHERE NumParts = {PARTS}";
            DataAdapter da15 = new SqlDataAdapter(cmd, conn);
            DataSet ds15 = new DataSet(); da15.Fill(ds15);
            DataView dv15 = ds15.Tables[0].DefaultView;

            //TODO: Handle conversions of RollWidths that contain extra stuff at the end...

            StringToNumber stn = new StringToNumber();
            float rollwidth = stn.Convert(ROLLWIDTH.ToString());

            float BaseCaseCharge = (rollwidth < 12) ? float.Parse(dv15[0]["SmallWidthCost"].ToString()) : float.Parse(dv15[0]["LargeWidthCost"].ToString());
            int docspercase = int.Parse(dv15[0]["DocsPerCase"].ToString());
            int CasesNeeded = (SelectedQty / docspercase);
            if (float.Parse(SelectedQty.ToString()) / float.Parse(docspercase.ToString()) != CasesNeeded) { CasesNeeded++; }    // Round up to the next integer
            CaseCost = BaseCaseCharge * CasesNeeded;

            if (DVFeat == null || DVFeat.Count == 0) return;
            for (int i = 0; i < DVFeat.Count; i++) { if (DVFeat[i][0].ToString() == "Cases") { DVFeat[i][1] = CaseCost; } }     // ShippingMaterialCost = CaseCost;

            // ------------------------------------------------------
            // Calculate carbon cost and store it in the DVFeat table
            // ------------------------------------------------------

            cmd = "SELECT * FROM [ESTIMATING].[dbo].[Carbon]";

            DataAdapter da16 = new SqlDataAdapter(cmd, conn);
            DataSet ds16 = new DataSet(); da16.Fill(ds16);
            DataView dv16 = ds16.Tables[0].DefaultView;

            StringToNumber sTOn = new StringToNumber();
            float DecimalCollatorCutSize = sTOn.Convert(COLLATORCUT.ToString());
            float TotalInches = DecimalCollatorCutSize * SelectedQty;
            float TotalFeet = TotalInches / 12;
            float CarbonWidth = sTOn.Convert(ROLLWIDTH);
            CarbonWidth -= 0.25F;
            float Units = (TotalFeet / 1000.0F) * CarbonWidth;

            float StockCarbonMaterialCost = 0.00F;
            float PatternCarbonMaterialCost = 0.00F;

            // Look up cost in the new Carbon table 
            cmd = "SELECT Value1 as StockColor, Value2 as PatternColor, Value3 AS StockNum, Value4 AS PatternNum"
                + $" FROM [ESTIMATING].[dbo].[QUOTE_DETAILS] WHERE QUOTE_NUM = '{QUOTE_NUM}' AND CATEGORY = 'Carbon'";
            DataAdapter da17 = new SqlDataAdapter(cmd, conn);
            DataSet ds17 = new DataSet(); da17.Fill(ds17);
            DataView dv17 = ds17.Tables[0].DefaultView;

            if (dv17.Count == 0) return;    // The is no Carbon entry in QUOTE_DETAILS

            string StockColor = dv17[0]["StockColor"].ToString();
            string PatternColor = dv17[0]["PatternColor"].ToString();
            int StockNum = int.Parse(dv17[0]["StockNum"].ToString());
            int PatternNum = int.Parse(dv17[0]["PatternNum"].ToString());

            dv16.RowFilter = $"Color  = '{StockColor}' AND Carbon = 'Stock'";
            StockCarbonMaterialCost = float.Parse(dv16[0]["UnitPrice"].ToString());

            dv16.RowFilter = $"Color  = '{PatternColor}' AND Carbon = 'Pattern'";
            PatternCarbonMaterialCost = float.Parse(dv16[0]["UnitPrice"].ToString());

            // Multiply it times Units, and store it in the DVFeat table:
            float StockCarbonCost = StockCarbonMaterialCost * StockNum * Units;
            float PatternCarbonCost = PatternCarbonMaterialCost * PatternNum * Units;
            float TotalCarbonCost = StockCarbonCost + PatternCarbonCost;

            for (int i = 0; i < DVFeat.Count; i++) { if (DVFeat[i][0].ToString() == "Carbon") { DVFeat[i][1] = TotalCarbonCost; } }

            CalcPanel.Visibility = Visibility.Visible;  // On Page 5
        }

        private void S1_Click(object sender, RoutedEventArgs e)
        {
            SelectedQty = Qty1; //txtQty1.Focus();
        }

        private void S2_Click(object sender, RoutedEventArgs e)
        {
            SelectedQty = Qty2; // txtQty2.Focus();
        }

        private void S3_Click(object sender, RoutedEventArgs e)
        {
            SelectedQty = Qty3; // txtQty3.Focus();
        }

        private void S4_Click(object sender, RoutedEventArgs e)
        {
            SelectedQty = Qty4; // txtQty4.Focus();
        }

        private void txtQty1_KeyDown(object sender, KeyEventArgs e)
        {
            //***********************************************
            // Make the {Enter} key act like the {Tab} key...
            // DISABLED FOR NOW...
            //***********************************************
            //var uiElement = e.OriginalSource as UIElement;
            //if (e.Key == Key.Enter && uiElement != null) 
            //  { e.Handled = true; uiElement.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next)); }
        }

        private void Calc_Click(object sender, RoutedEventArgs e)
        {
            CPM1a = 0; CPM2a = 0; CPM3a = 0; CPM4a = 0;
            CalcNow();
            dgSheetsOfPaper.SelectedIndex = 0;

        }

        private void CalcNow()
        {
            if (Qty1 > 0) { SelectedQty = Qty1; Q1(); }
            if (Qty2 > 0) { SelectedQty = Qty2; Q2(); }
            if (Qty3 > 0) { SelectedQty = Qty3; Q3(); }
            if (Qty4 > 0) { SelectedQty = Qty4; Q4(); }
        }

        private void cmbCollatorCut_LostFocus(object sender, RoutedEventArgs e)
        {
            //LoadGrid();
        }

        private void txtItemType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //            ItemType = ItemType.TrimEnd();
//            Title = "ItemType: |" + ItemType + "|";
            NoChgMsg = "";
            LoadSubWT();
            LoadColors();
            LoadRollWidth();
        }

        private void txtPaperType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadItemTypes();
        }

        private void cmbPaperType_LostFocus(object sender, RoutedEventArgs e)
        {
            LoadItemTypes();
            if (PAPERTYPE.Length > 0) { LoadRollWidth(); }
        }

        private void SaveCheck(object sender, RoutedEventArgs e)
        {
            // Change this to populate two tables in a dataset...
            string cmd = $"SELECT COUNT(*) FROM [ESTIMATING].[dbo].[QUOTE_DETAILS] WHERE QUOTE_NUM = '{QUOTE_NUM}'";
            conn = new SqlConnection(ConnectionString); conn.Open();
            DataTable dt21 = new DataTable(); SqlDataAdapter da21 = new SqlDataAdapter(cmd, conn); da21.Fill(dt21);
            int numDetails = int.Parse(dt21.Rows[0][0].ToString());

            cmd = $"SELECT COUNT(*) FROM [ESTIMATING].[dbo].[QUOTES] WHERE QUOTE_NUM = '{QUOTE_NUM}'";
            conn = new SqlConnection(ConnectionString); conn.Open();
            DataTable dt22 = new DataTable(); SqlDataAdapter da22 = new SqlDataAdapter(cmd, conn); da22.Fill(dt22);
            int numQuotes = int.Parse(dt22.Rows[0][0].ToString());

            if ((numDetails > 0) && (numQuotes == 0))
            {
                MessageBoxResult mbr = new MessageBoxResult();
                mbr = MessageBox.Show("Quote not saved - save now?", "Save?",
                      MessageBoxButton.YesNoCancel,
                      MessageBoxImage.Question);

                if (mbr == MessageBoxResult.Cancel) { return; }

                if (mbr == MessageBoxResult.Yes) { SaveQuote(); }

                if (mbr == MessageBoxResult.No)
                {
                    cmd = $"DELETE [ESTIMATING].[dbo].[QUOTE_DETAILS] WHERE QUOTE_NUM = '{QUOTE_NUM}'";
                    if (conn.State == ConnectionState.Closed) { conn.Open(); };
                    scmd = new SqlCommand(cmd, conn); scmd.ExecuteNonQuery(); conn.Close();
                }
            }
        }

    }
}