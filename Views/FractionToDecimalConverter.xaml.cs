using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ProDocEstimate.Views
{
    public partial class FractionToDecimalConverter : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

        private string fraction1; public string Fraction { get { return fraction1; } set { fraction1 = value; OnPropertyChanged(); } }
        private float  result1;   public float  Result   { get { return result1;   } set { result1   = value; OnPropertyChanged(); } }

        public FractionToDecimalConverter() 
        { 
            InitializeComponent(); 
            DataContext = this;
        }

        private void btnConvert_Click(object sender, RoutedEventArgs e)
        {
            StringToNumber sTOn = new StringToNumber();
            Result = sTOn.Convert(Fraction);
        }
    }
}