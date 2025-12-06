namespace HIDAeroService.Entity
{
    public sealed class TypeUseLimit : BaseTransactionType
    {
        public short useCount { get; set; }
        public long cardHolderId { get; set; }
    }
}
