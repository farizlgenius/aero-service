using System.ComponentModel.DataAnnotations;
using Aero.Domain.Interface;

namespace Aero.Infrastructure.Data.Entities
{
    public sealed class Procedure : IDatetime, IComponentId
    {
        [Key]
        public int Id { get; set; }
        public short proc_id { get; set; }
        public short component_id { get; set; }
        public string name { get; set; } = string.Empty;
        public Trigger trigger { get; set; }
        public ICollection<Action> actions { get; set; }
        public DateTime created_date { get; set; }
        public DateTime updated_date { get; set; }
        public short location_id { get; set; } = 1;
        public Location location { get; set; }


    }
}
