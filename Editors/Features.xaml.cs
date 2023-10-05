using SharpDX.Direct2D1;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace ProDocEstimate.Editors
{
    public partial class Features : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

        public string ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public SqlConnection conn;
        public SqlDataAdapter da;
        public SqlCommand scmd;
        public DataTable dt;
        public DataView dv;

        private bool   editing;             public bool   Editing        { get { return editing;        } set { editing         = value; OnPropertyChanged(); } }
        private string searchCategory = ""; public string SearchCategory { get { return searchCategory; } set { searchCategory  = value; OnPropertyChanged(); Filter(); } }
        private string searchSize     = ""; public string SearchSize     { get { return searchSize;     } set { searchSize      = value; OnPropertyChanged(); Filter(); } }
        private string searchFType    = ""; public string SearchFType    { get { return searchFType;    } set { searchFType     = value; OnPropertyChanged(); Filter(); } }

        public Features()
        {
            InitializeComponent();

            string cmd = "SELECT * FROM [ESTIMATING].[dbo].[FEATURES] ORDER BY GUID";
            
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlDataAdapter da = new SqlDataAdapter(cmd, conn);
            dt = new DataTable("Features"); 
            da.Fill(dt);
            dv = dt.DefaultView;
            grdData.ItemsSource = dv;
            DataContext = this;
        }

        private void Filter()
        {
                string filt = "";
            if (SearchCategory.Length > 0) { filt += $" AND CATEGORY   LIKE '{SearchCategory.ToString().TrimEnd()}%'"; }
            if (SearchSize.Length     > 0) { filt += $" AND PRESS_SIZE    = '{SearchSize.ToString().TrimEnd()}'"; }
            if (SearchFType.Length    > 0) { filt += $" AND F_TYPE     LIKE '{SearchFType.ToString().TrimEnd()}%'"; }
            if (filt.Length>0) dv.RowFilter = filt.Substring(5); // remove the first 4 character, which will contain "AND "
        }

        private void mnuFileExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void grdData_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            btnSave.IsEnabled = true;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Editing = false;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Editing = false;
        }

        private void grdData_CellEditEnding(object sender, System.Windows.Controls.DataGridCellEditEndingEventArgs e)
        {
            Editing = false;
            SqlConnection conn = new SqlConnection(ConnectionString);
        var selectedRow = grdData.SelectedItem as DataRowView;
            string ID = selectedRow[0].ToString();
            TextBox t = e.EditingElement as TextBox;
            string editedCellValue = t.Text.ToString();
            string colName = e.Column.SortMemberPath.ToString();

            string cmd = ""; string regExPattern = @"^[0-9]+$"; Regex pattern = new Regex(regExPattern);
            if (pattern.IsMatch(editedCellValue)) 
               { cmd = $"UPDATE [ESTIMATING].[dbo].[FEATURES] set {colName} =  {editedCellValue}  WHERE GUID = '{ID}'"; }
            else 
               { cmd = $"UPDATE [ESTIMATING].[dbo].[FEATURES] set {colName} = '{editedCellValue}' WHERE GUID = '{ID}'"; };
            
            if(conn.State != ConnectionState.Open) { conn.Open(); }
            SqlCommand cmd2 = new SqlCommand(cmd,conn);

            // IF it's not a string and quotes were used, report an error
            try     { cmd2.ExecuteNonQuery(); }
            catch   ( Exception ex) { MessageBox.Show(ex.Message + "\n" + cmd2); }
            finally { conn.Close(); }
            Title = "Table updated";
            Editing = false;
        }

        private void grdData_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Editing = true;
        }

        private void grdData_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Editing = true;
        }

        private void grdData_CurrentCellChanged(object sender, EventArgs e)
        {
            Editing = true;
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            SearchCategory = ""; SearchSize = ""; SearchFType = "";
            dv.RowFilter = "";
        }

        private void btnCloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            Close();
        }
    }
}
