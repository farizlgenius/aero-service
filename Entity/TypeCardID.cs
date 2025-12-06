namespace HIDAeroService.Entity
{
    public sealed class TypeCardID : BaseTransactionType
    {
        public short formatNumber { get; set; }
        public int cardHolderId { get; set; }
        public short floorNumber { get; set; }
        public short cardTypeFlag { get; set; }
    }
}
