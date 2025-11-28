using HIDAeroService.Entity.Interface;
using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class SubFeature : IComponentId
    {
        [Key]
        public int Id { get; set; }
        public short ComponentId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public short FeatureId { get; set; }
        public Feature Features { get; set; }
    }
}
