namespace HIDAeroService.Entity
{
    public sealed class Trigger : BaseEntity
    {
        public short Command { get; set; }
        public short ProcedureId { get; set; }
        public short SourceType { get; set; }
        public short SourceNumber { get; set; }
        public short TranType { get; set; }
        public short CodeMap { get; set; }
        public short TimeZone { get; set; }
        public Entity.Procedure Procedure { get; set; }

    }
}
