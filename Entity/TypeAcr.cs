namespace HIDAeroService.Entity
{
    public sealed class TypeAcr : BaseTransactionType
    {
        public short actlFlag { get; set; }
        public short priorFlag { get; set; }
        public string priorMode { get; set; } = string.Empty;
        public short actlFlagE {get; set;} 
        public short priorFlagE {get; set;}
    }
}
