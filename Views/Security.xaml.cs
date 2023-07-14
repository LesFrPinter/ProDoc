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

        #region Property definitions

        // 07/14/2023 LFP Changed Quote_Details to provide storage space for up to 21 F_TYPES:
        //ALTER TABLE[ESTIMATING].[dbo].[Quote_Details] ALTER COLUMN Value9 VarChar(100)   // '1001' means 'F_TYPES 1 and 4 are checked'
        //ALTER TABLE[ESTIMATING].[dbo].[Quote_Details] ALTER COLUMN Value10 VarChar(100)  // '30,,,20' assigns 30% to the first ftype and 20% to the fourth;
        //                                                                                     use 'string pct[] = split(",") to retrieve values into an array.

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

        public string ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public SqlConnection? conn;
        public SqlDataAdapter? da;
        public DataTable? dt;
        public DataView? dv;
        public SqlCommand? scmd;

        private string fieldList; public string FieldList { get { return fieldList; } set { fieldList = value; OnPropertyChanged(); } }

        private int num_F_Types = 21; public int Num_F_Types { get { return num_F_Types = 21; } set { num_F_Types = value; OnPropertyChanged(); } }

        private string   quoteNum;  public string   QuoteNum  { get { return quoteNum;  } set { quoteNum  = value; OnPropertyChanged(); } }
        private string   pressSize; public string   PressSize { get { return pressSize; } set { pressSize = value; OnPropertyChanged(); } }

        private bool[]   chk;    public bool[]   Chk    { get { return chk;     } set { chk     = value; OnPropertyChanged(); } }
        private string[] ftype;  public string[] Ftype  { get { return ftype;   } set { ftype   = value; OnPropertyChanged(); } }
        private string[] flat;   public string[] Flat   { get { return flat;    } set { flat    = value; OnPropertyChanged(); } }
        private string[] run;    public string[] Run    { get { return run;     } set { run     = value; OnPropertyChanged(); } }
        private string[] plate;  public string[] Plate  { get { return plate;   } set { plate   = value; OnPropertyChanged(); } }
        private string[] finish; public string[] Finish { get { return finish;  } set { finish  = value; OnPropertyChanged(); } }
        private string[] press;  public string[] Press  { get { return press;   } set { press   = value; OnPropertyChanged(); } }
        private string[] conv;   public string[] Conv   { get { return conv;    } set { conv    = value; OnPropertyChanged(); } }
        private int[]    mult;   public int[]    Mult   { get { return mult;    } set { mult    = value; OnPropertyChanged(); } }
        private float[]  ext;    public float[]  Ext    { get { return ext;     } set { ext     = value; OnPropertyChanged(); } }

        private bool startup = true; public bool Startup { get { return startup; } set { startup = value; OnPropertyChanged(); } }

        private bool chk01; public bool Chk01 { get { return chk01; } set { chk01 = value; OnPropertyChanged(); } }
        private bool chk02; public bool Chk02 { get { return chk02; } set { chk02 = value; OnPropertyChanged(); } }
        private bool chk03; public bool Chk03 { get { return chk03; } set { chk03 = value; OnPropertyChanged(); } }
        private bool chk04; public bool Chk04 { get { return chk04; } set { chk04 = value; OnPropertyChanged(); } }
        private bool chk05; public bool Chk05 { get { return chk05; } set { chk05 = value; OnPropertyChanged(); } }
        private bool chk06; public bool Chk06 { get { return chk06; } set { chk06 = value; OnPropertyChanged(); } }
        private bool chk07; public bool Chk07 { get { return chk07; } set { chk07 = value; OnPropertyChanged(); } }
        private bool chk08; public bool Chk08 { get { return chk08; } set { chk08 = value; OnPropertyChanged(); } }
        private bool chk09; public bool Chk09 { get { return chk09; } set { chk09 = value; OnPropertyChanged(); } }
        private bool chk10; public bool Chk10 { get { return chk10; } set { chk10 = value; OnPropertyChanged(); } }
        private bool chk11; public bool Chk11 { get { return chk11; } set { chk11 = value; OnPropertyChanged(); } }
        private bool chk12; public bool Chk12 { get { return chk12; } set { chk12 = value; OnPropertyChanged(); } }
        private bool chk13; public bool Chk13 { get { return chk13; } set { chk13 = value; OnPropertyChanged(); } }
        private bool chk14; public bool Chk14 { get { return chk14; } set { chk14 = value; OnPropertyChanged(); } }
        private bool chk15; public bool Chk15 { get { return chk15; } set { chk15 = value; OnPropertyChanged(); } }
        private bool chk16; public bool Chk16 { get { return chk16; } set { chk16 = value; OnPropertyChanged(); } }
        private bool chk17; public bool Chk17 { get { return chk17; } set { chk17 = value; OnPropertyChanged(); } }
        private bool chk18; public bool Chk18 { get { return chk18; } set { chk18 = value; OnPropertyChanged(); } }
        private bool chk19; public bool Chk19 { get { return chk19; } set { chk19 = value; OnPropertyChanged(); } }
        private bool chk20; public bool Chk20 { get { return chk20; } set { chk20 = value; OnPropertyChanged(); } }
        private bool chk21; public bool Chk21 { get { return chk21; } set { chk21 = value; OnPropertyChanged(); } }

        private int mult01; public int Mult01 { get { return mult01; } set { mult01 = value; OnPropertyChanged(); } }
        private int mult02; public int Mult02 { get { return mult02; } set { mult02 = value; OnPropertyChanged(); } }
        private int mult03; public int Mult03 { get { return mult03; } set { mult03 = value; OnPropertyChanged(); } }
        private int mult04; public int Mult04 { get { return mult04; } set { mult04 = value; OnPropertyChanged(); } }
        private int mult05; public int Mult05 { get { return mult05; } set { mult05 = value; OnPropertyChanged(); } }
        private int mult06; public int Mult06 { get { return mult06; } set { mult06 = value; OnPropertyChanged(); } }
        private int mult07; public int Mult07 { get { return mult07; } set { mult07 = value; OnPropertyChanged(); } }
        private int mult08; public int Mult08 { get { return mult08; } set { mult08 = value; OnPropertyChanged(); } }
        private int mult09; public int Mult09 { get { return mult09; } set { mult09 = value; OnPropertyChanged(); } }
        private int mult10; public int Mult10 { get { return mult10; } set { mult10 = value; OnPropertyChanged(); } }
        private int mult11; public int Mult11 { get { return mult11; } set { mult11 = value; OnPropertyChanged(); } }
        private int mult12; public int Mult12 { get { return mult12; } set { mult12 = value; OnPropertyChanged(); } }
        private int mult13; public int Mult13 { get { return mult13; } set { mult13 = value; OnPropertyChanged(); } }
        private int mult14; public int Mult14 { get { return mult14; } set { mult14 = value; OnPropertyChanged(); } }
        private int mult15; public int Mult15 { get { return mult15; } set { mult15 = value; OnPropertyChanged(); } }
        private int mult16; public int Mult16 { get { return mult16; } set { mult16 = value; OnPropertyChanged(); } }
        private int mult17; public int Mult17 { get { return mult17; } set { mult17 = value; OnPropertyChanged(); } }
        private int mult18; public int Mult18 { get { return mult18; } set { mult18 = value; OnPropertyChanged(); } }
        private int mult19; public int Mult19 { get { return mult19; } set { mult19 = value; OnPropertyChanged(); } }
        private int mult20; public int Mult20 { get { return mult20; } set { mult20 = value; OnPropertyChanged(); } }
        private int mult21; public int Mult21 { get { return mult21; } set { mult21 = value; OnPropertyChanged(); } }

        private float ext01; public float Ext01 { get { return ext01; } set { ext01 = value; OnPropertyChanged(); } }
        private float ext02; public float Ext02 { get { return ext02; } set { ext02 = value; OnPropertyChanged(); } }
        private float ext03; public float Ext03 { get { return ext03; } set { ext03 = value; OnPropertyChanged(); } }
        private float ext04; public float Ext04 { get { return ext04; } set { ext04 = value; OnPropertyChanged(); } }
        private float ext05; public float Ext05 { get { return ext05; } set { ext05 = value; OnPropertyChanged(); } }
        private float ext06; public float Ext06 { get { return ext06; } set { ext06 = value; OnPropertyChanged(); } }
        private float ext07; public float Ext07 { get { return ext07; } set { ext07 = value; OnPropertyChanged(); } }
        private float ext08; public float Ext08 { get { return ext08; } set { ext08 = value; OnPropertyChanged(); } }
        private float ext09; public float Ext09 { get { return ext09; } set { ext09 = value; OnPropertyChanged(); } }
        private float ext10; public float Ext10 { get { return ext10; } set { ext10 = value; OnPropertyChanged(); } }
        private float ext11; public float Ext11 { get { return ext11; } set { ext11 = value; OnPropertyChanged(); } }
        private float ext12; public float Ext12 { get { return ext12; } set { ext12 = value; OnPropertyChanged(); } }
        private float ext13; public float Ext13 { get { return ext13; } set { ext13 = value; OnPropertyChanged(); } }
        private float ext14; public float Ext14 { get { return ext14; } set { ext14 = value; OnPropertyChanged(); } }
        private float ext15; public float Ext15 { get { return ext15; } set { ext15 = value; OnPropertyChanged(); } }
        private float ext16; public float Ext16 { get { return ext16; } set { ext16 = value; OnPropertyChanged(); } }
        private float ext17; public float Ext17 { get { return ext17; } set { ext17 = value; OnPropertyChanged(); } }
        private float ext18; public float Ext18 { get { return ext18; } set { ext18 = value; OnPropertyChanged(); } }
        private float ext19; public float Ext19 { get { return ext19; } set { ext19 = value; OnPropertyChanged(); } }
        private float ext20; public float Ext20 { get { return ext20; } set { ext20 = value; OnPropertyChanged(); } }
        private float ext21; public float Ext21 { get { return ext21; } set { ext21 = value; OnPropertyChanged(); } }

        #endregion

        public Security(string PRESSSIZE, string QUOTENUM)
        {
            InitializeComponent();

            this.DataContext = this;

            Title = QUOTENUM;
            QuoteNum  = QUOTENUM.ToString();
            PressSize = PRESSSIZE.ToString();

            Chk     = new bool  [Num_F_Types];
            Ftype   = new string[Num_F_Types];
            Flat    = new string[Num_F_Types];
            Run     = new string[Num_F_Types];
            Plate   = new string[Num_F_Types];
            Finish  = new string[Num_F_Types];
            Press   = new string[Num_F_Types];
            Conv    = new string[Num_F_Types];
            Mult    = new int   [Num_F_Types];
            Ext     = new float [Num_F_Types];

            for( int j=0; j<Num_F_Types; j++ )       // Initialize all of the arrays.
            {   Chk[j]    = false;
                Ftype[j]  = ""; 
                Flat[j]   = "";
                Run[j]    = "";
                Plate[j]  = "";
                Finish[j] = "";
                Press[j]  = "";
                Conv[j]   = "";
                Mult[j]   = 0;
                Ext[j]    = 0.00F;
            }

            FieldList = "F_TYPE, FLAT_CHARGE, RUN_CHARGE, PLATE_MATL, FINISH_MATL, CONV_MATL, PRESS_MATL";

            LoadFtypes();
            LoadData();

            PreviewKeyDown += (s, e) => { if (e.Key == Key.Escape) Close(); };  // Esc to close window
        }

        public void OnLoad(object sender, RoutedEventArgs e) 
        { 
            Startup = false; 
        }

        public void LoadFtypes()
        {
            string cmd = $"SELECT {FieldList} FROM [Estimating].[dbo].[Features] WHERE CATEGORY = 'SECURITY' AND PRESS_SIZE = '{PressSize}'";
            conn = new SqlConnection(ConnectionString);
            da = new SqlDataAdapter(cmd, conn); 
            dt = new DataTable("Security"); da.Fill(dt);
            DataView dv = dt.AsDataView();
            int start = 0;
            for (int i = start; i < start + 20; i++)
            {   Ftype[i] = dv[i]["F_TYPE"].ToString();
                Flat[i]  = dv[i]["FLAT_CHARGE"].ToString();
                Run[i]   = dv[i]["RUN_CHARGE"].ToString();
            }
        }

        public void LoadData()
        {
            string cmd = $"SELECT * FROM [ESTIMATING].[dbo].[Quote_Details] WHERE QUOTE_NUM = '{QuoteNum}' AND CATEGORY = 'Security'";
            conn = new SqlConnection(ConnectionString); da = new SqlDataAdapter(cmd, conn);
            DataTable dt2 = new DataTable(); da.Fill(dt2); dv = dt2.DefaultView;

            // Iterate through the length of the string in "Value9"
            for(int i = 0; i < dv[0]["Value9"].ToString().Length; i++)
            {   string v = dv[0]["Value9"].ToString().Substring(i,1); chk[i] = (v=="1") ? true: false;
                switch (i)
                {
                    case 1: Chk01 = chk[i]; Calculate(i.ToString()); break;
                    case 2: Chk02 = chk[i]; Calculate(i.ToString()); break;
                    case 3: Chk03 = chk[i]; Calculate(i.ToString()); break;
                    case 4: Chk04 = chk[i]; Calculate(i.ToString()); break;
                    case 5: Chk05 = chk[i]; Calculate(i.ToString()); break;
                    case 6: Chk06 = chk[i]; Calculate(i.ToString()); break;
                    case 7: Chk07 = chk[i]; Calculate(i.ToString()); break;
                    case 8: Chk08 = chk[i]; Calculate(i.ToString()); break;
                    case 9: Chk09 = chk[i]; Calculate(i.ToString()); break;
                    case 10: Chk10 = chk[i]; Calculate(i.ToString()); break;
                    case 11: Chk11 = chk[i]; Calculate(i.ToString()); break;
                    case 12: Chk12 = chk[i]; break;
                    case 13: Chk13 = chk[i]; break;
                    case 14: Chk14 = chk[i]; break;
                    case 15: Chk15 = chk[i]; break;
                    case 16: Chk16 = chk[i]; break;
                    case 17: Chk17 = chk[i]; break;
                    case 18: Chk18 = chk[i]; break;
                    case 19: Chk19 = chk[i]; break;
                    case 20: Chk20 = chk[i]; break;
                    case 21: Chk21 = chk[i]; break;
                }
            }

        }

        private void NU01_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        {
            if(Startup) { return; }

            string idx = (sender as Control).Name.Substring(2).ToString();
            int nidx = int.Parse(idx);
            float pct = float.Parse(Mult[nidx-1].ToString());
            bool Checked = Chk[nidx-1];
            if (!Checked) 
            { Ext[nidx - 1] = 0.00F; } 
            else 
            { Ext[nidx - 1] = float.Parse(Flat[nidx - 1].ToString()) * (1 + pct / 100.00F); }
            
            switch (nidx)
            {
                case 1: Ext01 = Ext[nidx - 1]; break;
                case 2: Ext02 = Ext[nidx - 1]; break;
                case 3: Ext03 = Ext[nidx - 1]; break;
                case 4: Ext04 = Ext[nidx - 1]; break;
                case 5: Ext05 = Ext[nidx - 1]; break;
                case 6: Ext06 = Ext[nidx - 1]; break;
                case 7: Ext07 = Ext[nidx - 1]; break;
                case 8: Ext08 = Ext[nidx - 1]; break;
                case 9: Ext09 = Ext[nidx - 1]; break;
                case 10: Ext10 = Ext[nidx - 1]; break;
                case 11: Ext11 = Ext[nidx - 1]; break;
                case 12: Ext12 = Ext[nidx - 1]; break;
                case 13: Ext13 = Ext[nidx - 1]; break;
                case 14: Ext14 = Ext[nidx - 1]; break;
                case 15: Ext15 = Ext[nidx - 1]; break;
                case 16: Ext16 = Ext[nidx - 1]; break;
                case 17: Ext17 = Ext[nidx - 1]; break;
                case 18: Ext18 = Ext[nidx - 1]; break;
                case 19: Ext19 = Ext[nidx - 1]; break;
                case 20: Ext20 = Ext[nidx - 1]; break;
                case 21: Ext21 = Ext[nidx - 1]; break;
            }
        }

        public void Calculate(string idx)
        {
            int nidx = int.Parse(idx);
            float pct = float.Parse(Mult[nidx - 1].ToString());
            bool Checked = Chk[nidx - 1];
            if (!Checked)
            { Ext[nidx - 1] = 0.00F; }
            else
            { Ext[nidx - 1] = float.Parse(Flat[nidx - 1].ToString()) * (1 + pct / 100.00F); }

            switch (nidx)
            {
                case 1: Ext01 = Ext[nidx - 1]; break;
                case 2: Ext02 = Ext[nidx - 1]; break;
                case 3: Ext03 = Ext[nidx - 1]; break;
                case 4: Ext04 = Ext[nidx - 1]; break;
                case 5: Ext05 = Ext[nidx - 1]; break;
                case 6: Ext06 = Ext[nidx - 1]; break;
                case 7: Ext07 = Ext[nidx - 1]; break;
                case 8: Ext08 = Ext[nidx - 1]; break;
                case 9: Ext09 = Ext[nidx - 1]; break;
                case 10: Ext10 = Ext[nidx - 1]; break;
                case 11: Ext11 = Ext[nidx - 1]; break;
                case 12: Ext12 = Ext[nidx - 1]; break;
                case 13: Ext13 = Ext[nidx - 1]; break;
                case 14: Ext14 = Ext[nidx - 1]; break;
                case 15: Ext15 = Ext[nidx - 1]; break;
                case 16: Ext16 = Ext[nidx - 1]; break;
                case 17: Ext17 = Ext[nidx - 1]; break;
                case 18: Ext18 = Ext[nidx - 1]; break;
                case 19: Ext19 = Ext[nidx - 1]; break;
                case 20: Ext20 = Ext[nidx - 1]; break;
                case 21: Ext21 = Ext[nidx - 1]; break;
            }

        }

        private void TextBox_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        { 
            // This is only for testing; reassigning Ext[i] to Ext{i} fixes the data display problem.
            MessageBox.Show(Ext[0].ToString());
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        { MessageBox.Show("Saving...", "In progress"); this.Close(); }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        { this.Close(); }

        private void ChkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (Startup) { return; }

            string idx = (sender as Control).Name.Substring(3).ToString();
            int nidx = int.Parse(idx);
            float pct = float.Parse(Mult[nidx - 1].ToString());
            bool Checked = Chk[nidx - 1];
            if (!Checked)
            { Ext[nidx - 1] = 0.00F; }
            else
            { Ext[nidx - 1] = float.Parse(Flat[nidx - 1].ToString()) * (1 + pct / 100.00F); }

            switch (nidx)
            {
                case 1: Ext01 = Ext[nidx - 1]; break;
                case 2: Ext02 = Ext[nidx - 1]; break;
                case 3: Ext03 = Ext[nidx - 1]; break;
                case 4: Ext04 = Ext[nidx - 1]; break;
                case 5: Ext05 = Ext[nidx - 1]; break;
                case 6: Ext06 = Ext[nidx - 1]; break;
                case 7: Ext07 = Ext[nidx - 1]; break;
                case 8: Ext08 = Ext[nidx - 1]; break;
                case 9: Ext09 = Ext[nidx - 1]; break;
                case 10: Ext10 = Ext[nidx - 1]; break;
                case 11: Ext11 = Ext[nidx - 1]; break;
                case 12: Ext12 = Ext[nidx - 1]; break;
                case 13: Ext13 = Ext[nidx - 1]; break;
                case 14: Ext14 = Ext[nidx - 1]; break;
                case 15: Ext15 = Ext[nidx - 1]; break;
                case 16: Ext16 = Ext[nidx - 1]; break;
                case 17: Ext17 = Ext[nidx - 1]; break;
                case 18: Ext18 = Ext[nidx - 1]; break;
                case 19: Ext19 = Ext[nidx - 1]; break;
                case 20: Ext20 = Ext[nidx - 1]; break;
                case 21: Ext21 = Ext[nidx - 1]; break;
            }
        }

        private void ChkBox_Checked(object sender, RoutedEventArgs e)
        {
            if (Startup) { return; }

            string idx = (sender as Control).Name.Substring(3).ToString();
            int nidx = int.Parse(idx);
            float pct = float.Parse(Mult[nidx - 1].ToString());
            bool Checked = Chk[nidx - 1];
            if (!Checked)
            { Ext[nidx - 1] = 0.00F; }
            else
            { Ext[nidx - 1] = float.Parse(Flat[nidx - 1].ToString()) * (1 + pct / 100.00F); }

            switch (nidx)
            {
                case 1: Ext01 = Ext[nidx - 1]; break;
                case 2: Ext02 = Ext[nidx - 1]; break;
                case 3: Ext03 = Ext[nidx - 1]; break;
                case 4: Ext04 = Ext[nidx - 1]; break;
                case 5: Ext05 = Ext[nidx - 1]; break;
                case 6: Ext06 = Ext[nidx - 1]; break;
                case 7: Ext07 = Ext[nidx - 1]; break;
                case 8: Ext08 = Ext[nidx - 1]; break;
                case 9: Ext09 = Ext[nidx - 1]; break;
                case 10: Ext10 = Ext[nidx - 1]; break;
                case 11: Ext11 = Ext[nidx - 1]; break;
                case 12: Ext12 = Ext[nidx - 1]; break;
                case 13: Ext13 = Ext[nidx - 1]; break;
                case 14: Ext14 = Ext[nidx - 1]; break;
                case 15: Ext15 = Ext[nidx - 1]; break;
                case 16: Ext16 = Ext[nidx - 1]; break;
                case 17: Ext17 = Ext[nidx - 1]; break;
                case 18: Ext18 = Ext[nidx - 1]; break;
                case 19: Ext19 = Ext[nidx - 1]; break;
                case 20: Ext20 = Ext[nidx - 1]; break;
                case 21: Ext21 = Ext[nidx - 1]; break;
            }
        }

    }
}