
using Aero.Application.Interfaces;
using Aero.Domain.Entities;

namespace Aero.Application.DTOs
{
    public sealed class RoleDto 
    {
        public short ComponentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<FeatureDto> Features { get; set; } = new List<FeatureDto>();
    }
}
