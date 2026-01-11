using AeroService.DTO.Feature;
using AeroService.Entity;
using AeroService.Entity.Interface;
using System.ComponentModel.DataAnnotations;

namespace AeroService.DTO.Role
{
    public sealed class RoleDto : IComponentId
    {
        public short component_id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<FeatureDto> Features { get; set; }
    }
}
