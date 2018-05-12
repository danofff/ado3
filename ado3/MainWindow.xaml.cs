using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;

namespace ado3
{
    public class AccessTab
    {
        public int TabId { get; set; }
        public string TabName { get; set; }
    }
    public partial class MainWindow : Window
    {
        private SqlConnection connection;
        public MainWindow()
        {
            InitializeComponent();
            //connectionString = "DataSource=192.168.111.139;Initial Catalog=MCS;User ID=sa;Password=Ev4865";
            connection = new SqlConnection("Data Source=192.168.111.139;Initial Catalog=MCS;User ID=sa;Password=Ev4865");
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                    StatusConnect.Content = "Opened";                 
                }
                else
                {
                    StatusConnect.Content= "Connection's already opened";
                }            
            }

            catch (Exception ex)
            {
                StatusConnect.Content = ex.Message;
            }          
        }

        private void GetTable_Click(object sender, RoutedEventArgs e)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            SqlDataAdapter da = new SqlDataAdapter(String.Format("select * from {0}", TableName.Text), connection);
            DataSet ds = new DataSet();
            try
            {
                da.Fill(ds);
                if (!ds.HasErrors)
                {
                    DtColums.ItemsSource = ds.Tables[0].Columns;
                }
            }
            catch (Exception ex)
            {
                StatusConnect.Foreground = new SolidColorBrush(Colors.Red);
                StatusConnect.Content = ex.Message; 
            }          
        }

        private void GetAccessTab_Click(object sender, RoutedEventArgs e)
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from AccessTab", connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<AccessTab> listAccessTab = new List<AccessTab>();
            foreach (DataRow item in dt.Rows)
            {
                var row = item.ItemArray;
                AccessTab tab = new AccessTab();
                tab.TabId = Int32.Parse(row[0].ToString());
                tab.TabName = row[1].ToString();
                listAccessTab.Add(tab);
            }

            DtAccessTab.ItemsSource = listAccessTab;
        }
    }
}
