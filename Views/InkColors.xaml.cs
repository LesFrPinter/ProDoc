using System;
using System.Data;
using System.Windows;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Configuration;

namespace ProDocEstimate.Views
{
    public partial class InkColors : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

        private int    maxColors; public int    MaxColors { get { return maxColors; } set { maxColors = value; OnPropertyChanged(); } }
        private string pressSize; public string PressSize { get { return pressSize; } set { pressSize = value; OnPropertyChanged(); } }

        private int desens;     public int Desens   { get { return desens;   } set { desens   = value; OnPropertyChanged(); } }
        private int split;      public int Split    { get { return split;    } set { split    = value; OnPropertyChanged(); } }
        private int thermo;     public int Thermo   { get { return thermo;   } set { thermo   = value; OnPropertyChanged(); } }
        private int backer;     public int Backer   { get { return backer;   } set { backer   = value; OnPropertyChanged(); } }
        private int std;        public int Std      { get { return std;      } set { std      = value; OnPropertyChanged(); } }
        private int blackStd;   public int BlackStd { get { return blackStd; } set { blackStd = value; OnPropertyChanged(); } }
        private int pms;        public int PMS      { get { return pms;      } set { pms      = value; OnPropertyChanged(); } }

        public string ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public SqlConnection? conn;
        public SqlDataAdapter? da;
        public DataTable? dt;
        public SqlCommand? scmd;

        public InkColors(string PRESSSIZE, string QUOTENUM)
        {
            InitializeComponent();
            this.DataContext = this;
            Title = QUOTENUM;
            PressSize = PRESSSIZE;

            LoadMaxColors();
        }

        private void LoadMaxColors()
        {
            SqlConnection cn = new(ConnectionString);
            string str = "SELECT Number FROM [Estimating].[dbo].[PressSizeColors] WHERE Size = '" + PressSize + "'";
            SqlDataAdapter da = new(str, cn); DataTable dt = new DataTable(); da.Fill(dt);

            if (dt.Rows.Count == 0) { MessageBox.Show("Not found..."); return; }
            else { MaxColors = Int32.Parse(dt.Rows[0]["Number"].ToString()); }
        }

        private void RadNumericUpDown_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        {
            string chosen = ((System.Windows.FrameworkElement)sender).Name;
            switch (chosen)
            {   case "Std1":      { BlackStd = 0; PMS      = 0; break; }
                case "BlackStd1": { Std      = 0; PMS      = 0; break; }
                case "PMS1":      { Std      = 0; BlackStd = 0; break; }
            }
        }
    }
}