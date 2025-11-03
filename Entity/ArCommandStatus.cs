using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class ArCommandStatus : BaseEntity
    {
        public int TagNo { get; set; }
        public int ScpId { get; set; }
        public string? ScpMac { get; set; } = string.Empty;
        public string? Command { get; set; } = string.Empty;
        public char CommandStatus { get; set; }
        
        public string? NakReason { get; set; }
        public int NakDescCode { get; set; }
    }
}
