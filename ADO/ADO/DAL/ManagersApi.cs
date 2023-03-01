using ADO.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ADO.DAL
{
    public class ManagersApi
    {
        private readonly SqlConnection _connection;
        private readonly Service.ILogger logger;
        private DataContext dataContext { get; set; }
        public ManagersApi(SqlConnection connection, DataContext dataContext)
        {
            this.dataContext = dataContext;
            _connection = connection;
            logger = App.Logger;
        }

        public List<Manager> GetAll()
        {
            var Managers = new List<Manager>();
            try
            {
                using SqlCommand command = new("SELECT M.Id, M.Surname, M.Name, M.Secname, M.Id_main_dep, M.Id_sec_dep, M.Id_chief, M.FiredDt FROM Managers M", _connection);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var manager = new Manager(reader,dataContext);
                    Managers.Add(manager);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                logger.Log(ex.Message, "SERVE", this.GetType().Name, MethodInfo.GetCurrentMethod()?.Name ?? "");
            }
            return Managers;
        }

    }
}
