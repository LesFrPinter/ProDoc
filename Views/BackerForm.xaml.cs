using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Windows.UI.Notifications;

// The FEATURES table has the fixed and "change fee" for each press size. It's in an Excel spreadsheet now, but Tim is going to load it.

namespace ProDocEstimate.Views
{
    public partial class BackerForm : Window, INotifyPropertyChanged

    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

        private string pressSize = ""; public string PressSize { get { return pressSize; } set { pressSize = value; OnPropertyChanged(); } }

        private bool backer; public bool Backer { get { return backer; } set  { backer  = value; OnPropertyChanged(); } }

        private bool one;    public bool One    { get { return one;    } set  { one     = value; OnPropertyChanged(); Button1 = (One) ? 1 : 0; } }
        private bool two;    public bool Two    { get { return two;    } set  { two     = value; OnPropertyChanged(); Button2 = (Two) ? 1 : 0; } }
        private bool three;  public bool Three  { get { return three;  } set  { three   = value; OnPropertyChanged(); Button3 = (Three) ? 1 : 0; } }

        private int button1; public int Button1 { get { return button1; } set { button1 = value; OnPropertyChanged(); } }
        private int button2; public int Button2 { get { return button2; } set { button2 = value; OnPropertyChanged(); } }
        private int button3; public int Button3 { get { return button3; } set { button3 = value; OnPropertyChanged(); } }

        public BackerForm(string PSize)
        {
            InitializeComponent();
            DataContext = this;
            PressSize = PSize;
            Title = "PressSize: " + PressSize;      // passed in when the form is instantiated inside Quotations.xaml.cs.
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if((Button1+Button2+Button3)==0) { MessageBox.Show("Please select an option."); return; }
            // Look up the PressSize that was passed in and return the Flat and Run charges and the cost per change (PREP_DEPT_MATL - currently $5)
            // Display at the bottom of the form. Save the calculated result.
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
