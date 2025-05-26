using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Dtos.Base;

namespace Entity.Dtos.RolUserDTO
{
    /// <summary>
    /// DTO para la creación de una nueva asignación de rol a usuario (operación DELETE Logical)
    /// </summary>
    public class DeleteLogicalRolUserDto : BaseDto
    {
        public DeleteLogicalRolUserDto()
        {
            Status = false;
        }
    }
}
