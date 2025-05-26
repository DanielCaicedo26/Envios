using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Entity.Dtos.Base;

namespace Entity.Dtos.RolUserDTO
{
    /// <summary>
    /// DTO para mostrar información básica de una asignación de rol a usuario (operación GET ALL, CREATE, UPDATE(PATCH-PUT))
    /// </summary>
    public class RolUserDto : BaseDto
    {
        public int RolId { get; set; }
        public int UserId { get; set; }
    }
}
