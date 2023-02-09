using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ADO
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string ConnectionString { get => @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\luche\Desktop\ADO\ADO\ADO\ADO.mdf;Integrated Security=True"; }
        public static string ConnectionStringMySql { get => @"Server=eu-central.connect.psdb.cloud;Database=testing;user=unu6yunbow6yswtrw416;password=pscale_pw_GCzjevcRLmlL4n1W342E80Zn0FXGa20njpB2WjQy6TH;SslMode=VerifyFull;"; }
    }
}
