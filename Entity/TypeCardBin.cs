namespace HIDAeroService.Entity
{
    public sealed class TypeCardBin : BaseTransactionType
    {
        public int bitCount { get; set; }
        public string bitArray { get; set; } = string.Empty;
    }
}
