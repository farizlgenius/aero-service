using HIDAeroService.Entity;

namespace HIDAeroService.DTO.Feature
{
    public sealed class FeatureDto
    {
        public short ComponentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsWritable { get; set; }
        public bool IsAllow { get; set; }
    }
}
