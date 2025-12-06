namespace HIDAeroService.Entity
{
    public sealed class TypeCardFull : BaseTransactionType
    {
        public short formatNumber { get; set; }
        public int facilityCode { get; set; }
        public int cardHolderId { get; set; }
        public short issueCode { get; set; }
        public short floorNumber { get; set; }
        public string encodedCard { get; set; } = string.Empty;
    }
}
