using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace ProDocEstimate.Views
{
    public partial class ChargesBlock : UserControl, INotifyPropertyChanged
    {
        public string ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public SqlConnection? conn;
        public SqlDataAdapter? da;
        public DataTable dt;
        public SqlCommand? scmd;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

        private float flat; public float Flat { get { return flat; } set { flat = value; OnPropertyChanged(); } }
        private float chng; public float Chng { get { return chng; } set { chng = value; OnPropertyChanged(); } }
        private int changes; public int Changes { get { return changes; } set { changes = value; OnPropertyChanged(); } }
        private float? total; public float? Total { get { return total; } set { total = value; OnPropertyChanged(); } }
        private bool backer; public bool Backer { get { return backer; } set { backer = value; OnPropertyChanged(); } }

        private string quoteNo; public string QuoteNo { get { return quoteNo; } set { quoteNo = value; } }

        private float flatCharge; public float FlatCharge { get { return flatCharge; } set { flatCharge = value; OnPropertyChanged(); } }
        private float baseflatCharge; public float BaseFlatCharge { get { return baseflatCharge; } set { baseflatCharge = value; OnPropertyChanged(); } }
        private float flatChargePct; public float FlatChargePct { get { return flatChargePct; } set { flatChargePct = value; OnPropertyChanged(); } }
        private float calculatedflatCharge; public float CalculatedFlatCharge { get { return calculatedflatCharge; } set { calculatedflatCharge = value; OnPropertyChanged(); } }

        private float runCharge; public float RunCharge { get { return runCharge; } set { runCharge = value; OnPropertyChanged(); } }
        private float baserunCharge; public float BaseRunCharge { get { return baserunCharge; } set { baserunCharge = value; OnPropertyChanged(); } }
        private float runChargePct; public float RunChargePct { get { return runChargePct; } set { runChargePct = value; OnPropertyChanged(); } }
        private float calculatedrunCharge; public float CalculatedRunCharge { get { return calculatedrunCharge; } set { calculatedrunCharge = value; OnPropertyChanged(); } }

        private float plateCharge; public float PlateCharge { get { return plateCharge; } set { plateCharge = value; OnPropertyChanged(); } }
        private float baseplateCharge; public float BasePlateCharge { get { return baseplateCharge; } set { baseplateCharge = value; OnPropertyChanged(); } }
        private float plateChargePct; public float PlateChargePct { get { return plateChargePct; } set { plateChargePct = value; OnPropertyChanged(); } }
        private float calculatedplateCharge; public float CalculatedPlateCharge { get { return calculatedplateCharge; } set { calculatedplateCharge = value; OnPropertyChanged(); } }

        private float finishCharge; public float FinishCharge { get { return finishCharge; } set { finishCharge = value; OnPropertyChanged(); } }
        private float basefinishCharge; public float BaseFinishCharge { get { return basefinishCharge; } set { basefinishCharge = value; OnPropertyChanged(); } }
        private float finishChargePct; public float FinishChargePct { get { return finishChargePct; } set { finishChargePct = value; OnPropertyChanged(); } }
        private float calculatedfinishCharge; public float CalculatedFinishCharge { get { return calculatedfinishCharge; } set { calculatedfinishCharge = value; OnPropertyChanged(); } }

        private float pressCharge; public float PressCharge { get { return pressCharge; } set { pressCharge = value; OnPropertyChanged(); } }
        private float basepressCharge; public float BasePressCharge { get { return basepressCharge; } set { basepressCharge = value; OnPropertyChanged(); } }
        private float pressChargePct; public float PressChargePct { get { return pressChargePct; } set { pressChargePct = value; OnPropertyChanged(); } }
        private float calculatedpressCharge; public float CalculatedPressCharge { get { return calculatedpressCharge; } set { calculatedpressCharge = value; OnPropertyChanged(); } }

        private float convCharge; public float ConvCharge { get { return convCharge; } set { convCharge = value; OnPropertyChanged(); } }
        private float baseconvCharge; public float BaseConvCharge { get { return baseconvCharge; } set { baseconvCharge = value; OnPropertyChanged(); } }
        private float convChargePct; public float ConvChargePct { get { return convChargePct; } set { convChargePct = value; OnPropertyChanged(); } }
        private float calculatedconvCharge; public float CalculatedConvCharge { get { return calculatedconvCharge; } set { calculatedconvCharge = value; OnPropertyChanged(); } }

        private float flatTotal; public float FlatTotal { get { return flatTotal; } set { flatTotal = value; OnPropertyChanged(); } }
        private string fieldList; public string FieldList { get { return fieldList; } set { fieldList = value; OnPropertyChanged(); } }

        public ChargesBlock()
        {
            InitializeComponent();
        }

        private void LoadQuote()
        {
            string cmd = "SELECT * FROM [ESTIMATING].[dbo].[QUOTE_DETAILS] WHERE QUOTE_NUM = '" + QuoteNo + "' AND CATEGORY = 'Backer'";
            dt = new DataTable("Details");
            conn = new SqlConnection(ConnectionString);
            da = new SqlDataAdapter(cmd, conn); da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                string? F_TYPE = dt.Rows[0]["Param1"].ToString();

                //switch (F_TYPE)
                //{
                //    case "BACKER": One = true; break;
                //    case "BACKER STD": Two = true; break;
                //    case "BACKER PMS": Three = true; break;
                //}

                // Load percentges here....

                FlatChargePct = int.Parse(dt.Rows[0]["FlatChargePct"].ToString());
                RunChargePct = int.Parse(dt.Rows[0]["RunChargePct"].ToString());
                PlateChargePct = int.Parse(dt.Rows[0]["PlateChargePct"].ToString());
                FinishChargePct = int.Parse(dt.Rows[0]["FinishChargePct"].ToString());
                ConvChargePct = int.Parse(dt.Rows[0]["ConvertChargePct"].ToString());
                PressChargePct = int.Parse(dt.Rows[0]["PressChargePct"].ToString());

                Changes = Int32.Parse(dt.Rows[0]["Value2"].ToString());

                Total = float.Parse(dt.Rows[0]["Amount"].ToString());

            }

            conn.Close();
        }

    }
}