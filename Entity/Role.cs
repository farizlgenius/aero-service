using HIDAeroService.Entity.Interface;
using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class Role : IComponentId,IDatetime
    {
        [Key]
        public int Id { get; set; }
        public short ComponentId { get; set; }
        public string Name { get; set; } =string.Empty;
        public ICollection<FeatureRole> FeatureRoles { get; set; }
        public ICollection<Operator> Operators { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
