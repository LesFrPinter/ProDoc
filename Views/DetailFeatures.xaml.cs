using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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

        private float calculatedFlatCharge; public float CalculatedFlatCharge { get { return calculatedFlatCharge; } set { calculatedFlatCharge = value; OnPropertyChanged(); } }
        private float baseFlatCharge;       public float BaseFlatCharge       { get { return baseFlatCharge;       } set { baseFlatCharge       = value; OnPropertyChanged(); } }

        private float calculatedRunCharge;  public float CalculatedRunCharge { get { return calculatedRunCharge;   } set { calculatedRunCharge  = value; OnPropertyChanged(); } }
        private float baseRunCharge;        public float BaseRunCharge       { get { return baseRunCharge;         } set { baseRunCharge        = value; OnPropertyChanged(); } }

        public static readonly DependencyProperty FlatChargeProperty  = DependencyProperty.Register("FlatCharge",    typeof(float), typeof(DetailFeatures));
        public float FlatCharge  { get => (float)GetValue(FlatChargeProperty); set => SetValue(FlatChargeProperty,   value); }

        //, typeof(DetailFeatures), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)

        public static readonly DependencyProperty RunChargeProperty   = DependencyProperty.Register("RunCharge",     typeof(float), typeof(DetailFeatures));
        public float RunCharge   { get => (float)GetValue(RunChargeProperty);   set => SetValue(RunChargeProperty,   value); }

        public static readonly DependencyProperty PlateChargeProperty = DependencyProperty.Register("PlateCharge",   typeof(float), typeof(DetailFeatures));
        public float PlateCharge { get => (float)GetValue(PlateChargeProperty); set => SetValue(PlateChargeProperty, value); }

        public static readonly DependencyProperty FinishMatlProperty  = DependencyProperty.Register("FinishMatl",    typeof(float), typeof(DetailFeatures));
        public float FinishMatl  { get => (float)GetValue(FinishMatlProperty);  set => SetValue(FinishMatlProperty,  value); }

        public static readonly DependencyProperty ConvertMatlProperty = DependencyProperty.Register("ConvertMatl",   typeof(float), typeof(DetailFeatures));
        public float ConvertMatl { get => (float)GetValue(ConvertMatlProperty); set => SetValue(ConvertMatlProperty, value); }

        public static readonly DependencyProperty PressMatlProperty   = DependencyProperty.Register("PressMatl",     typeof(float), typeof(DetailFeatures));
        public float PressMatl   { get => (float)GetValue(PressMatlProperty);   set => SetValue(PressMatlProperty,   value); }

        public static DependencyProperty QUOTENUMProperty    = DependencyProperty.Register("QUOTENUM",      typeof(float), typeof(DetailFeatures));
        public float  QUOTENUM    { get => (float)GetValue(QUOTENUMProperty);    set => SetValue(QUOTENUMProperty,    value); }

        public static DependencyProperty CATEGORYProperty    = DependencyProperty.Register("CATEGORY",      typeof(float), typeof(DetailFeatures));
        public float  CATEGORY    { get => (float)GetValue(CATEGORYProperty);    set => SetValue(CATEGORYProperty,    value); }

        public static DependencyProperty PRESSSIZEProperty   = DependencyProperty.Register("PRESSSIZE",     typeof(float), typeof(DetailFeatures));
        public float  PRESSSIZE   { get => (float)GetValue(PRESSSIZEProperty);   set => SetValue(PRESSSIZEProperty,   value); }

        public static DependencyProperty FTYPEProperty       = DependencyProperty.Register("FTYPE",         typeof(float), typeof(DetailFeatures));
        public float  FTYPE       { get => (float)GetValue(FTYPEProperty);       set => SetValue(FTYPEProperty,       value); }

        public DetailFeatures() 
        {   InitializeComponent();

            this.DataContext = this;

            //Quote_Num  = "10001";
            //Category   = "MICR";
            //Press_Size = "11";

            GetFeatureCosts();
        }

        private void GetFeatureCosts()
        {
            string cmd = $"SELECT * FROM [ESTIMATING].[dbo].[FEATURES] WHERE Category = '{CATEGORY}' AND Press_size = '{PRESSSIZE}' AND F_TYPE = '{FTYPE}'";
            SqlConnection conn = new(ConnectionString); 
            DataTable dt = new DataTable();
            SqlDataAdapter da = new(cmd, conn);
            try { da.Fill(dt); }
            catch (Exception ex) { MessageBox.Show(ex.Message); return; }
            DataRow DRV = (DataRow)dt.Rows[0];

            BaseFlatCharge = float.Parse(DRV["FLAT_CHARGE"].ToString());
            BaseRunCharge  = float.Parse(DRV["RUN_CHARGE"].ToString());

            //PlateCharge = 33.03F;
            //FinishMatl = 44.04F;
            //ConvertMatl = 55.05F;
            //PressMatl = 66.06F;
        }

        private void FlatChargePct_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        { 
            CalculatedFlatCharge = (float)BaseFlatCharge * (1.00F + (float)FlatCharge/100.00F);
        }

        private void RunChargePct_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        { 
            CalculatedRunCharge = (float)BaseRunCharge * (1.00F + (float)RunCharge / 100.00F);
        }

    }
}
