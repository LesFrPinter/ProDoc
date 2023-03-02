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

//		private string? eclSetup;     public string? ECLSetup			{ get { return eclSetup;		 } set { eclSetup			= value; OnPropertyChanged(); } }
		private string? runCost;      public string? RunCost			{ get { return runCost;			 } set { runCost			= value; OnPropertyChanged(); } }
		private string? materialCost; public string? MaterialCost { get { return materialCost; } set { materialCost = value; OnPropertyChanged(); } }

		// Feature Standards properties

		//	private string? alert; public string? Alert { get { return alert; } set { alert = value; OnPropertyChanged(); } }
		//	

		private string? featureStandardNum; public string? FeatureStandardNum { get { return featureStandardNum; } set { featureStandardNum = value; OnPropertyChanged(); } }
		private string? featureNum;      public string? FeatureNum { get { return featureNum; } set { featureNum = value; OnPropertyChanged(); } }
		private string? featureDescription; public string? FeatureDescription { get { return featureDescription; } set { featureDescription = value; OnPropertyChanged(); } }
		private string? sort;            public string? Sort { get { return sort; } set { sort = value; OnPropertyChanged(); } }
		private string? flatPricing;     public string? FlatPricing { get { return flatPricing; } set { flatPricing = value; OnPropertyChanged(); } }
		private string? runPricing;      public string? RunPricing { get { return runPricing; } set { runPricing = value; OnPropertyChanged(); } }
		private string? priceByInch;     public string? PriceByInch { get { return priceByInch; } set { priceByInch = value; } }
		private string? priceBySqInch;   public string? PriceBySqInch { get { return priceBySqInch; } set { priceBySqInch = value; } }
		private string? sellDollars;     public string? SellDollars { get { return sellDollars; } set { sellDollars = value; OnPropertyChanged(); } }
		private string? costDollars;     public string? CostDollars { get { return costDollars; } set { costDollars = value; OnPropertyChanged(); } }
		private string? costMU1;         public string? CostMU1 { get { return costMU1; } set { costMU1 = value; OnPropertyChanged(); } }
		private string? costMU2;         public string? CostMU2 { get { return costMU2; } set { costMU2 = value; OnPropertyChanged(); } }

		private string? prepTime; public string? PrepTime { get { return prepTime; } set { prepTime = value; OnPropertyChanged(); } }
		private string? prepMaterial; public string? PrepMaterial { get { return prepMaterial; } set { prepMaterial = value; OnPropertyChanged(); } }
		private string? prepSlowdown; public string? PrepSlowdown { get { return prepSlowdown; } set { prepSlowdown = value; OnPropertyChanged(); } }

		private string? pressTime;       public string? PressTime { get { return pressTime; } set { pressTime = value; OnPropertyChanged(); } }
		private string? pressMaterial;   public string? PressMaterial { get { return pressMaterial; } set { pressMaterial = value; OnPropertyChanged(); } }
		private string? pressSlowdown;   public string? PressSlowdown { get { return pressSlowdown; } set { pressSlowdown = value; OnPropertyChanged(); } }

		private string? colorTime;       public string? ColorTime { get { return colorTime; } set { colorTime = value; OnPropertyChanged(); } }
		private string? colorMaterial;   public string? ColorMaterial { get { return colorMaterial; } set { colorMaterial = value; OnPropertyChanged(); } }
		private string? colorSlowdown;   public string? ColorSlowdown { get { return colorSlowdown; } set { colorSlowdown = value; OnPropertyChanged(); } }
		private string? bindCost;        public string? BindCost { get { return bindCost; } set { bindCost = value; OnPropertyChanged(); } }

		private string? otherTime;       public string? OtherTime { get { return otherTime; } set { otherTime = value; OnPropertyChanged(); } }
		private string? otherMaterial;   public string? OtherMaterial { get { return otherMaterial; } set { otherMaterial = value; OnPropertyChanged(); } }
		private string? oneTimeMaterial; public string? OneTimeMaterial { get { return oneTimeMaterial; } set { oneTimeMaterial = value; OnPropertyChanged(); } }
		private string? whatsThis;       public string? WhatsThis { get { return whatsThis; } set { whatsThis = value; OnPropertyChanged(); } }

		private string? optNumAround;    public string? OptNumAround  { get { return optNumAround; } set { optNumAround = value; OnPropertyChanged(); } }
		private string? optPart;         public string? OptPart       { get { return optPart; } set { optNumAround = value; OnPropertyChanged(); } }
		private string? optStream;       public string? OptStream     { get { return optStream; } set { optStream = value; OnPropertyChanged(); } }
		private string? askNumFlat;      public string? AskNumFlat    { get { return askNumFlat; } set { askNumFlat = value; OnPropertyChanged(); } }
		private string? askNumFlatDesc;  public string? AskNumFlatDesc { get { return askNumFlatDesc; } set { askNumFlatDesc = value; OnPropertyChanged(); } }
		private string? askNumRun;       public string? AskNumRun     { get { return askNumRun; } set { askNumRun = value; OnPropertyChanged(); } }
		private string? askNumRunDesc;   public string? AskNumRunDesc { get { return askNumRunDesc; } set { askNumRunDesc = value; OnPropertyChanged(); } }
		private string? optPercent;      public string? OptPercent    { get { return optPercent; } set { optPercent = value; OnPropertyChanged(); } }
		private string? optType;         public string? OptType       { get { return optType; } set { optType = value; OnPropertyChanged(); } }
		private string? eCLSetup;        public string? ECLSetup      { get { return eCLSetup; } set { eCLSetup = value; OnPropertyChanged(); } }
		private string? eCLRun;          public string? ECLRun        { get { return eCLRun; } set { web = value; OnPropertyChanged(); } }
		private string? eCLMaterial;     public string? ECLMaterial   { get { return eCLMaterial; } set { web = value; OnPropertyChanged(); } }
		private string? sets;            public string? Sets          { get { return sets; } set { sets = value; OnPropertyChanged(); } }
		private bool?   multStreamOK;    public bool?   MultStreamOK  { get { return multStreamOK; } set { multStreamOK = value; OnPropertyChanged(); } }
		private string? wasteSetup;      public string? WasteSetup    { get { return wasteSetup; } set { wasteSetup = value; OnPropertyChanged(); } }
		private string? wasteRunPct;     public string? WasteRunPct   { get { return wasteRunPct; } set { wasteRunPct = value; OnPropertyChanged(); } }
		private string? mRImpressions;   public string? MRImpressions { get { return mRImpressions; } set { mRImpressions = value; OnPropertyChanged(); } }
		private string? cartonQty;       public string? CartonQty     { get { return cartonQty; } set { cartonQty = value; OnPropertyChanged(); } }
		private string? calcType;        public string? CalcType      { get { return calcType; } set { calcType = value; OnPropertyChanged(); } }
		private string? alert;           public string? Alert         { get { return alert; } set { alert = value; OnPropertyChanged(); } }

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

			FeatureStandardNum = "02";
			FeatureNum = "03";
			FeatureDescription = "Description goes here";
			Sort = "A";
			FlatPricing = "2";
			RunPricing = "2";
			PriceByInch = "2";
			PriceBySqInch = "2";
			SellDollars = "2";
			CostDollars = "2";
			CostMU1 = "2";
			CostMU2 = "";

			PrepTime = "2";
			PrepMaterial = "2";
			PrepSlowdown = "2";

			PressTime = "2";
			PressMaterial = "2";
			PressSlowdown = "2";

			ColorTime = "2";
			ColorMaterial = "2";
			ColorSlowdown = "2";
			BindCost = "2";

			OtherTime = "2";
			OtherMaterial = "2";
			OneTimeMaterial = "2";
			WhatsThis = "2";

			OptNumAround = "2";
			OptPart = "2";
			OptStream = "2";
			AskNumFlat = "2";
			AskNumFlatDesc = "2";
			AskNumRun = "2";
			AskNumRunDesc = "2";
			OptPercent = "2";
			OptType = "2";
			ECLSetup = "2";
			ECLRun = "2";
			ECLMaterial = "2";
			Sets = "2";
			MultStreamOK = true;
			WasteSetup = "22";
			WasteRunPct = "2";
			MRImpressions = "2";
			CartonQty = "2";
			CalcType = "2";
			Alert = "Where does this come from?";
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