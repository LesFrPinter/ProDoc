using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace ProDocEstimate.Views
{
    public partial class StrikeIn : Window, INotifyPropertyChanged
    {
        public bool Removed = false;
        private float flatCharge;     public float FlatCharge     { get { return flatCharge;     } set { flatCharge     = value; OnPropertyChanged(); } }
        private int   setupMinutes;   public int   SetupMinutes   { get { return setupMinutes;   } set { setupMinutes   = value; OnPropertyChanged(); } }
        private int   bindingTime;    public int   BindingTime    { get { return bindingTime;    } set { bindingTime    = value; OnPropertyChanged(); } }
        private float finishMaterial; public float FinishMaterial { get { return finishMaterial; } set { finishMaterial = value; OnPropertyChanged(); } }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

        public StrikeIn(string PRESSSIZE, string QUOTENUM)
        { 
            InitializeComponent(); 
            Removed = true; // Will be removed from the "lstSelected" collection unless they entered some saveable parameters

            FlatCharge = 15.00F;
            SetupMinutes = 5;
            BindingTime = 20;
            FinishMaterial = 2.68F;

            DataContext = this;

            PreviewKeyDown += (s, e) => { if (e.Key == Key.Escape) Close(); };
        }

        public void OnLoad(object sender, RoutedEventArgs e)
        { this.Height = this.Height *= 1.8; this.Width = this.Width *= 1.8; Top = 25; }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}