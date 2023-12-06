using ProDocEstimate.Views;
using System;
using System.Windows;
using System.Windows.Input;

namespace ProDocEstimate
{
    public partial class MainWindow : Window
	{

        #region Properties

        private string? custName; public string? CustName { get { return custName; } set { custName = value; } }
        private string? custCode; public string? CustCode { get { return custCode; } set { custCode = value; } }
        private static double featureZoom; public static double FeatureZoom { get { return featureZoom; } set { featureZoom = value; } }

        #endregion

        public MainWindow() { 
			InitializeComponent();

            PreviewKeyDown += (s, e) => 
            { if (e.Key == Key.Escape)
                Application.Current.Shutdown();
                return;
            //  Close();
            };
        }

		public void OnLoad(object sender, RoutedEventArgs e)
        {
            double ScreenWidth = SystemParameters.VirtualScreenWidth;
            double ScreenHeight = SystemParameters.VirtualScreenHeight;

            FeatureZoom = 0.0F;
            if (ScreenWidth < 1200) { FeatureZoom = 0.1F; }
            else
            if (ScreenWidth >= 2400 && ScreenWidth < 3441) { FeatureZoom = 0.8F; }
            else
            { FeatureZoom = 0.3F; }

            this.Height = this.Height *= ( 1.0F + FeatureZoom);
            this.Width  = this.Width  *= ( 1.0F + FeatureZoom);
            this.Top = 10;

            Title =   " Screen (" + ScreenWidth.ToString() + ", "                      + ScreenHeight.ToString()
                  + ")  Window ( " + this.Width.ToString("N0").Replace(",", "") + ", " + this.Height.ToString("N0").Replace(",", "") + ") "
                  + "   Zoom = " + FeatureZoom.ToString("N1");

            Title = Title + "   Version 1.0.22";
        }

        private void mnuFileExit_Click(object sender, RoutedEventArgs e)
		{ Application.Current.Shutdown(); Environment.Exit(Environment.ExitCode); }

		private void mnuCustomers_Click(object sender, RoutedEventArgs e)
		{ 
			Window customers = new CustCont();
			customers.Owner = this;
			customers.ShowDialog();
		}

		private void mnuQuotations_Click(object sender, RoutedEventArgs e)
		{ 
			Window quotations = new Quotations();
			quotations.Owner = this;
			quotations.ShowDialog();
		}

		private void mnuPV01_Click(object sender, RoutedEventArgs e)
		{
		}

		private void mnuPV02_Click(object sender, RoutedEventArgs e)
		{
		}

		private void mnuEdit_Click(object sender, RoutedEventArgs e)
		{
	    }

		private void mnuCopy_Click(object sender, RoutedEventArgs e)
		{
		}

		private void mnuBlank_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Display a blank quote screen with a new quote number", "New", MessageBoxButton.OK, MessageBoxImage.Information);
		}

		private void mnuCombo_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("What is this?", "New", MessageBoxButton.OK, MessageBoxImage.Question);
		}

		private void mnuTest_Click(object sender, RoutedEventArgs e)
		{
    }

        private void mnuConverter_Click(object sender, RoutedEventArgs e)
        {
            FractionToDecimalConverter FtoD = new FractionToDecimalConverter();
			FtoD.ShowDialog();
        }

        private void mnuEditFeatures_Click(object sender, RoutedEventArgs e)
        {
			Editors.Features feat = new Editors.Features(); feat.ShowDialog();
        }

        private void mnuEditInkCost_Click(object sender, RoutedEventArgs e)
        {
			Editors.InkCost inkCost = new Editors.InkCost(); inkCost.ShowDialog();
        }

        private void mnuPressSpeeds_Click(object sender, RoutedEventArgs e)
        {
			Editors.PressSpeeds pressSpeeds = new Editors.PressSpeeds(); pressSpeeds.ShowDialog();
        }

        private void mnuBindery_Click(object sender, RoutedEventArgs e)
        {
			Editors.Bindery bindery = new Editors.Bindery(); bindery.ShowDialog();
        }

        private void mnuCollator_Click(object sender, RoutedEventArgs e)
        {
			Editors.CollatorSpeeds collatorSpeeds = new Editors.CollatorSpeeds(); collatorSpeeds.ShowDialog();
        }

        private void mnuFinishing_Click(object sender, RoutedEventArgs e)
        {
			Editors.FinishingSpeeds finishingSpeeds = new Editors.FinishingSpeeds(); finishingSpeeds.ShowDialog();
        }

        private void mnuPrePress_Click(object sender, RoutedEventArgs e)
        {
			Editors.PrePress prePress = new Editors.PrePress(); prePress.ShowDialog();
        }

        private void mnuPressColors_Click(object sender, RoutedEventArgs e)
        {
			Editors.PressSizeColors colors = new Editors.PressSizeColors(); colors.ShowDialog();
        }

        private void mnuDetails_Click(object sender, RoutedEventArgs e)
        {
			Editors.QuoteDetails details = new Editors.QuoteDetails(); details.ShowDialog();		
        }

        private void mnuQuotes_Click(object sender, RoutedEventArgs e)
        {
			Editors.Quotes quotes = new Editors.Quotes(); quotes.ShowDialog();
        }

        private void mnuInventory_Click(object sender, RoutedEventArgs e)
        {
			Editors.MasterInventory inventory = new Editors.MasterInventory(); inventory.ShowDialog();
        }

        private void mnuAddTest_Click(object sender, RoutedEventArgs e)
        {
			Editors.TestAddingRow testAddingRow = new Editors.TestAddingRow(); testAddingRow.ShowDialog();
        }

        private void mnuCases_Click(object sender, RoutedEventArgs e)
        {
            Editors.Cases cases = new Editors.Cases(); cases.ShowDialog();
        }

        private void mnuShipping_Click(object sender, RoutedEventArgs e)
        {
            Editors.ShippingTableEditor shipping = new Editors.ShippingTableEditor(); shipping.ShowDialog();
        }

    }
}