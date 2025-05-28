using Entity.Model.Base;
using System.ComponentModel.DataAnnotations;

namespace Entity.Model
{
    /// <summary>
    /// Entidad para registrar logs de auditoría de cambios en el sistema
    /// </summary>
    public class ConsoleLog : BaseEntity
    {
        /// <summary>
        /// Tabla que fue modificada
        /// </summary>
        [Required]
        [StringLength(100)]
        public string TableName { get; set; }

        /// <summary>
        /// ID del registro que fue modificado
        /// </summary>
        [Required]
        public int RecordId { get; set; }

        /// <summary>
        /// Tipo de operación realizada (INSERT, UPDATE, DELETE)
        /// </summary>
        [Required]
        [StringLength(10)]
        public string OperationType { get; set; }

        /// <summary>
        /// Valores anteriores del registro (en formato JSON)
        /// </summary>
        public string? OldValues { get; set; }

        /// <summary>
        /// Nuevos valores del registro (en formato JSON)
        /// </summary>
        public string? NewValues { get; set; }

        /// <summary>
        /// ID del usuario que realizó el cambio
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// Nombre de usuario que realizó el cambio
        /// </summary>
        [StringLength(100)]
        public string? UserName { get; set; }

        /// <summary>
        /// Dirección IP desde donde se realizó el cambio
        /// </summary>
        [StringLength(45)]
        public string? IpAddress { get; set; }

        /// <summary>
        /// User Agent del navegador/aplicación
        /// </summary>
        [StringLength(500)]
        public string? UserAgent { get; set; }

        /// <summary>
        /// Timestamp específico del cambio
        /// </summary>
        [Required]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Información adicional o comentarios
        /// </summary>
        public string? AdditionalInfo { get; set; }
    }
}