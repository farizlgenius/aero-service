using System.ComponentModel.DataAnnotations;
using Aero.Domain.Interface;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class Role : BaseEntity
    {

        public string name { get; set; } =string.Empty;
        public ICollection<FeatureRole> feature_roles { get; set; }
        public ICollection<Operator> operators { get; set; }

    }
}
