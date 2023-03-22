using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace ProDocEstimate.Views
{
    public partial class PressCalc : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

        public string ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public SqlConnection? cn = new();
        public SqlDataAdapter? da;

        #region Property Declarations
        private string? press;    public string? Press     { get { return press;     } set { press     = value; OnPropertyChanged(); } }
        private int? documents;   public int?    Documents { get { return documents; } set { documents = value; OnPropertyChanged(); } }
        private int? waste;       public int?    Waste     { get { return waste;     } set { waste     = value; OnPropertyChanged(); } }
        private string? material; public string? Material  { get { return material;  } set { material  = value; OnPropertyChanged(); } }
        private int? numUp;       public int?    NumUp     { get { return numUp;     } set { numUp     = value; OnPropertyChanged(); } }

        private decimal? materialLbs;    public decimal? MaterialLbs    { get { return materialLbs;    } set { materialLbs    = value; OnPropertyChanged(); } }
        private decimal? productionRate; public decimal? ProductionRate { get { return productionRate; } set { productionRate = value; OnPropertyChanged(); } }
        private decimal? productionTime; public decimal? ProductionTime { get { return productionTime; } set { productionTime = value; OnPropertyChanged(); } }
        private decimal? linearFeet;     public decimal? LinearFeet     { get { return linearFeet;     } set { linearFeet     = value; OnPropertyChanged(); } }
        private decimal? materialCost;   public decimal? MaterialCost   { get { return materialCost;   } set { materialCost   = value; OnPropertyChanged(); } }
        #endregion

        public PressCalc()
        { InitializeComponent(); DataContext = this; LoadDummyData(); LoadPresses(); LoadDescriptions(); }

        private void LoadPresses()
        { string cmd = "SELECT distinct Press FROM EquipmentProductionRates";
          SqlConnection cn = new(ConnectionString); cn.Open(); SqlDataAdapter da = new(cmd, cn);
          DataSet ds = new("CollatorCuts"); da.Fill(ds); DataTable dt = ds.Tables[0];
          for (int r = 0; r < dt.DefaultView.Count; r++) { cmbPress.Items.Add(dt.DefaultView[r][0].ToString()); }
          cmbPress.SelectedIndex = 0;
        }

        private void LoadDescriptions()
        { string cmd = "SELECT Description FROM Description_and_CostPerFactor ORDER BY Description";
          SqlConnection cn = new(ConnectionString); cn.Open(); SqlDataAdapter da = new(cmd, cn);
          DataSet ds = new("Material"); da.Fill(ds); DataTable dt = ds.Tables[0];
           for (int r = 0; r < dt.DefaultView.Count; r++) { cmbMaterial.Items.Add(dt.DefaultView[r][0].ToString()); }
          cmbMaterial.SelectedIndex = 0;
        }

        private void LoadDummyData()
        {
            Press = "28";
            Documents = 150000;
            Waste = 5;
            Material = "BOND 15# GRN 22";
            NumUp = 2;
        }

        private void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
#if (false)
            double x1, x2, y1, y2, m, b, amt, documents, cost = 0.0D;

            Documents = (((Waste / 100 + 1) * (Documents / NumUp)) * Press) / 12;
            LinearFeet = Documents;

            //amt = SqlHandler.LinearFt_LBS(Documents, txtDescription.Text)
            //Material = AMT;

            //x2 = SqlHandler.sqlExist("Select top 1 Documents from EquipmentProductionRates where Documents >= '" & txtDocuments.Text & "' order by Documents")
            //y2 = SqlHandler.sqlExist("Select top 1 Rate  From EquipmentProductionRates where Documents >= '" & txtDocuments.Text & "' order by Documents")
            //x1 = SqlHandler.sqlExist("Select top 1 Documents From EquipmentProductionRates where Documents < '" & txtDocuments.Text & "' order by documents desc")
            //y1 = SqlHandler.sqlExist("Select top 1 Rate From EquipmentProductionRates where Documents < '" & txtDocuments.Text & "' order by documents desc")
            //Cost = SqlHandler.sqlExist("Select costperfactor from MasterInventory where Description = '" & txtDescription.Text & "'")

            amt = 0.0D;
            x1  = 0.0D;
            x2  = 0.0D;
            y1  = 0.0D;
            y2  = 0.0D;

            cost *= amt;

            m = (y2 - y1) / (x2 - x1);
            b = y2 - (x2 * m);

            double x = double.Parse(Documents.ToString());
            ProductionRate = decimal.Parse((( x * m) + b).ToString());
            ProductionRate = Documents / ProductionRate;
            MaterialCost = decimal.Parse(cost.ToString());
#endif

            MaterialLbs = 3892;
            ProductionRate = 12000.100010011m;
            ProductionTime = 12.4998958237837m;
            LinearFeet = 183750;
            MaterialCost = 3463.88m;
        }


        private void btnClose_Click(object sender, RoutedEventArgs e)
        { Close(); }

        private void txtMaterial_GotFocus(object sender, RoutedEventArgs e)
        { (sender as TextBox)?.SelectAll(); }

        private void txtPress_GotFocus(object sender, RoutedEventArgs e)
        { (sender as TextBox)?.SelectAll(); }

    }
}