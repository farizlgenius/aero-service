
using System.ComponentModel.DataAnnotations;
using Aero.Domain.Interface;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class Feature 
    {
        [Key]
        public int id { get; set; }
        public short component_id { get; set; }
        public string name { get; set; } = string.Empty;
        public string path { get; set; } = string.Empty;
        public ICollection<FeatureRole> feature_role { get; set; }
        public ICollection<SubFeature> sub_feature { get; set; }
    }
}
