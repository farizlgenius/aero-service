using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class FeatureRole
    {
        [Key]
        public int Id { get; set; }
        public short FeatureId { get; set; }
        public Feature Feature { get; set; }
        public short RoleId { get; set; }
        public Role Role { get; set; }
    }
}
