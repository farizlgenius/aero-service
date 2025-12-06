namespace HIDAeroService.Entity
{
    public sealed class TypeCardBcd : BaseTransactionType
    {
        public int digitCount { get; set; }
        public string bcdArray { get; set; } = string.Empty;
    }
}
