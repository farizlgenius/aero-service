namespace HIDAeroService.Entity
{
    public sealed class TypeCosElevatorAccess : BaseTransactionType
    {
        public long cardHolderId { get; set; }
        public string floor { get; set; } = string.Empty;
        public short nCardFormat { get; set; }
    }
}
