using System.ComponentModel.DataAnnotations;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class FeatureRole
    {
        [Key]
        public int id { get; set; }
        public int feature_id { get; set; }
        public Feature feature { get; set; }
        public short role_id { get; set; }
        public Role role { get; set; }
        public bool is_allow { get; set; }
        public bool is_create { get; set; }
        public bool is_modify { get; set; }
        public bool is_delete { get; set; }
        public bool is_action { get; set; }

        public FeatureRole(){}

        public FeatureRole(int feature,short role,bool allow,bool create,bool modify,bool delete,bool action)
        {
            this.feature_id = feature;
            this.role_id = role;
            this.is_allow = allow;
            this.is_create = create;
            this.is_modify = modify;
            this.is_delete = delete;
            this.is_action = action;
        }

       

    }
}
