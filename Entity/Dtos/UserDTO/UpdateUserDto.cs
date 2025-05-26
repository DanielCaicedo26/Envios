using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Entity.Dtos.Base;

namespace Entity.Dtos.UserDTO
{
    /// <summary>
    /// DTO para mostrar información básica de un usuario (operación get all, create, update(patch-put))
    /// </summary>
    public class UpdateUserDto : BaseDto
    {
        public  string Email { get; set; }
        public  string Password { get; set; }
    }
}
