using System.Collections;
using System.Windows;
using System.Windows.Input;

namespace ProDocEstimate.Views
{
	public partial class TreeViewCustSearch : Window
	{
		public TreeViewCustSearch()
		{
			InitializeComponent();
		}
		private int custno;
		public int Custno { get { return custno; } set { custno = value; /*OnPropertyChanged(nameof(CustNo));*/ } }

		private string? custName;
		public string? CustName { get { return custName; } set { custName = value; } }


		private void tvCustomers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			var x = tvCustomers.SelectedItem.ToString();
			int ContactIsAt = x.IndexOf("Contact: ") + 9;

			if(ContactIsAt == 8) 
			{ MessageBox.Show("Please click on a contact name", "Pick a contact"); return; }

			CustName = x.Substring(ContactIsAt);
			int ContactEndAt = CustName.IndexOf("|");
			CustName = CustName.Substring(0,ContactEndAt-1);

			// In production, the company and location would be returned from SQL. This is just dummied.
			switch (CustName.Trim()) 
			{ 
				case "George Raft":  Custno = 40314; break;
				case "Sam Billart": Custno = 40314; break;
				case "Sally": Custno = 40314; break;
				case "Donald Shellman": Custno = 65202; break;
				default: Custno = -1; break;
			}
			this.Hide();
    }

		private void btnSelect_Click(object sender, RoutedEventArgs e)
		{
			var x = tvCustomers.SelectedItem.ToString();
			MessageBox.Show(x);
			CustName = x;
		}
	}
}