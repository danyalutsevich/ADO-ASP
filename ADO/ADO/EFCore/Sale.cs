using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.EFCore
{
    public class Sale
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
            ProductId = Guid.Empty;
            ManagerId = Guid.Empty;
            Count = 0;
            SaleDt = DateTime.Now;
            DeleteDt = null;
        }
        // public Manager Manager { get; set; }
        // public Product Product { get; set; }
    }
}
