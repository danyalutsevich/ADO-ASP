using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace ADO.Entity
{
    public class Product : ICRUD
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public DateTime? DeleteDt { get; set; }

        public Product()
        {
        
        }

        public Product(SqlDataReader reader)
        {
            Id = reader.GetGuid(0);
            Name = reader.GetString(1);
            Price = reader.GetDouble(2);
            DeleteDt = reader.IsDBNull(3) ? null : reader.GetDateTime(3);
        }

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
            using SqlCommand command = new("UPDATE Products SET DeleteDt = CURRENT_TIMESTAMP WHERE Id=@Id", connection);
            command.Parameters.AddWithValue("@Id", Id);
            command.ExecuteNonQuery();
        }

        public static void Create(string Name, double Price)
        {
            using SqlConnection connection = new(App.ConnectionString);
            connection.Open();
            using SqlCommand command = new("INSERT INTO Products (Id,Name,Price) VALUES (NEWID(),@Name,@Price)", connection);
            command.Parameters.AddWithValue("@Name", Name);
            command.Parameters.AddWithValue("@Price", Price);
            command.ExecuteNonQuery();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
