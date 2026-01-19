
using Aero.Application.Interfaces;

namespace Aero.Application.DTOs
{
    public sealed class RoleDto : IComponentId
    {
        public short component_id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<FeatureDto> Features { get; set; }
    }
}
