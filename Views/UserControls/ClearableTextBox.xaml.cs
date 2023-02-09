using System.Windows;
using System.Windows.Controls;

namespace NewWpf1.View.UserControls
{
	public partial class ClearableTextBox : UserControl
    {
      public ClearableTextBox()
        { InitializeComponent(); }

		private string placeHolder;

		public string PlaceHolder
		{
			get { return placeHolder; }
			set { 
				placeHolder = value;
				tbPlaceHolder.Text = PlaceHolder;
			}
		}

		private void btnClear_Click(object sender, RoutedEventArgs e)
		{
      txtInput.Clear();
      txtInput.Focus();
    }

		private void txtInput_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (string.IsNullOrEmpty(txtInput.Text))
				tbPlaceHolder.Visibility = Visibility.Visible;
			else
				tbPlaceHolder.Visibility = Visibility.Hidden;
		}
	}
}