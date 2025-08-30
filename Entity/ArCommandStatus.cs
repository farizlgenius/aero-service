using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class ArCommandStatus : ArBaseEntity
    {
        public int TagNo { get; set; }
        public int ScpId { get; set; }
        public string ScpMac { get; set; }
        public string Command { get; set; }
        public char CommandStatus { get; set; }
        
        public string? NakReason { get; set; }
        public int NakDescCode { get; set; }
    }
}
