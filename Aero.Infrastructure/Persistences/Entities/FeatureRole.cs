using System.ComponentModel.DataAnnotations;

namespace Aero.Infrastructure.Data.Entities
{
    public sealed class FeatureRole
    {
        //[Key]
        //public int id { get; set; }
        public short feature_id { get; set; }
        public Feature feature { get; set; }
        public short role_id { get; set; }
        public Role role { get; set; }
        public bool is_allow { get; set; }
        public bool is_create { get; set; }
        public bool is_modify { get; set; }
        public bool is_delete { get; set; }
        public bool is_action { get; set; }
    }
}
