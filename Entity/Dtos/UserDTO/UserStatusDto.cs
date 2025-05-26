using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos.UserDTO
{
    /// <summary>
    /// DTO para cambiar el estado de activación de un usuario
    /// </summary>
    public class UserStatusDto
    {
        public bool IsActive { get; set; }
    }
}