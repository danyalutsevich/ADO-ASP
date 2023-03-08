using ADO.EFCore;
using ADO.View.Edit;
using Microsoft.EntityFrameworkCore;
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

namespace ADO.View
{
    /// <summary>
    /// Interaction logic for EFCore.xaml
    /// </summary>
    public partial class EFCoreWindow : Window
    {
        public EFContext efContext { get; set; }
        public EFCoreWindow()
        {
            InitializeComponent();
            efContext = new();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateCount();
            efContext.Departments.Load();
            efContext.Products.Load();
            efContext.Managers.Load();
            efContext.Sales.Load();
            DepartmentsList.ItemsSource = efContext.Departments.Local.ToObservableCollection().Where(d => d.DeleteDt == null);
            ManagersList.ItemsSource = efContext.Managers.Local.ToObservableCollection();
            //ProductsList.ItemsSource = efContext.Products.Local.ToObservableCollection();

        }

        private void UpdateCount()
        {
            MonitorBlock.Content = "Departments count: " + efContext.Departments.Where(d => d.DeleteDt == null).Count();
            MonitorBlock.Content += "\nManagers count: " + efContext.Managers.Count();
            MonitorBlock.Content += "\nProducts count: " + efContext.Products.Count();
            MonitorBlock.Content += "\nSales count: " + efContext.Sales.Count();
        }

        private void ButtonDepartment_Click(object sender, RoutedEventArgs e)
        {
            var newDepartment = new Department();
            efContext.Departments.Add(newDepartment);
            var editWindow = new EFEditDepartment(newDepartment);
            editWindow.Owner = this;
            editWindow.ShowDialog();
            efContext.SaveChanges();
            UpdateCount();
        }

        private void ButtonManagers_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new EFEdit(new Manager());
            editWindow.Owner = this;
            if (editWindow.ShowDialog() == true)
            {
                //efContext.Managers.Add(editWindow.manager);
                efContext.SaveChanges();
                UpdateCount();
            }
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem item)
            {
                if (item.Content is Department department)
                {
                    var edit = new EFEditDepartment(department);
                    edit.Owner = this;
                    edit.DataContext = this;
                    edit.ShowDialog();
                    efContext.SaveChanges();
                }
            }
        }

        private void ShowDeletedCheck_Click(object sender, RoutedEventArgs e)
        {
            if (ShowDeletedCheck.IsChecked == true)
            {
                DepartmentsList.ItemsSource = efContext.Departments.Local.ToObservableCollection();
            }
            else
            {
                DepartmentsList.ItemsSource = efContext.Departments.Local.ToObservableCollection().Where(d => d.DeleteDt == null);
            }
        }
    }
}
