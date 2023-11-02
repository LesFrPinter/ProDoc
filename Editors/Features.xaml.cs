using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ProDocEstimate.Editors
{
    public partial class Features : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

        #region Properties
        public string ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public SqlConnection conn;
        public SqlDataAdapter da;
        public SqlCommand scmd;
        public DataTable dt;
        public DataTable dt2;
        public DataView dv;

        private bool   editing;             public bool   Editing        { get { return editing;        } set { editing         = value; OnPropertyChanged(); } }
        private string searchCategory = ""; public string SearchCategory { get { return searchCategory; } set { searchCategory  = value; OnPropertyChanged(); Filter(); } }
        private string searchSize     = ""; public string SearchSize     { get { return searchSize;     } set { searchSize      = value; OnPropertyChanged(); Filter(); } }
        private string searchFType    = ""; public string SearchFType    { get { return searchFType;    } set { searchFType     = value; OnPropertyChanged(); Filter(); } }
        #endregion

        public Features()
        {
            InitializeComponent();

            string cmd = "SELECT *, CAST(REPLACE(Number,' -','') AS Integer) AS NUMBER2 FROM [ESTIMATING].[dbo].[FEATURES]";
            
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlDataAdapter da = new SqlDataAdapter(cmd, conn);
            dt = new DataTable("Features"); 
            da.Fill(dt);
            dv = dt.DefaultView;
            grdData.ItemsSource = dv;

            DataContext = this;

            PreviewKeyDown += (s, e) =>
            {
                if (e.Key == Key.Escape) { Close(); }
                else if (e.Key == Key.Delete) { e.Handled = true; }
            };
        }

        private void Filter()
        {
            string filt = "";
            if (SearchCategory.Length > 0) { filt += $" AND CATEGORY   LIKE '{SearchCategory.ToString().TrimEnd()}%'"; }
            if (SearchSize.Length     > 0) { filt += $" AND PRESS_SIZE    = '{SearchSize.ToString().TrimEnd()}'"; }
            if (SearchFType.Length    > 0) { filt += $" AND F_TYPE     LIKE '{SearchFType.ToString().TrimEnd()}%'"; }
            if (filt.Length>0) dv.RowFilter = filt.Substring(5); // remove the first 4 character, which will contain "AND "
            CatChanged();
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

        private void CAT_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CatChanged();            
        }

        private void CatChanged()
        {
            if (SearchCategory == null || SearchCategory.ToString().Length == 0) return;

            string cmd = $"SELECT DISTINCT PRESS_SIZE, CASE WHEN PRESS_SIZE LIKE '%SP' THEN 1 ELSE 0 END AS GRP FROM [ESTIMATING].[dbo].[FEATURES] WHERE CATEGORY = '{SearchCategory}' AND PRESS_SIZE NOT LIKE '%SP' UNION SELECT DISTINCT PRESS_SIZE, CASE WHEN PRESS_SIZE LIKE '%SP' THEN 1 ELSE 0 END AS GRP FROM [ESTIMATING].[dbo].[FEATURES] WHERE CATEGORY = '{SearchCategory}' AND PRESS_SIZE LIKE '%SP' ORDER BY GRP";

            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlDataAdapter da = new SqlDataAdapter(cmd, conn);
            dt = new DataTable("Sizes");
            da.Fill(dt);
            DataView dv = dt.DefaultView;
            cmbSize.Items.Clear();
            for (int i = 0; i < dv.Count; i++) { cmbSize.Items.Add(dv[i][0].ToString()); }

            cmd = $"SELECT DISTINCT F_TYPE FROM [ESTIMATING].[dbo].[FEATURES] WHERE CATEGORY = '{SearchCategory}' ORDER BY F_TYPE";

            SqlDataAdapter da2 = new SqlDataAdapter(cmd, conn);
            dt2 = new DataTable("FTypes");
            da2.Fill(dt2);
            DataView dv2 = dt2.DefaultView;
            cmbFType.Items.Clear();
            for (int i = 0; i < dv2.Count; i++) { cmbFType.Items.Add(dv2[i][0].ToString()); }
        }

    }
}