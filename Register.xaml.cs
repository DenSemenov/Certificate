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
using System.Windows.Shapes;

namespace Certificate
{
    /// <summary>
    /// Логика взаимодействия для Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        SqlConnection constr = new SqlConnection(@"Data Source=.\SQLEXPRESS; Integrated Security=true; Initial Catalog=datatest;");
        public Register()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            loginBox.Clear();
            PasswordBox.Clear();
            PasswordBox2.Clear();
            Name.Clear();
            Surname.Clear();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
        }

        private void ReadyButton_Click(object sender, RoutedEventArgs e)
        {
            if (loginBox.Text != "" && PasswordBox.Password != "" && PasswordBox2.Password != "" && Name.Text != "" && Surname.Text != "")
            {
                if (PasswordBox.Password.Length > 5 && PasswordBox2.Password.Length > 5)
                {
                    if (PasswordBox.Password == PasswordBox2.Password)
                    {
                        bool isExist = false;

                        SqlCommand users = new SqlCommand("select login from \"User\"", constr);
                        SqlDataAdapter usersA = new SqlDataAdapter(users);
                        DataTable usersD = new DataTable();
                        usersA.Fill(usersD);

                        for (int i = 0; i < usersD.Rows.Count; i++)
                        {
                            if (usersD.Rows[i][0].ToString() == loginBox.Text)
                            {
                                isExist = true;
                            }
                        }

                        if (isExist == false)
                        {
                            SqlCommand com = new SqlCommand("insert into \"User\"(login, password,name,surname) values('" + loginBox.Text + "','" + PasswordBox.Password + "', '" + Name.Text + "', '" + Surname.Text + "')", constr);
                            SqlDataAdapter adapter = new SqlDataAdapter(com);
                            DataTable data = new DataTable();
                            adapter.Fill(data);

                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Такой логин уже существует");
                        }
                    }

                    else
                    {
                        MessageBox.Show("Пароли не совпадают");
                    }
                }
                else
                {
                    MessageBox.Show("Пароль должен быть больше 6 символов");
                }
            }
            else
            {
                MessageBox.Show("Заполните все поля");
            }
        }
    }
}
