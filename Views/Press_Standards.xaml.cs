using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ProDocEstimate.Views {
	public partial class Press_Standards : Window, INotifyPropertyChanged 
		{

		private bool? editing; public bool? Editing { get { return editing; } set { editing = value; NotEditing = !editing; OnPropertyChanged(); } }
		private bool? notediting; public bool? NotEditing { get { return notediting; } set { notediting = value; OnPropertyChanged(); } }

		private string? pressStandardNum; public string? PRESS_STANDARD { get { return pressStandardNum; } set { pressStandardNum = value; OnPropertyChanged(); } }
		private string? pressDescription; public string? PRESS_DESCRIPTION { get { return pressDescription; } set { pressDescription = value; OnPropertyChanged(); } }
		private string? pressNum; public string? PRESS_NUM { get { return pressNum; } set { pressNum = value; OnPropertyChanged(); } }

		private string? web; public string? WEB { get { return web; } set { web = value; OnPropertyChanged(); } }
		private string? web2; public string? WEB2 { get { return web2; } set { web2 = value; OnPropertyChanged(); } }
		private string? width; public string? WIDTH { get { return width; } set { width = value; OnPropertyChanged(); } }

		private string? towers; public string? TOWERS { get { return towers; } set { towers = value; OnPropertyChanged(); } }
		private string? variable; public string? VARIABLE { get { return variable; } set { variable = value; OnPropertyChanged(); } }
		private string? restrictTo; public string? RESTRICTD { get { return restrictTo; } set { restrictTo = value; OnPropertyChanged(); } }
		private string? slowdown; public string? Slowdown { get { return slowdown; } set { slowdown = value; OnPropertyChanged(); } }

		private float? press_cost; public float? PRESS_COST { get { return press_cost; } set { press_cost = value; OnPropertyChanged(); } }
		private float? press_sell; public float? PRESS_SELL { get { return press_sell; } set { press_sell = value; OnPropertyChanged(); } }
		private string? baseSpeed; public string? BASESPEED { get { return baseSpeed; } set { baseSpeed = value; OnPropertyChanged(); } }

		private string? speed1; public string? SPEED1 { get { return speed1; } set { speed1 = value; OnPropertyChanged(); } }
		private string? speed2; public string? SPEED2 { get { return speed2; } set { speed2 = value; OnPropertyChanged(); } }
		private string? speed3; public string? SPEED3 { get { return speed3; } set { speed3 = value; OnPropertyChanged(); } }
		private string? speed4; public string? SPEED4 { get { return speed4; } set { speed4 = value; OnPropertyChanged(); } }
		private string? speed5; public string? SPEED5 { get { return speed5; } set { speed5 = value; OnPropertyChanged(); } }
		private string? speed6; public string? SPEED6 { get { return speed6; } set { speed6 = value; OnPropertyChanged(); } }
		private string? speed7; public string? SPEED7 { get { return speed7; } set { speed7 = value; OnPropertyChanged(); } }
		private string? speed8; public string? SPEED8 { get { return speed8; } set { speed8 = value; OnPropertyChanged(); } }

		private string? footage1; public string? FOOTAGE1 { get { return footage1; } set { footage1 = value; OnPropertyChanged(); } }
		private string? footage2; public string? FOOTAGE2 { get { return footage2; } set { footage2 = value; OnPropertyChanged(); } }
		private string? footage3; public string? FOOTAGE3 { get { return footage3; } set { footage3 = value; OnPropertyChanged(); } }
		private string? footage4; public string? FOOTAGE4 { get { return footage4; } set { footage4 = value; OnPropertyChanged(); } }
		private string? footage5; public string? FOOTAGE5 { get { return footage5; } set { footage5 = value; OnPropertyChanged(); } }
		private string? footage6; public string? FOOTAGE6 { get { return footage6; } set { footage6 = value; OnPropertyChanged(); } }
		private string? footage7; public string? FOOTAGE7 { get { return footage7; } set { footage7 = value; OnPropertyChanged(); } }
		private string? footage8; public string? FOOTAGE8 { get { return footage8; } set { footage8 = value; OnPropertyChanged(); } }

		private string? setupECL; public string? SETUP_ECL { get { return setupECL; } set { setupECL = value; OnPropertyChanged(); } }
		private string? runECL; public string? RUN_ECL { get { return runECL; } set { runECL = value; OnPropertyChanged(); } }
		private string? matlECL; public string? MATL_ECL { get { return matlECL; } set { matlECL = value; OnPropertyChanged(); } }

		public event PropertyChangedEventHandler? PropertyChanged;
		protected void OnPropertyChanged([CallerMemberName] string? name = null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

		public Press_Standards() {
			InitializeComponent();
			DataContext = this;

			PRESS_STANDARD = "01";
			PRESS_NUM = "01";
			PRESS_DESCRIPTION = "22 inch Press";

			WEB = "1.2";
			WEB2 = "1.3";
			WIDTH = @"14 7/8""";

			TOWERS = "2";
			VARIABLE = "Y";
			RESTRICTD = "S";
			Slowdown = "1";  // WHAT IS THIS?

			PRESS_COST = 1.12F;
			PRESS_SELL = 2.21F;
			BASESPEED = "428";

			SPEED1 = "91";
			SPEED2 = "153";
			SPEED3 = "185";
			SPEED4 = "244";
			SPEED5 = "275";
			SPEED6 = "325";
			SPEED7 = "360";
			SPEED8 = "450";

			FOOTAGE1 = "1.38";
			FOOTAGE2 = "4.58";
			FOOTAGE3 = "6.88";
			FOOTAGE4 = "9.17";
			FOOTAGE5 = "13.75";
			FOOTAGE6 = "45.80";
			FOOTAGE7 = "91.70";
			FOOTAGE8 = "183.00";

			SETUP_ECL = "151.10"; // prefix with "PRESS_"
			RUN_ECL = "151.20";   //    "
			MATL_ECL = "802.00";  //
														//
			Editing = false;

		}

		private void mnuExit_Click(object sender, RoutedEventArgs e) {
			Close();
		}

		private void mnuEdit_Click(object sender, RoutedEventArgs e) {
			Editing = true;
		}

		private void mnuNew_Click(object sender, RoutedEventArgs e) {
			Editing = true;
		}

		private void mnuSave_Click(object sender, RoutedEventArgs e) {
			Editing = false;
		}

		private void mnuCancel_Click(object sender, RoutedEventArgs e) {
			Editing = false;
		}

	}

}