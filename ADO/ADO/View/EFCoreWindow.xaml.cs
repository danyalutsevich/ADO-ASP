using ADO.EFCore;
using ADO.Entity;
using ADO.View.Edit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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
using System.Diagnostics;

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
            SalesList.ItemsSource = efContext.Sales.Local.ToObservableCollection();
            UpdateDailyStatistics();
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
            var newDepartment = new EFCore.Department();
            efContext.Departments.Add(newDepartment);
            var editWindow = new EFEditDepartment(newDepartment);
            editWindow.Owner = this;
            editWindow.ShowDialog();
            efContext.SaveChanges();
            UpdateCount();
        }

        private void ButtonManagers_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new EFEdit(new EFCore.Manager());
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
                if (item.Content is EFCore.Department department)
                {
                    var edit = new EFEditDepartment(department);
                    edit.Owner = this;
                    edit.DataContext = this;
                    edit.ShowDialog();
                    efContext.SaveChanges();
                }
            }
        }

        private void UpdateDailyStatistics()
        {
            // Статистика продажів за сьогодні:
            // загалом продажів (чеків, записів у Sales) за сьогодні (усіх, у т.ч. видалених)
            var soldToday = efContext.Sales.Where(s => s.SaleDt.Date == DateTime.Now.Date);
            Total.Content = "Total: " + soldToday.Count();
            // загальна кількість проданих товарів (сума)
            Start.Content = "Sale Start: " + soldToday.Min(s => s.SaleDt);
            End.Content = "Sale End: " + soldToday.Max(s => s.SaleDt);
            // максимальна кількість товарів у одному чеку (за сьогодні)
            MaxCheckCnt.Content = "Max Check: " + soldToday.Max(s => s.Count);
            // "середній чек" за кількістю - середнє значення кількості 
            //  проданих товарів на один чек
            AvgCheckCnt.Content = "Avg Check: " + soldToday.Average(s => s.Count);
            // Повернення - чеки, що є видаленими (кількість чеків за сьогодні)
            DeletedCheckCnt.Content = "Deleted Count: " + soldToday.Where(s => s.DeleteDt != null).Count();

            // var group = soldToday.GroupBy(s => s.ProductId).ToList()
            // .Join(efContext.Products, grp => grp.Key, p => p.Id, (grp, p) => new { Name = p.Name, Count = grp.Count() });
            var group = efContext.Products.GroupJoin(
                soldToday,
                p => p.Id,
                s => s.ProductId,
                (p, s) => new { Name = p.Name, Checks = s.Count(), Count = s.Sum(s => s.Count), Sum = s.Sum(s => s.Count) * p.Price });

            // BestProduct.Content = "Best Product: " + group.OrderByDescending(g => g.Count).First().Name + " - " + group.OrderByDescending(g => g.Count).First().Count;
            var bestByChecks = group.OrderByDescending(g => g.Checks).First();
            var bestByCount = group.OrderByDescending(g => g.Count).First();
            var bestBySum = group.OrderByDescending(g => g.Sum).First();

            BestProductByChecks.Content = "Best By Checks: " + bestByChecks.Name + " - " + bestByChecks.Checks + " items\n";
            BestProductByCount.Content = "Best By Count: " + bestByCount.Name + " - " + bestByCount.Count + " items\n";
            BestProductBySum.Content = "Best By Sum: " + bestBySum.Name + " - " + bestBySum.Sum + " UAH\n";

            var managers = efContext.Managers.GroupJoin(soldToday,
            m => m.Id,
            s => s.ManagerId,
            (m, s) => new
            {
                Name = m.Name,
                Count = s.Count(),
                Sum = s.Sum(s => s.Count),
                prodId = s.Select(s => s.ProductId),
                UAH = s.Sum(s => s.Count) * s.Select(s => s.ProductId)
                .Join(efContext.Products, p => p, s => s.Id, (p, s) => s.Price).First()
            });

            // var managersAndProducts = managers.Join(efContext.Products, s => s.prodId, p => p.Id,
            // (s, p) => new
            // {
            //     Name = s.Name,
            //     Count = s.Count,
            //     Sum = s.Sum,
            //     UAH = s.Count * p.Price,
            // });

            var bestManager = managers.OrderByDescending(m => m.Count).First();
            var topManagers = managers.OrderByDescending(m => m.Sum).Take(3);
            var topSales = managers.OrderByDescending(m => m.UAH).Take(3);

            BestManager.Content = "Best Manager: " + bestManager.Name + " - " + bestManager.Count + " checks\n";
            BestManagerTop3.Content = "Top 3 Managers:\n" + string.Join("\n", topManagers.Select(m => m.Name + " - " + m.Sum + " items"));
            TopSales.Content = "Top Sales:\n" + string.Join("\n", topSales.Select(m => m.Name + " - " + m.UAH.ToString("00") + " UAH"));
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

        double Map(double value, double min1, double max1, double min2, double max2)
        {
            return min2 + (value - min1) * (max2 - min2) / (max1 - min1);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Random rnd = new();

            for (int i = 0; i < 100; i++)
            {
                var sale = new EFCore.Sale();
                sale.Id = Guid.NewGuid();
                sale.ProductId = efContext.Products.Skip(rnd.Next(0, efContext.Products.Count())).First().Id;
                sale.ManagerId = efContext.Managers.Skip(rnd.Next(0, efContext.Managers.Count())).First().Id;
                sale.SaleDt = DateTime.Now.AddDays(-rnd.Next(0, 2));
                sale.DeleteDt = rnd.Next(0, 2) > 0 ? sale.SaleDt.AddDays(rnd.Next(0, 2)) : null;

                var price = efContext.Products.Where(p => p.Id == sale.ProductId).First().Price;
                var maxPrice = efContext.Products.Max(p => p.Price);
                var minPrice = efContext.Products.Min(p => p.Price);
                var maxCount = (int)Map(maxPrice - price + 10, 2, 100, minPrice, maxPrice);
                sale.Count = rnd.Next(1, maxCount);

                efContext.Sales.Add(sale);
            }
            efContext.SaveChanges();
            UpdateCount();
            UpdateDailyStatistics();
        }
    }
}
