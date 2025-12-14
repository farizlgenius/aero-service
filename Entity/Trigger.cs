namespace HIDAeroService.Entity
{
    public sealed class Trigger : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public short Command { get; set; }
        public short ProcedureId { get; set; }
        public short SourceType { get; set; }
        public short SourceNumber { get; set; }
        public short TranType { get; set; }
        public ICollection<TriggerTranCode> CodeMap { get; set; }
        public short TimeZone { get; set; }
        public Entity.Procedure Procedure { get; set; }

    }
}
