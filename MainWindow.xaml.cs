using System;
using System.Collections.Generic;
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

namespace Certificate
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection constr = new SqlConnection(@"Data Source=.\SQLEXPRESS; Integrated Security=true; Initial Catalog=datatest;");
        public MainWindow()
        {
            InitializeComponent();
        }
        DataTable data = new DataTable();
        private void LoginForm_Loaded(object sender, RoutedEventArgs e)
        {
            LoginBox.Items.Clear();
            SqlCommand com = new SqlCommand("select * from \"User\"", constr);
            SqlDataAdapter adapter = new SqlDataAdapter(com);

            adapter.Fill(data);

            for (int i = 0; i < data.Rows.Count; i++)
            {
                LoginBox.Items.Add(data.Rows[i][1].ToString());
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool loggined = false;

            for (int i = 0; i < data.Rows.Count; i++)
            {
                if (LoginBox.Text == data.Rows[i][1].ToString() && Password.Password == data.Rows[i][2].ToString())
                {
                    loggined = true;
                    Properties.Settings.Default.UserID = data.Rows[i][0].ToString();
                    Properties.Settings.Default.UserLogin = data.Rows[i][1].ToString();
                    Properties.Settings.Default.UserName = data.Rows[i][3].ToString();
                    Properties.Settings.Default.UserSurname = data.Rows[i][4].ToString();
                }
            }

            if (loggined)
            {
                UserPanel up = new UserPanel();
                up.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Неверный пароль");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Register r = new Register();
            r.ShowDialog();

            LoginBox.Items.Clear();
            SqlCommand com = new SqlCommand("select * from \"User\"", constr);
            SqlDataAdapter adapter = new SqlDataAdapter(com);

            adapter.Fill(data);

            for (int i = 0; i < data.Rows.Count; i++)
            {
                LoginBox.Items.Add(data.Rows[i][1].ToString());
            }
        }

        private void LoginForm_Activated(object sender, EventArgs e)
        {
        
        }
    }
}
