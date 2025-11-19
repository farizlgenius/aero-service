using HIDAeroService.Entity.Interface;
using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class CommandStatus : IDatetime
    {
        [Key]
        public int Id { get; set; }
        public string Uuid { get; set; } = Guid.NewGuid().ToString();
        public int TagNo { get; set; }
        public int ScpId { get; set; }
        public string? ScpMac { get; set; } = string.Empty;
        public string? Command { get; set; } = string.Empty;
        public char Status { get; set; }   
        public string? NakReason { get; set; }
        public int NakDescCode { get; set; }
        public string MacAddress { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
