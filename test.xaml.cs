using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Certificate
{
    /// <summary>
    /// Логика взаимодействия для test.xaml
    /// </summary>
    public partial class test : Window
    {
        SqlConnection constr = new SqlConnection(@"Data Source=.\SQLEXPRESS; Integrated Security=true; Initial Catalog=datatest;");
        DataTable data = new DataTable();
        int QuestionID = 0;
        int CBCount = 0;
        int Otvet = 0;
        int Time = 119;
        int AllTime = 0;
        double sum = 0;
        int count = 0;
        bool TimeMod = false;


        RadioButton[] rbArray = new RadioButton[10];
        public test()
        {
            InitializeComponent();
        }

        private void testForm_Loaded(object sender, RoutedEventArgs e)
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();

            DispatcherTimer timer2 = new DispatcherTimer();
            timer2.Tick += new EventHandler(timer_Tick2);
            timer2.Interval = new TimeSpan(0, 0, 1/2);
            timer2.Start();

            TestName.Content = "Тест на тему \""+Properties.Settings.Default.TestName+"\"";

            SqlCommand com = new SqlCommand("select * from BankOfQuestions where ID_test = (select ID_test from Test where NameTest = '"+Properties.Settings.Default.TestName+"')", constr);
            SqlDataAdapter adapter = new SqlDataAdapter(com);
            adapter.Fill(data);

            FlowDocument document = new FlowDocument();
            Paragraph paragraph = new Paragraph();
            paragraph.Inlines.Add(new Bold(new Run(data.Rows[0][2].ToString())));
            document.Blocks.Add(paragraph);
            Question.Document = document;

            count = data.Rows.Count;

            for (int i = 3; i < 16; i++)
            {
                if (data.Rows[0][i].ToString() != ""){
                    CBCount++;
                }
            }

            for (int i = 0; i < CBCount-1; i++){
                RadioButton c = new RadioButton();
                rbArray[i] = c;
                c.Margin = new Thickness(10, 10, 10, 10);
                c.Name = "Answer"+(i+1).ToString();
                c.Content = data.Rows[0][i+3].ToString();
                c.Checked+=c_Checked;
                checkBoxItems.Children.Add(c);
            }

        }

        private void c_Checked(object sender, RoutedEventArgs e)
        {
            Otvet = Int32.Parse((sender as RadioButton).Name.ToString().Replace("Answer", ""));
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (Time / 60 > 0)
            {
                TimeLabel.Content = "Осталось: " + (Time / 60).ToString() + " мин. " + (Time % 60).ToString() + " сек.";
            }
            else
            {
                TimeLabel.Content = "Осталось: " + (Time % 60).ToString() + " сек.";
               
            }
            Time--;
            AllTime++;

            if (Time == 0)
            {
                NextQ();
                Time = 119;
            }
        }

        private void timer_Tick2(object sender, EventArgs e)
        {
            if (Time < 10)
            {
                if (TimeMod == false)
                {
                    TimeLabel.Foreground = System.Windows.Media.Brushes.Red;
                    TimeMod = true;
                }
                else
                {
                    TimeLabel.Foreground = System.Windows.Media.Brushes.Black;
                    TimeMod = false;
                }
            }
            else
            {
                TimeLabel.Foreground = System.Windows.Media.Brushes.Black;
            }
        }
        private void End_Click(object sender, RoutedEventArgs e)
        {
            if (QuestionID == count - 1)
            {
                Next_Click(sender, e);
                int t = AllTime;
                int hh = AllTime / 1200;
                int mm = AllTime / 60 - 60 * hh;
                int ss = AllTime - 60 * 1200 * hh;

                string rdyTime = hh.ToString() + ":" + mm.ToString() + ":" + ss.ToString();

                SqlCommand c = new SqlCommand("if (select ID_Test from Result where ID_Test = (select ID_Test from Test where NameTest = '" + Properties.Settings.Default.TestName + "') and ID_User = '"+Properties.Settings.Default.UserID+"') = (select ID_Test from Test where NameTest = '" + Properties.Settings.Default.TestName + "') update Result set Mark = '" + sum.ToString() + "', TimeResult = '" + rdyTime + "', DateResult = getdate() where ID_Test = (select ID_Test from Test where NameTest = '" + Properties.Settings.Default.TestName + "') and ID_User = '" + Properties.Settings.Default.UserID + "' else insert into Result(ID_User, ID_Test, Mark, TimeResult, DateResult) values('" + Properties.Settings.Default.UserID + "', (select ID_Test from Test where NameTest='" + Properties.Settings.Default.TestName + "'), '" + sum.ToString() + "', '" + rdyTime + "', getdate())", constr);
                SqlDataAdapter aC = new SqlDataAdapter(c);
                DataTable dC = new DataTable();
                aC.Fill(dC);

                MessageBox.Show("Ваш результат " + sum.ToString() + " баллов");

                this.Close();
            }
            else
            {
                this.Close();
            }
        }

        private void NextQ()
        {
            if (Otvet.ToString() == data.Rows[QuestionID][15].ToString())
            {
                //MessageBox.Show("Верно");
                sum = sum + 100 / count;
            }
            else
            {
                //MessageBox.Show("Неверно");
            }

            for (int i = 0; i < CBCount - 1; i++)
            {
                checkBoxItems.Children.Remove(rbArray[i]);
            }

            QuestionID++;
            CBCount = 0;

            if (QuestionID == count-1)
            {
                Next.IsEnabled = false;
            }

            if (QuestionID != count)
            {
                FlowDocument document = new FlowDocument();
                Paragraph paragraph = new Paragraph();
                paragraph.Inlines.Add(new Bold(new Run(data.Rows[QuestionID][2].ToString())));
                document.Blocks.Add(paragraph);
                Question.Document = document;

                for (int i = 3; i < 16; i++)
                {
                    if (data.Rows[QuestionID][i].ToString() != "")
                    {
                        CBCount++;
                    }
                }

                for (int i = 0; i < CBCount - 1; i++)
                {
                    RadioButton c = new RadioButton();
                    rbArray[i] = c;
                    c.Margin = new Thickness(10, 10, 10, 10);
                    c.Name = "Answer" + (i + 1).ToString();
                    c.Content = data.Rows[QuestionID][i + 3].ToString();
                    c.Checked += c_Checked;
                    checkBoxItems.Children.Add(c);
                }

                Time = 120;
            }
        }
        private void Next_Click(object sender, RoutedEventArgs e)
        {
            NextQ();               
        }
    }
}
