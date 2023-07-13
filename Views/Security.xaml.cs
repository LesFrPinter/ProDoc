using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ProDocEstimate.Views
{
    public partial class Security : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

        public string ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public SqlConnection? conn;
        public SqlDataAdapter? da;
        public DataTable? dt;
        public SqlCommand? scmd;

        private string fieldList; public string FieldList { get { return fieldList; } set { fieldList = value; OnPropertyChanged(); } }

        private string   quoteNum;  public string   QuoteNum  { get { return quoteNum;  } set { quoteNum  = value; OnPropertyChanged(); } }
        private string   pressSize; public string   PressSize { get { return pressSize; } set { pressSize = value; OnPropertyChanged(); } }

        private bool[]   chk;       public bool[]   Chk       { get { return chk;       } set { chk       = value; OnPropertyChanged(); } }
        private string[] ftype;     public string[] Ftype     { get { return ftype;     } set { ftype     = value; OnPropertyChanged(); } }
        private string[] flat;      public string[] Flat      { get { return flat;      } set { flat      = value; OnPropertyChanged(); } }
        private string[] run;       public string[] Run       { get { return run;       } set { run       = value; OnPropertyChanged(); } }
        private int[]    mult;      public int[]    Mult      { get { return mult;      } set { mult      = value; OnPropertyChanged(); } }
        private float[]  ext;       public float[]  Ext       { get { return ext;       } set { ext       = value; OnPropertyChanged(); } }

        public Security(string PRESSSIZE, string QUOTENUM)
        {
            InitializeComponent();
            this.DataContext = this;
            Title = QUOTENUM;
            QuoteNum = QUOTENUM.ToString();
            PressSize = PRESSSIZE.ToString();
            this.DataContext = this;

            Title= QUOTENUM.ToString();

            Chk     = new bool[21];
            Ftype   = new string[21];
            Flat    = new string[21];
            Mult    = new int[21];
            Ext     = new float[21];

            for(int j=0;j<21;j++)       // Initialize all of the arrays.
            {   Chk[j]   = false;
                Ftype[j] = ""; 
                Flat[j]  = "";
                Mult[j]  = 0;
                Ext[j]   = 0.00F;
            }

            LoadFtypes();

            PreviewKeyDown += (s, e) => { if (e.Key == Key.Escape) Close(); };
        }

        public void LoadFtypes()
        {
            FieldList = "F_TYPE, FLAT_CHARGE, RUN_CHARGE, PLATE_MATL, FINISH_MATL, CONV_MATL, PRESS_MATL";
            string cmd = $"SELECT {FieldList} FROM [Estimating].[dbo].[Features] WHERE CATEGORY = 'SECURITY' AND PRESS_SIZE = '{PressSize}'";
            conn = new SqlConnection(ConnectionString);
            da = new SqlDataAdapter(cmd, conn); 
            dt = new DataTable("Security"); da.Fill(dt);
            DataView dv = dt.AsDataView();
            int start = 0;
            for (int i = start; i < start + 20; i++)
            {   Chk[i]   = false; 
                Ftype[i] = dv[i]["F_TYPE"].ToString();
                Flat[i]  = dv[i]["FLAT_CHARGE"].ToString();
                Mult[i]  = 0;
                Ext[i]   = float.Parse(dv[i]["FLAT_CHARGE"].ToString());
            }


        }

        //private void NU01_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e) {}
        //private void NU01_LostFocus(object sender, RoutedEventArgs e) {}

        private void NU01_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        {
            string idx = (sender as Control).Name.Substring(2).ToString();
            int nidx = int.Parse(idx) - 1;
            float pct = float.Parse(Mult[nidx].ToString());
            Ext[nidx] = float.Parse(Flat[nidx].ToString()) * (1 + pct / 100.00F);
        }
    }
}