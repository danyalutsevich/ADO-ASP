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

namespace ADO.View.Edit
{
    /// <summary>
    /// Interaction logic for EFEditManager.xaml
    /// </summary>
    public partial class EFEditManager : Window
    {
        public EFEditManager(Manager manager)
        {
            InitializeComponent();
            this.manager = manager;
        }
        
        public Manager manager { get; set; }

        private void Button_Save(object sender, RoutedEventArgs e)
        {
            manager.Id = Guid.Parse(Id.Text);
            manager.Name = Name.Text;
            manager.Surname = Surname.Text;
            manager.Secname = Secname.Text;
            manager.Id_main_dep = Guid.Parse(Id_main_dep.Text);
            manager.Id_sec_dep = Guid.Parse(Id_sec_dep.Text);
            manager.Id_chief = Guid.Parse(Id_chief.Text);
            manager.FiredDt = DateTime.Parse(FiredDt.Text);
            DialogResult = true;
        }
        private void Button_Delete(object sender, RoutedEventArgs e) { }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Id.Text = manager.Id.ToString();
            Name.Text = manager.Name;
            Surname.Text = manager.Surname;
            Id_main_dep.Text = manager.Id_main_dep.ToString();
            Id_chief.Text = manager.Id_chief.ToString();
            FiredDt.Text = manager.FiredDt.ToString();
        }
    }
}
