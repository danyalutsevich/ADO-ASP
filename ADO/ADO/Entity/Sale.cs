using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.Entity
{
    public class Sale : ICRUD
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid ManagerId { get; set; }
        public int Count { get; set; }
        public DateTime SaleDt { get; set; }
        public DateTime? DeleteDt { get; set; }

        public Sale()
        {
            Id = Guid.NewGuid();
            Count = 1;
            SaleDt = DateTime.Now;
        }
        
        public Sale(SqlDataReader reader)
        {
            Id = reader.GetGuid("Id");
            ProductId = reader.GetGuid("ProductId");
            ManagerId = reader.GetGuid("ManagerId");
            Count = reader.GetInt32("Cnt");
            SaleDt = reader.GetDateTime("SaleDt");
            DeleteDt = reader.IsDBNull("DeleteDt") ? null : reader.GetDateTime("DeleteDt");
        }
        
        public void Create()
        {
            using var connection = new System.Data.SqlClient.SqlConnection(App.ConnectionString);
            connection.Open();
            using SqlCommand command = new("INSERT INTO Sales (Id, ProductId, ManagerId, Count, SaleDt) VALUES (@Id, @ProductId, @ManagerId, @Count, @SaleDt)", connection);
            command.Parameters.AddWithValue("@Id", Id);
            command.Parameters.AddWithValue("@ProductId", ProductId);
            command.Parameters.AddWithValue("@ManagerId", ManagerId);
            command.Parameters.AddWithValue("@Count", Count);
            command.Parameters.AddWithValue("@SaleDt", SaleDt);
            command.ExecuteNonQuery();
        }

        public void Delete()
        {
            using var connection = new System.Data.SqlClient.SqlConnection(App.ConnectionString);
            connection.Open();
            using SqlCommand command = new("UPDATE Sales SET DeleteDt = @DeleteDt WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", Id);
            command.Parameters.AddWithValue("@DeleteDt", DateTime.Now);
            command.ExecuteNonQuery();
        }

        public void Save()
        {
            using var connection = new System.Data.SqlClient.SqlConnection(App.ConnectionString);
            connection.Open();
            using SqlCommand command = new("UPDATE Sales SET ProductId = @ProductId, ManagerId = @ManagerId, Count = @Count, SaleDt = @SaleDt WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", Id);
            command.Parameters.AddWithValue("@ProductId", ProductId);
            command.Parameters.AddWithValue("@ManagerId", ManagerId);
            command.Parameters.AddWithValue("@Count", Count);
            command.Parameters.AddWithValue("@SaleDt", SaleDt);
            command.ExecuteNonQuery();
        }

        public override string ToString()
        {
            return $"{ProductId} {Count} {SaleDt}";
        }

    }
}
