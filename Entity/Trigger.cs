namespace HIDAeroService.Entity
{
    public sealed class Trigger : BaseEntity
    {
        public string name { get; set; } = string.Empty;
        public short command { get; set; }
        public short procedure_id { get; set; }
        public short source_type { get; set; }
        public short source_number { get; set; }
        public short tran_type { get; set; }
        public string hardware_mac { get; set; } = string.Empty;
        public Hardware hardware { get; set; }
        public ICollection<TriggerTranCode> code_map { get; set; }
        public short timezone { get; set; }
        public Entity.Procedure procedure { get; set; }

    }
}
