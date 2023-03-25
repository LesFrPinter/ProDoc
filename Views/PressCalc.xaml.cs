using System;
using System.Data;
using System.Windows;
using System.Configuration;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Windows.Controls;
using System.Runtime.CompilerServices;

//TODO: Change all "Material" references to "Description".

namespace ProDocEstimate.Views
{
    public partial class PressCalc : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

        public string ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public SqlConnection? cn = new();
        public SqlDataAdapter? da;
        public DataSet ds;
        public DataTable dt;

        double x1, x2, y1, y2, m, b, amt, docs, cost = 0.0D;

        #region Property Declarations
        private string?  press;          public string?  Press          { get { return press;           } set { press           = value; OnPropertyChanged(); } }
        private int?     documents;      public int?     Documents      { get { return documents;       } set { documents       = value; OnPropertyChanged(); } }
        private int?     waste;          public int?     Waste          { get { return waste;           } set { waste           = value; OnPropertyChanged(); } }
        private string?  material;       public string?  Material       { get { return material;        } set { material        = value; OnPropertyChanged(); } }
        private int?     numUp;          public int?     NumUp          { get { return numUp;           } set { numUp           = value; OnPropertyChanged(); } }

        private decimal? materialLbs;    public decimal? MaterialLbs    { get { return materialLbs;     } set { materialLbs     = value; OnPropertyChanged(); } }
        private decimal? productionRate; public decimal? ProductionRate { get { return productionRate;  } set { productionRate  = value; OnPropertyChanged(); } }
        private decimal? productionTime; public decimal? ProductionTime { get { return productionTime;  } set { productionTime  = value; OnPropertyChanged(); } }
        private decimal? linearFeet;     public decimal? LinearFeet     { get { return linearFeet;      } set { linearFeet      = value; OnPropertyChanged(); } }
        private decimal? materialCost;   public decimal? MaterialCost   { get { return materialCost;    } set { materialCost    = value; OnPropertyChanged(); } }
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
        { 
          string cmd = "SELECT Description FROM MasterInventory ORDER BY Description";
          SqlConnection cn = new(ConnectionString); cn.Open(); SqlDataAdapter da = new(cmd, cn);
          DataSet ds = new("Material"); da.Fill(ds); DataTable dt = ds.Tables[0];
           for (int r = 0; r < dt.DefaultView.Count; r++) 
               { cmbMaterial.Items.Add(dt.DefaultView[r][0].ToString()); }
        }

        private void LoadDummyData()
        {
            Press = "28";
            Documents = 150000;
            Waste = 5;
            Material = "100# WHT TAG BOND 16";
            NumUp = 2;
        }

        private void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
            docs = (((double.Parse(Waste.ToString()) / 100 + 1) * (double.Parse(Documents.ToString()) / double.Parse(NumUp.ToString())) * double.Parse(Press.ToString()) / 12));
            LinearFeet = decimal.Parse(docs.ToString());
            amt = LinearFt_LBS(docs, Material);

            GetX2();
            GetY2();
            GetX1();
            GetY1();

            cost = CostPerFactor();
            cost *= amt;

            m = (y2 - y1) / (x2 - x1);
            b = y2 - (x2 * m);

            double x = double.Parse(Documents.ToString());
            ProductionRate = decimal.Parse(((x * m) + b).ToString());
            ProductionRate = Documents / ProductionRate;
            MaterialCost = decimal.Parse(cost.ToString());
        }

        private void GetX2()
        {
            string cmd = "SELECT TOP 1 Documents FROM EquipmentProductionRates WHERE Documents >= " + Documents.ToString() + " ORDER BY Documents";
            SqlConnection cn = new(ConnectionString); cn.Open(); SqlDataAdapter da = new(cmd, cn);
            DataSet ds = new("Material"); da.Fill(ds); DataTable dt = ds.Tables[0];
            x2 = double.Parse(dt.DefaultView[0][0].ToString());
        }

        private void GetY2()
        {
            string cmd = "SELECT TOP 1 Rate FROM EquipmentProductionRates WHERE Documents >= " + Documents.ToString() + " ORDER BY Documents";
            SqlConnection cn = new(ConnectionString); cn.Open(); SqlDataAdapter da = new(cmd, cn);
            DataSet ds = new("Material"); da.Fill(ds); DataTable dt = ds.Tables[0];
            y2 = double.Parse(dt.DefaultView[0][0].ToString());
        }

        private void GetX1()
        {
            string cmd = "SELECT TOP 1 Documents FROM EquipmentProductionRates WHERE Documents < " + Documents.ToString() + " ORDER BY Documents";
            SqlConnection cn = new(ConnectionString); cn.Open(); SqlDataAdapter da = new(cmd, cn);
            DataSet ds = new("Material"); da.Fill(ds); DataTable dt = ds.Tables[0];
            x1 = double.Parse(dt.DefaultView[0][0].ToString());
        }

        private void GetY1()
        {
            string cmd = "SELECT TOP 1 Rate FROM EquipmentProductionRates WHERE Documents < " + Documents.ToString() + " ORDER BY Documents";
            SqlConnection cn = new(ConnectionString); cn.Open(); SqlDataAdapter da = new(cmd, cn);
            DataSet ds = new("Material"); da.Fill(ds); DataTable dt = ds.Tables[0];
            y1 = double.Parse(dt.DefaultView[0][0].ToString());
        }

        private double CostPerFactor()
        {
            string cmd = "SELECT CostPerFactor from MasterInventory WHERE Description = '" + cmbMaterial.Text.TrimEnd() + "'";
            SqlConnection cn = new(ConnectionString); cn.Open(); SqlDataAdapter da = new(cmd, cn);
            DataSet ds = new("Cost"); da.Fill(ds); DataTable dt = ds.Tables[0];
            cost = double.Parse(dt.DefaultView[0][0].ToString());
            return cost;
        }

        private double LinearFt_LBS(double iDocs, string MasterId)
        {
            string PaperType, Size, Stock;
            int CylSize;
            double BasicArea, BasisWT, PaperWD, Amt;

            string cmd = "Select ProdType from MasterInventory where Description = '" + MasterId + "'";
            cn = new(ConnectionString);
            da = new SqlDataAdapter(cmd, cn);
            ds = new DataSet("Stock"); 
            da.Fill(ds); 
            dt = ds.Tables[0];
            Stock = dt.DefaultView[0][0].ToString().TrimEnd();

            if(Stock == "ROLL")
            {
                cmd = "Select ItemType from MasterInventory where Description = '" + MasterId + "'";
                SqlConnection cn = new(ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter(cmd, cn);
                DataSet ds = new("PaperType");
                da.Fill(ds);
                DataTable dt = ds.Tables[0];
                PaperType = dt.DefaultView[0][0].ToString().TrimEnd();

                //  Size = sqlExist("Select Size from MasterInventory where Description = '" & MasterId & "'")
                cmd = "Select Size from MasterInventory where Description = '" + MasterId + "'";
                cn = new(ConnectionString); cn.Open();
                da.SelectCommand = new SqlCommand(cmd);
                ds = new("Size");
                da.SelectCommand.Connection = cn;
                da.Fill(ds);
                dt = ds.Tables[0];
                Size = dt.DefaultView[0][0].ToString();

                //  BasicArea = sqlExist("Select Decimal from ItemTypes where ItemType = '" & PaperType & "'")
                cmd = "Select Decimal from ItemTypes where ItemType = '" + PaperType + "'";
                cn = new(ConnectionString); cn.Open();
                da.SelectCommand = new SqlCommand(cmd);
                da.SelectCommand.Connection = cn;
                ds = new("BasicArea");
                da.Fill(ds);
                dt = ds.Tables[0];
                BasicArea = double.Parse(dt.DefaultView[0][0].ToString());

                //  BasisWT = sqlExist("Select SubWT from MasterInventory where Description = '" & MasterId & "'")
                cmd = "Select SubWT from MasterInventory where Description = '" + MasterId + "'";
                cn = new(ConnectionString); cn.Open();
                da.SelectCommand = new SqlCommand(cmd);
                da.SelectCommand.Connection = cn;
                ds = new("BasisWT");
                da.Fill(ds);
                dt = ds.Tables[0];
                BasisWT = double.Parse(dt.DefaultView[0][0].ToString());

                //  PaperWD = sqlExist("Select Decimal from Sizes where Size = '" & Size & "'")
                cmd = "Select Decimal from Sizes where Size = '" + Size + "'";
                cn = new(ConnectionString); cn.Open();
                da.SelectCommand = new SqlCommand(cmd);
                da.SelectCommand.Connection = cn;
                ds = new("PaperWD");
                da.Fill(ds);
                dt = ds.Tables[0];
                PaperWD = double.Parse(dt.DefaultView[0][0].ToString());

                Amt = Math.Ceiling(double.Parse(iDocs.ToString())
                                 * (BasisWT) 
                                 * (PaperWD * 12.0D)
                                 / (BasicArea) * 500.0D);
            }
            else
            {
                Amt = iDocs;
            }

            return Amt;

        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        { Close(); }

        private void txtMaterial_GotFocus(object sender, RoutedEventArgs e)
        { (sender as TextBox)?.SelectAll(); }

        private void txtPress_GotFocus(object sender, RoutedEventArgs e)
        { (sender as TextBox)?.SelectAll(); }

    }
}