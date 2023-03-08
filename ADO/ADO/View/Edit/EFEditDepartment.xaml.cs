using ADO.EFCore;
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
    /// Interaction logic for EFEdit.xaml
    /// </summary>
    public partial class EFEditDepartment : Window
    {
        public EFEditDepartment(Department department)
        {
            InitializeComponent();
            Department = department;
        }
        
        public EFCore.Department Department { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var owner = Owner as EFCoreWindow;
            try
            {
                owner.efContext.Departments.Where(x => x.Id == Department.Id).First().Name = NameBox.Text;
            }
            catch
            {
                Department.Name = NameBox.Text;
                owner.efContext.Departments.Add(Department);
            }
            this.Close();
        }
        
        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            (Owner as EFCoreWindow).efContext.Departments.Where(x => x.Id == Department.Id).First().DeleteDt = DateTime.Now;
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            NameBox.Text = Department.Name;
            DeleteDtBox.Text = Department.DeleteDt.ToString();
        }
    }
}
