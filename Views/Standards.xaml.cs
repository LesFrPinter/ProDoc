using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ProDocEstimate.Views
{
	public partial class Standards : Window, INotifyPropertyChanged
    {

		// Press Standards properties
		private bool? editing;    public bool? Editing    { get { return editing;    } set { editing = value; NotEditing = !editing; OnPropertyChanged(); } }
		private bool? notediting; public bool? NotEditing { get { return notediting; } set { notediting = value; OnPropertyChanged(); } }

		private string? pressStandardNum;   public string? PressStandardNum   { get { return pressStandardNum;   } set { pressStandardNum   = value; OnPropertyChanged(); } }
		private string? pressDescription;   public string? PressDescription   { get { return pressDescription;   } set { pressDescription   = value; OnPropertyChanged(); } }
		private string? pressNum;						public string? PressNum						{ get { return pressNum;					 } set { pressNum						= value; OnPropertyChanged(); } }

		private string? web;        public string? Web { get { return web; } set { web = value; OnPropertyChanged(); } }
		private string? webFrac;    public string? WebFrac { get { return webFrac; } set { webFrac = value; OnPropertyChanged(); } }
		private string? paperWidth; public string? PaperWidth { get { return paperWidth; } set { paperWidth = value; OnPropertyChanged(); } }

		private string? towers;     public string? Towers { get { return towers; } set { towers = value; OnPropertyChanged(); } }
		private string? variable;   public string? Variable { get { return variable; } set { variable = value; OnPropertyChanged(); } }
		private string? restrictTo; public string? RestrictTo { get { return restrictTo; } set { restrictTo = value; OnPropertyChanged(); } }
		private string? slowdown;   public string? Slowdown { get { return slowdown; } set { slowdown = value; OnPropertyChanged(); } }

		private string? costPerHr;  public string? CostPerHr { get { return costPerHr; } set { costPerHr = value; OnPropertyChanged(); } }
		private string? sellPerHr;  public string? SellPerHr { get { return sellPerHr; } set { sellPerHr = value; OnPropertyChanged(); } }
		private string? baseSpeed;  public string? BaseSpeed { get { return baseSpeed; } set { baseSpeed = value; OnPropertyChanged(); } }

		private string? thouPerFt1; public string? ThouPerFt1 { get { return thouPerFt1; } set { thouPerFt1 = value; OnPropertyChanged(); } }
		private string? thouPerFt2; public string? ThouPerFt2 { get { return thouPerFt2; } set { thouPerFt2 = value; OnPropertyChanged(); } }
		private string? thouPerFt3; public string? ThouPerFt3 { get { return thouPerFt3; } set { thouPerFt3 = value; OnPropertyChanged(); } }
		private string? thouPerFt4; public string? ThouPerFt4 { get { return thouPerFt4; } set { thouPerFt4 = value; OnPropertyChanged(); } }
		private string? thouPerFt5; public string? ThouPerFt5 { get { return thouPerFt5; } set { thouPerFt5 = value; OnPropertyChanged(); } }
		private string? thouPerFt6; public string? ThouPerFt6 { get { return thouPerFt6; } set { thouPerFt6 = value; OnPropertyChanged(); } }
		private string? thouPerFt7; public string? ThouPerFt7 { get { return thouPerFt7; } set { thouPerFt7 = value; OnPropertyChanged(); } }
		private string? thouPerFt8; public string? ThouPerFt8 { get { return thouPerFt8; } set { thouPerFt8 = value; OnPropertyChanged(); } }

		private string? ftPerMin1;  public string? FtPerMin1 { get { return ftPerMin1; } set { ftPerMin1 = value; OnPropertyChanged(); } }
		private string? ftPerMin2;  public string? FtPerMin2 { get { return ftPerMin2; } set { ftPerMin2 = value; OnPropertyChanged(); } }
		private string? ftPerMin3;  public string? FtPerMin3 { get { return ftPerMin3; } set { ftPerMin3 = value; OnPropertyChanged(); } }
		private string? ftPerMin4;  public string? FtPerMin4 { get { return ftPerMin4; } set { ftPerMin4 = value; OnPropertyChanged(); } }
		private string? ftPerMin5; public string? FtPerMin5 { get { return ftPerMin5; } set { ftPerMin5 = value; OnPropertyChanged(); } }
		private string? ftPerMin6; public string? FtPerMin6 { get { return ftPerMin6; } set { ftPerMin6 = value; OnPropertyChanged(); } }
		private string? ftPerMin7; public string? FtPerMin7 { get { return ftPerMin7; } set { ftPerMin7 = value; OnPropertyChanged(); } }
		private string? ftPerMin8; public string? FtPerMin8 { get { return ftPerMin8; } set { ftPerMin8 = value; OnPropertyChanged(); } }

		private string? eclSetup;     public string? ECLSetup			{ get { return eclSetup;		 } set { eclSetup			= value; OnPropertyChanged(); } }
		private string? runCost;      public string? RunCost			{ get { return runCost;			 } set { runCost			= value; OnPropertyChanged(); } }
		private string? materialCost; public string? MaterialCost { get { return materialCost; } set { materialCost = value; OnPropertyChanged(); } }

		// Feature Standards properties

		//	private string? web; public string? Web { get { return web; } set { web = value; OnPropertyChanged(); } }
		//	private string? web; public string? Web { get { return web; } set { web = value; OnPropertyChanged(); } }

		private string? featureStandardNum; public string? FeatureStandardNum { get { return featureStandardNum; } set { featureStandardNum = value; OnPropertyChanged(); } }
		private string? featureNum; public string? FeatureNum { get { return featureNum; } set { featureNum = value; OnPropertyChanged(); } }
		private string? featureDescription; public string? FeatureDescription { get { return featureDescription; } set { featureDescription = value; OnPropertyChanged(); } }
		private string? sort; public string? Sort { get { return sort; } set { sort = value; OnPropertyChanged(); } }
		private string? flatPricing; public string? FlatPricing { get { return flatPricing; } set { flatPricing = value; OnPropertyChanged(); } }
		private string? runPricing; public string? RunPricing { get { return runPricing; } set { runPricing = value; OnPropertyChanged(); } }
		private string? priceByInch; public string? PriceByInch { get { return priceByInch; } set { priceByInch = value; } }
		private string? priceBySqInch; public string? PriceBySqInch { get { return priceBySqInch; } set { priceBySqInch = value; } }
		private string? sellDollars; public string? SellDollars { get { return sellDollars; } set { sellDollars = value; OnPropertyChanged(); } }
		private string? costDollars; public string? CostDollars { get { return costDollars; } set { costDollars = value; OnPropertyChanged(); } }
		private string? costMU1; public string? CostMU1 { get { return costMU1; } set { costMU1 = value; OnPropertyChanged(); } }
		private string? costMU2; public string? CostMU2 { get { return costMU2; } set { costMU2 = value; OnPropertyChanged(); } }
		private string? pressTime; public string? PressTime { get { return pressTime; } set { pressTime = value; OnPropertyChanged(); } }
		private string? pressMaterial; public string? PressMaterial { get { return pressMaterial; } set { pressTime = value; OnPropertyChanged(); } }
		private string? pressSlowdown; public string? PressSlowdown { get { return pressSlowdown; } set { pressSlowdown = value; OnPropertyChanged(); } }


		public Standards()
      {
        InitializeComponent();
				Editing = false;
				DataContext = this;
			// assign values to all properties

			PressStandardNum = "01";
			FeatureStandardNum = "03";
			FeatureNum = "01";
			PressDescription = "Description of selected press";
			FeatureDescription = "Description of selected feature";
			PressNum = "14";
			Sort = "ASC";

			Web = "1.2";
			WebFrac = "1.3";
			PaperWidth = @"14 7/8""";

			Towers = "1";
			Variable = "1";
			RestrictTo = "1";
			Slowdown = "1";

			CostPerHr = "1";
			SellPerHr = "1";
			BaseSpeed = "1";

			ThouPerFt1 = "1";
			ThouPerFt2 = "1";
			ThouPerFt3 = "1";
			ThouPerFt4 = "1";
			ThouPerFt5 = "1";
			ThouPerFt6 = "1";
			ThouPerFt7 = "1";
			ThouPerFt8 = "1";

			FtPerMin1 = "1";
			FtPerMin2 = "1";
			FtPerMin3 = "1";
			FtPerMin4 = "1";
			FtPerMin5 = "1";
			FtPerMin6 = "1";
			FtPerMin7 = "1";
			FtPerMin8 = "1";

			ECLSetup = "1";
			RunCost = "1";
			MaterialCost = "1";

		}

		public event PropertyChangedEventHandler? PropertyChanged;
		protected void OnPropertyChanged([CallerMemberName] string? name = null)
		{ PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

		private void mnuExit_Click(object sender, RoutedEventArgs e)
		{
      Close();
    }

		private void mnuEdit_Click(object sender, RoutedEventArgs e)
		{
			Editing = true;
		}

		private void mnuNew_Click(object sender, RoutedEventArgs e)
		{
			Editing = true;
		}

		private void mnuSave_Click(object sender, RoutedEventArgs e)
		{
			Editing = false;
		}

		private void mnuCancel_Click(object sender, RoutedEventArgs e)
		{
			Editing = false;
		}
	}
}