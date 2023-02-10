using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ADO.Entity
{
    public class Department : ICRUD
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public void Save()
        {
            using var connection = new System.Data.SqlClient.SqlConnection(App.ConnectionString);
            connection.Open();
            using var command = new System.Data.SqlClient.SqlCommand("UPDATE Departments SET Name = @Name WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", Id);
            command.Parameters.AddWithValue("@Name", Name);
            command.ExecuteNonQuery();
        }

        public void Delete()
        {
            using var connection = new System.Data.SqlClient.SqlConnection(App.ConnectionString);
            connection.Open();
            using var command = new System.Data.SqlClient.SqlCommand("DELETE FROM Departments WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", Id);
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
