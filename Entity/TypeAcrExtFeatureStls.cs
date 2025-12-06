namespace HIDAeroService.Entity
{
    public sealed class TypeAcrExtFeatureStls : BaseTransactionType
    {
        public short nExtFeatureType { get; set; }
        public string nHardwareType { get; set; } = string.Empty;

    }
}
