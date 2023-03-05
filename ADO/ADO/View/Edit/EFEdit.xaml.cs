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
    /// Interaction logic for EFEdit.xaml
    /// </summary>
    public partial class EFEdit : Window
    {
        public EFEdit(object item)
        {
            InitializeComponent();
            this.item = item;
        }
        private readonly object item;

        private void Render()
        {
            var fields = item.GetType().GetProperties();
            foreach (var field in fields)
            {
                var label = new Label();
                label.Content = field.Name;
                stackpanel.Children.Add(label);
                var value = field.GetValue(item)?.ToString();
                var fieldType = field.GetValue(item)?.GetType();
                
                if ((fieldType?.Name == typeof(Guid).Name|| fieldType?.Name == null) && field.Name != "Id")
                {
                    var combobox = new ComboBox();
                    var owner = (Owner as EFCoreWindow).efContext;

                    if ((bool)owner?.Departments.Any(d => d.Id.ToString() == value)||value ==null)
                    {
                        owner.Departments.ToList().ForEach(d =>
                        {
                            combobox.Items.Add(d);
                        });
                    }
                    if((bool)owner?.Managers.Any(m => m.Id.ToString() == value)||value == null)
                    {
                        owner.Managers.ToList().ForEach(m =>
                        {
                            combobox.Items.Add(m);
                        });
                    }
                    if((bool)owner?.Products.Any(p => p.Id.ToString() == value)||value == null)
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
            button.Click += Button_Save;
            stackpanel.Children.Add(button);

            var buttonDelete = new Button();
            buttonDelete.Content = "Delete";
            buttonDelete.Click += Button_Delete;
            stackpanel.Children.Add(buttonDelete);

        }

        public void Button_Save(object sender, RoutedEventArgs args)
        {
            if (item as Manager != null)
            {
                var manager = item as Manager;

                manager.Id = Guid.Parse((stackpanel.Children[1] as TextBox).Text);
                manager.Name = (stackpanel.Children[3] as TextBox).Text;
                manager.Surname = (stackpanel.Children[5] as TextBox).Text;
                manager.Id_main_dep = ((stackpanel.Children[7] as ComboBox).SelectedValue as Department).Id;
                manager.Id_sec_dep = ((stackpanel.Children[9] as ComboBox).SelectedValue as Department).Id;
                manager.Id_chief = ((stackpanel.Children[11] as ComboBox).SelectedValue as Department).Id;

                (Owner as EFCoreWindow)?.efContext.Managers.Add(manager);
            }
            else if (item as Product != null)
            {
                var product = item as Product;
                (Owner as EFCoreWindow)?.efContext.Products.Remove(product);
            }
            else if (item as Department != null)
            {
                var department = item as Department;
                (Owner as EFCoreWindow)?.efContext.Departments.Remove(department);
            }
            else if (item as Sale != null)
            {
                var sale = item as Sale;
                (Owner as EFCoreWindow)?.efContext.Sales.Remove(sale);
            }
        }

        public void Button_Delete(object sender, RoutedEventArgs args)
        {
            if (item as Manager != null)
            {
                (Owner as EFCoreWindow).efContext.Managers.Remove(item as Manager);
            }
            else if (item as Product != null)
            {
                (Owner as EFCoreWindow).efContext.Products.Remove(item as Product);
            }
            else if (item as Department != null)
            {
                (Owner as EFCoreWindow).efContext.Departments.Remove(item as Department);
            }
            else if (item as Sale != null)
            {
                (Owner as EFCoreWindow).efContext.Sales.Remove(item as Sale);
            }
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Render();
        }
    }
}
