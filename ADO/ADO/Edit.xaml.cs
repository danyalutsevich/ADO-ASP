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

namespace ADO
{
    /// <summary>
    /// Interaction logic for Edit.xaml
    /// </summary>
    public partial class Edit : Window
    {
        public Edit(object item, ORM collection)
        {
            InitializeComponent();
            this.item = item;
            Departments = (ObservableCollection<Department>)collection.DataContext.GetType().GetProperty("Departments").GetValue(collection.DataContext);
            Managers = (ObservableCollection<Manager>)collection.DataContext.GetType().GetProperty("Managers").GetValue(collection.DataContext);
        }

        ObservableCollection<Department> Departments { get; set; }
        ObservableCollection<Manager> Managers { get; set; }

        object item;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //stackpanel.Children.Clear();
            var fields = item.GetType().GetProperties();
            foreach (var field in fields)
            {
                var label = new Label();
                label.Content = field.Name;
                stackpanel.Children.Add(label);
                var textbox = new TextBox();
                textbox.Text = field.GetValue(item).ToString();
                stackpanel.Children.Add(textbox);
            }
            var button = new Button();
            button.Content = "Save";
            button.Click += Button_Click;
            stackpanel.Children.Add(button);

            MessageBox.Show(item.GetType().ToString());

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (item is Department)
            {
                item.GetType().GetProperty("Id").SetValue(item, Guid.Parse(((TextBox)stackpanel.Children[1]).Text));
                item.GetType().GetProperty("Name").SetValue(item, ((TextBox)stackpanel.Children[3]).Text);

                int index = Departments.IndexOf(item as Department);
                Departments.RemoveAt(index);
                Departments.Insert(index, item as Department);
                this.Close();
            }
        }
    }
}
