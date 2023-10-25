using ProDocEstimate.Views;
using System;
using System.Windows;
using System.Windows.Input;

namespace ProDocEstimate
{
    public partial class MainWindow : Window
	{
		public MainWindow() { InitializeComponent(); PreviewKeyDown += (s, e) => { if (e.Key == Key.Escape) Close(); };
        }

        private string? custName; public string? CustName { get { return custName; } set { custName = value; } }
		private string? custCode; public string? CustCode { get { return custCode; } set { custCode = value; } }

		private static int featureZoom; public static int FeatureZoom { get { return featureZoom; } set { featureZoom = value; } }

		public void OnLoad(object sender, RoutedEventArgs e)
        {
            this.Height = this.Height *= 1.4;
            this.Width = this.Width *= 1.4;
            this.Top = 10;
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

		private void mnuPressStandards_Click(object sender, RoutedEventArgs e) {
			Press_Standards ps = new Press_Standards(); ps.ShowDialog();
    }

		private void mnuCollatorStandards_Click(object sender, RoutedEventArgs e) {
			Collator_Standards cs = new Collator_Standards(); cs.ShowDialog();
    }

		private void mnuFeatureStandards_Click(object sender, RoutedEventArgs e) {
			Feature_Standards fs = new Feature_Standards(); fs.ShowDialog();
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

    }
}