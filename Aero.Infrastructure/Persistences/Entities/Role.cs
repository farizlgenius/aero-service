using System.ComponentModel.DataAnnotations;
using Aero.Domain.Interface;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class Role : BaseEntity
    {
        public short driver_id { get; set; }
        public string name { get; set; } = string.Empty;
        public ICollection<FeatureRole> feature_roles { get; set; }
        public ICollection<Operator> operators { get; set; }

        public Role(){}

        public Role(Aero.Domain.Entities.Role data) : base(data.LocationId)
        {
            this.driver_id = data.DriverId;
            this.name = data.Name;
            this.feature_roles = data.Features.Select(f => new FeatureRole(f.Id,data.DriverId,f.IsAllow,f.IsCreate,f.IsModify,f.IsDelete,f.IsAction)).ToList();
        }

        public void Update(Aero.Domain.Entities.Role data)
        {
            this.driver_id = data.DriverId;
            this.name = data.Name;
            this.feature_roles = data.Features.Select(f => new FeatureRole(f.Id, data.DriverId, f.IsAllow, f.IsCreate, f.IsModify, f.IsDelete, f.IsAction)).ToList();
            this.updated_date = DateTime.UtcNow;
        }

    }
}
