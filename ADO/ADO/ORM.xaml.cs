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
                using SqlCommand command = new("SELECT D.Id, D.Name, D.DeleteDt FROM Departments D", connection);
                using var reader = command.ExecuteReader();
                Departments.Clear();
                while (reader.Read())
                {
                    var department = new Department(reader);
                    Departments.Add(department);
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Departments: "+ex.Message);
                this.Close();

            }
        }

        private void GetManagers()
        {
            try
            {
                using SqlCommand command = new("SELECT M.Id, M.Surname, M.Name, M.Secname, M.Id_main_dep, M.Id_sec_dep, M.Id_chief, M.FiredDt FROM Managers M", connection);
                using var reader = command.ExecuteReader();
                Managers.Clear();
                while (reader.Read())
                {
                    var manager = new Manager(reader);
                    Managers.Add(manager);
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Managers: "+ ex.Message);
                this.Close();

            }
        }

        private void GetProducts()
        {
            try
            {
                using SqlCommand command = new("SELECT P.Id, P.Name, P.Price, P.DeleteDt FROM Products P", connection);
                using var reader = command.ExecuteReader();
                Products.Clear();
                while (reader.Read())
                {
                    var product = new Product(reader);
                    Products.Add(product);
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Products: "+ex.Message);
                this.Close();

            }
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem item)
            {
                if (item.Content is Department department)
                {
                    var edit = new Edit(department);
                    edit.Owner = this;
                    edit.DataContext = this;
                    edit.ShowDialog();
                    GetDepartments();
                }
            }

            if (sender is ListViewItem itemManager)
            {
                if (itemManager.Content is Manager manager)
                {
                    var edit = new Edit(manager);
                    edit.Owner = this;
                    edit.DataContext = this;
                    edit.ShowDialog();
                    GetManagers();
                }
            }

            if (sender is ListViewItem itemProduct)
            {
                if (itemProduct.Content is Product product)
                {
                    var edit = new Edit(product);
                    edit.Owner = this;
                    edit.DataContext = this;
                    edit.ShowDialog();
                    GetProducts();
                }
            }
            //this.Close();


        }

        private void Button_DepartmentsAdd(object sender, RoutedEventArgs e)
        {
            Department.Create("New Department");
            GetDepartments();
        }

        private void Button_ManagersAdd(object sender, RoutedEventArgs e)
        {
            Manager.Create("New Manager", "New Manager", "New Manager", Departments[0], Departments[0], Managers[0]);
            GetManagers();
        }

        private void Button_ProductsAdd(object sender, RoutedEventArgs e)
        {
            Product.Create("New Product", 0);
            GetProducts();
        }
    }
}
