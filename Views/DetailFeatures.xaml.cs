using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace ProDocEstimate.Views
{
    public partial class DetailFeatures : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

        public string ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public SqlConnection? conn;
        public SqlDataAdapter? da;
        public DataTable? dt;
        public SqlCommand? scmd;

        #region Properties and DependencyProperties

        private float calculatedFlatCharge; public float CalculatedFlatCharge { get { return calculatedFlatCharge; } set { calculatedFlatCharge = value; OnPropertyChanged(); } }
        private float baseFlatCharge;       public float BaseFlatCharge       { get { return baseFlatCharge;       } set { baseFlatCharge       = value; OnPropertyChanged(); } }

        private float calculatedRunCharge;  public float CalculatedRunCharge { get { return calculatedRunCharge;   } set { calculatedRunCharge  = value; OnPropertyChanged(); } }
        private float baseRunCharge;        public float BaseRunCharge       { get { return baseRunCharge;         } set { baseRunCharge        = value; OnPropertyChanged(); } }

        public static readonly DependencyProperty FlatChargeProperty  = 
            DependencyProperty.Register("FlatCharge",    typeof(float), typeof(DetailFeatures));
        public float FlatCharge  { get => (float)GetValue(FlatChargeProperty); set => SetValue(FlatChargeProperty,   value); }

        public static readonly DependencyProperty RunChargeProperty   = 
            DependencyProperty.Register("RunCharge",     typeof(float), typeof(DetailFeatures));
        public float  RunCharge   { get => (float)GetValue(RunChargeProperty);   set => SetValue(RunChargeProperty,  value); }

        public static readonly DependencyProperty PlateChargeProperty = 
            DependencyProperty.Register("PlateCharge",   typeof(float), typeof(DetailFeatures));
        public float  PlateCharge { get => (float)GetValue(PlateChargeProperty); set => SetValue(PlateChargeProperty,value); }

        public static readonly DependencyProperty FinishMatlProperty  = 
            DependencyProperty.Register("FinishMatl",    typeof(float), typeof(DetailFeatures));
        public float  FinishMatl  { get => (float)GetValue(FinishMatlProperty);  set => SetValue(FinishMatlProperty, value); }

        public static readonly DependencyProperty ConvertMatlProperty = 
            DependencyProperty.Register("ConvertMatl",   typeof(float), typeof(DetailFeatures));
        public float  ConvertMatl { get => (float)GetValue(ConvertMatlProperty); set => SetValue(ConvertMatlProperty,value); }

        public static readonly DependencyProperty PressMatlProperty   = 
            DependencyProperty.Register("PressMatl",     typeof(float), typeof(DetailFeatures));
        public float  PressMatl   { get => (float)GetValue(PressMatlProperty);   set => SetValue(PressMatlProperty,  value); }

        // ------------ THREE DEPENDENCY PROPERTIES PASSED IN WITHIN THE COMPONENT DECLARATION ------------
        public static DependencyProperty QUOTENUMProperty    = 
            DependencyProperty.RegisterAttached(
                nameof(QUOTENUM),
                typeof(string),
                typeof(DetailFeatures), 
                new FrameworkPropertyMetadata(
                    string.Empty, 
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string QUOTENUM
            {  get => (string)GetValue(QUOTENUMProperty);    
               set =>         SetValue(QUOTENUMProperty, value); 
            }

        public static DependencyProperty CATEGORYProperty    = 
            DependencyProperty.RegisterAttached(
                nameof(CATEGORY),
                typeof(string), 
                typeof(DetailFeatures), 
                new FrameworkPropertyMetadata(
                    string.Empty, 
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string CATEGORY
            {  get => (string)GetValue(CATEGORYProperty);    
               set => SetValue(CATEGORYProperty,   value); 
            }

        public static DependencyProperty PRESSSIZEProperty   = 
            DependencyProperty.Register(
                nameof(PRESSSIZE),
                typeof(string), 
                typeof(DetailFeatures), 
                new FrameworkPropertyMetadata(
                    string.Empty, 
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string PRESSSIZE
            {  get => (string)GetValue(PRESSSIZEProperty);
               set => SetValue(PRESSSIZEProperty,  value); 
            }
        // ------------ THREE DEPENDENCY PROPERTIES PASSED IN WITHIN THE COMPONENT DECLARATION ------------

        #endregion

        private string quoteNum;  public string QuoteNum  { get { return quoteNum;  } set { quoteNum  = value; OnPropertyChanged(); } }
        private string category;  public string Category  { get { return category;  } set { category  = value; OnPropertyChanged(); } }
        private string pressSize; public string PressSize { get { return pressSize; } set { pressSize = value; OnPropertyChanged(); } }

        public DetailFeatures() 
        {
            InitializeComponent();

            GetFeatureCosts();
        }

        private void Loaded()
        {
            QuoteNum = QUOTENUM.ToString();
            Category = CATEGORY.ToString();
            PressSize = PRESSSIZE.ToString();
        }

        private void GetFeatureCosts()
        {

            if( CATEGORY.Length == 0 || PRESSSIZE.Length == 0 ) return;

            string cmd = $"SELECT * FROM [ESTIMATING].[dbo].[FEATURES] WHERE Category = '{CATEGORY}' AND Press_size = '{PRESSSIZE}'";

            SqlConnection conn = new(ConnectionString); 
            DataTable dt = new DataTable();
            SqlDataAdapter da = new(cmd, conn);
            try { da.Fill(dt); }
            catch (Exception ex) { MessageBox.Show(ex.Message); return; }

            if(dt.Rows.Count == 0) { MessageBox.Show(cmd + " returned no data from SQL.", "In GetFeatureCosts of DetailFeatures"); Debugger.Break(); }

            DataRow DRV = (DataRow)dt.Rows[0];

            BaseFlatCharge = float.Parse(DRV["FLAT_CHARGE"].ToString());
            BaseRunCharge  = float.Parse(DRV["RUN_CHARGE"].ToString());

            PlateCharge = 33.03F;
            FinishMatl  = 44.04F;
            ConvertMatl = 55.05F;
            PressMatl   = 66.06F;
        }

        private void FlatChargePct_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        { 
//            CalculatedFlatCharge = (float)BaseFlatCharge * (1.00F + (float)FlatCharge/100.00F);
        }

        private void RunChargePct_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        { 
//            CalculatedRunCharge = (float)BaseRunCharge * (1.00F + (float)RunCharge / 100.00F);
        }

    }
}
