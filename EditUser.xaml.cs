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
    /// Логика взаимодействия для EditUser.xaml
    /// </summary>
    public partial class EditUser : Window
    {
        SqlConnection constr = new SqlConnection(@"Data Source=.\SQLEXPRESS; Integrated Security=true; Initial Catalog=datatest;");
        public EditUser()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SqlCommand com = new SqlCommand("select login, password, name, surname from \"User\" where ID_User = '"+Properties.Settings.Default.UserID+"'", constr);
            SqlDataAdapter a = new SqlDataAdapter(com);
            DataTable data = new DataTable();
            a.Fill(data);

            loginBox.Text = data.Rows[0][0].ToString();
            PasswordBox.Password = data.Rows[0][1].ToString();
            PasswordBox2.Password = data.Rows[0][1].ToString();
            Name.Text = data.Rows[0][2].ToString();
            Surname.Text = data.Rows[0][3].ToString();

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
                                if (usersD.Rows[i][0].ToString() != Properties.Settings.Default.UserLogin)
                                {
                                    isExist = true;
                                }
                            }
                        }

                        if (isExist == false)
                        {
                            SqlCommand com = new SqlCommand("update \"User\" set login = '" + loginBox.Text + "', password = '" + PasswordBox.Password + "', name = '" + Name.Text + "', surname = '" + Surname.Text + "' where id_user = '" + Properties.Settings.Default.UserID + "'", constr);
                            SqlDataAdapter a = new SqlDataAdapter(com);
                            DataTable data = new DataTable();
                            a.Fill(data);

                            Properties.Settings.Default.UserLogin = loginBox.Text;
                            Properties.Settings.Default.UserName = Name.Text;
                            Properties.Settings.Default.UserSurname = Surname.Text;

                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Такой логин уже сущестует");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Пароли не совпадают");
                    }
                }
                else
                {
                    MessageBox.Show("Пароль не может быть меньше 6 символов");
                }
            }
            else
            {
                MessageBox.Show("Заполните все поля");
            }
        }
    }
}
