using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.EFCore
{
    public class Manager
    {
        public Guid Id { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Secname { get; set; }
        public Guid? Id_main_dep { get; set; } // NOT NULL
        public Guid? Id_sec_dep { get; set; } // NULL
        public Guid? Id_chief { get; set; }
        public DateTime? FiredDt { get; set; }

        public Manager()
        {
            Id = Guid.NewGuid();
            Surname = "";
            Name = "";
            Secname = "";
            Id_main_dep = null;
            Id_sec_dep = null;
            Id_chief = null;
            FiredDt = null;
        }
    }
}
