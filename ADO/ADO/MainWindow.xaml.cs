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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Windows.Markup;

namespace ADO
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SqlConnection connection;

        public MainWindow()
        {
            InitializeComponent();
            connection = new(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\luche\Desktop\ADO\ADO\ADO\ADO.mdf;Integrated Security=True");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                connection.Open();
                Status.Content = "Open";
                Status.Foreground = Brushes.Green;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Status.Content = "Unable to open";
                Status.Foreground = Brushes.Red;
                this.Close();
            }

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (connection?.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SqlCommand command = new();
            command.Connection = connection;
            command.CommandText =
                @"CREATE TABLE Departments (
  Id      UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
  Name    NVARCHAR(50) NOT NULL
) ;";

            try
            {
                command.ExecuteNonQuery();
                MessageBox.Show("Departments created");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Dispose();
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SqlCommand command = new();
            command.Connection = connection;
            command.CommandText =
                @"
INSERT INTO Departments 
  ( Id, Name )
VALUES 
  ( 'D3C376E4-BCE3-4D85-ABA4-E3CF49612C94',  N'IT отдел'        ), 
  ( '131EF84B-F06E-494B-848F-BB4BC0604266',  N'Бухгалтерия'     ), 
  ( '8DCC3969-1D93-47A9-8B79-A30C738DB9B4',  N'Служба безопасности'), 
  ( 'D2469412-0E4B-46F7-80EC-8C522364D099',  N'Отдел кадров'     ),
  ( '1EF7268C-43A8-488C-B761-90982B31DF4E',  N'Канцелярия'     ), 
  ( '415B36D9-2D82-4A92-A313-48312F8E18C6',  N'Отдел продаж'     ), 
  ( '624B3BB5-0F2C-42B6-A416-099AAB799546',  N'Юридическая служба' )";

            try
            {
                command.ExecuteNonQuery();
                MessageBox.Show("Departments filled");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Dispose();
        }
    }
}
