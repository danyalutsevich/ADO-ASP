using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.Entity
{
    public class Product : ICRUD
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        public void Save()
        {
            using SqlConnection connection = new(App.ConnectionString);
            connection.Open();
            using SqlCommand command = new("UPDATE Products SET Name=@Name,Price=@Price WHERE Id=@Id", connection);
            command.Parameters.AddWithValue("@Name", Name);
            command.Parameters.AddWithValue("@Price", Price);
            command.Parameters.AddWithValue("@Id", Id);
            command.ExecuteNonQuery();
        }
        
        public void Delete()
        {
            using SqlConnection connection = new(App.ConnectionString);
            connection.Open();
            using SqlCommand command = new("DELETE FROM Products WHERE Id=@Id", connection);
            command.Parameters.AddWithValue("@Id", Id);
            command.ExecuteNonQuery();
        }

        public static void Create(string Name,double Price)
        {
            using SqlConnection connection = new(App.ConnectionString);
            connection.Open();
            using SqlCommand command = new("INSERT INTO Products (Id,Name,Price) VALUES (NEWID(),@Name,@Price)", connection);
            command.Parameters.AddWithValue("@Name", Name);
            command.Parameters.AddWithValue("@Price", Price);
            command.ExecuteNonQuery();
        }
    }
}
