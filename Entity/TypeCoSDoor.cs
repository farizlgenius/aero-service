namespace HIDAeroService.Entity
{
    public sealed class TypeCoSDoor : BaseTransactionType
    {
        public short doorStatus { get; set; }
        public short apStatus { get; set; }
        public short apPrior {  get; set; }
        public short doorPrior { get; set; }
    }
}
