using System.ComponentModel.DataAnnotations;
using Aero.Domain.Interface;

namespace Aero.Infrastructure.Data.Entities
{
    public sealed class SubFeature : IComponentId
    {
        [Key]
        public int id { get; set; }
        public short component_id { get; set; }
        public string name { get; set; } = string.Empty;
        public string path { get; set; } = string.Empty;
        public short feature_id { get; set; }
        public Feature feature { get; set; }
    }
}
