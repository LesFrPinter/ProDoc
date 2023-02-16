using System.ComponentModel;
using System.Windows;

namespace ProDocEstimate.Views
{
	public partial class CustomerSearch : Window, INotifyPropertyChanged, INotifyPropertyChanging
	{

		public event PropertyChangedEventHandler? PropertyChanged;
		public event PropertyChangingEventHandler? PropertyChanging;

		private int custno;
		public  int Custno      { get { return custno; }   set { custno = value; /*OnPropertyChanged(nameof(CustNo));*/ } }

		private string ?custName;
		public  string ?CustName { get { return custName; } set { custName = value; } }

		public CustomerSearch()
		{
			InitializeComponent();
		}

		private void btnSelect_Click(object sender, RoutedEventArgs e)
		{
			//			txtCustName.Text = dgCusts.SelectedValue as string;
			if (txtCustName.Text != null) { CustName = txtCustName.Text; Custno = int.Parse(txtCustNo.Text); }
			this.Hide();
    }

		private void btnCancel_Click(object sender, RoutedEventArgs e)
		{
			txtCustName.Text = "";
			custName = "";
			this.Hide();
		}

		private void btnOptions_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("What happens here?", "Set options", MessageBoxButton.OK, MessageBoxImage.Question);
    }
  }
}