using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ADO.Entity
{
    public class Manager : ICRUD
    {
        public Guid Id { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Secname { get; set; }
        public Guid Id_main_dep { get; set; } // NOT NULL
        public Guid? Id_sec_dep { get; set; } // NULL
        public Guid? Id_chief { get; set; }


        public void Save()
        {
            using var connection = new System.Data.SqlClient.SqlConnection(App.ConnectionString);
            connection.Open();
            using var command = new System.Data.SqlClient.SqlCommand("UPDATE Managers SET Surname = @Surname, Name = @Name, Secname = @Secname, Id_main_dep = @Id_main_dep, Id_sec_dep = @Id_sec_dep, Id_chief = @Id_chief WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", Id);
            command.Parameters.AddWithValue("@Surname", Surname);
            command.Parameters.AddWithValue("@Name", Name);
            command.Parameters.AddWithValue("@Secname", Secname);
            command.Parameters.AddWithValue("@Id_main_dep", Id_main_dep);
            command.Parameters.AddWithValue("@Id_sec_dep", Id_sec_dep);
            command.Parameters.AddWithValue("@Id_chief", Id_chief);
            command.ExecuteNonQuery();
        }

        public void Delete()
        {
            using var connection = new System.Data.SqlClient.SqlConnection(App.ConnectionString);
            connection.Open();
            using var command = new System.Data.SqlClient.SqlCommand("DELETE FROM Managers WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", Id);
            command.ExecuteNonQuery();
        }


        public static void Create(string surname, string name, string secname, Department main_dep, Department sec_dep, Manager chief)
        {
            using var connection = new System.Data.SqlClient.SqlConnection(App.ConnectionString);
            connection.Open();
            using var command = new System.Data.SqlClient.SqlCommand("INSERT INTO Managers (Id,Surname, Name, Secname, Id_main_dep, Id_sec_dep, Id_chief) VALUES (NEWID(), @Surname, @Name, @Secname, @Id_main_dep, @Id_sec_dep, @Id_chief)", connection);
            command.Parameters.AddWithValue("@Surname", surname);
            command.Parameters.AddWithValue("@Name", name);
            command.Parameters.AddWithValue("@Secname", secname);
            command.Parameters.AddWithValue("@Id_main_dep", main_dep.Id);
            command.Parameters.AddWithValue("@Id_sec_dep", sec_dep.Id);
            command.Parameters.AddWithValue("@Id_chief", chief.Id);
            command.ExecuteScalar();
        }
    }
}
