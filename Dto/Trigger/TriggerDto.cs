using HIDAeroService.Entity.Interface;

namespace HIDAeroService.DTO.Trigger
{
    public class TriggerDto : NoMacBaseDto
    {
        public short Command { get; set; }
        public short ProcedureId { get; set; }
        public short SourceType { get; set; }
        public short SourceId { get; set; }
        public short TransactionType { get; set; }
        public int CodeMap {  get; set; }
        public short TimeZoneId { get; set; }
        public ProcedureDto procedurDto { get; set; }
    }
}
