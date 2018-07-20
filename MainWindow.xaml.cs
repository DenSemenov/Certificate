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

       /* private void ex_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Office.Interop.Excel.Application ObjExcel = new Microsoft.Office.Interop.Excel.Application();
            //Открываем книгу.                                                                                                                                                        
            Microsoft.Office.Interop.Excel.Workbook ObjWorkBook = ObjExcel.Workbooks.Open(@"D:\Programs\Учеба\Olimp2018\olymp_fos_res\Ресурсы\Resourse.xlsx", 0, false, 5, "", "", false, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "", true, false, 0, true, false, false);
            //Выбираем таблицу(лист).
            Microsoft.Office.Interop.Excel.Worksheet ObjWorkSheet;
            ObjWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)ObjWorkBook.Sheets[1];

            for (int i = 2; i < 1000; i++)
            {
                SqlCommand com = new SqlCommand("insert into BankOfQuestions values('" + ObjWorkSheet.Cells[i, 1].Text.Replace("'", "") + "', '" + ObjWorkSheet.Cells[i, 2].Text.Replace("'", "") + "', '" + ObjWorkSheet.Cells[i, 3].Text.Replace("'", "") + "','" + ObjWorkSheet.Cells[i, 4].Text.Replace("'", "") + "','" + ObjWorkSheet.Cells[i, 5].Text.Replace("'", "") + "','" + ObjWorkSheet.Cells[i, 6].Text.Replace("'", "") + "','" + ObjWorkSheet.Cells[i, 7].Text.Replace("'", "") + "','" + ObjWorkSheet.Cells[i, 8].Text.Replace("'", "") + "','" + ObjWorkSheet.Cells[i, 9].Text.Replace("'", "") + "','" + ObjWorkSheet.Cells[i, 10].Text.Replace("'", "") + "','" + ObjWorkSheet.Cells[i, 11].Text.Replace("'", "") + "','" + ObjWorkSheet.Cells[i, 12].Text.Replace("'", "") + "','" + ObjWorkSheet.Cells[i, 13].Text.Replace("'", "") + "','" + ObjWorkSheet.Cells[i, 14].Text.Replace("'", "") + "','" + ObjWorkSheet.Cells[i, 15].Text.Replace("'", "") + "','" + ObjWorkSheet.Cells[i, 16].Text.Replace("'", "") + "')", constr);
                SqlDataAdapter a = new SqlDataAdapter(com);
                DataTable d = new DataTable();
                a.Fill(d);

            }
               
            ObjExcel.Quit();
        }*/
    }
}
