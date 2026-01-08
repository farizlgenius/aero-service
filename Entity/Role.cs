using HIDAeroService.Entity.Interface;
using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class Role : IComponentId,IDatetime
    {
        [Key]
        public int id { get; set; }
        public short component_id { get; set; }
        public string name { get; set; } =string.Empty;
        public ICollection<FeatureRole> feature_roles { get; set; }
        public ICollection<Operator> operators { get; set; }
        public DateTime created_date { get; set; }
        public DateTime updated_date { get; set; }
    }
}
