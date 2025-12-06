namespace HIDAeroService.Entity
{
    public sealed class TypeAcrExtFeatureCoS : BaseTransactionType
    {
        public short nExtFeatureType { get; set; }
        public string nHardwareType { get; set; } = string.Empty;
        public string nExtFeaturePoint { get; set; } = string.Empty;
        public short nStatus { get; set; } 
        public short nStatusPrior {  get; set; }
    }
}
