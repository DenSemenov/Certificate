using System;
using System.Collections.Generic;
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
using iTextSharp;
using iTextSharp.text.pdf;
using System.Drawing;
using System.Data.SqlClient;
using System.Data;
using iTextSharp.text;
using System.IO;
using Microsoft.Win32;
using System.Drawing.Printing;

namespace Certificate
{
    /// <summary>
    /// Логика взаимодействия для PDF.xaml
    /// </summary>
    public partial class PDF : Window
    {
        SqlConnection constr = new SqlConnection(@"Data Source=.\SQLEXPRESS; Integrated Security=true; Initial Catalog=datatest;");
        public PDF()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SqlCommand com = new SqlCommand("select ID_result, CAST(dateResult AS DATE) from Result where ID_Test =(select ID_Test from Test where NameTest ='" + Properties.Settings.Default.Test + "')", constr);
            SqlDataAdapter adapter = new SqlDataAdapter(com);
            DataTable data = new DataTable();
            adapter.Fill(data);
            

            Bitmap bmp = new Bitmap(@"D:\Programs\Хлам\sertificate.jpg");
            Graphics g = Graphics.FromImage(bmp);

            g.DrawString("СЕРТИФИКАТ", new System.Drawing.Font("Arial", 10), new SolidBrush(System.Drawing.Color.Black), new RectangleF(1198 / 2 - g.MeasureString("СЕРТИФИКАТ", new System.Drawing.Font("Arial", 10)).ToSize().Width / 2, 120, 0, 0), new StringFormat(StringFormatFlags.NoWrap));
            g.DrawString("Подтверждает, что " + Properties.Settings.Default.UserName + " " + Properties.Settings.Default.UserSurname + " успешно сдал тест ", new System.Drawing.Font("Arial", 4), new SolidBrush(System.Drawing.Color.Black), new RectangleF(1198 / 2 - g.MeasureString("Подтверждает, что " + Properties.Settings.Default.UserName + " " + Properties.Settings.Default.UserSurname + " успешно сдал тест ", new System.Drawing.Font("Arial", 4)).ToSize().Width / 2, 300, 0, 0), new StringFormat(StringFormatFlags.NoWrap));
            if (Properties.Settings.Default.Test.Length < 50)
            {
                g.DrawString(Properties.Settings.Default.Test + " с результатом " + Properties.Settings.Default.Mark + " баллов", new System.Drawing.Font("Arial", 3), new SolidBrush(System.Drawing.Color.Black), new RectangleF(1198 / 2 - g.MeasureString(Properties.Settings.Default.Test + " с результатом " + Properties.Settings.Default.Mark + " баллов", new System.Drawing.Font("Arial", 3)).ToSize().Width / 2, 350, 0, 0), new StringFormat(StringFormatFlags.NoWrap));
            }
            else
            {
                g.DrawString(Properties.Settings.Default.Test, new System.Drawing.Font("Arial", 3), new SolidBrush(System.Drawing.Color.Black), new RectangleF(1198 / 2 - g.MeasureString(Properties.Settings.Default.Test, new System.Drawing.Font("Arial", 3)).ToSize().Width / 2, 350, 0, 0), new StringFormat(StringFormatFlags.NoWrap));
                g.DrawString("с результатом " + Properties.Settings.Default.Mark + " баллов", new System.Drawing.Font("Arial", 3), new SolidBrush(System.Drawing.Color.Black), new RectangleF(1198 / 2 - g.MeasureString("с результатом " + Properties.Settings.Default.Mark + " баллов", new System.Drawing.Font("Arial", 3)).ToSize().Width / 2, 400, 0, 0), new StringFormat(StringFormatFlags.NoWrap));
            }

            g.DrawString("Номер сертификата: " +data.Rows[0][0].ToString(), new System.Drawing.Font("Arial", 4), new SolidBrush(System.Drawing.Color.Black), new RectangleF(720, 630, 0, 0), new StringFormat(StringFormatFlags.NoWrap));
            g.DrawString("Дата: " + data.Rows[0][1].ToString().Replace("0:00:00",""), new System.Drawing.Font("Arial", 4), new SolidBrush(System.Drawing.Color.Black), new RectangleF(150, 630, 0, 0), new StringFormat(StringFormatFlags.NoWrap));
            g.DrawString("Семёнов Денис Юриевич", new System.Drawing.Font("Arial", 3), new SolidBrush(System.Drawing.Color.DarkSlateGray), new RectangleF(1198 / 2 - g.MeasureString("Семёнов Денис Юриевич", new System.Drawing.Font("Arial", 3)).ToSize().Width / 2, 773, 0, 0), new StringFormat(StringFormatFlags.NoWrap));

            bmp.Save(@"D:\Programs\Хлам\Sertificate" + data.Rows[0][0].ToString() + ".jpg");

            BitmapImage bm1 = new BitmapImage();
            bm1.BeginInit();
            bm1.UriSource = new Uri(@"D:\Programs\Хлам\Sertificate" + data.Rows[0][0].ToString() + ".jpg", UriKind.Relative);
            bm1.CacheOption = BitmapCacheOption.OnLoad;
            bm1.EndInit();

            image.Source = bm1;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintVisual(image, "Распечатываем элемент Canvas");
            }
        }

        private void pdfButton_Click(object sender, RoutedEventArgs e)
        {
            var doc = new Document();
            PdfWriter.GetInstance(doc, new FileStream(@"D:\Programs\Хлам\Sertificate1.pdf", FileMode.Create));
            doc.SetPageSize(PageSize.A5.Rotate());
            doc.Open();
            
            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(@"D:\Programs\Хлам\Sertificate1.jpg");
            jpg.SetAbsolutePosition(0, 5);
            jpg.ScalePercent(50f);
            jpg.Alignment = Element.ALIGN_CENTER;
            doc.Add(jpg);
            doc.Close();


            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "(*.pdf)|*.pdf";

            if (saveFileDialog1.ShowDialog() == true)
            {
                using (StreamWriter sw = new StreamWriter(saveFileDialog1.OpenFile(), System.Text.Encoding.Default))
                {
                    sw.Close();
                    try
                    {
                        File.Delete(saveFileDialog1.FileName);
                    }
                    catch { }
                 

                    File.Copy(@"D:\Programs\Хлам\Sertificate1.pdf", saveFileDialog1.FileName);
                    
                }
            }

        }
    }
}
