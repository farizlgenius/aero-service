using HIDAeroService.DTO.Procedure;
using HIDAeroService.DTO.Transactions;
using HIDAeroService.Entity.Interface;

namespace HIDAeroService.DTO.Trigger
{
    public class TriggerDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public short Command { get; set; }
        public short ProcedureId { get; set; }
        public short SourceType { get; set; }
        public short SourceNumber { get; set; }
        public short TranType { get; set; }
        public List<TransactionCodeDto> CodeMap { get; set; }
        public short TimeZone { get; set; }

    }
}
