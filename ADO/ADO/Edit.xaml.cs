﻿using ADO.Entity;
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
                var textbox = new TextBox();
                textbox.Text = field.GetValue(item)?.ToString();
                var fieldType = field.GetValue(item)?.GetType();
                //if (fieldType?.Name == typeof(Guid).Name) // TO BLOCK ALL GUID TEXTBOXES
                if (field.Name=="Id")
                {
                    textbox.IsEnabled = false;
                }
                stackpanel.Children.Add(textbox);
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
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (item is Department)
            {
                var department = item as Department;
                department.Id = Guid.Parse((stackpanel.Children[1] as TextBox).Text);
                department.Name = (stackpanel.Children[3] as TextBox).Text;
                //department.Save();
            }
            if (item is Manager)
            {
                var manager = item as Manager;
                manager.Id = Guid.Parse((stackpanel.Children[1] as TextBox)?.Text);
                manager.Surname = (stackpanel.Children[3] as TextBox).Text;
                manager.Name = (stackpanel.Children[5] as TextBox).Text;
                manager.Secname = (stackpanel.Children[7] as TextBox).Text;
                manager.Id_main_dep = Guid.Parse((stackpanel.Children[9] as TextBox)?.Text);
                manager.Id_sec_dep = Guid.Parse((stackpanel.Children[11] as TextBox)?.Text);
                manager.Id_chief = Guid.Parse((stackpanel.Children[13] as TextBox)?.Text);
                //manager.Save();
            }
            if (item is Product)
            {
                var product = item as Product;
                product.Id = Guid.Parse((stackpanel.Children[1] as TextBox)?.Text);
                product.Name = (stackpanel.Children[3] as TextBox).Text;
                product.Price = double.Parse((stackpanel.Children[5] as TextBox).Text);
                //product.Save();
            }
            
            (item as ICRUD).Save();
            this.Close();

        }
    }
}