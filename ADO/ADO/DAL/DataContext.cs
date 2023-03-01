using ADO.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ADO.DAL
{
    public  class DataContext
    {
        public DepartmentsApi Departments { get; set; }
        public ManagersApi Managers { get; set; }
        private readonly SqlConnection _connection;
     
        public DataContext()
        {
            try
            {
                _connection = new(App.ConnectionString);
                _connection.Open();
            }
            catch (Exception ex)
            {
                App.Logger.Log(ex.Message, "SERVE", this.GetType().Name, MethodInfo.GetCurrentMethod()?.Name ?? "");
                throw new Exception("Data context init error. See logs for details");
            }

            Departments = new DepartmentsApi(_connection,this);
            Managers = new ManagersApi(_connection,this);
        }
    }
}
