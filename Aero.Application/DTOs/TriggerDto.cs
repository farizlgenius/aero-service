
using Aero.Domain.Entities;

namespace Aero.Application.DTOs
{
    public class TriggerDto : BaseEntity
    {
        public short TrigId {get; set;}
        public string Name { get; set; } = string.Empty;
        public short Command { get; set; }
        public short ProcedureId { get; set; }
        public short SourceType { get; set; }
        public short SourceNumber { get; set; }
        public short TranType { get; set; }
        public List<TransactionCodeDto> CodeMap { get; set; } = new List<TransactionCodeDto>();
        public short TimeZone { get; set; }

    }
}
