using HIDAeroService.Entity.Interface;
using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class Feature : IComponentId
    {
        [Key]
        public int Id { get; set; }
        public short ComponentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsWritable { get; set; }
        public ICollection<FeatureRole> FeatureRoles { get; set; }

    }
}
