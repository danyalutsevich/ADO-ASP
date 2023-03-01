using ADO.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace ADO.DAL
{
    public class DepartmentsApi
    {
        private readonly SqlConnection _connection;
        private readonly Service.ILogger logger;
        public DataContext context;
        public DepartmentsApi(SqlConnection connection, DataContext context)
        {
            _connection = connection;
            logger = App.Logger;
           this.context = context;
        }

        public List<Department> GetAll()
        {
            var departments = new List<Department>();
            
            try
            {
                using SqlCommand command = new("SELECT D.Id, D.Name, D.DeleteDt FROM Departments D", _connection);
                using var reader = command.ExecuteReader();
                departments.Clear();
                while (reader.Read())
                {
                    var department = new Department(reader,context);
                    departments.Add(department);
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                logger.Log(ex.Message, "SERVE", this.GetType().Name, MethodInfo.GetCurrentMethod()?.Name ?? "");
            }
            return departments;
        }

        public void Save(Department department)
        {
            using var connection = new System.Data.SqlClient.SqlConnection(App.ConnectionString);
            connection.Open();
            using var command = new System.Data.SqlClient.SqlCommand("UPDATE Departments SET Name = @Name WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", department);
            command.Parameters.AddWithValue("@Name", department);
            command.ExecuteNonQuery();
        }

        public void Delete(Department department)
        {
            using var connection = new System.Data.SqlClient.SqlConnection(App.ConnectionString);
            connection.Open();
            using var command = new System.Data.SqlClient.SqlCommand("UPDATE Departments SET DeleteDt = CURRENT_TIMESTAMP WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", department);
            command.ExecuteNonQuery();
        }

        public static void Create(string name)
        {
            using var connection = new System.Data.SqlClient.SqlConnection(App.ConnectionString);
            connection.Open();
            using var command = new System.Data.SqlClient.SqlCommand("INSERT INTO Departments (Id,Name) VALUES (NEWID(),@Name)", connection);
            command.Parameters.AddWithValue("@Name", name);
            command.ExecuteNonQuery();
        }


    }
}
