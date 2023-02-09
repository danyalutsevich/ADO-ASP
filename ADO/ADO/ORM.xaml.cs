using ADO.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace ADO
{
    /// <summary>
    /// Interaction logic for ORM.xaml
    /// </summary>
    public partial class ORM : Window
    {
        public ORM()
        {
            InitializeComponent();
            Departments = new();
            Managers = new();
            Products = new();
            DataContext = this;
            connection = new(App.ConnectionString);
        }

        public ObservableCollection<Department> Departments { get; set; }
        public ObservableCollection<Manager> Managers { get; set; }
        public ObservableCollection<Product> Products { get; set; }

        private SqlConnection connection { get; set; }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            connection.Open();
            GetDepartments();
            GetManagers(); 
            GetProducts();
        }

        private void GetDepartments()
        {
            try
            {
                using SqlCommand command = new("SELECT D.Id, D.Name FROM Departments D", connection);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var department = new Department();
                    department.Id = reader.GetGuid(0);
                    department.Name = reader.GetString(1);
                    Departments.Add(department);
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.Close();

            }
        }

        private void GetManagers()
        {
            try
            {
                //(Id, Surname, Name, Secname, Id_main_dep, Id_sec_dep, Id_chief)
                using SqlCommand command = new("SELECT M.Id, M.Surname, M.Name, M.Secname, M.Id_main_dep, M.Id_sec_dep, M.Id_chief FROM Managers M", connection);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var manager = new Manager();
                    manager.Id = reader.GetGuid(0);
                    manager.Name = reader.GetString(1);
                    manager.Surname = reader.GetString(2);
                    manager.Secname = reader.GetString(3);
                    manager.Id_main_dep = reader.GetGuid(4);
                    manager.Id_sec_dep = reader.GetValue(5) == DBNull.Value ? null : reader.GetGuid(5);
                    manager.Id_chief = reader.IsDBNull(6) ? null : reader.GetGuid(6);
                    Managers.Add(manager);
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.Close();

            }
        }

        private void GetProducts()
        {
            try
            {
                using SqlCommand command = new("SELECT P.Id, P.Name, P.Price FROM Products P", connection);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var product = new Product();
                    product.Id = reader.GetGuid(0);
                    product.Name = reader.GetString(1);
                    product.Price = reader.GetDouble(2);
                    Products.Add(product);
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.Close();

            }
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem item)
            {
                if (item.Content is Department department)
                {
                    new Edit(department,this).ShowDialog();
                    //int index = Departments.IndexOf(department);
                    //Departments.RemoveAt(index);
                    //Departments.Insert(index, department);
                    //MessageBox.Show(department.Id + " " + department.Name);
                }
            }
            
            if (sender is ListViewItem itemManager)
            {
                if (itemManager.Content is Manager manager)
                {
                    //new Edit(manager,Managers).ShowDialog();
                    
                    //MessageBox.Show(manager.Id + " " + manager.Name);
                }
            }
            //this.Close();
            

        }
    }
}
