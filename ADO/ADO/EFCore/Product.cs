using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.EFCore
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public DateTime? DeleteDt { get; set; }

        public Product()
        {
            Id = Guid.NewGuid();
            Name = "";
            Price = 0;
            DeleteDt = null;
        }

        // public Manager manager { get; set; }
        // public Product product { get; set; }
        public List<Sale> Sales { get; set; }
        public List<Manager> Managers { get; set; }
    }
}
