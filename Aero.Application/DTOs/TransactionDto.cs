using Aero.Domain.Entities;

namespace Aero.Application.DTOs
{
    public sealed class TransactionDto : BaseDomain
    {
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
        public int SerialNumber { get; set; }
        public string Actor { get; set; } = string.Empty;
        public double Source { get; set; }
        public string SourceDesc { get; set; } = string.Empty;
        public string Origin { get; set; } = string.Empty;
        public string SourceModule { get; set; } = string.Empty;
        public double Type { get; set; }
        public string TypeDesc { get; set; } = string.Empty;
        public double TranCode { get; set; }
        public string Image { get; set; } = string.Empty;
        public string TranCodeDesc { get; set; } = string.Empty;
        public string ExtendDesc { get; set; } = string.Empty;
        public string Remark { get; set; } = string.Empty;
        public List<TransactionFlagDto> TransactionFlags { get; set; } = new List<TransactionFlagDto>();

    }
}
