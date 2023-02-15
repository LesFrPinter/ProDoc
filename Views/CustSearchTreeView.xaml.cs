using System.Windows;
using System.Windows.Input;
using System.ComponentModel;

namespace ProDocEstimate.Views
{
	public partial class CustSearchTreeView : Window, INotifyPropertyChanged
	{
		public CustSearchTreeView()
		{
			InitializeComponent();
		}

		private string ?custname = "";
		public string ?Custname { get { return custname; } set { custname = value; } }


		public event PropertyChangedEventHandler? PropertyChanged;

		private void tvCustomers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			Custname = tvCustomers.SelectedItem.ToString();
			this.Hide();
    }
  }
}