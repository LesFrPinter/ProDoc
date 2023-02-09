using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ProDocEstimate
{
	public partial class Customers : Window, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler? PropertyChanged;

		private bool notediting;
		public bool NotEditing
		{ get { return notediting; }
			set
			{ notediting = value;
				OnPropertyChanged();
			}
		}

		private bool editing;
		public bool Editing
		{ get { return editing; }
			set { editing = value;
						NotEditing = !editing;
						OnPropertyChanged(); }
		}

		public Customers()
		{ InitializeComponent();
			this.DataContext = this;
			this.Editing = false;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Editing = true;
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			Editing = true; 
		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			Editing = false;
		}

		private void Button_Click_3(object sender, RoutedEventArgs e)
		{
			Editing = false;
		}

		protected virtual void OnPropertyChanged([CallerMemberName] string ?propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private void btnDelete_Click(object sender, RoutedEventArgs e)
		{
			var confirmResult = 
				MessageBox.Show(
					"Delete?",
					"Confirm delete",
					MessageBoxButton.YesNo,MessageBoxImage.Question);
			if (confirmResult == MessageBoxResult.OK) { } else { }
		}

		private void btnClear_Click(object sender, RoutedEventArgs e)
		{
			
		}
	}
}