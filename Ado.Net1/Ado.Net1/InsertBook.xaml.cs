using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.Windows.Shapes;

namespace Ado.Net1
{
    
    public partial class InsertBook : Window
    {
        public InsertBook()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txb_Id.Text) ||string.IsNullOrEmpty(txb_IdAuthor.Text) || 
                string.IsNullOrEmpty(txb_IdCategory.Text) ||
                string.IsNullOrEmpty(txb_IdPress.Text) || string.IsNullOrEmpty(txb_IdThemes.Text) ||
                string.IsNullOrEmpty(txb_Name.Text) || string.IsNullOrEmpty(txb_Pages.Text) ||
                string.IsNullOrEmpty(txb_Quantity.Text) || string.IsNullOrEmpty(txb_YearPress.Text))
            {
                MessageBox.Show("Fill All Data");
            }
            else
            {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
                {
                    SqlCommand command = new SqlCommand(@"Insert Into Books(Id,Name,Pages,YearPress,Id_Themes,Id_Category,
                                                        Id_Author,Id_Press,Comment,Quantity)
Values('" +txb_Id.Text+ "','"+ txb_Name.Text+ "','" +txb_Pages.Text+ "','" +txb_YearPress.Text+ "','" +txb_IdThemes.Text+ "','" +txb_IdCategory.Text+ "','" +txb_IdAuthor.Text+ "','" +txb_IdPress.Text+ "','" +txb_Comment.Text+ "','" +txb_Quantity.Text+ "')",conn);


                    conn.Open();
                    command.ExecuteNonQuery();
                }
                MessageBox.Show("Insert succesully!");
            }
        }
    }
}
