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
    /// Логика взаимодействия для UserPanel.xaml
    /// </summary>
    public partial class UserPanel : Window
    {
        SqlConnection constr = new SqlConnection(@"Data Source=.\SQLEXPRESS; Integrated Security=true; Initial Catalog=datatest;");
        DataTable data2 = new DataTable();
        DataTable data3 = new DataTable();

        Grid[] panelsArr = new Grid[100];
        Label[] nameTestArr = new Label[100];
        Label[] resultArr = new Label[100];
        Label[] timeArr = new Label[100];
        Label[] dateArr = new Label[100];
        Button[] buttonArr = new Button[100];

        public UserPanel()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void UserPanel1_Activated(object sender, EventArgs e)
        {

        }

        private void GridButtonClick(object sender, RoutedEventArgs e)
        {
          
        }

        private void UserPanel1_Loaded(object sender, RoutedEventArgs e)
        {
            TestCombo.Items.Clear();

            SqlCommand com = new SqlCommand("select NameTest from test", constr);
            SqlDataAdapter adapter = new SqlDataAdapter(com);
            DataTable data = new DataTable();
            adapter.Fill(data);

            for (int i = 0; i < data.Rows.Count; i++)
            {
                TestCombo.Items.Add(data.Rows[i][0].ToString());
            }
           
            SqlCommand com2 = new SqlCommand("select t.NameTest, r.Mark, r.TimeResult, r.DateResult from Test t, Result r where t.ID_Test=r.ID_Test and id_user = '" + Properties.Settings.Default.UserID + "'", constr);
            SqlDataAdapter adapter2 = new SqlDataAdapter(com2);
            adapter2.Fill(data2);

            

            for (int i = 0; i < data2.Rows.Count; i++)
            {
                Grid g = new Grid();
                panelsArr[i] = g;
                g.Width = 980;
                g.Height = 50;
                g.Background = Brushes.Black;
                
                panelsArr[0].Margin = new Thickness(0, 0, 0, 0);

                Label nameT = new Label();
                nameTestArr[i] = nameT;
                nameT.Content = data2.Rows[i][0].ToString();
                nameT.Background = Brushes.White;
                nameT.Margin = new Thickness(1, 0, 1, 1);
                nameT.Width = 264;
                nameT.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;

                Label result = new Label();
                resultArr[i] = result;
                result.Content = data2.Rows[i][1].ToString();
                result.Background = Brushes.White;
                result.Margin = new Thickness(266, 0, 1, 1);
                result.Width = 126;
                result.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;

                Label time = new Label();
                timeArr[i] = time;
                time.Content = data2.Rows[i][2].ToString();
                time.Background = Brushes.White;
                time.Margin = new Thickness(393, 0, 1, 1);
                time.Width = 199;
                time.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;

                Label date = new Label();
                dateArr[i] = date;
                date.Content = data2.Rows[i][3].ToString();
                date.Background = Brushes.White;
                date.Margin = new Thickness(593, 0, 1, 1);
                date.Width = 199;
                date.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;

                Button but = new Button();
                buttonArr[i] = but;
                if (Double.Parse(data2.Rows[i][1].ToString()) >= 70)
                {
                    but.Content = "Сертификат";
                    but.IsEnabled = true;
                }
                else
                {
                    but.Content = "Не сдан";
                    but.IsEnabled = false;
                }
                but.Background = Brushes.White;
                but.Margin = new Thickness(793, 0, 1, 1);
                but.Width = 183;
                but.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                but.Click += but_Click;
                but.Name = "but"+i.ToString();


                g.Children.Add(nameT);
                g.Children.Add(result);
                g.Children.Add(time);
                g.Children.Add(date);
                g.Children.Add(but);

                testsTable.Children.Add(g);
                testsTable.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            }
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (TestCombo.Text != "")
            {
                Properties.Settings.Default.TestName = TestCombo.Text;

                test t = new test();
                t.ShowDialog();

                data2.Clear();

                for (int i = 0; i < panelsArr.Count(); i++)
                {
                    testsTable.Children.Remove(panelsArr[i]);
                }

                SqlCommand com2 = new SqlCommand("select t.NameTest, r.Mark, r.TimeResult, r.DateResult from Test t, Result r where t.ID_Test=r.ID_Test and id_user = '" + Properties.Settings.Default.UserID + "'", constr);
                SqlDataAdapter adapter2 = new SqlDataAdapter(com2);
                adapter2.Fill(data2);



                for (int i = 0; i < data2.Rows.Count; i++)
                {
                    Grid g = new Grid();
                    panelsArr[i] = g;
                    g.Width = 980;
                    g.Height = 50;
                    g.Background = Brushes.Black;

                    panelsArr[0].Margin = new Thickness(0, 0, 0, 0);

                    Label nameT = new Label();
                    nameTestArr[i] = nameT;
                    nameT.Content = data2.Rows[i][0].ToString();
                    nameT.Background = Brushes.White;
                    nameT.Margin = new Thickness(1, 0, 1, 1);
                    nameT.Width = 264;
                    nameT.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;

                    Label result = new Label();
                    resultArr[i] = result;
                    result.Content = data2.Rows[i][1].ToString();
                    result.Background = Brushes.White;
                    result.Margin = new Thickness(266, 0, 1, 1);
                    result.Width = 126;
                    result.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;

                    Label time = new Label();
                    timeArr[i] = time;
                    time.Content = data2.Rows[i][2].ToString();
                    time.Background = Brushes.White;
                    time.Margin = new Thickness(393, 0, 1, 1);
                    time.Width = 199;
                    time.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;

                    Label date = new Label();
                    dateArr[i] = date;
                    date.Content = data2.Rows[i][3].ToString();
                    date.Background = Brushes.White;
                    date.Margin = new Thickness(593, 0, 1, 1);
                    date.Width = 199;
                    date.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;

                    Button but = new Button();
                    buttonArr[i] = but;
                    if (Double.Parse(data2.Rows[i][1].ToString()) >= 70)
                    {
                        but.Content = "Сертификат";
                        but.IsEnabled = true;
                    }
                    else
                    {
                        but.Content = "Не сдан";
                        but.IsEnabled = false;
                    }
                    but.Background = Brushes.White;
                    but.Margin = new Thickness(793, 0, 1, 1);
                    but.Width = 183;
                    but.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    but.Click += but_Click;
                    but.Name = "but" + i.ToString();


                    g.Children.Add(nameT);
                    g.Children.Add(result);
                    g.Children.Add(time);
                    g.Children.Add(date);
                    g.Children.Add(but);

                    testsTable.Children.Add(g);
                    testsTable.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                }
            }
            else
            {
                MessageBox.Show("Выберите тест");
            }
        }

        private void but_Click(object sender, RoutedEventArgs e)
        {
            int k = Int32.Parse((sender as Button).Name.Replace("but",""));
            Properties.Settings.Default.Test = nameTestArr[k].Content.ToString();
            Properties.Settings.Default.Mark = resultArr[k].Content.ToString();
            Properties.Settings.Default.Date = dateArr[k].Content.ToString();

            PDF pdf = new PDF();
            pdf.ShowDialog();
        }

        private void ChangeProfile_Click(object sender, RoutedEventArgs e)
        {
            EditUser eu = new EditUser();
            eu.ShowDialog();
        }

        private void SwapUser_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.UserID = null;
            Properties.Settings.Default.UserLogin = null;
            Properties.Settings.Default.UserName = null;
            Properties.Settings.Default.UserSurname = null;

            MainWindow mw = new MainWindow();
            mw.Show();
            this.Hide();
        }
    }
}
