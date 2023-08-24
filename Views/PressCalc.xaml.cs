using MediaFoundation;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Windows;

//TODO: Change all "Material" references to "Description".

namespace ProDocEstimate.Views
{
    public partial class PressCalc : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

        #region Property declarations
        [MaybeNull]
        private string pressSize; public string PressSize { get { return pressSize; } set { pressSize = value; } }
        [MaybeNull]
        private int numDocs; public int NumDocs { get { return numDocs; } set { numDocs = value; } }
        [MaybeNull]
        private int wastePct; public int WastePct { get { return wastePct; } set { wastePct = value; } }
        [MaybeNull]
        private string material; public string Material { get { return material; } set { material = value; } }
        [MaybeNull]
        private int up; public int Up { get { return up; } set { up = value; } }
        [MaybeNull]
        private int pounds; public int Pounds { get { return pounds; } set { pounds = value; } }
        [MaybeNull]
        private int prodRate; public int ProdRate { get { return prodRate; } set { prodRate = value; } }
        [MaybeNull]
        private double prodTime; public double ProdTime { get { return prodTime; } set { prodTime = value; } }
        [MaybeNull]
        private int linearFt; public int LinearFt { get { return linearFt; } set { linearFt = value; } }
        [MaybeNull]
        private double matlCost; public double MatlCost { get { return matlCost; } set { matlCost = value; } }
        #endregion

        double y1 = 0.0D;
        double y2 = 0.0D;
        double x1 = 0.0D;
        double x2 = 0.0D;

        double m = 0.0D;
        double b = 0.0D;
        double Documents = 0.0D;
        double Cost = 0.0D;
        double Amt = 0.0D;

        public void MainWindow()
        {
            InitializeComponent();

            PressSize = "28";
            NumDocs = 150000;
            WastePct = 5;
            Material = "BOND 15# GRN 22";
            Up = 2;
            Pounds = 3892;
            ProdRate = 12000;
            ProdTime = 12.5D;
            LinearFt = 183750;
            MatlCost = 3463.88D;

            PressSize = "";
            NumDocs = 0;
            WastePct = 0;
            Material = "";
            Up = 0;
            Pounds = 0;
            ProdRate = 0;
            ProdTime = 0.0D;
            LinearFt = 0;
            MatlCost = 0.00D;

            this.DataContext = this;
        }

        public void OnLoad(object sender, RoutedEventArgs e)
        {
            this.Height = this.Height *= 1.8;
            this.Width = this.Width *= 1.8;
        }


        public void Calc()
        {
            Documents = (((double.Parse(WastePct.ToString()) / 100.0D + 1) * (double.Parse(NumDocs.ToString()) / double.Parse(Up.ToString())) * double.Parse(PressSize) / 12.0D));
            LinearFt = Convert.ToInt32(Documents);
            Amt = LinearFt_LBS(LinearFt, Material);
            Pounds = int.Parse(Amt.ToString());
            string str = "";

            try
            {
                str = "Select top 1 Documents from EquipmentProductionRates where Documents  >= '" + NumDocs.ToString() + "' order by Documents";
                x2 = double.Parse(SqlHandler(str));

                str = "Select top 1 Rate      from EquipmentProductionRates where Documents  >= '" + NumDocs.ToString() + "' order by Documents";
                y2 = double.Parse(SqlHandler(str));

                str = "Select top 1 Documents from EquipmentProductionRates where Documents   < '" + NumDocs.ToString() + "' order by documents desc";
                x1 = double.Parse(SqlHandler(str));

                str = "Select top 1 Rate      from EquipmentProductionRates where Documents   < '" + NumDocs.ToString() + "' order by documents desc";
                y1 = double.Parse(SqlHandler(str));

                str = "Select costperfactor from MasterInventory          where Description = '" + Material + "'";
                Cost = double.Parse(SqlHandler(str));

                Cost *= Amt;
            }
            catch (Exception ex) {  MessageBox.Show(ex.Message,str); }

            double m = (y2 - y1) / (x2 - x1);
            double b = y2 - (x2 * m);

            double pRate = (Documents * m) + b;
            ProdRate = Convert.ToInt32(pRate);  // lblrate.Text = (txtDocuments.Text * m) + b
            ProdTime = Documents / pRate;       // lblProduction.Text = txtDocuments.Text / lblrate.Text
            MatlCost = Cost;                    // lblCost.Text = Cost

            OnPropertyChanged("Pounds");
            OnPropertyChanged("ProdRate");
            OnPropertyChanged("ProdTime");
            OnPropertyChanged("LinearFt");
            OnPropertyChanged("MatlCost");
        }

        private void btnCalc_Click(object sender, RoutedEventArgs e)
        {
            Calc();
        }

        string SqlHandler(string cmd)
        {
            string cs = @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=PROVISIONDEV;Data Source=PDS-SQL\XMPIE;Trusted_Connection=False;User ID=lespinter;PWD=RememberMe$;";
            SqlConnection cn = new(cs);
            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand sc = new SqlCommand();
            sc.CommandText = cmd; cn.ConnectionString = cs; cn.Open();
            sc.Connection = cn;
            da.SelectCommand = sc;
            DataTable dt = new DataTable();
            da.Fill(dt);
            string s = dt.Rows[0][0].ToString().TrimEnd();
            return s;
        }

        double LinearFt_LBS(int Documents, string MasterId)
        {
            string PaperType = "";
            string Size = "";
            //       int CylSize = 0;
            double BasicArea = 0.0D;
            double BasisWT = 0.0D;
            double PaperWD = 0.0D;
            double Amt = 0.0D;
            string Stock = "";
            string qry = "";

            try
            {
                qry = "Select ProdType from MasterInventory where Description = '" + MasterId.TrimEnd() + "'";
                Stock = SqlHandler(qry); 
            }
            catch (Exception ex) 
            {   MessageBox.Show("Description not found",qry.Substring(43));
            }

            switch (Stock)
            {
                case "ROLL":
                    PaperType = SqlHandler(               "Select ItemType from MasterInventory where Description = '" + MasterId  + "'");
                    Size = SqlHandler(                    "Select Size     from MasterInventory where Description = '" + MasterId  + "'");
                    BasicArea = double.Parse(SqlHandler  ("Select Decimal  from ItemTypes       where ItemType = '"    + PaperType + "'"));
                    BasisWT   = double.Parse(SqlHandler  ("Select SubWT    from MasterInventory where Description = '" + MasterId  + "'"));
                    PaperWD   = double.Parse(SqlHandler  ("Select Decimal  from Sizes           where Size = '"        + Size      + "'"));
                    Amt = Math.Ceiling((Documents * (BasisWT * (PaperWD * 12))) / (BasicArea * 500));
                    break;
                case "SHT":
                    Amt = double.Parse(Documents.ToString());
                    break;
            }
            return Amt;

        }
    }
}