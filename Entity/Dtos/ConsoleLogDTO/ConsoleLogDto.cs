using System.ComponentModel.DataAnnotations;
using Entity.Dtos.Base;

namespace Entity.Dtos.ConsoleLogDTO
{
    public class ConsoleLogDto : BaseDto
    {
        [Required]
        public string TableName { get; set; }

        [Required]
        public int RecordId { get; set; }

        [Required]
        public string OperationType { get; set; }

        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        public string? AdditionalInfo { get; set; }
    }
}