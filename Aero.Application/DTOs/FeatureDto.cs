

namespace Aero.Application.DTOs
{
    public sealed class FeatureDto
    {
        public short ComponentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public List<SubFeatureDto> SubItems { get; set; } = new List<SubFeatureDto>();
        public bool IsAllow { get; set; }
        public bool IsCreate { get; set; }
        public bool IsModify { get; set; }
        public bool IsDelete { get; set; }
        public bool IsAction { get; set; }
    }
}
