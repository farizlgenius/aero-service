namespace HIDAeroService.Entity
{
    public sealed class TypeCosElevator : BaseTransactionType
    {
        public string prevFloorStatus { get; set; } = string.Empty;
        public short floorNumber { get; set; }
    }
}
