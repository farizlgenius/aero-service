using HIDAeroService.DTO.Feature;
using HIDAeroService.Entity;
using HIDAeroService.Entity.Interface;
using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.DTO.Role
{
    public sealed class RoleDto : IComponentId
    {
        public short ComponentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<FeatureDto> Fetures { get; set; }
    }
}
