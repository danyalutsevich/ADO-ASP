using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ADO.Entity
{
    public class Manager
    {
        public Guid Id { get; set; }
        public string Surname { get; set; } 
        public string Name { get; set; } 
        public string Secname { get; set; } 
        public Guid Id_main_dep { get; set; } // NOT NULL
        public Guid? Id_sec_dep { get; set; } // NULL
        public Guid? Id_chief { get; set; }
    }
}
