using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
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

namespace Ado.Net1
{
   
    public partial class MainWindow : Window
    {
        DataTable dataTable = null;
        SqlDataAdapter dataAdapter = null;
        SqlConnection conn = null;
        
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString);
            
            ReadData();
        }

        public void ReadData()
        {
            using (var conn=new SqlConnection(ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
            {
                SqlCommand command1 = new SqlCommand("Select FirstName as FullName From Authors",conn);
                conn.Open();

                SqlDataReader dataReader1 = command1.ExecuteReader();
                while (dataReader1.Read())
                {
                    Authorcmbx.SelectedIndex = 0;
                    datagrid.ItemsSource = null;
                    Authorcmbx.Items.Add(dataReader1["FullName"]);
                }
                dataReader1.Close();

                SqlCommand command2 = new SqlCommand("Select Name From Categories",conn);
                

                SqlDataReader dataReader2 = command2.ExecuteReader();
                while (dataReader2.Read())
                {
                    Categorycmbx.SelectedIndex = 0;
                    datagrid.ItemsSource = null;
                    Categorycmbx.Items.Add(dataReader2["Name"]);
                }


                dataReader2.Close();
            }
        }

        private void Authorcmbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string selectSQL = @"Select Books.Id,Books.Name,Pages,YearPress,Id_Themes,Id_Category,Id_Author,Id_Press,Comment,Quantity
                                 From Books Inner Join Authors
                                 On Books.Id_Author = Authors.Id
                                 Where Authors.FirstName = @p1 ";

                dataAdapter = new SqlDataAdapter(selectSQL, conn);


                var firstname = Authorcmbx.SelectedValue.ToString();
                dataAdapter.SelectCommand.Parameters.AddWithValue("@p1", firstname);

                dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                datagrid.ItemsSource = dataTable.AsDataView();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Exception" + ex.Message);
            }
           
        }

        private void Categorycmbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string selectSQL = @"Select Books.Id,Books.Name,Pages,YearPress,Id_Themes,Id_Category,Id_Author,Id_Press,Comment,Quantity
                                 From Books Inner Join Categories
                                 On Books.Id_Category=Categories.Id
                                 Where Categories.Name=@p1";

                dataAdapter = new SqlDataAdapter(selectSQL, conn);

                var categoryname = Categorycmbx.SelectedValue.ToString();
                dataAdapter.SelectCommand.Parameters.AddWithValue("@p1", categoryname);

                dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                datagrid.ItemsSource = dataTable.AsDataView();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Exception" + ex.Message);
            }
            
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txb.Text))
                {
                    dataAdapter = new SqlDataAdapter($"select * from Books Where Name Like '{txb.Text}%'", conn);
                    dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    datagrid.ItemsSource = dataTable.AsDataView();
                }
                else
                {
                    datagrid.ItemsSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception:" + ex.Message);
            }
            
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            InsertBook insertBook = new InsertBook();
            insertBook.ShowDialog();
        }

        private void btn_update_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dataAdapter = new SqlDataAdapter($"select * from Books", conn);
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
                dataAdapter.Update(dataTable);
                MessageBox.Show("Update successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception" + ex.Message);
               
            }
            
        }

    }
}
