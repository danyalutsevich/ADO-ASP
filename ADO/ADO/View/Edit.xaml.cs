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
        public Edit(object item)
        {
            InitializeComponent();
            this.item = item;
            //DataContext = this.Owner.DataContext;
        }

        private object item;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var fields = item.GetType().GetProperties();
            foreach (var field in fields)
            {
                var label = new Label();
                label.Content = field.Name;
                stackpanel.Children.Add(label);
                var value = field.GetValue(item)?.ToString();
                var fieldType = field.GetValue(item)?.GetType();

                if (fieldType?.Name == typeof(Guid).Name && field.Name != "Id")  // TO BLOCK ALL GUID TEXTBOXES
                {
                    var combobox = new ComboBox();
                    var owner = Owner as ORM;

                    if ((bool)owner?.Departments.Any(d => d.Id.ToString() == value))
                    {
                        owner.Departments.ToList().ForEach(d =>
                        {
                            combobox.Items.Add(d);
                        });
                    }
                    else if ((bool)owner?.Managers.Any(m => m.Id.ToString() == value))
                    {
                        owner.Managers.ToList().ForEach(m =>
                        {
                            combobox.Items.Add(m);
                        });
                    }
                    else if ((bool)owner?.Products.Any(p => p.Id.ToString() == value))
                    {
                        owner.Products.ToList().ForEach(p =>
                        {
                            combobox.Items.Add(p);
                        });
                    }
                    var reset = new Button();
                    reset.Content = "Reset";
                    reset.Click += (object sender, RoutedEventArgs args) =>
                    {
                        combobox.SelectedValue = null;
                    };
                    stackpanel.Children.Add(combobox);
                    stackpanel.Children.Add(reset);

                }
                else
                {
                    var textbox = new TextBox();
                    textbox.Name = field.Name;
                    textbox.Text = value;

                    if (field.Name == "Id")
                    {
                        textbox.IsEnabled = false;
                    }
                    stackpanel.Children.Add(textbox);
                }
            }
            var button = new Button();
            button.Content = "Save";
            button.Click += Button_Click;
            stackpanel.Children.Add(button);

            var buttonDelete = new Button();
            buttonDelete.Content = "Delete";
            buttonDelete.Click += Button_Delete;
            stackpanel.Children.Add(buttonDelete);

        }

        private void Button_Delete(object sender, RoutedEventArgs e)
        {
            (item as ICRUD).Delete();
            this.Close();
        }

        private dynamic GetValueByChildIndex(int index, Type type)
        {
            var child = stackpanel.Children[index];

            if (child is TextBox)
            {
                try
                {
                    var value = (child as TextBox).Text;
                    return Convert.ChangeType(value,type);
                }
                catch
                {
                    return null;
                }
            }
            else if (child is ComboBox)
            {
                var value = (child as ComboBox).SelectedValue;
                return type.GetProperty("Id").GetValue(value);
            }
            return null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (item is Department)
            {
                var department = item as Department;
                department.Id = Guid.Parse((stackpanel.Children[1] as TextBox).Text);
                department.Name = (stackpanel.Children[3] as TextBox).Text;
            }
            if (item is Manager)
            {
                var manager = item as Manager;
                manager.Id = Guid.Parse((stackpanel.Children[1] as TextBox)?.Text);
                manager.Surname = (stackpanel.Children[3] as TextBox).Text;
                manager.Name = (stackpanel.Children[5] as TextBox).Text;
                manager.Secname = (stackpanel.Children[7] as TextBox).Text;
                manager.Id_main_dep = ((stackpanel.Children[9] as ComboBox)?.SelectedItem as Department)?.Id;
                manager.Id_sec_dep = ((stackpanel.Children[11] as ComboBox)?.SelectedItem as Department)?.Id;
                manager.Id_chief = ((stackpanel.Children[13] as ComboBox)?.SelectedItem as Manager)?.Id;
            }
            if (item is Product)
            {
                var product = item as Product;
                product.Id = Guid.Parse((stackpanel.Children[1] as TextBox)?.Text);
                product.Name = (stackpanel.Children[3] as TextBox).Text;
                product.Price = double.Parse((stackpanel.Children[5] as TextBox).Text);
            }
            if (item is Sale)
            {
                var sale = item as Sale;
                sale.Id = Guid.Parse((stackpanel.Children[1] as TextBox)?.Text);
                //sale.ProductId = ()
            }

            (item as ICRUD).Save();
            this.Close();

        }
    }
}
