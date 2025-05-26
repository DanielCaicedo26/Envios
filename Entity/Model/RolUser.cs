using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Model.Base;

namespace Entity.Model
{
    public class RolUser : BaseEntity
    {
        public int RolId { get; set; }
        public int UserId { get; set; }

        public  Rol Rol { get; set; }
        public  User User { get; set; }
    }
}
