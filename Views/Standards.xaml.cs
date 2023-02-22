using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ProDocEstimate.Views
{
	public partial class Standards : Window, INotifyPropertyChanged
    {

		private bool editing;    public bool Editing    { get { return editing;    } set { editing = value; NotEditing = !editing; OnPropertyChanged(); } }
		private bool notediting; public bool NotEditing { get { return notediting; } set { notediting = value; OnPropertyChanged(); } }

		public Standards()
      {
        InitializeComponent();
				Editing = false;
				DataContext = this;
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