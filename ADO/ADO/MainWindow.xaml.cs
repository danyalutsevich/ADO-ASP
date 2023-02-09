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
using System.IO;
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
            connection = new(App.ConnectionString);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                connection.Open();
                Status.Content = "Open";
                Status.Foreground = Brushes.Green;
                DepartmentsQty();
                ProductsQty();
                ManagersQty();
                ShowDepartments();
                ShowManagers();
                ShowProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Status.Content = "Unable to open";
                Status.Foreground = Brushes.Red;
                //this.Close();
            }

        }

        private void DepartmentsQty()
        {
            using SqlCommand command = new("SELECT COUNT(*) FROM Departments", connection);
            try
            {
                int qty = (int)command.ExecuteScalar();
                DepQty.Content = qty;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
            command.Dispose();
            DepartmentsQty();
        }

        private void ProductsQty()
        {
            using SqlCommand command = new("SELECT COUNT(*) FROM Products", connection);
            try
            {
                int qty = (int)command.ExecuteScalar();
                ProdQty.Content = qty;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            using SqlCommand command = new(@"CREATE TABLE Products (
	            Id			UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	            Name		NVARCHAR(50) NOT NULL,
            	Price		FLOAT  NOT NULL
                ) ;", connection);
            try
            {
                command.ExecuteNonQuery();
                MessageBox.Show("Products created");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void ManagersQty()
        {
            using SqlCommand command = new("SELECT COUNT(*) FROM Managers", connection);
            try
            {
                int qty = (int)command.ExecuteScalar();
                ManageQty.Content = qty;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            using SqlCommand command = new(@"INSERT INTO Products
	          ( Id, Name,	Price	)
                VALUES
            ( 'DA1E17BB-A90D-4C79-B801-5462FB070F57', N'Гвоздь 100мм',			10.50	),
            ( 'A8E6BE17-5447-4804-AB61-F31ABF5A76D3', N'Шуруп 4х35',			4.25	),
            ( '21B0F444-2E4F-47D8-80C1-E69BF1C34CA8', N'Гайка М4',				6.50	),
            ( '2DCA5E44-B06D-4613-BB6A-D3BC91430BFE', N'Гровер М4',			    5.99	),
            ( '64A4DF8A-0733-4BE9-AABA-C01B4EC3612A', N'Болт 4х60',			    9.98	),
            ( 'B6D20749-B495-4B1A-BA1C-80B88E78B7CD', N'Гвоздь 80мм',			19.98	),
            ( '7B08197B-C55F-4389-891F-BF12A575DFFB', N'Отвертка PZ2',			35.50	),
            ( '870DA1A9-44F4-4018-B7FC-727A2058FAF0', N'Шуруповерт',			799		),
            ( '8FF90E21-DCDB-4D55-A557-7C6D57DBB029', N'Молоток',				216.50	),
            ( 'F7F1E576-AF8D-4749-869E-4A794FE69D42', N'Набор ""Новосел""',		52.40	),
            ( 'BB29F63D-1261-41F2-89E8-88F44D5EC409', N'Сверло 6х80',			39.98	),
            ( 'D17A4442-0A71-4673-B450-36929048ADEF', N'Шуруп 5х45',			5.98	),
            ( '69B125D7-99CC-42D6-A6FA-46687F333749', N'Винт ""потай"" 3х16',	3.98	),
            ( '94BC671A-A6B6-417A-BC9F-8AE4871A58EC', N'Дюбель 6х60',			5.50	),
            ( 'EFC6578A-00B7-4766-A7E3-79CDBA8C294B', N'Органайзер для шурупов',199		),
            ( '9654271B-AB52-4225-A30C-D75054B1733F', N'Лазерный дальномер',	1950	),
            ( 'F2585221-1ACA-4EFE-A5E8-C2F4534D1F92', N'Дрель электрическая',	990		),
            ( '4A550D3B-D1F2-40EF-AE4E-963612C6713A', N'Сварочный аппарат',		2099	),
            ( '17DB11D1-F50E-4CF4-9C54-CF1BD45802EA', N'Электроды 3мм',			49.98	),
            ( '7264D33A-16B9-4E22-B3F1-63D6DAE60078', N'Паяльник 40 Вт',		199.98	)", connection);

            try
            {
                command.ExecuteNonQuery();
                MessageBox.Show("Products filled");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            ProductsQty();

        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {

            using SqlCommand command = new(@"CREATE TABLE Managers (
        	Id			UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
        	Surname		NVARCHAR(50) NOT NULL,
        	Name		NVARCHAR(50) NOT NULL,
        	Secname		NVARCHAR(50) NOT NULL,
        	Id_main_dep UNIQUEIDENTIFIER NOT NULL REFERENCES Departments( Id ),
        	Id_sec_dep	UNIQUEIDENTIFIER REFERENCES Departments( Id ),
        	Id_chief	UNIQUEIDENTIFIER
            ) ;", connection);
            try
            {
                command.ExecuteNonQuery();
                MessageBox.Show("Managers created");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            var commandText = File.ReadAllText("Managers.sql");
            using SqlCommand command = new(commandText, connection);
            try
            {
                command.ExecuteNonQuery();
                ManagersQty();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ShowDepartments()
        {
            using SqlCommand command = new("SELECT * FROM Departments", connection);
            try
            {
                using SqlDataReader reader = command.ExecuteReader();
                StringBuilder result = new StringBuilder();
                while (reader.Read())
                {
                    var guid = reader.GetGuid(0).ToString();
                    var start = guid.Substring(0, 4);
                    var end = guid.Substring(guid.Length - 4, 4);
                    result.AppendLine(start + "..." + end + " " + reader.GetString(1));
                }
                ViewDepartments.Content = result.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void ShowManagers()
        {
            using SqlCommand command = new("SELECT * FROM Managers JOIN Departments ON Id_main_dep = Departments.Id", connection);
            try
            {
                using SqlDataReader reader = command.ExecuteReader();
                StringBuilder result = new StringBuilder();
                while (reader.Read())
                {
                    var guid = reader.GetGuid(0).ToString();
                    var start = guid.Substring(0, 4);
                    var end = guid.Substring(guid.Length - 4, 4);
                    var surname = reader.GetString(1);
                    var name = reader.GetString(2);
                    var lastname = reader.GetString(3);
                    var department = reader.GetString(8);

                    result.AppendLine(start + "..." + end + " " + surname + " " + name[0] + ". " + lastname[0] + ". " + department);
                }
                ViewManagers.Content = result.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ShowProducts()
        {
            using SqlCommand command = new("SELECT * FROM Products", connection);
            try
            {
                using SqlDataReader reader = command.ExecuteReader();
                StringBuilder result = new StringBuilder();
                while (reader.Read())
                {
                    var guid = reader.GetGuid(0).ToString();
                    var start = guid.Substring(0, 4);
                    var end = guid.Substring(guid.Length - 4, 4);
                    var name = reader.GetString(1);
                    var price = reader.GetDouble(2);
                    result.AppendLine(start + "..." + end + " " + name + " " + price);
                }
                ViewProducts.Content = result.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new ORM().ShowDialog();
            this.Show();
        }
    }
}
