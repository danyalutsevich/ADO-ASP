using ADO.DAL;
using ADO.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace ADO.View
{
    /// <summary>
    /// Interaction logic for DALWindow.xaml
    /// </summary>
    public partial class DALWindow : Window
    {
        private readonly DataContext _context;
        public ObservableCollection<Department> DepartmentsList { get; set; }
        public ObservableCollection<Manager> ManagersList { get; set; }

        public DALWindow()
        {
            InitializeComponent();
            _context = new();
            this.DataContext = this;
            DepartmentsList = new(_context.Departments.GetAll());
            ManagersList = new(_context.Managers.GetAll());
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(_context.Departments.GetAll().Count.ToString());
        }
    }
}
