using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Model.Base;

namespace Entity.Model
{
    public class Rol : BaseEntity
    {
        public  string Name { get; set; }
        public  string Description { get; set; }
        public  IEnumerable<RolUser> RolUsers { get; set; } 
    }
}
