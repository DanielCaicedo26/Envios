using Entity.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
     public class Permission : BaseEntity
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<RolFormPermission> RolPermissions { get; set; }


    }
}
