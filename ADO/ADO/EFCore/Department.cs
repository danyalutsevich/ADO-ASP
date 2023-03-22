using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.EFCore
{
    public class Department
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime? DeleteDt { get; set; }

        public Department()
        {
            Id = Guid.NewGuid();
            Name = "";
            DeleteDt = null;
        }

        public List<Manager> Workers { get; set; }
        public List<Manager> SecWorkers { get; set; }

    }
}
